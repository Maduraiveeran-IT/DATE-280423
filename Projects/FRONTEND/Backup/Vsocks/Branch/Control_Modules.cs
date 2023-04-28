using System;
using System.Windows.Forms;
using System.Drawing;
using DotnetVFGrid;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
//using Microsoft.Office.Core;
using Microsoft.Win32;
using Accounts;
using System.Data;
using System.Net;
using System.Xml;
using System.Runtime.InteropServices;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Net.Mail;
using System.Net.Mime;
using System.Diagnostics;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Text;

namespace Accounts_ControlModules
{
    class Control_Modules
    {
        [DllImport("Kernel32.dll")]
        public static extern bool Beep(UInt32 frequency, UInt32 duration);
        public OdbcConnection Cn = new OdbcConnection();
        public SqlConnection SqlCn = new SqlConnection();        
        public SqlConnection SizingCn = new SqlConnection();
        public SqlCommand SQLCmd;
        public OdbcCommand ODBCCmd;
        public SqlTransaction SQLTrans;
        public OdbcTransaction ODBCTrans;
        public OdbcConnection BackupCn = new OdbcConnection();
        public OdbcConnection TallyCn = new OdbcConnection();
        public OdbcConnection DBFCn = new OdbcConnection();
        public String Server_Name = String.Empty, DB_Name = String.Empty, UserName = String.Empty, Pwd = String.Empty;
        public String BackupDB="PSRBACKUPDB";
        public String DBF_SQL_DB = "VaahiniPSC_Clipper";
        public String DBF_SQL_SERVER = "DATA-SERVER\\SERVER2005";
        public Int32 Def_Height =0, Def_Width = 0;
        public Int32 Def_MaxHeight = 0, Def_MaxWidth = 0;
        public Int32 TermsCount = 0, ItemDetailsCount = 0;
        public DateTime DtDaily;
        public String[] CusAddress;
        public String[] AboveAddress;
        public String Domain_User = String.Empty, Domain_Pwd = String.Empty;
        public String[] TermsArr, ItemDetailsArr, Update_Ledger_BreakupR;
        public String YearCode = String.Empty;
        String DBF_CCode = string.Empty;
        String DBF_Year = string.Empty;
        String Base_Dir = "C:\\Vaahrep";
        public String OraDBName = String.Empty;
        public double Rate_Item_Rate = 0;
        StreamWriter Tally_Edit;
        public Int32 UCode;
        public Int32 EmplNo_TA;
        public Int32 Emplno = 0;

        public enum Grid_Design_Mode
        {
            Column_Wise=0,
            Row_Wise=1,
        }

        public enum FreezeBY
        {
            Column_Wise =0,
            Row_Wise = 1,
        }

        public enum StockUpdate
        {
            Add=0,
            Subtract=1,
        }

        public String User_Code ()
        {
            return System.Environment.GetEnvironmentVariable("User_Code");
        }

        public void Lock_DatetimePicker(ref DateTimePicker Dtp, DateTime Dt)
        {
            try
            {
                Dtp.MinDate = Convert.ToDateTime("01-Jan-1899");
                Dtp.MaxDate = GetServerDate();

                Dtp.MaxDate = Dt;
                Dtp.MinDate = Dt;
                Dtp.Value = Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CurBal_table_Creation_WO_OPBal(int Ledger_Code, DateTime Sdate, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "'  and v1.Approval = 'True' AND V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "'  and v1.Approval = 'True' AND V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode ";

                Execute_Qry(Str, "CBal");

                //Execute_Tbl("Select * from Cbal", "CBal1");
                Execute_Qry("Select * from Cbal", "CBal1");

                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1 ", "Cbal_Mon");

                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon group by ledger_Code, Month_Code", "Cbal_Mon1");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) credit from Cbal_Mon1 v3 ", "Cbal_Month");
                //Execute("update cbal_month set debit = null where debit = 0");
                //Execute("update cbal_month set Credit = null where Credit = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Date_Control(ref DateTimePicker Dtp, Int16 Days)
        {
            DataTable Dt = new DataTable();
            try
            {
                Load_Data("Select cast(GetDate() as Date) DateT", ref Dt);
                Dtp.Enabled = true;
                Dtp.Value = Convert.ToDateTime(Dt.Rows[0][0]);
                Dtp.MaxDate = Convert.ToDateTime(Dt.Rows[0][0]);
                Dtp.MinDate = Dtp.Value.AddDays(2 * (-1));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Validate_Date_For_Entry(DateTime Condition_Date, int Days, DateTime Actual_Date)
        {
            Boolean Flag = false;
            int j = 0;
            try
            {
                if (Actual_Date <= Condition_Date.AddDays(Days))
                {
                    return true;
                }
                else
                {
                    for (DateTime Dt = Condition_Date.AddDays (1); Dt <= Actual_Date; Dt = Dt.AddDays (1))
                    {
                        if (Dt.DayOfWeek.ToString().ToUpper() != "SUNDAY")
                        {
                            j++;
                        }
                    }

                    if (j <= Days)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception ex)
            {
                return Flag;
            }
        }

        public void Change_Connection_String()
        {
            Object ODBC = String.Empty;
            Object SQL = String.Empty;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\Test.xls", Missing, Missing, Missing, (Object)"Vaahini5274", Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                WSheet = (Excel.Worksheet)WBook.Sheets[0];

                ODBC = (Object)WSheet.Cells[1, 1];
                SQL = (Object)WSheet.Cells[2, 1];

                MessageBox.Show(ODBC.ToString());
                WBook.Close(Missing, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CurBal_table_Creation_WO_OPBal_IN_Period(int Ledger_Code, DateTime Sdate, DateTime From, DateTime TO, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v2.user_Date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", TO) + "'  and v1.Approval = 'True' AND V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v2.user_Date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", TO) + "'  and v1.Approval = 'True' AND V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode ";
                Execute_Qry(Str, "CBal");

                //Execute_Tbl("Select * from Cbal", "CBal1");
                Execute_Qry("Select * from Cbal", "CBal1");

                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1 ", "Cbal_Mon");

                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon group by ledger_Code, Month_Code", "Cbal_Mon1");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) credit from Cbal_Mon1 v3 ", "Cbal_Month");
                //Execute("update cbal_month set debit = null where debit = 0");
                //Execute("update cbal_month set Credit = null where Credit = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean Send_SMS_For_Accounts(Int64 Vcode, DateTime Date, int Company_Code, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Int32 Vmode = 0;
            String Ledger = String.Empty, Mobile = String.Empty;
            Double Amount = 0;
            String Message = String.Empty;
            try
            {
                Load_Data("select v1.vcode, v1.vdate, v1.vmode, l1.Ledger_Phone, l1.ledger_Name, v1.vno, v1.user_date, (case when v2.Credit > 0 then v2.credit else v2.debit end) Amount,  (case when v2.Credit > 0 then cast(cast(v2.credit as Numeric(20,2)) as varchar(20)) + ' Cr' else cast(cast(v2.debit as Numeric(20,2)) as varchar(20)) + ' Dr' end) Amount_Text from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code =v2.company_Code and v1.year_Code = v2.year_Code left join ledger_Master l1 on v2.ledger_Code = l1.ledger_Code and l1.company_Code = v2.company_Code and l1.year_Code = v2.year_Code where v1.company_Code = " + Company_Code + " and v1.year_Code = '" + Year_Code + "' and v1.vcode = " + Vcode + " and v1.vdate = '" + String.Format ("{0:dd-MMM-yyyy}", Date) + "' and v2.ledger_Code in (Select ledger_Code from ledger_Master where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_group_Code  in (Select groupcode from groupmas where groupreserved in (4700, 4800) and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'))", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    Vmode = Convert.ToInt32(Dt.Rows[0]["vmode"]);
                    Ledger = Dt.Rows[0]["Ledger_Name"].ToString();
                    Mobile = Dt.Rows[0]["Ledger_Phone"].ToString();
                    Amount = Convert.ToDouble(Dt.Rows[0]["Amount"]);
                    if (Mobile == String.Empty)
                    {
                        MessageBox.Show("Check Mobile Number ...For " + Ledger);
                        return false;
                    }
                    if (Vmode == 5)
                    {
                        Message = "Invoice Raised ...!";
                        Send_SMS(Message, Mobile);
                    }
                    else if (Vmode == 2)
                    {
                        Message = "Cheque Received ...!";
                        Send_SMS(Message, Mobile);
                    }
                    else if (Vmode == 1)
                    {
                        Message = "Payment Raised ...!";
                        Send_SMS(Message, Mobile);
                    }
                    return true;
                }
                else
                {
                    MessageBox.Show("Invalid Details ...!");
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Splitup_lsrce()
        {
            String ODBC, SQL;
            StreamReader S = null;
            StreamWriter R = null;
            try
            {
                if (File.Exists("C:\\Windows\\System32\\lsrce.log"))
                {
                    S = new StreamReader("C:\\Windows\\System32\\lsrce.log");
                    ODBC = S.ReadLine();
                    SQL = S.ReadLine();
                    S.Close();

                    R = new StreamWriter("C:\\Windows\\System32\\lsrceO.log");
                    R.WriteLine(ODBC);
                    R.Close();

                    R = new StreamWriter("C:\\Windows\\System32\\lsrceS.log");
                    R.WriteLine(SQL);
                    R.Close();

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Grid_RowHeight(ref DataGridView Grid, int Height)
        {
            try
            {
                foreach (DataGridViewRow Dr in Grid.Rows)
                {
                    Dr.Height = Height;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_RowHeight(ref DotnetVFGrid.MyDataGridView Grid, int Height)
        {
            try
            {
                foreach (DataGridViewRow Dr in Grid.Rows)
                {
                    Dr.Height = Height;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Insert_SMS(String Message, String NUmber)
        {
            try
            {
                if (Check_Table("SMS_Log") == false)
                {
                    Execute("Create table SMS_Log (SMS_At Datetime, Mobile Varchar(15), Message varchar(3000))");
                }
                Execute("Insert into SMS_Log Values ('" + String.Format("{0:dd-MMM-yyyy} {0:T}", DateTime.Now) + "', '" + NUmber + "', '" + Message + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Send_SMS(String Message, String Number)
        {
            String result = String.Empty;
            try
            {
                HttpWebRequest Req = (HttpWebRequest)HttpWebRequest.Create("http://myc2s.com/sms/sendsms.aspx?uid=rrmills&pass=software&msg=" + Message + "&tonum=" + Number);
                HttpWebResponse Response = (HttpWebResponse)Req.GetResponse();
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                result = Reader.ReadToEnd();
                Insert_SMS(Message, Number);
                if (Get_RecordCount("Socks_Companymas", "COMPNAME LIKE 'RAJARAM%'") > 0)
                {
                    Send_SMS_Rajaram (Message, "9677777807");
                    Send_SMS_Rajaram (Message, "9360000003");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public void Send_SMS_Without(String Message, String Number)
        {
            String result = String.Empty;
            try
            {
                HttpWebRequest Req = (HttpWebRequest)HttpWebRequest.Create("http://myc2s.com/sms/sendsms.aspx?uid=rrmills&pass=software&msg=" + Message + "&tonum=" + Number);
                HttpWebResponse Response = (HttpWebResponse)Req.GetResponse();
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                result = Reader.ReadToEnd();
                Insert_SMS(Message, Number);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Send_SMS_Rajaram (String Message, String Number)
        {
            String result = String.Empty;
            try
            {
                HttpWebRequest Req = (HttpWebRequest)HttpWebRequest.Create("http://myc2s.com/sms/sendsms.aspx?uid=rrmills&pass=software&msg=" + Message + "&tonum=" + Number);
                HttpWebResponse Response = (HttpWebResponse)Req.GetResponse();
                StreamReader Reader = new StreamReader(Response.GetResponseStream());
                result = Reader.ReadToEnd();
                Insert_SMS(Message, Number);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public Int32 Insert_Cotton_Issue(String BDate, Int32 COmpCode, String YearCode, String Mixno, String MixDate, String IssDate, Double Qty, Double Rate, Double Amount)
        {
            Int32 Code = 0;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Insert_ERP_Cotton_ISS '" + BDate + "', " + COmpCode + ", '" + YearCode + "', '" + Mixno + "', '" + MixDate + "', '" + IssDate + "', " + Qty + ", " + Rate + ", " + Amount, Cn);
                Code = Convert.ToInt32(Cmd.ExecuteScalar());
                return Code;
            }
            catch (Exception ex)
            {
                return Code;
            }
        }


        public Int32 Insert_Yarn_Prod(String BDate, Int32 COmpCode, String YearCode, String Stock, String Mixno, String MixDate, String IssDate, Double Qty, Double Rate, Double Amount, Int32 CntCode, double Kgs)
        {
            Int32 Code = 0;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Insert_ERP_yarn_production '" + BDate + "', " + COmpCode + ", '" + YearCode + "', '" + Stock + "', '" + Mixno + "', '" + MixDate + "', '" + IssDate + "', " + Qty + ", " + Rate + ", " + Amount + ", " + CntCode + ", " + Kgs, Cn);
                Code = Convert.ToInt32(Cmd.ExecuteScalar());
                return Code;
            }
            catch (Exception ex)
            {
                return Code;
            }
        }
        String Breakup_XML(Int64 Vcode, DateTime Vdate, int ledger_Code, int CompCode, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Xml_Str = String.Empty;
            try
            {
                Load_Data("select vcode, vdate, ledger_Code, Mode, Refdoc, refDate, (case when Debit <> 0 then (-1) * Debit else Credit end) Amount from voucher_breakup_bills where vcode = " + Vcode + " and vdate = '" + String.Format ("{0:dd-MMM-yyyy}", Vdate) + "' and ledger_Code = " + ledger_Code + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'Order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Xml_Str += " <BILLALLOCATIONS.LIST>";
                    Xml_Str += " <NAME>" + Dt.Rows[i]["refdoc"].ToString() + "</NAME>";
                    Xml_Str += " <BILLCREDITPERIOD>" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["refdate"])) + "</BILLCREDITPERIOD>";
                    if (Dt.Rows[i]["Mode"].ToString() == "N")
                    {
                        Xml_Str += " <BILLTYPE>New Ref</BILLTYPE>";
                    }
                    else if (Dt.Rows[i]["Mode"].ToString() == "A")
                    {
                        Xml_Str += " <BILLTYPE>Agst Ref</BILLTYPE>";
                    }
                    Xml_Str += " <AMOUNT>" + String.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[i]["Amount"])) + "</AMOUNT>";
                    Xml_Str += " </BILLALLOCATIONS.LIST>";
                }
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Byte[] Read_Image_File(String FPath)
        {
            try
            {
                Byte[] data = null;
                FileInfo File_Info = new FileInfo(FPath);
                long NumBytes = File_Info.Length;
                FileStream Fstream = new FileStream(FPath, FileMode.Open, FileAccess.Read);
                BinaryReader Br = new BinaryReader(Fstream);
                data = Br.ReadBytes((int)NumBytes);
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Save_Images(Byte[] Var, String Image_Col_Name, String table_Name, int ledger_Code, int Company_Code, String Year_Code)
        {
            try
            {
                Cn.Open();
                OdbcCommand Cmd = new OdbcCommand();
                Cmd.CommandText = "Insert into Ledger_Scan (Ledger_Code, Company_Code, year_Code, ledger_Image) values (?,?,?,?)";
                Cmd.Connection = Cn;

                Cmd.Parameters.Add("@Ledger_Code", OdbcType.Int, 4);
                Cmd.Parameters.Add("@company_Code", OdbcType.Int, 4);
                Cmd.Parameters.Add("@year_Code", OdbcType.VarChar, 10);
                Cmd.Parameters.Add("@Ledger_Image", OdbcType.Image);
                
                Cmd.Parameters["@Ledger_Code"].Value = ledger_Code;
                Cmd.Parameters["@company_Code"].Value = Company_Code;
                Cmd.Parameters["@year_Code"].Value = Year_Code;
                Cmd.Parameters["@Ledger_Image"].Value = Var;

                int result = Cmd.ExecuteNonQuery();
                Cn.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Set_Picture(ref PictureBox Picture1, ref DataRow Dr, String Image_Col_Name)
        {
            try
            {
                Byte[] ImageData = (Byte[])Dr[Image_Col_Name];
                Image Img1;
                using (MemoryStream Ms = new MemoryStream(ImageData, 0, ImageData.Length))
                {
                    Ms.Write(ImageData, 0, ImageData.Length);
                    Img1 = Image.FromStream(Ms,true);
                }
                Picture1.Image = Img1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Get_Domain_User_Password()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                if (Check_Table("DMDTL") == false)
                {
                    Domain_User = String.Empty;
                    Domain_Pwd = String.Empty;
                }
                else
                {
                    Load_Data("Select * from DMDTl", ref Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0]["name"] == null || Dt.Rows[0]["name"] == DBNull.Value)
                        {
                            Domain_User = String.Empty;
                            Domain_Pwd = String.Empty;
                        }
                        else
                        {
                            Domain_User = Dt.Rows[0]["name"].ToString().Trim();
                            Domain_Pwd = Ascii_Reverse(Dt.Rows[0]["pwd"].ToString());
                        }
                    }
                    else
                    {
                        Domain_User = String.Empty;
                        Domain_Pwd = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        String Ledger_Breakup_XML(int ledger_Code,int CompCode, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Xml_Str = String.Empty;
            try
            {
                Load_Data("select ledger_Code, Mode, refDoc, RefDate, (Case when debit <> 0 then (-1) * Debit else Credit end) Amount, slno, DueDays from ledger_breakup where ledger_Code = " + ledger_Code + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and Term = 'Ledger' order by slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Xml_Str += " <BILLALLOCATIONS.LIST>";
                    Xml_Str += " <BILLDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[i]["refdate"])) + "</BILLDATE>";
                    Xml_Str += " <BILLDATE>" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["refdate"])) + "</BILLDATE>";
                    Xml_Str += " <NAME>" + Dt.Rows[i]["refdoc"].ToString() + "</NAME>";
                    if (Dt.Rows[i]["DueDays"] != null && Dt.Rows[i]["DueDays"] != DBNull.Value)
                    {
                        Xml_Str += " <BILLCREDITPERIOD>" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["refdate"]).AddDays(Convert.ToDouble(Dt.Rows[i]["DueDays"]))) + "</BILLCREDITPERIOD>";
                    }
                    else
                    {
                        Xml_Str += " <BILLCREDITPERIOD>" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["refdate"])) + "</BILLCREDITPERIOD>";
                    }
                    Xml_Str += " <ISADVANCE>No</ISADVANCE>";
                    Xml_Str += " <OPENINGBALANCE>" + String.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[i]["Amount"])) + "</OPENINGBALANCE>";
                    Xml_Str += " </BILLALLOCATIONS.LIST>";
                }
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Is_ledger_breakup_Available(int CompCode, String Year_Code, int Ledger_Code)
        {
            try
            {
                if (Get_RecordCount("Ledger_Breakup", "Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Ledger_Code) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public void Save_Images_Multiple_Company(String CompCode_String, int[] Multiple_Company_Code, String Company_Address_String, String[] Multiple_Company_Address, Byte[] Var, String Image_Col_Name, String table_Name, int ledger_Code, int Company_Code, String Year_Code)
        {
            String SqlInstanceName = SqlServer_InstanceName();
            OdbcTransaction Trans = null;
            OdbcCommand Cmd = null;
            String Sql = String.Empty;
            try
            {
                Cn.Open();
                Trans = Cn.BeginTransaction();


                Cmd = Cn.CreateCommand();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                Cmd.CommandText = "SET XACT_ABORT ON";
                Cmd.ExecuteNonQuery();

                for (int i = 0; i <= Multiple_Company_Code.Length - 1; i++)
                {
                    if (Check_Instance_Running(Multiple_Company_Address[i]))
                    {
                        Cmd = Cn.CreateCommand();
                        Cmd.CommandType = CommandType.Text;
                        Cmd.Transaction = Trans;
                        Cmd.Connection = Cn;

                        Sql = "Insert into " + Company_Address_String + "Ledger_Scan (Ledger_Code, Company_Code, year_Code, ledger_Image) values (?,?,?,?)";

                        Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], i);

                        Cmd.Parameters.Add("@Ledger_Code", OdbcType.Int, 4);
                        Cmd.Parameters.Add("@company_Code", OdbcType.Int, 4);
                        Cmd.Parameters.Add("@year_Code", OdbcType.VarChar, 10);
                        Cmd.Parameters.Add("@Ledger_Image", OdbcType.Image);

                        Cmd.Parameters["@Ledger_Code"].Value = ledger_Code;
                        Cmd.Parameters["@company_Code"].Value = Multiple_Company_Code[i];
                        Cmd.Parameters["@year_Code"].Value = Year_Code;
                        Cmd.Parameters["@Ledger_Image"].Value = Var;

                        int result = Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();

                Cn.Close();
            }
            catch (Exception ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw ex;
            }
        }



        public DateTime GetServerDate()
        {
            DateTime Dt = DateTime.Now;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select GetDate()", Cn);
                Dt = Convert.ToDateTime(Cmd.ExecuteScalar());
                return Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dt));
            }
            catch (Exception ex)
            {
                return Convert.ToDateTime(String.Format("{0:dd-MMM-yyyy}", Dt));
            }
        }


        public DateTime GetServerDateTime()
        {
            DateTime Dt = DateTime.Now;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select GetDate()", Cn);
                Dt = Convert.ToDateTime(Cmd.ExecuteScalar());
                return Dt;
            }
            catch (Exception ex)
            {
                return Dt;
            }
        }




        public void Vaahini_Tally_Export_Group(String Company, String Server_Address, int CompCode, String YearCode, String Check_Condition, int Group_Code)
        {
            String Xml_Str = String.Empty, Str = String.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Group = String.Empty, Parent = string.Empty, Alias = String.Empty, OBal = String.Empty, SortPosition = "290";
            try
            {
                Load_Data("select g1.groupName gname, g1.groupname Alias, g2.groupName Parent from groupmas g1 left join groupmas G2 on g1.groupunder = g2.groupcode and g1.company_Code = g2.company_Code and g1.year_Code = g2.year_Code where g1.company_Code = " + CompCode + " and g1.year_Code = '" + YearCode + "' and G1.GroupCode = " + Group_Code + " order by g1.groupcode", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Group = Dt.Rows[i]["gname"].ToString().Replace("&", "&amp;");
                    Alias = Dt.Rows[i]["Alias"].ToString().Replace("&", "&amp;");
                    Parent = Dt.Rows[i]["parent"].ToString().Replace("&", "&amp;");
                    if (Group == Parent)
                    {
                        Parent = String.Empty;
                    }
                    Xml_Str = "<ENVELOPE>"; Xml_Str += "<HEADER>"; Xml_Str += "<TALLYREQUEST>Import Data</TALLYREQUEST>";
                    Xml_Str += "</HEADER>"; Xml_Str += "<BODY>"; Xml_Str += "<IMPORTDATA>";
                    Xml_Str += "<REQUESTDESC>"; Xml_Str += "<REPORTNAME>All Masters</REPORTNAME>"; Xml_Str += "<STATICVARIABLES>";
                    Xml_Str += "<SVCURRENTCOMPANY>" + Company + "</SVCURRENTCOMPANY>"; Xml_Str += "</STATICVARIABLES>";
                    Xml_Str += "</REQUESTDESC>"; Xml_Str += "<REQUESTDATA>"; Xml_Str += "<TALLYMESSAGE>";
                    
                    Str = " <GROUP NAME=!1111! RESERVEDNAME=!1111!>";
                    Str = Str.Replace("1111", Group);
                    Xml_Str += Str;

                    if (GetData_InString("Socks_Companymas", "compcode", "1", "compname").ToUpper().Contains("DHANA"))
                    {
                        Xml_Str += "   <NAME.LIST>";
                        Xml_Str += "     <NAME>" + Group + "</NAME>";
                        Xml_Str += "   </NAME.LIST>";
                    }

                    if (Parent.Trim() != String.Empty)
                    {
                        Xml_Str += "  <PARENT>" + Parent + "</PARENT>";
                    }
                    else
                    {
                        Xml_Str += "  <PARENT/>";
                    }
                    Xml_Str += "  <ISBILLWISEON>No</ISBILLWISEON>";
                    Xml_Str += "  <ISADDABLE>No</ISADDABLE>";
                    Xml_Str += "  <ISSUBLEDGER>No</ISSUBLEDGER>";
                    Xml_Str += "  <ISREVENUE>No</ISREVENUE>";
                    Xml_Str += "  <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>";
                    Xml_Str += "  <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>";
                    Xml_Str += "  <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>";
                    Xml_Str += "  <ISCONDENSED>No</ISCONDENSED>";
                    Xml_Str += "  <SORTPOSITION>" + SortPosition + "</SORTPOSITION>";
                    if (GetData_InString("Socks_Companymas", "compcode", "1", "compname").ToUpper().Contains("DHANA"))
                    {
                    }
                    else
                    {
                        Xml_Str += "  <LANGUAGENAME.LIST>";
                        Xml_Str += "   <NAME.LIST>";
                        Xml_Str += "     <NAME>" + Group + "</NAME>";
                        Xml_Str += "   </NAME.LIST>";
                        Xml_Str += "   <LANGUAGEID> 1033</LANGUAGEID>";
                        Xml_Str += "  </LANGUAGENAME.LIST>";
                    }
                    Xml_Str += " </GROUP>";

                    Xml_Str += "</TALLYMESSAGE>";
                    Xml_Str += "</REQUESTDATA>";
                    Xml_Str += "</IMPORTDATA>";
                    Xml_Str += "</BODY>";
                    Xml_Str += "</ENVELOPE>";

                    StreamWriter s = new StreamWriter("c:\\VaahRep\\XmlStr.Xml");
                    s.WriteLine(Xml_Str.Replace("!", Chr(34).ToString()));
                    s.Close();
                    //Tally_Export.Send_To_Tally(ref Server_Address, ref Xml_Str, ref Check_Condition);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Make_indian_Rupee_Grid(ref DataGridView DGV, ref System.Data.DataTable Dt, params String[] Column_Names)
        {
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    foreach (String Str in Column_Names)
                    {
                        if (DGV[Str, i].Value != DBNull.Value)
                        {
                            if (Dt.Columns[Str].DataType != Type.GetType("System.String"))
                            {
                                Dt.Columns[Str].DataType = Type.GetType("System.String");
                            }
                            DGV[Str, i].Value = Convert.ToDouble(Indian_Rupee(Convert.ToDouble(DGV[Str, i].Value)));
                            DGV[Str, i].Style.Alignment = DataGridViewContentAlignment.MiddleRight;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Fill_Narration(ref DataGridView Dgv, String Narration_Col_Name, ref System.Windows.Forms.TextBox Txt)
        {
            try
            {
                if (Dgv.CurrentCell != null)
                {
                    Txt.Text = Dgv[Narration_Col_Name, Dgv.CurrentCell.RowIndex].Value.ToString();
                    Txt.CharacterCasing = CharacterCasing.Normal;
                    Txt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Is_Column_Available(ref DataGridView Dgv, String ColName)
        {
            try
            {
                for (int i = 0; i <= Dgv.Columns.Count - 1; i++)
                {
                    if (Dgv.Columns[i].Name.ToUpper() == ColName.ToUpper())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Fill_Narration(ref DataGridView Dgv, String Narration_Col_Name, ref V_Components.MyTextBox Txt)
        {
            try
            {
                if (Dgv.CurrentCell != null)
                {
                    Txt.Text = Dgv[Narration_Col_Name, Dgv.CurrentCell.RowIndex].Value.ToString();
                    Txt.CharacterCasing = CharacterCasing.Normal;
                    Txt.ScrollBars = System.Windows.Forms.ScrollBars.Both;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Selection_Tool_KeyPress(KeyPressEventArgs e)
        {
            Char Ch;
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Enter) || e.KeyChar == Convert.ToChar(Keys.Escape))
                {

                }
                else
                {
                    Ch = e.KeyChar;
                    e.Handled = true;
                    SendKeys.Send("{Down}");
                    SendKeys.Send(Ch.ToString().ToUpper());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Indian_Rupee(Double Amount)
        {
            String Str = String.Empty;
            try
            {
                if (Amount >= 100000000)
                {
                    Str = String.Format("{0:00@00@00@000.00}", Amount);
                }
                else if (Amount >= 10000000)
                {
                    Str = String.Format("{0:0@00@00@000.00}", Amount);
                }
                else if (Amount >= 1000000)
                {
                    Str = String.Format("{0:00@00@000.00}", Amount);
                }
                else if (Amount >= 100000)
                {
                    Str = String.Format("{0:0@00@000.00}", Amount);
                }
                else if (Amount >= 10000)
                {
                    Str = String.Format("{0:00@000.00}", Amount);
                }
                else if (Amount >= 1000)
                {
                    Str = String.Format("{0:0@000.00}", Amount);
                }
                else
                {
                    Str = Amount.ToString();
                }
                return Str.Replace("@", ",");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String DateinMonth_Table(DateTime Firstdate)
        {
            String TableName = GetSystemNameForTable() + "_M" + Firstdate.Month +"Y" + Firstdate.Year;
            int Count = DateTime.DaysInMonth(Firstdate.Year, Firstdate.Month);
            String Dat = String.Empty;
            try
            {
                if (Check_Table(TableName))
                {
                    Execute("Drop table " + TableName);
                }
                Execute("Create table " + TableName + " (EDate Datetime)");
                for (int i = 1; i <= Count; i++)
                {
                    Dat = String.Format("{0:00}", i) + "-" + String.Format("{0:MMM}", Firstdate) + "-" + String.Format("{0:yyyy}", Firstdate);
                    Execute("Insert into " + TableName + "(Edate) values ('" + Dat + "')");
                }
                return TableName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable Return_Daily_Balance_View (int Ledger_Code, DateTime FirstDate, int CompCode, string Year_Code)
        {
            String TableName = GetSystemNameForTable() + "_" + Ledger_Code;
            String DateinMonth_TableName = DateinMonth_Table(FirstDate);
            System.Data.DataTable Dt = new System.Data.DataTable();
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            System.Data.DataTable Result_DT = new System.Data.DataTable();
            Double CAmount = 0;
            try
            {
                Execute_Qry("select v2.user_date vdate, sum(v1.debit) debit, sum(v1.Credit) Credit from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_code = v2.company_Code and v1.year_Code = v2.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.ledger_Code = " + Ledger_Code + " and month(v2.user_date) = " + FirstDate.Month + " and Year(v2.user_date) = " + FirstDate.Year + " group by v2.user_date ", "Abs_Date");
                Execute_Tbl("select v2.edate, cast(0 as numeric(18, 2)) as OpBal, 'AB' TY1, sum(debit) Debit, (-1) * SUm(Credit) Credit, cast(0 as numeric(18,2)) as ClBal, 'AB' TY2 from " + DateinMonth_TableName + " v2 left join Abs_Date v1 on v2.edate = v1.vdate group by v2.edate order by v2.edate", "Abs_Day");
                CAmount = Get_Balance(FirstDate, Ledger_Code, CompCode, Year_Code);
                Execute("Update Abs_day set OPbal = 0 where opbal is null");
                Execute("Update Abs_day set debit = 0 where Debit is null");
                Execute("Update Abs_day set Credit = 0 where Credit is null");
                Execute("Update Abs_day set ClBal = 0 where Clbal is null");
                Load_Data("select Edate from abs_day order by edate", ref Dt);
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        Execute("Update Abs_day set OpBal = " + CAmount + ", ClBal = (" + CAmount + ") + (isnull(Debit, 0)) + (isnull(Credit, 0)) where edate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["EDate"])) + "'");
                    }
                    else
                    {
                        Load_Data("Select isnull(ClBal, 0) ClBal from Abs_day where eDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["EDate"]).AddDays(-1)) + "'", ref Dt1);
                        CAmount = Convert.ToDouble(Dt1.Rows[0]["ClBal"]);
                        Execute("Update Abs_day set OpBal = " + CAmount + ", ClBal = (" + CAmount + ") + (isnull(Debit, 0)) + (isnull(Credit, 0)) where edate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["EDate"])) + "'");
                    }
                }
                Execute("UPdate Abs_Day Set Debit = null where debit = 0");
                Execute("UPdate Abs_Day Set Credit = null where Credit = 0");
                Execute("UPdate Abs_Day Set Credit = (-1) * Credit where Credit < 0");
                Execute("UPdate Abs_Day Set Ty1 = 'Dr' where OpBal >= 0");
                Execute("UPdate Abs_Day Set Ty1 = 'Cr' where OpBal < 0");
                Execute("UPdate Abs_Day Set Ty2 = 'Dr' where ClBal >= 0");
                Execute("UPdate Abs_Day Set Ty2 = 'Cr' where ClBal < 0");
                Execute("UPdate Abs_Day Set OpBal = (-1) * OpBal where OpBal < 0");
                Execute("UPdate Abs_Day Set ClBal = (-1) * ClBal where ClBal < 0");
                Load_Data("Select * from abs_day order by edate", ref Result_DT);
                return Result_DT;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable Fill_With_Datarows(ref System.Data.DataTable Original, ref DataRow[] Dr, out System.Data.DataTable Dt)
        {
            try
            {
                Dt = Original.Clone();
                foreach (DataRow Dr1 in Dr)
                {
                    Dt.ImportRow(Dr1);
                }
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double CurBal_Period_For_LedgerCode(int Ledger_Code, DateTime Sdate, DateTime FromDt, DateTime ToDt, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "' and '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v1.Approval = 'True' group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "' and '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v1.Approval = 'True' group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select ledger_Code, cast('" + Sdate + "' as datetime) as Vdate, 0 as Vmode, ledger_Odebit debit, ledger_OCredit as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from Ledger_master where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' ";
                Execute_Qry(Str, "CBal_Period");
                Execute_Qry("Select * from Cbal_Period", "CBal1_Period");
                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1_Period ", "Cbal_Mon_Period");
                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon_Period group by ledger_Code, Month_Code", "Cbal_Mon1_Period");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) Credit from Cbal_Mon1_Period v3 ", "Cbal_Month_Period");

                Str = "select ledger_Code, sum(Debit) debit, SUm(Credit) Credit from Cbal1_Period group by ledger_Code";
                Execute_Qry(Str, "CBal2_Period");
                Str = "select ledger_Code, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2_Period";
                Execute_Qry(Str, "CurBal1_Period");
                Execute_Qry("select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1_Period", "CurBal_Period");

                Load_Data("select isnull((case when mode = 'Dr' then CurBalance else curBalance * (-1) end), 0) Balance from CurBal1_Period where ledger_Code = " + Ledger_Code, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Convert.ToDouble(Dt.Rows[0]["Balance"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public void CurBal_Period(DateTime Sdate, DateTime FromDt, DateTime ToDt, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "' and '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v1.Approval = 'True' group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "' and '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v1.Approval = 'True' group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select ledger_Code, cast('" + Sdate + "' as datetime) as Vdate, 0 as Vmode, ledger_Odebit debit, ledger_OCredit as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from Ledger_master where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' ";
                Execute_Qry(Str, "CBal_Period");
                Execute_Qry("Select * from Cbal_Period", "CBal1_Period");
                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1_Period ", "Cbal_Mon_Period");
                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon_Period group by ledger_Code, Month_Code", "Cbal_Mon1_Period");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) Credit from Cbal_Mon1_Period v3 ", "Cbal_Month_Period");

                Str = "select ledger_Code, sum(Debit) debit, SUm(Credit) Credit from Cbal1_Period group by ledger_Code";
                Execute_Qry(Str, "CBal2_Period");
                Str = "select ledger_Code, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2_Period";
                Execute_Qry(Str, "CurBal1_Period");
                Execute_Qry("select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1_Period", "CurBal_Period");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CurBal_Period_WO_OpBal(DateTime Sdate, DateTime FromDt, DateTime ToDt, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "' and '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v1.Approval = 'True' group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "' and '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' and v1.Approval = 'True' group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                
                // Opening Balance
                //Str += " select ledger_Code, cast('" + Sdate + "' as datetime) as Vdate, 0 as Vmode, ledger_Odebit debit, ledger_OCredit as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from Ledger_master where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' ";

                /// Opening Stock
                Str += " Select ledger_Code, edate, 0, debit, 0, company_Code, year_Code from Closing_Stock where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' and Edate = (Select Max(Edate) from Closing_Stock where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' and Edate < '" + String.Format("{0:dd-MMM-yyyy}", FromDt) + "') union ";
                
                /// Closing Stock

                Str += " Select -1, edate, 0, 0, debit, company_Code, year_Code from Closing_Stock where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' and Edate = (Select Max(Edate) from Closing_Stock where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' and Edate <= '" + String.Format("{0:dd-MMM-yyyy}", ToDt) + "')";

                Execute_Qry(Str, "CBal_Period");
                Execute_Qry("Select * from Cbal_Period", "CBal1_Period");
                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1_Period ", "Cbal_Mon_Period");
                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon_Period group by ledger_Code, Month_Code", "Cbal_Mon1_Period");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) Credit from Cbal_Mon1_Period v3 ", "Cbal_Month_Period");

                Str = "select ledger_Code, sum(Debit) debit, SUm(Credit) Credit from Cbal1_Period group by ledger_Code";
                Execute_Qry(Str, "CBal2_Period");
                Str = "select ledger_Code, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2_Period";
                Execute_Qry(Str, "CurBal1_Period");
                Execute_Qry("select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1_Period", "CurBal_Period");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Get_First_Column(ref DataGridView Grid)
        {
            try
            {
                foreach (DataGridViewColumn Dc in Grid.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        return Dc.Name;
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Double Convert_ToDouble(String Amount)
        {
            try
            {
                return Convert.ToDouble(Amount.Replace(",", ""));
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public DataRow Ledger_Selection(Form OwnerForm, String Title, String TblName, String Ledger_FiledName, String Ledger_Code, String Condition)
        {
            try
            {
                Frm_Ledger_Selection Frm = new Frm_Ledger_Selection();
                MDIMain Myparent = (MDIMain)OwnerForm.MdiParent;
                Frm.CompName = Myparent.CompName;
                Frm.TblName = TblName;
                Frm.CodeName = Ledger_Code;
                Frm.Text = Title;
                Frm.Condition = Condition;
                Frm.FldName = Ledger_FiledName;
                Frm.Load_Data();
                Frm.ShowDialog(OwnerForm);
                return Frm.Dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CurBal_table_Creation(int Ledger_Code, DateTime Sdate, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "'  and v1.Approval = 'True' AND V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "'  and v1.Approval = 'True' AND V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select ledger_Code, cast('" + String.Format("{0:dd-MMM-yyyy}", Sdate) + "' as datetime) as Vdate, 0 as Vmode, ledger_Odebit debit, ledger_OCredit as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from Ledger_master where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' ";
                Execute_Qry(Str, "CBal");
                Execute_Qry("Select * from Cbal", "CBal1");
                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1 ", "Cbal_Mon");
                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon group by ledger_Code, Month_Code", "Cbal_Mon1");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) Credit from Cbal_Mon1 v3 ", "Cbal_Month");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void CurBal_table_Creation_In_Period(int Ledger_Code, DateTime Sdate, DateTime From, DateTime TO, Int32 COmpCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                Str = "select v1.ledger_Code, v2.user_date vdate, v2.vmode, Sum(Debit) debit, 0 as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' AND v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", TO) + "'  and v1.Approval = 'True' and V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select v1.ledger_Code, v2.user_date vdate, v2.vmode, 0 debit, Sum(credit) as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate where v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' and v2.company_Code = " + COmpCode + " and v2.year_Code = '" + Year_Code + "' AND v2.user_date between '" + String.Format("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format("{0:dd-MMM-yyyy}", TO) + "'  and v1.Approval = 'True' and V1.ledger_code <> 0 group by v1.ledger_Code, v2.user_date, v2.vmode union ";
                Str += " select ledger_Code, cast('" + String.Format("{0:dd-MMM-yyyy}", Sdate) + "' as datetime) as Vdate, 0 as Vmode, ledger_Odebit debit, ledger_OCredit as credit, " + COmpCode + " as Company_Code, '" + Year_Code + "' as Year_Code from Ledger_master where company_Code = " + COmpCode + " and year_Code = '" + Year_Code + "' ";
                Execute_Qry(Str, "CBal");

                //Execute_Tbl("Select * from Cbal", "CBal1");
                Execute_Qry("Select * from Cbal", "CBal1");

                Execute_Qry("select ledger_Code, datepart(M, vdate) Month_Code, Debit, Credit from CBal1 ", "Cbal_Mon");

                Execute_Qry("select ledger_Code, Month_Code, SUm(Debit) debit, Sum(credit) credit from Cbal_Mon group by ledger_Code, Month_Code", "Cbal_Mon1");
                //Execute_Tbl("select ledger_Code, Month_Order, Month_name, Debit, Credit from Month_Master m1 left join Cbal_Mon1 v3 on m1.month_Code = v3.Month_Code", "Cbal_Month");
                Execute_Qry("select ledger_Code, Month_Code, (case when Debit = 0 then null else debit end) debit, (case when Credit =0 then null else credit end) Credit from Cbal_Mon1 v3 ", "Cbal_Month");
                //Execute("update cbal_month set debit = null where debit = 0");
                //Execute("update cbal_month set Credit = null where Credit = 0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Year_CarryOver()
        {
            try
            {
                String Prv_year = "2009-2010";
                String New_year = "2010-2011";

                // Company Master
                Execute_Tbl("Select * from Socks_Companymas where year_Code = '" + Prv_year + "'", "C1");
                Execute("Update c1 set SDt = '01-Apr-2010', EDt = '31-Mar-2011'");
                Execute("Insert into Socks_Companymas select * from c1");

                // Ledger Master
                Execute_Tbl("Select * from ledger_master where year_Code = '" + Prv_year + "'", "C1");
                Execute("Update C1 Set Year_Code = '" + New_year + "'");
                Execute("Insert into Ledger_Master select * from c1");

                // Group Master
                Execute_Tbl("Select * from Groupmas where year_Code = '" + Prv_year + "'", "C1");
                Execute("Update C1 Set Year_Code = '" + New_year + "'");
                Execute("Insert into Groupmas select * from c1");

                // Voucher Group Master
                Execute_Tbl("Select * from Voucher_Group where Year_Code = '" + Prv_year + "'", "C1");
                Execute("Update C1 Set Year_Code = '" + New_year + "'");
                Execute("Insert into Voucher_Group select * from c1");

                // Division Master
                Execute_Tbl("Select * from Division_master where Year_Code = '" + Prv_year + "'", "C1");
                Execute("Update C1 Set Year_Code = '" + New_year + "'");
                Execute("Insert into Division_master select * from c1");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_keyDown_VDatagrid(ref DataGridView DGV, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (IsGrid_VGrid(ref DGV))
                {
                    if (DGV.CurrentCell.RowIndex >= DGV.Rows.Count - 3)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ChequeBook_Update_Issued(String BookENo, Boolean Reverse, int CompCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                if (Reverse == false)
                {
                    Str = "Update ChequeBook_master set leafs_used = leafs_used + 1, leafs_pending = leafs_pending - 1 where Eno = " + BookENo + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                }
                else
                {
                    Str = "Update ChequeBook_master set leafs_used = leafs_used - 1, leafs_pending = leafs_pending + 1 where Eno = " + BookENo + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ChequeBook_Update_Cancelled(String BookENo, Boolean Reverse, int CompCode, String Year_Code)
        {
            String Str = String.Empty;
            try
            {
                if (Reverse == false)
                {
                    Str = "Update ChequeBook_master set leafs_Cancelled = leafs_Cancelled + 1, leafs_pending = leafs_pending - 1 where Eno = " + BookENo + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                }
                else
                {
                    Str = "Update ChequeBook_master set leafs_Cancelled = leafs_Cancelled - 1, leafs_pending = leafs_pending + 1 where Eno = " + BookENo + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String[] ChQBk_Queries_Rev(String Code, DateTime Vdate, int CompCode, String Year_Code)
        {
            String Str = String.Empty;
            System.Data.DataTable ChqDtRev = new System.Data.DataTable();
            String[] ChequeBook_QueriesRev;
            try
            {
                Load_Data("Select BookNo BookEno, chq_No from Cheque_Details where Vcode = " + Code + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", ref ChqDtRev);
                ChequeBook_QueriesRev = new String[ChqDtRev.Rows.Count];
                for (int i = 0; i <= ChqDtRev.Rows.Count - 1; i++)
                {
                    Str = ChequeBook_Update_Issued(ChqDtRev.Rows[i]["BookEno"].ToString(), true, CompCode, Year_Code);
                    ChequeBook_QueriesRev[i] = Str;
                }
                return ChequeBook_QueriesRev;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Voucher_Delete(long VCode, DateTime Vdate, int CompCode, string Year_Code, Boolean Term, int User_Code, int Sys_Code)
        {
            String Sql_Del, Sql1_Del, Sql2_Del, Sql_Chq, Sql_Ass;
            String Sql_Del_Master, Sql_Del_Details, Sql_Del_Bills, Sql_Del_Cheque, Sql_Del_Reference;
            String[] CheqRev;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                if (Term)
                {
                    MessageBox.Show("Entry doesn't Have Delete Option ....!", "Vaahini");
                    return false;
                }
                Load_Data("Select vmode, Vtype from voucher_master where vcode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (Get_RecordCount("Socks_Companymas", "Compname like '%DHANALAKSHMI%'") > 0)
                    {
                        if (Convert.ToDouble(Dt.Rows[0]["vmode"]) == 5 || Convert.ToDouble(Dt.Rows[0]["vmode"]) == 7)
                        {
                            if (Dt.Rows[0]["Vtype"].ToString().ToUpper() != "OTHERS")
                            {
                                MessageBox.Show("Can't Delete this Entry, Linked from Other Modules", "Vaahini");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToDouble(Dt.Rows[0]["vmode"]) == 5 || Convert.ToDouble(Dt.Rows[0]["vmode"]) == 6 || Convert.ToDouble(Dt.Rows[0]["vmode"]) == 7)
                        {
                            if (Dt.Rows[0]["Vtype"].ToString().ToUpper() != "OTHERS")
                            {
                                MessageBox.Show("Can't Delete this Entry, Linked from Other Modules", "Vaahini");
                                return false;
                            }
                        }
                    }
                }


                if (Get_RecordCount("Socks_Companymas", "((Compname like '%GAINUP%') or (Compname like '%ALAMELU%'))") > 0)
                {
                    MessageBox.Show("You Does'nt Have Rights to Delete !", "Vaahini");
                    return false;
                }

                if (Update_BreakUpR(VCode, Vdate, CompCode, Year_Code) == false)
                {
                    Sql_Del = "Delete from Voucher_Master where Vcode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql1_Del = "Delete from Voucher_Details where Vcode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql2_Del = "delete from Voucher_breakup_Bills where VCode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql_Chq = "Delete from cheque_Details where vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql_Ass = "Delete from voucher_Update_purchase_Assesible where vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";

                    Sql_Del_Master = "Insert into Voucher_Master_Deleted select * from Voucher_Master where Vcode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql_Del_Details = "Insert into Voucher_Details_Deleted select * from Voucher_Details where Vcode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql_Del_Bills = "Insert into Voucher_Breakup_bills_Deleted select * from Voucher_breakup_Bills where VCode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql_Del_Cheque = "Insert into Cheque_Details_Deleted Select * from cheque_Details where vcode = " + VCode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'";
                    Sql_Del_Reference = "Insert into voucher_Deleted_Reference Values (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + CompCode + ", '" + Year_Code + "', " + User_Code + ", " + Sys_Code + ", '" + string.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "')";

                    CheqRev = ChQBk_Queries_Rev(VCode.ToString(), Vdate, CompCode, Year_Code);
                    Run(Update_Ledger_BreakupR, CheqRev, Sql_Del_Master, Sql_Del_Details, Sql_Del_Bills, Sql_Del_Cheque, Sql_Del_Reference, Sql_Del, Sql1_Del, Sql2_Del, Sql_Chq, Sql_Ass);
                    MessageBox.Show("Deleted ...!");
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Update_BreakUpR(long VCode, DateTime Vdate, int CompCode, string Year_Code)
        {
            System.Data.DataTable UPR = new System.Data.DataTable();
            System.Data.DataTable TUpr = new System.Data.DataTable();
            System.Data.DataTable Temp = new System.Data.DataTable();
            String Str = String.Empty;
            try
            {
                int j = 0;
                Update_Ledger_BreakupR = new String[100];
                Str = "select * from voucher_breakup_bills where Vcode = " + VCode + " and Vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and COmpany_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'";
                Load_Data(Str, ref UPR);
                for (int i = 0; i <= UPR.Rows.Count - 1; i++)
                {
                    if (UPR.Rows[i]["Mode"].ToString() == "A")
                    {
                        if (UPR.Rows[i]["Bterm"].ToString() == "CR")
                        {
                            Str = "Update Ledger_Breakup set Amount_Cl = Amount_Cl - " + UPR.Rows[i]["Debit"].ToString().Trim() + " where RefDoc = '" + UPR.Rows[i]["RefDoc"].ToString() + "' and RefDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(UPR.Rows[i]["RefDate"])) + "' and Ledger_Code = " + UPR.Rows[i]["Ledger_Code"].ToString() + " and Ref = '" + UPR.Rows[i]["Ref"].ToString() + "' and Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'";
                        }
                        else
                        {
                            Str = "Update Ledger_Breakup set Amount_Cl = Amount_Cl - " + UPR.Rows[i]["Credit"].ToString().Trim() + " where RefDoc = '" + UPR.Rows[i]["RefDoc"].ToString() + "' and RefDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(UPR.Rows[i]["RefDate"])) + "' and Ledger_Code = " + UPR.Rows[i]["Ledger_Code"].ToString() + " and Ref = '" + UPR.Rows[i]["Ref"].ToString() + "' and Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'";
                        }
                    }
                    else
                    {
                        Load_Data("Select * from ledger_breakup where Ledger_Code = " + UPR.Rows[i]["Ledger_Code"].ToString() + " and RefDoc = '" + UPR.Rows[i]["RefDoc"].ToString() + "' and RefDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(UPR.Rows[i]["RefDate"].ToString())) + "' and Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'", ref TUpr);
                        if (TUpr.Rows.Count > 0)
                        {
                            if (Convert.ToDouble(TUpr.Rows[0]["Amount_CL"]) > 0)
                            {
                                //Load_Data("select v2.Vno, v2.user_date vdate, v3.VchTypeName from Voucher_Breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join Voucher_type v3 on v2.Vmode = v3.vchtypeno where v1.mode = 'A' and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.ledger_Code = " + TUpr.Rows[0]["Ledger_Code"] + " and v1.refDoc = '" + TUpr.Rows[0]["RefDoc"].ToString() + "' and v1.refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TUpr.Rows[0]["RefDate"])) + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'", ref Temp);
                                Load_Data("select v2.Vno, v2.user_date vdate, v3.VchTypeName from Voucher_Breakup_bills v1 left join voucher_Master v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join Voucher_type v3 on v2.Vmode = v3.vchtypeno where v1.mode = 'A' and v1.ledger_Code = " + TUpr.Rows[0]["Ledger_Code"] + " and v1.refDoc = '" + TUpr.Rows[0]["RefDoc"].ToString() + "' and v1.refDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(TUpr.Rows[0]["RefDate"])) + "' and v1.vcode <> '" + VCode + "' and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "'", ref Temp);
                                if (Temp.Rows.Count > 0)
                                {
                                    MessageBox.Show("Can't Delete .... Reference : " + TUpr.Rows[0]["RefDoc"].ToString() + " On " + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(TUpr.Rows[0]["RefDate"])) + " Used in " + Temp.Rows[0]["VchTypeName"].ToString() + " No - " + Temp.Rows[0]["Vno"].ToString() + " Date - " + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Temp.Rows[0]["Vdate"])));
                                    return true;
                                }
                            }
                        }
                        Str = "Delete from Ledger_BreakUp where ledger_Code = " + UPR.Rows[i]["Ledger_Code"].ToString() + " and RefDoc = '" + UPR.Rows[i]["RefDoc"].ToString() + "' and RefDate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(UPR.Rows[i]["RefDate"].ToString())) + "' and Ref = '" + UPR.Rows[i]["Ref"].ToString() + "' and Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'";
                    }
                    Update_Ledger_BreakupR[j] = Str;
                    j += 1;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Current_Balance_UptoDate(int Ledger_Code, DateTime Upto, DateTime Sdate, Int32 COmpCode, String Year_Code, Boolean OpBal)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Str = String.Empty;
            try
            {
                if (OpBal)
                {
                    CurBal_table_Creation(Ledger_Code, Sdate, COmpCode, Year_Code);
                }
                else
                {
                    CurBal_table_Creation_WO_OPBal(Ledger_Code, Sdate, COmpCode, Year_Code);
                }

                Str = "select ledger_Code, sum(Debit) debit, SUm(Credit) Credit from Cbal1 where vdate <= '" + String.Format("{0:dd-MMM-yyyy}", Upto) + "' and ledger_Code = " + Ledger_Code + " group by ledger_Code";
                Execute_Qry(Str, "CBal2");

                Str = "select ledger_Code, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2";
                Execute_Qry(Str, "CurBal1");

                Execute_Qry("select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1", "CurBal");

                Str = "Select Balance from curbal where ledger_Code = " + Ledger_Code;
                if (Ledger_Code > 0)
                {
                    Load_Data(Str, ref Dt);

                    Execute_Qry("select v1.vcode, v1.Ledger_Code, v1.vdate, v2.user_date, v1.company_Code, v1.year_Code from voucher_Details v1, voucher_master v2 where v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v2.user_Date < '" + String.Format("{0:dd-MMM-yyyy}", Upto) + "' and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' ", "Noof_Voucher");

                    if (Dt.Rows.Count > 0)
                    {
                        return Dt.Rows[0]["Balance"].ToString();
                    }
                    else
                    {
                        return "0.00 Dr";
                    }
                }
                else
                {
                    return "0.00 Dr";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String Current_Balance(int Ledger_Code, DateTime Sdate, Int32 COmpCode, String Year_Code, Boolean OpBal)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Str = String.Empty;
            try
            {
                if (OpBal)
                {
                    CurBal_table_Creation(Ledger_Code, Sdate, COmpCode, Year_Code);
                }
                else
                {
                    CurBal_table_Creation_WO_OPBal(Ledger_Code, Sdate, COmpCode, Year_Code);
                }

                Str = "Select ledger_Code, sum(Debit) debit, SUm(Credit) Credit from Cbal1 group by ledger_Code";
                Execute_Qry(Str, "CBal2");

                Str = "Select ledger_Code, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2";
                Execute_Qry(Str, "CurBal1");

                Execute_Qry("Select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1", "CurBal");
                Execute_Qry("Select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1", "CurBal_Vch");

                Str = "Select Balance from curbal where ledger_Code = " + Ledger_Code;
                if (Ledger_Code > 0)
                {
                    Load_Data(Str, ref Dt);

                    Execute_Qry("select v1.vcode, v1.Ledger_Code, v1.vdate, v2.user_date, v1.company_Code, v1.year_Code from voucher_Details v1, voucher_master v2 where v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' ", "Noof_Voucher");

                    if (Dt.Rows.Count > 0)
                    {
                        return Dt.Rows[0]["Balance"].ToString();
                    }
                    else
                    {
                        return "0.00 Dr";
                    }
                }
                else
                {
                    return "0.00 Dr";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Current_Balance_In_Period(int Ledger_Code, DateTime Sdate, DateTime From, DateTime TO, Int32 COmpCode, String Year_Code, Boolean OpBal)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Str = String.Empty;
            try
            {
                if (OpBal)
                {
                    CurBal_table_Creation_In_Period(Ledger_Code, Sdate, From, TO, COmpCode, Year_Code);
                }
                else
                {
                    CurBal_table_Creation_WO_OPBal_IN_Period(Ledger_Code, Sdate, From, TO, COmpCode, Year_Code);
                }

                Str = "select ledger_Code, sum(Debit) debit, SUm(Credit) Credit from Cbal1 where vdate between '" + String.Format ("{0:dd-MMM-yyyy}", From) + "' and '" + String.Format ("{0:dd-MMM-yyyy}", TO) + "' group by ledger_Code";
                Execute_Qry(Str, "CBal2");

                Str = "select ledger_Code, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2";
                Execute_Qry(Str, "CurBal1");

                Execute_Qry("select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1", "CurBal");

                Execute_Qry("select Ledger_Code, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1", "CurBal_Vch");

                Str = "Select Balance from curbal where ledger_Code = " + Ledger_Code;
                if (Ledger_Code > 0)
                {
                    Load_Data(Str, ref Dt);

                    Execute_Qry("select v1.vcode, v1.Ledger_Code, v1.vdate, v2.user_date, v1.company_Code, v1.year_Code from voucher_Details v1, voucher_master v2 where v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code and v1.company_Code = " + COmpCode + " and v1.year_Code = '" + Year_Code + "' ", "Noof_Voucher");

                    if (Dt.Rows.Count > 0)
                    {
                        return Dt.Rows[0]["Balance"].ToString();
                    }
                    else
                    {
                        return "0.00 Dr";
                    }
                }
                else
                {
                    return "0.00 Dr";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Current_Balance_Month(int Ledger_Code, DateTime Sdate, Int32 COmpCode, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Str = String.Empty;
            try
            {
                CurBal_table_Creation(Ledger_Code, Sdate, COmpCode, Year_Code);

                Str = "select Month(Vdate) MOnth_, sum(Debit) debit, SUm(Credit) Credit from Cbal1 where ledger_Code = " + Ledger_Code + " group by Vdate";
                Execute_Qry(Str, "CBal2_M");

                Str = "select Month_, (case when (Debit - Credit) > 0 then debit - Credit else Credit - debit end) curBalance, (case when debit > credit then 'Dr' else 'Cr' end) as Mode from Cbal2_M";
                Execute_Tbl(Str, "CurBal1_M");

                Execute_Tbl("select Month_, cast(cast(curbalance as Numeric(15,2)) as varchar(30)) + ' ' + Mode as Balance, CurBalance Bal_Amount, Mode from CurBal1_M", "CurBal_M");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //public String Current_Balance(int Ledger_Code, Int32 COmpCode, String Year_Code)
        //{
        //    System.Data.DataTable Dt = new System.Data.DataTable();
        //    System.Data.DataTable DtOp = new System.Data.DataTable();
        //    Double OPD = 0, OpC = 0, Cr = 0, Dr = 0, Res = 0, Deb = 0, Cre = 0;
        //    String ResChar = String.Empty;
        //    try
        //    {
        //        Load_Data("select Sum(Debit) Debit, Sum(Credit) Credit from Voucher_Details  where ledger_Code = " + Ledger_Code + " and Company_Code = " + COmpCode + " and Year_Code = '" + Year_Code + "'", ref Dt);
        //        if (Dt.Rows.Count > 0)
        //        {
        //            if (Dt.Rows[0]["Debit"] != DBNull.Value)
        //            {
        //                Dr = Convert.ToDouble(Dt.Rows[0]["Debit"]);
        //            }
        //            else
        //            {
        //                Dr = 0;
        //            }
        //            if (Dt.Rows[0]["Credit"] != DBNull.Value)
        //            {
        //                Cr = Convert.ToDouble(Dt.Rows[0]["Credit"]);
        //            }
        //            else
        //            {
        //                Cr = 0;
        //            }
        //        }
        //        else
        //        {
        //            Dr = 0;
        //            Cr = 0;
        //        }
        //        Load_Data("select Ledger_Odebit Debit, Ledger_OCredit Credit from Ledger_master where ledger_Code = " + Ledger_Code + " and Company_Code = " + COmpCode + " and Year_Code = '" + Year_Code + "'", ref DtOp);
        //        if (DtOp.Rows.Count > 0)
        //        {
        //            if (DtOp.Rows[0]["Debit"] != DBNull.Value)
        //            {
        //                OPD = Convert.ToDouble(DtOp.Rows[0]["Debit"]);
        //            }
        //            else
        //            {
        //                OPD = 0;
        //            }
        //            if (DtOp.Rows[0]["Credit"] != DBNull.Value)
        //            {
        //                OpC = Convert.ToDouble(DtOp.Rows[0]["Credit"]);
        //            }
        //            else
        //            {
        //                OpC = 0;
        //            }
        //        }
        //        else
        //        {
        //            OPD = 0;
        //            OpC = 0;
        //        }
        //        Deb = (Dr + OPD);
        //        Cre = (OpC + Cr);
        //        if (Deb > Cre)
        //        {
        //            Res = Deb - Cre;
        //            ResChar = "Dr";
        //        }
        //        else if (Deb < Cre)
        //        {
        //            Res = Cre - Deb;
        //            ResChar = "Cr";
        //        }
        //        else if (Deb < Cre)
        //        {
        //            Res = 0;
        //            ResChar = "Dr";
        //        }
        //        return String.Format("{0:0.00}", Res) + " " + ResChar;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Read_XML1(String FilePath)
        {
            try
            {
                XmlTextReader X = new XmlTextReader(FilePath);
                while (X.Read())
                {
                    //if (X.Name.Trim() != String.Empty)
                    //{
                    //    MessageBox.Show(X.Name);
                    //}
                    if (X.Value.Trim() != String.Empty)
                    {
                        MessageBox.Show(X.Value);
                    }
                }


                // This One Method
                //XmlDocument Doc = new XmlDocument();
                //Doc.Load("D:\\tally\\daybook1.xml");
                //XmlElement El = Doc.DocumentElement;
                //XmlNodeList Root = El.SelectNodes("/VOUCHER");

                //foreach (XmlNode Node in Root)
                //{
                //    MessageBox.Show(Node["VOUCHERNUMBER"].InnerText);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Header_XML(String Company)
        {
            try
            {
                Tally_Edit.WriteLine("<ENVELOPE>");
                Tally_Edit.WriteLine("<HEADER>");
                Tally_Edit.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>");
                Tally_Edit.WriteLine("</HEADER>");
                Tally_Edit.WriteLine("<BODY>");
                Tally_Edit.WriteLine("<IMPORTDATA>");
                Tally_Edit.WriteLine("<REQUESTDESC>");
                Tally_Edit.WriteLine("<REPORTNAME>All Masters</REPORTNAME>");
                Tally_Edit.WriteLine("<STATICVARIABLES>");
                Tally_Edit.WriteLine("<SVCURRENTCOMPANY>" + Company + "</SVCURRENTCOMPANY>");
                Tally_Edit.WriteLine("</STATICVARIABLES>");
                Tally_Edit.WriteLine("</REQUESTDESC>");
                Tally_Edit.WriteLine("<REQUESTDATA>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        String Header_XML_Voucher(String Company)
        {
            String Xml_Str = String.Empty;
            try
            {
                Xml_Str = "<ENVELOPE>";
                Xml_Str += "<HEADER>";
                Xml_Str += "<TALLYREQUEST>Import Data</TALLYREQUEST>";
                Xml_Str += "</HEADER>";
                Xml_Str += "<BODY>";
                Xml_Str += "<IMPORTDATA>";
                Xml_Str += "<REQUESTDESC>";
                Xml_Str += "<REPORTNAME>All Masters</REPORTNAME>";
                Xml_Str += "<STATICVARIABLES>";
                Xml_Str += "<SVCURRENTCOMPANY>" + Company + "</SVCURRENTCOMPANY>";
                Xml_Str += "</STATICVARIABLES>";
                Xml_Str += "</REQUESTDESC>";
                Xml_Str += "<REQUESTDATA>";
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        void Header_XML_ledger(String Company)
        {
            try
            {
                Tally_Edit.WriteLine("<ENVELOPE>");
                Tally_Edit.WriteLine("<HEADER>");
                Tally_Edit.WriteLine("<TALLYREQUEST>Import Data</TALLYREQUEST>");
                Tally_Edit.WriteLine("</HEADER>");
                Tally_Edit.WriteLine("<BODY>");
                Tally_Edit.WriteLine("<IMPORTDATA>");
                Tally_Edit.WriteLine("<REQUESTDESC>");
                Tally_Edit.WriteLine("<REPORTNAME>All Masters</REPORTNAME>");
                Tally_Edit.WriteLine("<STATICVARIABLES>");
                Tally_Edit.WriteLine("<SVCURRENTCOMPANY>" + Company + "</SVCURRENTCOMPANY>");
                Tally_Edit.WriteLine("</STATICVARIABLES>");
                Tally_Edit.WriteLine("</REQUESTDESC>");
                Tally_Edit.WriteLine("<REQUESTDATA>");
                Tally_Edit.WriteLine("<tallyMessage>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_Xml_Group()
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Footer_XML()
        {
            try
            {
                //Tally_Edit.WriteLine("</TALLYMESSAGE>");
                Tally_Edit.WriteLine("</REQUESTDATA>");
                Tally_Edit.WriteLine("</IMPORTDATA>");
                Tally_Edit.WriteLine("</BODY>");
                Tally_Edit.WriteLine("</ENVELOPE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        String Footer_XML_Voucher()
        {
            String Xml_Str = String.Empty;
            try
            {
                Xml_Str = "</REQUESTDATA>";
                Xml_Str += "</IMPORTDATA>";
                Xml_Str += "</BODY>";
                Xml_Str += "</ENVELOPE>";
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Footer_XML_Ledger()
        {
            try
            {
                Tally_Edit.WriteLine("</TALLYMESSAGE>");
                Tally_Edit.WriteLine("</REQUESTDATA>");
                Tally_Edit.WriteLine("</IMPORTDATA>");
                Tally_Edit.WriteLine("</BODY>");
                Tally_Edit.WriteLine("</ENVELOPE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_Ledger_Master_XML(String Ledger, String Alias, String Parent, String TIN, String OBal)
        {
            String Str = String.Empty;
            try
            {
                Str = " <LEDGER NAME=!1111! RESERVEDNAME=!!>";
                Str = Str.Replace("1111", Ledger);
                Tally_Edit.WriteLine(Str.Replace('!','"'));
                Tally_Edit.WriteLine("  <ADDITIONALNAME.LIST>");
                Tally_Edit.WriteLine("    <ADDITIONALNAME>" + Ledger +"</ADDITIONALNAME>");
                Tally_Edit.WriteLine("  </ADDITIONALNAME.LIST>");
                Tally_Edit.WriteLine("  <CURRENCYNAME>Rs.</CURRENCYNAME>");
                Tally_Edit.WriteLine("  <STATENAME>Tamil Nadu</STATENAME>");
                Tally_Edit.WriteLine("  <SALESTAXNUMBER>" + TIN + "</SALESTAXNUMBER>");
                Tally_Edit.WriteLine("  <PARENT>" + Parent + "</PARENT>");
                Tally_Edit.WriteLine("  <TAXCLASSIFICATIONNAME/>");
                Tally_Edit.WriteLine("  <GSTTYPE/>");
                Tally_Edit.WriteLine("  <SERVICECATEGORY/>");
                Tally_Edit.WriteLine("  <TRADERLEDNATUREOFPURCHASE/>");
                Tally_Edit.WriteLine("  <TDSDEDUCTEETYPE/>");
                Tally_Edit.WriteLine("  <TDSRATENAME/>");
                Tally_Edit.WriteLine("  <LEDGERFBTCATEGORY/>");
                Tally_Edit.WriteLine("  <ISINTERESTON>No</ISINTERESTON>");
                Tally_Edit.WriteLine("  <ALLOWINMOBILE>No</ALLOWINMOBILE>");
                Tally_Edit.WriteLine("  <ISCONDENSED>No</ISCONDENSED>");
                Tally_Edit.WriteLine("  <FORPAYROLL>No</FORPAYROLL>");
                Tally_Edit.WriteLine("  <INTERESTONBILLWISE>No</INTERESTONBILLWISE>");
                Tally_Edit.WriteLine("  <OVERRIDEINTEREST>No</OVERRIDEINTEREST>");
                Tally_Edit.WriteLine("  <OVERRIDEADVINTEREST>No</OVERRIDEADVINTEREST>");
                Tally_Edit.WriteLine("  <USEFORVAT>No</USEFORVAT>");
                Tally_Edit.WriteLine("  <IGNORETDSEXEMPT>No</IGNORETDSEXEMPT>");
                Tally_Edit.WriteLine("  <ISTCSAPPLICABLE>No</ISTCSAPPLICABLE>");
                Tally_Edit.WriteLine("  <ISTDSAPPLICABLE>No</ISTDSAPPLICABLE>");
                Tally_Edit.WriteLine("  <ISFBTAPPLICABLE>No</ISFBTAPPLICABLE>");
                Tally_Edit.WriteLine ("  <ISGSTAPPLICABLE>No</ISGSTAPPLICABLE>");
                Tally_Edit.WriteLine ("  <SHOWINPAYSLIP>No</SHOWINPAYSLIP>");
                Tally_Edit.WriteLine ("  <USEFORGRATUITY>No</USEFORGRATUITY>");
                Tally_Edit.WriteLine ("  <FORSERVICETAX>No</FORSERVICETAX>");
                Tally_Edit.WriteLine ("  <ISINPUTCREDIT>No</ISINPUTCREDIT>");
                Tally_Edit.WriteLine ("  <ISEXEMPTED>No</ISEXEMPTED>");
                Tally_Edit.WriteLine ("  <TDSDEDUCTEEISSPECIALRATE>No</TDSDEDUCTEEISSPECIALRATE>");
                Tally_Edit.WriteLine ("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine ("  <SORTPOSITION> 1000</SORTPOSITION>");
                Tally_Edit.WriteLine ("  <OPENINGBALANCE>" + OBal + "</OPENINGBALANCE>");
                Tally_Edit.WriteLine ("  <LANGUAGENAME.LIST>");
                Tally_Edit.WriteLine ("   <NAME.LIST>");
                Tally_Edit.WriteLine ("     <NAME>" + Ledger + "</NAME>");
                Tally_Edit.WriteLine ("   </NAME.LIST>");
                Tally_Edit.WriteLine ("   <LANGUAGEID> 1033</LANGUAGEID>");
                Tally_Edit.WriteLine ("  </LANGUAGENAME.LIST>");
                Tally_Edit.WriteLine (" </LEDGER>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_Group_Master_XML(String Group, String Alias, String Parent, String SortPosition)
        {
            String Str = String.Empty;
            try
            {
                Str = " <GROUP NAME=!1111! RESERVEDNAME=!1111!>";
                Str = Str.Replace("1111", Group);
                Tally_Edit.WriteLine(Str.Replace('!','"'));
                if (Parent.Trim() != String.Empty)
                {
                    Tally_Edit.WriteLine("  <PARENT>" + Parent + "</PARENT>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <PARENT/>");
                }
                Tally_Edit.WriteLine("  <ISBILLWISEON>No</ISBILLWISEON>");
                Tally_Edit.WriteLine("  <ISADDABLE>No</ISADDABLE>");
                Tally_Edit.WriteLine("  <ISSUBLEDGER>No</ISSUBLEDGER>");
                Tally_Edit.WriteLine("  <ISREVENUE>No</ISREVENUE>");
                Tally_Edit.WriteLine("  <AFFECTSGROSSPROFIT>No</AFFECTSGROSSPROFIT>");
                Tally_Edit.WriteLine("  <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                Tally_Edit.WriteLine("  <TRACKNEGATIVEBALANCES>No</TRACKNEGATIVEBALANCES>");
                Tally_Edit.WriteLine("  <ISCONDENSED>No</ISCONDENSED>");
                Tally_Edit.WriteLine("  <SORTPOSITION>" + SortPosition + "</SORTPOSITION>");
                Tally_Edit.WriteLine("  <LANGUAGENAME.LIST>");
                Tally_Edit.WriteLine("   <NAME.LIST>");
                Tally_Edit.WriteLine("     <NAME>" + Group + "</NAME>");
                if (Alias.Trim() != String.Empty)
                {
                    Tally_Edit.WriteLine("     <NAME>" + Alias + "</NAME>");
                }
                Tally_Edit.WriteLine("   </NAME.LIST>");
                Tally_Edit.WriteLine("   <LANGUAGEID> 1033</LANGUAGEID>");
                Tally_Edit.WriteLine("  </LANGUAGENAME.LIST>");
                Tally_Edit.WriteLine(" </GROUP>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write_Xml_Group(String Company, ref System.Data.DataTable Dt)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Group.txt");
                Header_XML_ledger(Company);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["parent"] == DBNull.Value)
                    {
                        Body_Group_Master_XML(Dt.Rows[i]["gname"].ToString().Replace("&", "&amp;"), String.Empty, String.Empty, "290");
                    }
                    else
                    {
                        if (Dt.Rows[i]["gname"].ToString() == Dt.Rows[i]["parent"].ToString())
                        {
                            Body_Group_Master_XML(Dt.Rows[i]["gname"].ToString().Replace("&", "&amp;"), String.Empty, String.Empty, "290");
                        }
                        else
                        {
                            Body_Group_Master_XML(Dt.Rows[i]["gname"].ToString().Replace("&", "&amp;"), String.Empty, Dt.Rows[i]["Parent"].ToString().Replace("&", "&amp;"), "290");
                        }
                    }
                }
                Footer_XML_Ledger();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Group.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Group.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Update_Group_True(Int32 GroupCode, Int32 GroupReserved, int Company_Code, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            Int32 GrpReserved = 0;
            try
            {
                Load_Data("Select groupcode from groupmas where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and groupunder = " + GroupCode, ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("UPdate groupmas set groupreserved = " + GroupReserved + " where groupunder = " + GroupCode + " and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'");
                    Update_Group_True(Convert.ToInt32(Dt.Rows[i][0]), GroupReserved, Company_Code, Year_Code);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_GroupReserved(int Company_Code, String Year_Code)
        {
            try
            {
                System.Data.DataTable Dt = new System.Data.DataTable();
                System.Data.DataTable Dt1 = new System.Data.DataTable();
                Load_Data("select distinct groupcode from groupmas where groupcode = groupunder and groupunder = groupreserved and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' order by groupcode", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Load_Data("select groupcode, groupreserved from groupmas where groupreserved = " + Dt.Rows[i]["groupcode"].ToString() + " and groupcode <> " + Dt.Rows[i]["groupcode"].ToString(), ref Dt1);
                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                    {
                        Update_Group_True(Convert.ToInt32(Dt1.Rows[j][0]), Convert.ToInt32(Dt1.Rows[j][1]), Company_Code, Year_Code);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Import_Ledgers_From_Tally_Xml(String FilePath, int Company_Code, String Year_Code)
        {
            try
            {
                String ledger_Name = String.Empty;
                System.Data.DataTable Dt = new System.Data.DataTable();
                Double OPBal = 0;
                String[] Address = new String[10];
                Int32 ledger_Code = 0;
                Int16 Address_i = 0;
                String Full_Address = String.Empty;
                Boolean ledger = false;
                Int32 Group_Code = 0;
                String Str = String.Empty;
                Int32 Max_Slno = 0;
                String IncomeTaxNumber = String.Empty;
                String SalesTaxNumber = String.Empty;
                String BillNo = String.Empty;
                DateTime Billdate = Convert.ToDateTime("01-Jan-1899");
                double BillAmount = 0;
                String Parent = String.Empty;
                XmlTextReader Reader = new XmlTextReader(FilePath);
                Int32 i = 0;
                Reader.Read();
                while (Reader.Read())
                {
                    if (Reader.IsEmptyElement == false)
                    {
                        if (Reader.Name.ToUpper() == "TALLYMESSAGE")
                        {
                            i += 1;
                        }
                        else if (Reader.Name.ToUpper() == "LEDGER")
                        {
                            if (ledger == false)
                            {
                                ledger = true;
                                Address_i = 0;
                                ledger_Name = String.Empty;
                                OPBal = 0;
                                IncomeTaxNumber = String.Empty;
                                SalesTaxNumber = String.Empty;
                                BillNo = string.Empty;
                                BillAmount = 0;
                            }
                            else
                            {
                                if (ledger_Name.ToUpper().Contains("PROFIT & LOSS") == false)
                                {
                                    if (OPBal < 0)
                                    {
                                        Execute("Insert into Ledger_Master(ledger_Code, ledger_Name, Ledger_title, ledger_Inprint, ledger_group_Code, ledger_Odebit, ledger_OCredit, ledger_Address, ledger_Tin, PanNo, Company_Code, Year_Code) values (" + ledger_Code + ", '" + ledger_Name + "', 'M/s', '" + ledger_Name + "', " + Group_Code + ", " + Convert.ToDouble(OPBal * (-1)) + ", 0, '" + Full_Address + "', '" + IncomeTaxNumber + "', '" + SalesTaxNumber + "', " + Company_Code + ", '" + Year_Code + "')");
                                    }
                                    else
                                    {
                                        Execute("Insert into Ledger_Master(ledger_Code, ledger_Name, Ledger_title, ledger_Inprint, ledger_group_Code, ledger_Odebit, ledger_OCredit, ledger_Address, ledger_Tin, PanNo, Company_Code, Year_Code) values (" + ledger_Code + ", '" + ledger_Name + "', 'M/s', '" + ledger_Name + "', " + Group_Code + ",  0, " + OPBal + ", '" + Full_Address + "', '" + IncomeTaxNumber + "', '" + SalesTaxNumber + "', " + Company_Code + ", '" + Year_Code + "')");
                                    }
                                }
                                ledger = false;
                            }
                        }
                        else if (Reader.Name.ToUpper() == "GROUP")
                        {
                            ledger = false;
                        }
                        else if (Reader.Name.ToUpper() == "NAME")
                        {
                            if (ledger)
                            {

                                Reader.Read();
                                if (ledger_Name == String.Empty)
                                {
                                    ledger_Name = Reader.Value.Replace("'", "`");
                                }
                                ledger = true;
                                ledger_Code = Convert.ToInt32(Max("Ledger_Master", "Ledger_Code", "company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", true));
                                Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "ADDRESS")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                Address[Address_i] = Reader.Value.Replace("'", "`");
                                Address_i += 1;
                                Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "INCOMETAXNUMBER")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                IncomeTaxNumber = Reader.Value; Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "SALESTAXNUMBER")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                SalesTaxNumber = Reader.Value; Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "PARENT")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                Parent = Reader.Value.Replace("'", "`"); Reader.Read();
                                Group_Code = Convert.ToInt32(GetData_InNumberWC("GroupMas", "groupname", Parent, "groupcode", Year_Code, Company_Code));
                            }
                        }
                        else if (Reader.Name.ToUpper() == "OPENINGBALANCE")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                OPBal = Convert.ToDouble(Reader.Value); Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "BILLDATE")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                Str = Reader.Value;
                                Billdate = Convert.ToDateTime(Str.Substring(6, 2) + "-" + Str.Substring(4, 2) + "-" + Str.Substring(0, 4));
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                BillNo = Reader.Value.Replace("'", "`");
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                if (Reader.Name.ToUpper() == "BILLCREDITPERIOD")
                                {
                                    Reader.Read();
                                    Reader.Read();
                                    Reader.Read();
                                    Reader.Read();
                                }
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                BillAmount = Convert.ToDouble(Reader.Value);
                                Reader.Read();
                                Max_Slno = Convert.ToInt32(Max ("Ledger_breakup", "Slno", "Ledger_Code = " + ledger_Code + " and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", true));
                                if (BillAmount < 0)
                                {
                                    Execute("Insert into ledger_Breakup values (" + ledger_Code + ", 'Ledger', " + Max_Slno + ", 'N', '" + BillNo + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Billdate)) + "', 0, " + Convert.ToDouble(BillAmount * (-1)) + ", 0, 1, 0, 0, 'L1', " + Company_Code + ", '" + Year_Code + "', null)");
                                }
                                else
                                {
                                    Execute("Insert into ledger_Breakup values (" + ledger_Code + ", 'Ledger', " + Max_Slno + ", 'N', '" + BillNo + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Billdate)) + "', " + BillAmount + ", 0, 0, 1, 0, 0, 'L1', " + Company_Code + ", '" + Year_Code + "', null)");
                                }
                            }
                        }
                    }
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Import_Ledgers_From_Tally_Xml_7(String FilePath, int Company_Code, String Year_Code)
        {
            try
            {
                String ledger_Name = String.Empty;
                System.Data.DataTable Dt = new System.Data.DataTable();
                Double OPBal = 0;
                String[] Address = new String[10];
                Int32 ledger_Code = 0;
                Int16 Address_i = 0;
                String Full_Address = String.Empty;
                Boolean ledger = false;
                Int32 Group_Code = 0;
                String Str = String.Empty;
                Int32 Max_Slno = 0;
                String IncomeTaxNumber = String.Empty;
                String SalesTaxNumber = String.Empty;
                String BillNo = String.Empty, BillNo1 = String.Empty;
                DateTime Billdate = Convert.ToDateTime("01-Jan-1899");
                double BillAmount = 0;
                String Parent = String.Empty;
                XmlTextReader Reader = new XmlTextReader(FilePath);
                Int32 i = 0;
                Reader.Read();
                while (Reader.Read())
                {
                    if (Reader.IsEmptyElement == false)
                    {
                        if (Reader.Name.ToUpper() == "TALLYMESSAGE")
                        {
                            i += 1;
                        }
                        else if (Reader.Name.ToUpper() == "LEDGER")
                        {
                            if (ledger == false)
                            {
                                ledger = true;
                                Address_i = 0;
                                ledger_Name = String.Empty;
                                OPBal = 0;
                                IncomeTaxNumber = String.Empty;
                                SalesTaxNumber = String.Empty;
                                BillNo = string.Empty;
                                BillAmount = 0;
                            }
                            else
                            {
                                if (ledger_Name.ToUpper().Contains("PROFIT & LOSS") == false)
                                {
                                    if (OPBal < 0)
                                    {
                                        Execute("Insert into Ledger_Master(ledger_Code, ledger_Name, Ledger_title, ledger_Inprint, ledger_group_Code, ledger_Odebit, ledger_OCredit, ledger_Address, ledger_Tin, PanNo, Company_Code, Year_Code) values (" + ledger_Code + ", '" + ledger_Name + "', 'M/s', '" + ledger_Name + "', " + Group_Code + ", " + Convert.ToDouble(OPBal * (-1)) + ", 0, '" + Full_Address + "', '" + IncomeTaxNumber + "', '" + SalesTaxNumber + "', " + Company_Code + ", '" + Year_Code + "')");
                                    }
                                    else
                                    {
                                        Execute("Insert into Ledger_Master(ledger_Code, ledger_Name, Ledger_title, ledger_Inprint, ledger_group_Code, ledger_Odebit, ledger_OCredit, ledger_Address, ledger_Tin, PanNo, Company_Code, Year_Code) values (" + ledger_Code + ", '" + ledger_Name + "', 'M/s', '" + ledger_Name + "', " + Group_Code + ",  0, " + OPBal + ", '" + Full_Address + "', '" + IncomeTaxNumber + "', '" + SalesTaxNumber + "', " + Company_Code + ", '" + Year_Code + "')");
                                    }
                                }
                                ledger = false;
                            }
                        }
                        else if (Reader.Name.ToUpper() == "GROUP")
                        {
                            ledger = false;
                        }
                        else if (Reader.Name.ToUpper() == "NAME")
                        {
                            if (ledger)
                            {

                                Reader.Read();
                                if (ledger_Name == String.Empty)
                                {
                                    ledger_Name = Reader.Value.Replace("'", "`");
                                }
                                else
                                {
                                    BillNo = Reader.Value.Replace("'", "`");
                                }
                                ledger = true;
                                ledger_Code = Convert.ToInt32(MaxWOCC("Ledger_Master", "Ledger_Code", "company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'"));
                                Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "ADDRESS")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                Address[Address_i] = Reader.Value.Replace("'", "`");
                                Address_i += 1;
                                Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "INCOMETAXNUMBER")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                IncomeTaxNumber = Reader.Value; Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "SALESTAXNUMBER")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                SalesTaxNumber = Reader.Value; Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "PARENT")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                Parent = Reader.Value.Replace("'", "`"); Reader.Read();
                                Group_Code = Convert.ToInt32(GetData_InNumberWC("GroupMas", "groupname", Parent, "groupcode", Year_Code, Company_Code));
                            }
                        }
                        else if (Reader.Name.ToUpper() == "OPENINGBALANCE")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                OPBal = Convert.ToDouble(Reader.Value); Reader.Read();
                            }
                        }
                        else if (Reader.Name.ToUpper() == "BILLDATE")
                        {
                            if (ledger)
                            {
                                Reader.Read();
                                Str = Reader.Value;
                                Billdate = Convert.ToDateTime(Str.Substring(6, 2) + "-" + Str.Substring(4, 2) + "-" + Str.Substring(0, 4));
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                BillNo1 = Reader.Value.Replace("'", "`");
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                if (Reader.Name.ToUpper() == "BILLCREDITPERIOD")
                                {
                                    Reader.Read();
                                    Reader.Read();
                                    Reader.Read();
                                    Reader.Read();
                                }
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                Reader.Read();
                                if (Reader.Value.ToString() == "\r\n   ")
                                {
                                    Reader.Read();
                                    Reader.Read();
                                }
                                else if (Reader.Value.ToUpper() == "NO")
                                {
                                    Reader.Read();
                                    Reader.Read();
                                    Reader.Read();
                                    Reader.Read();
                                }
                                BillAmount = Convert.ToDouble(Reader.Value);
                                Reader.Read();
                                Max_Slno = Convert.ToInt32(Max("Ledger_breakup", "Slno", "Ledger_Code = " + ledger_Code + " and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", true));
                                if (BillAmount < 0)
                                {
                                    Execute("Insert into Ledger_breakup values (" + ledger_Code + ", 'Ledger', " + Max_Slno + ", 'N', '" + BillNo + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Billdate)) + "', 0, " + Convert.ToDouble(BillAmount * (-1)) + ", 0, 1, 0, 0, 'L1', " + Company_Code + ", '" + Year_Code + "', null)");
                                }
                                else
                                {
                                    Execute("Insert into Ledger_breakup values (" + ledger_Code + ", 'Ledger', " + Max_Slno + ", 'N', '" + BillNo + "', '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Billdate)) + "', " + BillAmount + ", 0, 0, 1, 0, 0, 'L1', " + Company_Code + ", '" + Year_Code + "', null)");
                                }
                                BillNo = String.Empty;
                            }
                        }
                    }
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Avaneetha_Debtors_Breakup(String FilePath, int Company_Code, String Year_Code)
        {
            try
            {
                String BillNo = String.Empty;
                DateTime BillDate = Convert.ToDateTime("01-Jan-1899");
                System.Data.DataTable Dt = new System.Data.DataTable();
                Double Amount = 0;
                Int32 Ledger_Code = 0;
                Boolean Insert = false;
                XmlTextReader Reader = new XmlTextReader(FilePath);
                Reader.Read();
                while (Reader.Read())
                {
                    if (Reader.IsEmptyElement == false)
                    {
                        if (Reader.Name.ToUpper() == "BILLPARTY")
                        {
                            Reader.Read();
                            Load_Data("Select * from ledger_master where ledger_name = '" + Reader.Value.Replace("'", "`") + "' and ledger_group_Code in (Select groupcode from groupmas where groupreserved = 4800 and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "') and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", ref Dt);
                            if (Dt.Rows.Count > 0)
                            {
                                Ledger_Code = Convert.ToInt32(Dt.Rows[0]["ledger_Code"]);
                                Insert = true;
                            }
                            else
                            {
                                Insert = false;
                                BillNo = string.Empty;
                                BillDate = Convert.ToDateTime("01-Jan-1899");
                                Amount = 0;
                            }
                            Reader.Read();
                            Reader.Read();
                            Reader.Read();
                        }
                        else if (Reader.Name.ToUpper() == "BILLREF")
                        {
                            Reader.Read();
                            BillNo = Reader.Value.Replace("'", "`");
                            Reader.Read();
                        }
                        else if (Reader.Name.ToUpper() == "BILLDATE")
                        {
                            Reader.Read();
                            BillDate = Convert.ToDateTime(Reader.Value);
                            Reader.Read();
                        }
                        else if (Reader.Name.ToUpper() == "BILLFIXED")
                        {
                            Reader.Read();
                            BillNo = string.Empty;
                            BillDate = Convert.ToDateTime("01-Jan-1899");
                            Insert = false;
                            Amount = 0;
                        }
                        else if (Reader.Name.ToUpper() == "BILLFINAL")
                        {
                            Reader.Read();
                            if (Reader.Value.Trim() == String.Empty)
                            {
                                BillNo = string.Empty;
                                BillDate = Convert.ToDateTime("01-Jan-1899");
                                Insert = false;
                                Amount = 0;
                            }
                            else
                            {
                                Amount = Convert.ToDouble(Reader.Value);
                            }
                            Reader.Read();
                        }

                        if (Insert == true && Amount != 0)
                        {
                            Dt = new System.Data.DataTable();
                            Load_Data ("Select isnull(max(Slno), 0) from ledger_breakup where ledger_Code = " + Ledger_Code + " and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", ref Dt);
                            if (Amount < 0)
                            {
                                Amount = (-1) * Amount;
                                Execute("Insert into ledger_Breakup values (" + Ledger_Code + ", 'Ledger', " + Convert.ToInt32(Convert.ToInt32(Dt.Rows[0][0]) + 1) + ", 'N', '" + BillNo + "', '" + String.Format("{0:dd-MMM-yyyy}", BillDate) + "', 0, " + Amount + ", 0, 5, 0, 0, 'L1', " + Company_Code + ", '" + Year_Code + "', 0)");
                            }
                            else
                            {
                                Execute("Insert into ledger_Breakup values (" + Ledger_Code + ", 'Ledger', " + Convert.ToInt32(Convert.ToInt32(Dt.Rows[0][0]) + 1) + ", 'N', '" + BillNo + "', '" + String.Format("{0:dd-MMM-yyyy}", BillDate) + "', " + Amount + ", 0, 0, 5, 0, 0, 'L1', " + Company_Code + ", '" + Year_Code + "', 0)");
                            }
                            BillNo = string.Empty;
                            BillDate = Convert.ToDateTime("01-Jan-1899");
                            Insert = false;
                            Amount = 0;
                        }
                    }
                }
                Reader.Close();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Read_Xml(String FilePath, int Company_Code, String Year_Code)
        {
            String Str = String.Empty;
            String P_Node = String.Empty;
            Boolean Breakup_Flag = false, Invalid_Amount = false;
            Double BR_Amount = 0;
            Int64 VCode = 0;
            Int32 VMode = 0, Code = 0, Slno = 0, LedgerCode = 0;
            DateTime Vdate = DateTime.Now, RefDate = DateTime.Now;
            DateTime BRSDate = Convert.ToDateTime("01-Jan-1899");
            // Master
            String Vno = String.Empty, Vtype = String.Empty, Remarks = String.Empty;
            String RefDoc = String.Empty, Mode = String.Empty;
            double Amount = 0;
            System.Data.DataTable Dt = new System.Data.DataTable();
            System.Data.DataTable TempDt = new System.Data.DataTable();

            try
            {
                Execute("Delete from voucher_type where vchtypeNo > 8");

                XmlTextReader Reader = new XmlTextReader(FilePath);
                Reader.Read();
                while (Reader.Read())
                {
                    if (Reader.IsEmptyElement == false)
                    {
                        if (Reader.Name.ToUpper() == "GUID" || Reader.Name.ToUpper() == "DATE" || Reader.Name.ToUpper() == "NARRATION" || Reader.Name.ToUpper() == "VOUCHERTYPENAME" || Reader.Name.ToUpper() == "LEDGERNAME" || Reader.Name.ToUpper() == "AMOUNT" || Reader.Name.ToUpper() == "NAME" || Reader.Name.ToUpper() == "BILLTYPE" || Reader.Name.ToUpper() == "BASICBANKERSDATE")
                        {
                            Str = Reader.Name.ToUpper();
                            Reader.Read();
                            if (Str == "VOUCHERTYPENAME")
                            {
                                if (Get_RecordCount("Voucher_type", "vchtypename = '" + Reader.Value + "'") == 0)
                                {
                                    Code = Convert.ToInt32(MaxWOCC("Voucher_Type", "VchtypeNo", String.Empty));
                                    Execute("Insert into voucher_type values (" + Code + ", '" + Reader.Value + "')");
                                }
                                else
                                {
                                    Code = Convert.ToInt32(GetData_InNumber("Voucher_type", "vchtypeName", Reader.Value, "vchtypeNo"));
                                }
                                VMode = Code;
                            }
                            if (Str == "BASICBANKERSDATE")
                            {
                                BRSDate = Convert.ToDateTime(Reader.Value.Substring(6, 2) + "/" + Reader.Value.Substring(4, 2) + "/" + Reader.Value.Substring(0, 4));
                            }
                            if (Str == "DATE")
                            {
                                if (VMode > 0)
                                {
                                    if (VCode > 0)
                                    {
                                        Execute(" Insert into Voucher_master values (" + VCode + ", " + VMode + ", '" + Vno + "', ' ', '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', '', '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', null, null, null, null,null, null, " + Company_Code + ", '" + Year_Code + "', 1, '" + String.Format ("{0:dd-MMM-yyyy}", BRSDate) + "', null, null, null, null, null, NULL)");
                                        BRSDate = Convert.ToDateTime("01-Jan-1899");
                                    }
                                }
                                Slno = 0;
                                VCode = 0;
                                Vno = String.Empty;
                                Remarks = String.Empty;
                                BR_Amount = 0;
                                VMode = 0;
                                Amount = 0;
                                LedgerCode = 0;
                                Breakup_Flag = false;
                                Invalid_Amount = false;

                                VCode = Convert.ToInt64(MaxWOCC("Voucher_Master", "Vcode", "Company_Code = " + Company_Code + " and Year_Code = '" + Year_Code + "'"));
                                Vdate = Convert.ToDateTime(Reader.Value.Substring(6,2) + "/" + Reader.Value.Substring(4,2) + "/" + Reader.Value.Substring(0,4));
                                RefDate = Vdate;
                            }
                            if (Str == "NARRATION")
                            {
                                if (Remarks == String.Empty)
                                {
                                    Remarks = Reader.Value.Replace("'", "`");
                                }
                            }
                            if (Str == "LEDGERNAME")
                            {
                                LedgerCode = Convert.ToInt32(GetData_InNumberWC ("Ledger_Master", "Ledger_Name", Reader.Value.Replace("'","`"), "Ledger_Code", Year_Code, Company_Code));
                                if (LedgerCode == 0)
                                {
                                    if (Check_Table("Ledger_Zero") == false)
                                    {
                                        Execute("Create table Ledger_Zero (Name varchar(200))");
                                    }
                                    if (Get_RecordCount("Ledger_Zero", "Name = '" + Reader.Value.Replace("'", "`") + "'") == 0)
                                    {
                                        Execute("Insert into Ledger_Zero values ('" + Reader.Value.Replace("'", "`") + "')");
                                    }
                                }
                                Invalid_Amount = false;
                            }
                            if (Str == "NAME")
                            {
                                RefDoc = Reader.Value.Replace("'", "`");
                            }
                            if (Str == "BILLTYPE")
                            {
                                Breakup_Flag = true;
                                Mode = Reader.Value.Substring(0,1);
                                if (Mode != "N" && Mode != "A")
                                {
                                    Mode = "N";
                                }
                            }
                            
                            Load_Data("Select isnull(Max(cast(Vno as Numeric(12))), 0) Vno from voucher_master where vmode  = " + VMode + "  and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", ref Dt);
                            if (Dt.Rows.Count == 0)
                            {
                                Vno = "1";
                            }
                            else
                            {
                                Vno = Convert.ToString(Convert.ToInt32(Dt.Rows[0]["Vno"]) + 1);
                            }

                            //Vno = Convert.ToString(Max("Voucher_master", "vno", "vmode = " + VMode + " and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", true));

                            if (Str == "AMOUNT")
                            {
                                if (Invalid_Amount == false)
                                {
                                    if (Breakup_Flag)
                                    {
                                        Breakup_Flag = false;
                                        BR_Amount = Convert.ToDouble(Reader.Value);
                                        if (BR_Amount < 0)
                                        {
                                            Execute("Insert into voucher_Breakup_bills values (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + LedgerCode + ", 1, '" + Mode + "', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', " + (-1) * BR_Amount + ", 0, 0, " + VCode + ", 'CR', " + Company_Code + ", '" + Year_Code + "', Null, 0)");
                                        }
                                        else
                                        {
                                            Execute("Insert into voucher_Breakup_bills values (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + LedgerCode + ", 1, '" + Mode + "', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', 0, " + BR_Amount + ", 0, " + VCode + ", 'DR', " + Company_Code + ", '" + Year_Code + "', Null, 0)");
                                        }
                                        if (Mode == "N")
                                        {
                                            if (BR_Amount < 0)
                                            {
                                                Execute("Insert into Ledger_breakup values (" + LedgerCode + ", 'VOUCHER', 1, 'N', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', 0, " + (-1) * BR_Amount + ", 0, " + VMode + ", 0, 0, '" + VCode + "', " + Company_Code + ", '" + Year_Code + "', Null)");
                                            }
                                            else
                                            {
                                                Execute("Insert into Ledger_breakup values (" + LedgerCode + ", 'VOUCHER', 1, 'N', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', " + BR_Amount + ", 0, 0, " + VMode + ", 0, 0, '" + VCode + "', " + Company_Code + ", '" + Year_Code + "', Null)");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Slno += 1;
                                        Amount = Convert.ToDouble(Reader.Value);
                                        if (Amount < 0)
                                        {
                                            Execute("Insert into Voucher_Details values (" + VCode + ", '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + Slno + ", 'BY', " + LedgerCode + ", " + Convert.ToDouble((-1) * Amount) + ", 0, '" + Remarks + "', " + Company_Code + ", '" + Year_Code + "', 0, 'True', 'True', 'True')");
                                        }
                                        else
                                        {
                                            Str = "Insert into Voucher_Details values (" + VCode + ", '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + Slno + ", 'TO', " + LedgerCode + ", 0, " + Amount + ", '" + Remarks + "', " + Company_Code + ", '" + Year_Code + "', 0, 'True', 'True', 'True')";
                                            Execute(Str);
                                        }
                                    }
                                }
                                else
                                {
                                    Invalid_Amount = false;
                                }
                            }
                            Reader.Read();
                        }
                        else if (Reader.Name.ToUpper() == "GODOWNNAME" || Reader.Name.ToUpper() == "STOCKITEMNAME" || Reader.Name.ToUpper() == "COSTCENTREALLOCATIONS.LIST")
                        {
                            if (Reader.Name.ToUpper() == "COSTCENTREALLOCATIONS.LIST")
                            {
                                Invalid_Amount = true;
                            }
                            else
                            {
                                Invalid_Amount = true;
                            }
                        }
                    }
                }
                Reader.Close();

                Load_Data("select vcode, vdate, company_Code, year_Code, byto, Min(slno) slno from voucher_details group by vcode, vdate, company_Code, year_Code, byto order by vcode, slno", ref Dt);
                for (int i=0;i<=Dt.Rows.Count - 1;i++)
                {
                    Load_Data("Select isnull(Ledger_Code, 0) ledger_Code from voucher_details where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "' and slno = " + Dt.Rows[i]["Slno"] + " and byto = '" + Dt.Rows[i]["Byto"].ToString() + "'", ref TempDt);
                    if (TempDt.Rows.Count > 0)
                    {
                        Execute ("update voucher_Details set rev_ledCode = " + TempDt.Rows[0]["Ledger_Code"].ToString() + " where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "' and byto <> '" + Dt.Rows[i]["Byto"].ToString() + "'");
                    }
                }

                Load_Data("Select *, (case when Debit > 0 then debit else credit end) Amount from voucher_breakup_bills where Mode = 'A' and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Dt.Rows[i]["Debit"]) > 0)
                    {
                        if (Get_RecordCount("ledger_breakup", "company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and Credit > 0") > 0)
                        {
                            Execute("Update ledger_breakup set Amount_Cl = Amount_Cl + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and Credit > 0");
                        }
                        else
                        {
                            Execute("Update Voucher_breakup_bills set Mode = 'N' where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Refdate"])) + "' and Company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["year_Code"].ToString() + "' and Mode = 'A'");
                        }
                    }
                    else
                    {
                        if (Get_RecordCount("ledger_breakup", "company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and Debit > 0") > 0)
                        {
                            Execute("Update ledger_breakup set Amount_Cl = Amount_Cl + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and debit > 0");
                        }
                        else
                        {
                            Execute("Update Voucher_breakup_bills set Mode = 'N' where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Refdate"])) + "' and Company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["year_Code"].ToString() + "' and Mode = 'A'");
                        }
                    }
                }
                Execute("Delete from voucher_master where vmode in (select vchtypeno from voucher_type where vchtypeName = 'Stock Journal')");
                Execute("Delete from voucher_master where vmode in (select vchtypeno from voucher_type where vchtypeName = 'Physical Stock')");
                Execute("Delete from voucher_master where vmode in (select vchtypeno from voucher_type where vchtypeName = 'Receipt Note')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Read_Xml_7(String FilePath, int Company_Code, String Year_Code)
        {
            String Str = String.Empty;
            String P_Node = String.Empty;
            Boolean Breakup_Flag = false, Invalid_Amount = false;
            Double BR_Amount = 0;
            Int64 VCode = 0;
            Int32 VMode = 0, Code = 0, Slno = 0, LedgerCode = 0;
            DateTime Vdate = DateTime.Now, RefDate = DateTime.Now;
            DateTime BRSDate = Convert.ToDateTime("01-Jan-1899");
            // Master
            String Vno = String.Empty, Vtype = String.Empty, Remarks = String.Empty;
            String RefDoc = String.Empty, Mode = String.Empty;
            double Amount = 0;
            Int32 P_VMode = 0;
            System.Data.DataTable Dt = new System.Data.DataTable();
            System.Data.DataTable TempDt = new System.Data.DataTable();

            try
            {
                Execute("Delete from voucher_type where vchtypeNo > 8");

                XmlTextReader Reader = new XmlTextReader(FilePath);
                Reader.Read();
                while (Reader.Read())
                {
                    if (Reader.IsEmptyElement == false)
                    {
                        if (Reader.Name.ToUpper() == "GUID" || Reader.Name.ToUpper() == "DATE" || Reader.Name.ToUpper() == "NARRATION" || Reader.Name.ToUpper() == "VOUCHERTYPENAME" || Reader.Name.ToUpper() == "LEDGERNAME" || Reader.Name.ToUpper() == "AMOUNT" || Reader.Name.ToUpper() == "NAME" || Reader.Name.ToUpper() == "BILLTYPE" || Reader.Name.ToUpper() == "BASICBANKERSDATE")
                        {
                            Str = Reader.Name.ToUpper();
                            Reader.Read();
                            if (Str == "VOUCHERTYPENAME")
                            {

                                if (VMode > 0)
                                {
                                    if (VCode > 0)
                                    {
                                        Execute(" Insert into Voucher_master values (" + VCode + ", " + VMode + ", '" + Vno + "', ' ', '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', '', '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', null, null, null, null,null, null, " + Company_Code + ", '" + Year_Code + "', 1, '" + String.Format("{0:dd-MMM-yyyy}", BRSDate) + "', null, null, null, null, null, NULL)");
                                        BRSDate = Convert.ToDateTime("01-Jan-1899");
                                    }
                                }
                                Slno = 0;
                                VCode = 0;
                                Vno = String.Empty;
                                Remarks = String.Empty;
                                BR_Amount = 0;
                                VMode = 0;
                                Amount = 0;
                                LedgerCode = 0;
                                Breakup_Flag = false;
                                Invalid_Amount = false;


                                if (Get_RecordCount("Voucher_type", "vchtypename = '" + Reader.Value + "'") == 0)
                                {
                                    Code = Convert.ToInt32(MaxWOCC("Voucher_Type", "VchtypeNo", String.Empty));
                                    Execute("Insert into voucher_type values (" + Code + ", '" + Reader.Value + "')");
                                }
                                else
                                {
                                    Code = Convert.ToInt32(GetData_InNumber("Voucher_type", "vchtypeName", Reader.Value, "vchtypeNo"));
                                }
                                VMode = Code;

                            }
                            if (Str == "BASICBANKERSDATE")
                            {
                                BRSDate = Convert.ToDateTime(Reader.Value.Substring(6, 2) + "/" + Reader.Value.Substring(4, 2) + "/" + Reader.Value.Substring(0, 4));
                            }
                            if (Str == "DATE")
                            {
                                VCode = Convert.ToInt64(MaxWOCC("Voucher_Master", "Vcode", "Company_Code = " + Company_Code + " and Year_Code = '" + Year_Code + "'"));
                                Vdate = Convert.ToDateTime(Reader.Value.Substring(6, 2) + "/" + Reader.Value.Substring(4, 2) + "/" + Reader.Value.Substring(0, 4));
                                RefDate = Vdate;
                            }
                            if (Str == "NARRATION")
                            {
                                if (Remarks == String.Empty)
                                {
                                    Remarks = Reader.Value.Replace("'", "`");
                                }
                            }
                            if (Str == "LEDGERNAME")
                            {
                                LedgerCode = Convert.ToInt32(GetData_InNumberWC("Ledger_Master", "Ledger_Name", Reader.Value.Replace("'", "`"), "Ledger_Code", Year_Code, Company_Code));
                                if (LedgerCode == 0)
                                {
                                    if (Check_Table("Ledger_Zero") == false)
                                    {
                                        Execute("Create table Ledger_Zero (Name varchar(200))");
                                    }
                                    if (Get_RecordCount("Ledger_Zero", "Name = '" + Reader.Value.Replace("'", "`") + "'") == 0)
                                    {
                                        Execute("Insert into Ledger_Zero values ('" + Reader.Value.Replace("'", "`") + "')");
                                    }
                                }
                                Invalid_Amount = false;
                            }
                            if (Str == "NAME")
                            {
                                RefDoc = Reader.Value.Replace("'", "`");
                            }
                            if (Str == "BILLTYPE")
                            {
                                Breakup_Flag = true;
                                Mode = Reader.Value.Substring(0, 1);
                                if (Mode != "N" && Mode != "A")
                                {
                                    Mode = "N";
                                }
                            }

                            Load_Data("Select isnull(Max(cast(Vno as Numeric(12))), 0) Vno from voucher_master where vmode  = " + VMode + "  and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", ref Dt);
                            if (Dt.Rows.Count == 0)
                            {
                                Vno = "1";
                            }
                            else
                            {
                                Vno = Convert.ToString(Convert.ToInt32(Dt.Rows[0]["Vno"]) + 1);
                            }

                            //Vno = Convert.ToString(Max("Voucher_master", "vno", "vmode = " + VMode + " and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", true));

                            if (Str == "AMOUNT")
                            {
                                if (Invalid_Amount == false)
                                {
                                    if (Breakup_Flag)
                                    {
                                        Breakup_Flag = false;
                                        BR_Amount = Convert.ToDouble(Reader.Value);
                                        if (BR_Amount < 0)
                                        {
                                            Execute("Insert into voucher_Breakup_bills values (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + LedgerCode + ", 1, '" + Mode + "', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', " + (-1) * BR_Amount + ", 0, 0, " + VCode + ", 'CR', " + Company_Code + ", '" + Year_Code + "', Null, 0)");
                                        }
                                        else
                                        {
                                            Execute("Insert into voucher_Breakup_bills values (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + LedgerCode + ", 1, '" + Mode + "', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', 0, " + BR_Amount + ", 0, " + VCode + ", 'DR', " + Company_Code + ", '" + Year_Code + "', Null, 0)");
                                        }
                                        if (Mode == "N")
                                        {
                                            if (BR_Amount < 0)
                                            {
                                                Execute("Insert into Ledger_breakup values (" + LedgerCode + ", 'VOUCHER', 1, 'N', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', 0, " + (-1) * BR_Amount + ", 0, " + VMode + ", 0, 0, '" + VCode + "', " + Company_Code + ", '" + Year_Code + "', Null)");
                                            }
                                            else
                                            {
                                                Execute("Insert into Ledger_breakup values (" + LedgerCode + ", 'VOUCHER', 1, 'N', '" + RefDoc.Replace("'", "`") + "', '" + String.Format("{0:dd-MMM-yyyy}", RefDate) + "', " + BR_Amount + ", 0, 0, " + VMode + ", 0, 0, '" + VCode + "', " + Company_Code + ", '" + Year_Code + "', Null)");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Slno += 1;
                                        Amount = Convert.ToDouble(Reader.Value);
                                        if (Amount < 0)
                                        {
                                            Execute("Insert into Voucher_Details values (" + VCode + ", '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + Slno + ", 'BY', " + LedgerCode + ", " + Convert.ToDouble((-1) * Amount) + ", 0, '" + Remarks + "', " + Company_Code + ", '" + Year_Code + "', 0, 'True', 'True', 'True')");
                                        }
                                        else
                                        {
                                            Str = "Insert into Voucher_Details values (" + VCode + ", '" + string.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + Slno + ", 'TO', " + LedgerCode + ", 0, " + Amount + ", '" + Remarks + "', " + Company_Code + ", '" + Year_Code + "', 0, 'True', 'True', 'True')";
                                            Execute(Str);
                                        }
                                    }
                                }
                                else
                                {
                                    Invalid_Amount = false;
                                }
                            }
                            Reader.Read();
                        }
                        else if (Reader.Name.ToUpper() == "GODOWNNAME" || Reader.Name.ToUpper() == "STOCKITEMNAME" || Reader.Name.ToUpper() == "COSTCENTREALLOCATIONS.LIST")
                        {
                            if (Reader.Name.ToUpper() == "COSTCENTREALLOCATIONS.LIST")
                            {
                                Invalid_Amount = true;
                            }
                            else
                            {
                                Invalid_Amount = true;
                            }
                        }
                    }
                }
                Reader.Close();

                Load_Data("select vcode, vdate, company_Code, year_Code, byto, Min(slno) slno from voucher_details group by vcode, vdate, company_Code, year_Code, byto order by vcode, slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Load_Data("Select isnull(Ledger_Code, 0) ledger_Code from voucher_details where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "' and slno = " + Dt.Rows[i]["Slno"] + " and byto = '" + Dt.Rows[i]["Byto"].ToString() + "'", ref TempDt);
                    if (TempDt.Rows.Count > 0)
                    {
                        Execute("update voucher_Details set rev_ledCode = " + TempDt.Rows[0]["Ledger_Code"].ToString() + " where vcode = " + Dt.Rows[i]["vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["Year_Code"].ToString() + "' and byto <> '" + Dt.Rows[i]["Byto"].ToString() + "'");
                    }
                }

                Load_Data("Select *, (case when Debit > 0 then debit else credit end) Amount from voucher_breakup_bills where Mode = 'A' and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Dt.Rows[i]["Debit"]) > 0)
                    {
                        if (Get_RecordCount("ledger_breakup", "company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and Credit > 0") > 0)
                        {
                            Execute("Update ledger_breakup set Amount_Cl = Amount_Cl + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and Credit > 0");
                        }
                        else
                        {
                            Execute("Update Voucher_breakup_bills set Mode = 'N' where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Refdate"])) + "' and Company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["year_Code"].ToString() + "' and Mode = 'A'");
                        }
                    }
                    else
                    {
                        if (Get_RecordCount("ledger_breakup", "company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and Debit > 0") > 0)
                        {
                            Execute("Update ledger_breakup set Amount_Cl = Amount_Cl + " + Convert.ToDouble(Dt.Rows[i]["Amount"]) + " where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and Mode = 'N' and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["RefDate"])) + "' and debit > 0");
                        }
                        else
                        {
                            Execute("Update Voucher_breakup_bills set Mode = 'N' where vcode = " + Dt.Rows[i]["Vcode"].ToString() + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["vdate"])) + "' and ledger_Code = " + Dt.Rows[i]["Ledger_Code"].ToString() + " and refDoc = '" + Dt.Rows[i]["RefDoc"].ToString().Replace("'", "`") + "' and refdate = '" + string.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["Refdate"])) + "' and Company_Code = " + Dt.Rows[i]["Company_Code"].ToString() + " and year_Code = '" + Dt.Rows[i]["year_Code"].ToString() + "' and Mode = 'A'");
                        }
                    }
                }
                Execute("Delete from voucher_master where vmode in (select vchtypeno from voucher_type where vchtypeName = 'Stock Journal')");
                Execute("Delete from voucher_master where vmode in (select vchtypeno from voucher_type where vchtypeName = 'Physical Stock')");
                Execute("Delete from voucher_master where vmode in (select vchtypeno from voucher_type where vchtypeName = 'Receipt Note')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Write_XML_Ledger(String Company, ref System.Data.DataTable Dt)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Ledger.txt");
                Header_XML_ledger(Company);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToDouble(Dt.Rows[i]["ODebit"]) > 0)
                    {
                        Body_Ledger_Master_XML(Dt.Rows[i]["prtclr"].ToString().Replace("&", "&amp;"), String.Empty, Dt.Rows[i]["parent"].ToString().Replace("&", "&amp;"), Dt.Rows[i]["Tinno"].ToString(), Dt.Rows[i]["ODebit"].ToString());
                    }
                    else
                    {
                        Body_Ledger_Master_XML(Dt.Rows[i]["prtclr"].ToString().Replace("&", "&amp;"), String.Empty, Dt.Rows[i]["parent"].ToString().Replace("&", "&amp;"), Dt.Rows[i]["Tinno"].ToString(), "-" + Dt.Rows[i]["OCredit"].ToString());
                    }
                }
                Footer_XML_Ledger();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Ledger.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Ledger.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Tally_Date(DateTime Dat)
        {
            String Y, M, D;
            try
            {
                Y = String.Format("{0:00}", Convert.ToDouble(Dat.Year));
                M = String.Format("{0:00}", Convert.ToDouble(Dat.Month));
                D = String.Format("{0:00}", Convert.ToDouble(Dat.Day));
                return Y + M + D;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Tally Payment
        public void Write_XML_Payment(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Payment.txt");
                Header_XML(Company);
                Upload_Payment();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Payment.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Payment.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_Payment()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 1 and Byto = 'BY' and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Payment(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64 (Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_Receipt_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 2 and Byto = 'TO' and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Receipt_New(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        String Body_Voucher_Entry(int CompCode, String Year_Code, Int64 Vcode, DateTime Vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Xml_Str = String.Empty;
            String VchTYpe = String.Empty;
            Int32 VchTypeCode = 0;
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            try
            {
                VchTypeCode = Convert.ToInt32(GetData_InNumberWC("Voucher_Master", "vcode", Vcode.ToString(), "vmode", Year_Code, CompCode));
                //{
                    Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate, Recon_Date, v1.remarks, v1.invoice_no from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and Byto = 'TO' and v1.vcode = " + Vcode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.vcode in (select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' group by vcode having cast(Sum(Debit) as Numeric(15,2)) = cast(SUm(credit) as Numeric(15,2))) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                //}
                //else
                //{
                //    Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and Byto = 'TO' and v1.vcode = " + Vcode + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "' and v1.vcode in (select vcode from voucher_details where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' group by vcode having Sum(Debit) = SUm(credit)) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                //}
                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                    if (Dt.Rows.Count > 0)
                    {
                        Load_Data("Select * from Socks_Companymas where upper(compname) like '%AVANEETHA%'", ref Dt1);
                        if (Dt1.Rows.Count > 0)
                        {
                            VchTYpe = GetData_InString("Voucher_type", "vchtypeNo", VchTypeCode.ToString(), "vchtypeName");
                            if (Dt.Rows[0]["Recon_Date"] == null || Dt.Rows[0]["Recon_Date"] == DBNull.Value || Convert.ToDateTime(Dt.Rows[0]["recon_Date"]) == Convert.ToDateTime("01/01/1899"))
                            {
                                Xml_Str = Body_Voucher_Entry_XMl(VchTypeCode, VchTYpe, Convert.ToInt64(Dt.Rows[0]["vcode"]), Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])), Dt.Rows[0]["Ledger"].ToString(), Dt.Rows[0]["Remarks"].ToString(), Convert.ToInt64(Dt.Rows[0]["Vcode"]), Convert.ToDateTime(Dt.Rows[0]["Vdate"]), CompCode, Year_Code, Dt.Rows[0]["Invoice_NO"].ToString(), Convert.ToDateTime("01/01/1899"));
                            }
                            else
                            {
                                Xml_Str = Body_Voucher_Entry_XMl(VchTypeCode, VchTYpe, Convert.ToInt64(Dt.Rows[0]["vcode"]), Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])), Dt.Rows[0]["Ledger"].ToString(), Dt.Rows[0]["Remarks"].ToString(), Convert.ToInt64(Dt.Rows[0]["Vcode"]), Convert.ToDateTime(Dt.Rows[0]["Vdate"]), CompCode, Year_Code, Dt.Rows[0]["Invoice_NO"].ToString(), Convert.ToDateTime(Dt.Rows[0]["recon_Date"]));
                            }
                        }
                        else
                        {
                            VchTYpe = GetData_InString("Voucher_type", "vchtypeNo", VchTypeCode.ToString(), "vchtypeName");
                            if (Dt.Rows[0]["Recon_Date"] == null || Dt.Rows[0]["Recon_Date"] == DBNull.Value || Convert.ToDateTime(Dt.Rows[0]["recon_Date"]) == Convert.ToDateTime("01/01/1899"))
                            {
                                Xml_Str = Body_Voucher_Entry_XMl(VchTypeCode, VchTYpe, Convert.ToInt64(Dt.Rows[0]["vcode"]), Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])), Dt.Rows[0]["Ledger"].ToString(), Dt.Rows[0]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[0]["Vcode"]), Convert.ToDateTime(Dt.Rows[0]["Vdate"]), CompCode, Year_Code, Dt.Rows[0]["vno"].ToString(), Convert.ToDateTime("01/01/1899"));
                            }
                            else
                            {
                                Xml_Str = Body_Voucher_Entry_XMl(VchTypeCode, VchTYpe, Convert.ToInt64(Dt.Rows[0]["vcode"]), Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])), Dt.Rows[0]["Ledger"].ToString(), Dt.Rows[0]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[0]["Vcode"]), Convert.ToDateTime(Dt.Rows[0]["Vdate"]), CompCode, Year_Code, Dt.Rows[0]["vno"].ToString(), Convert.ToDateTime(Dt.Rows[0]["recon_Date"]));
                            }
                        }
                    }
                //}
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }







        void Upload_Contra_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 3 and Byto = 'TO' and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Contra_New(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_Journal_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 4 and Byto = 'TO' and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Journal_New(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_Sales_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 5 and Byto = 'TO' and v1.vcode in (select vcode from voucher_details where company_Code = 1 and year_Code = '2010-2011' group by vcode having Sum(Debit) = SUm(credit)) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Sales_New(Dt.Rows[i]["vno"].ToString(), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Upload_Purchase_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 6 and Byto = 'TO' and v1.vcode in (select vcode from voucher_details where company_Code = 1 and year_Code = '2010-2011' group by vcode having Sum(Debit) = SUm(credit)) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Purchase_New(Dt.Rows[i]["vno"].ToString(), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_DebitNote_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 7 and Byto = 'TO' and v1.vcode in (select vcode from voucher_details where company_Code = 1 and year_Code = '2010-2011' group by vcode having Sum(Debit) = SUm(credit)) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_DebitNote_New(Dt.Rows[i]["vno"].ToString(), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_CreditNote_New()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 8 and Byto = 'TO' and v1.vcode in (select vcode from voucher_details where company_Code = 1 and year_Code = '2010-2011' group by vcode having Sum(Debit) = SUm(credit)) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_CreditNote_New(Dt.Rows[i]["vno"].ToString(), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Upload_All_DayBook_Entries(int Vmode, int Company_Code, string Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = " + Company_Code + " and v1.year_Code = '" + Year_Code + "' and v1.vmode= " + Vmode + " and Byto = 'TO' and v1.vcode in (select vcode from voucher_details where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' group by vcode having Sum(Debit) = SUm(credit)) and l1.ledger_NAme is not null order by v1.vcode, v2.slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_CreditNote_New(Dt.Rows[i]["vno"].ToString(), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Convert.ToInt64(Dt.Rows[i]["Vcode"]), Convert.ToDateTime(Dt.Rows[i]["Vdate"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Body_Payment(Int64 VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine (Str.Replace ('!','"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!Payment! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo));
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Payment</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");
  
                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                Payment_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Payment_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                //Load_Data("select e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 1 and e1.vno = " + VCHNO, ref Dt);
                Load_Data("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 1 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format ("{0:dd-MMM-yyyy}", vdate) + "' ", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        void Body_Receipt_New(Int64 VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!Receipt! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + 10000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Receipt</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                ReceiptNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        String Body_Voucher_Entry_XMl(int Vmode, String Voucher_Type, Int64 VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate, int CompCode, String year_Code, string Voucher_No, DateTime Recon_Date)
        {
            String Str = String.Empty, Xml_Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            System.Data.DataTable Dt2;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Xml_Str = Str;

                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a8" + CompCode + "-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a8" + CompCode + "-00009d4c:00000008! VCHTYPE=!" + Voucher_Type + "! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                if (Get_RecordCount("Socks_Companymas", "Compname like '%GAINUP%'") > 0)
                {
                    StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + (100000));
                }
                else if (Get_RecordCount("Socks_Companymas", "Compname like '%DHANA%'") > 0)
                {
                    StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + (100000));
                }
                else if (Get_RecordCount("Socks_Companymas", "Compname like '%AVANEETHA%'") > 0)
                {
                    StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + (100000));
                }
                else
                {
                    StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + ((10000 * Vmode)));
                }
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Xml_Str += Str;
                if (Vmode == 5)
                {
                    Ledger_Address(Convert.ToInt32(GetData_InNumber("Ledger_MAster", "ledger_Name", Ledger, "Ledger_Code")), CompCode, year_Code);
                    if (CusAddress[1] != String.Empty)
                    {
                        Xml_Str += " <BASICBUYERADDRESS.LIST TYPE=!STRING!> ";
                        Xml_Str += " <BASICBUYERADDRESS>" + CusAddress[1] + "<BASICBUYERADDRESS>";
                    }
                    if (CusAddress[2] != String.Empty)
                    {
                        Xml_Str += " <BASICBUYERADDRESS>" + CusAddress[2] + "<BASICBUYERADDRESS>";
                    }
                    if (CusAddress[3] != String.Empty)
                    {
                        Xml_Str += " <BASICBUYERADDRESS>" + CusAddress[3] + "<BASICBUYERADDRESS>";
                    }
                    if (CusAddress[4] != String.Empty)
                    {
                        Xml_Str += " <BASICBUYERADDRESS>" + CusAddress[4] + "<BASICBUYERADDRESS>";
                    }
                    if (CusAddress[1] != String.Empty)
                    {
                        Xml_Str += " </BASICBUYERADDRESS.LIST>";
                    }
                }
                if (Vmode == 1 || Vmode == 3 || Vmode == 2)
                {
                    if (Get_RecordCount("Socks_Companymas", "((Compname like '%Avaneetha%') or (Compname like '%Gainup%'))") > 0)
                    {
                        if (Recon_Date == Convert.ToDateTime("01/01/1899"))
                        {
                            //Xml_Str += "<BANKALLOCATIONS.LIST>";
                            //Xml_Str += "<DATE>" + Tally_Date(Recon_Date) + "</DATE>";
                            //Xml_Str += "<STATUS>No</STATUS>";
                            //Xml_Str += "</BANKALLOCATIONS.LIST>";
                        }
                        else
                        {
                            //Xml_Str += "<BANKALLOCATIONS.LIST>";
                            //Xml_Str += "<DATE>" + Tally_Date(Recon_Date) + "</DATE>";
                            //Xml_Str += "<STATUS>Yes</STATUS>";
                            //Xml_Str += "</BANKALLOCATIONS.LIST>";
                        }
                    }
                    else
                    {
                        if (Recon_Date != Convert.ToDateTime("01/01/1899"))
                        {
                            Xml_Str += " <BASICBANKERSDATE.LIST TYPE=!Date!>";
                            Xml_Str += " <BASICBANKERSDATE>" + Tally_Date(Recon_Date) + "</BASICBANKERSDATE>";
                            Xml_Str += " </BASICBANKERSDATE.LIST>";
                        }
                    }
                }
                Xml_Str += "  <DATE>" + Date + "</DATE>";
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a8" + CompCode + "-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Xml_Str += Str;
                if (Narration.Contains("&"))
                {
                    Xml_Str += "  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>";
                }
                else
                {
                    Xml_Str += "  <NARRATION>" + Narration + "</NARRATION>";
                }
                Xml_Str += "  <VOUCHERTYPENAME>" + Voucher_Type + "</VOUCHERTYPENAME>";
                if (Voucher_No != String.Empty)
                {
                    Xml_Str += "  <VOUCHERNUMBER>" + Voucher_No + "</VOUCHERNUMBER>";
                }
                else
                {
                    Xml_Str += "  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>";
                }
                if (Vmode == 6)
                {
                    Dt2 = new System.Data.DataTable();
                    Load_Data("Select isnull(Invoice_No, '') invoice_No, INvoice_Date from voucher_Master where vcode = " + VchNo + " and company_Code = " + CompCode + " and year_Code = '" + year_Code + "'", ref Dt2);
                    if (Dt2.Rows.Count > 0)
                    {
                        Xml_Str += "  <REFERENCE>" + Dt2.Rows[0]["INVOICE_NO"].ToString().Replace("&", "&amp;") + "</REFERENCE>";
                    }
                }
                Xml_Str += "  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>";
                Xml_Str += "  <CSTFORMISSUETYPE/>";
                Xml_Str += "  <CSTFORMRECVTYPE/>";
                Xml_Str += "  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>";
                Xml_Str += "  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>";
                Xml_Str += "  <VCHGSTCLASS/>";
                Xml_Str += "  <DIFFACTUALQTY>No</DIFFACTUALQTY>";
                Xml_Str += "  <AUDITED>No</AUDITED>";
                Xml_Str += "  <FORJOBCOSTING>No</FORJOBCOSTING>";
                Xml_Str += "  <ISOPTIONAL>No</ISOPTIONAL>";
                Xml_Str += "  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>";
                Xml_Str += "  <USEFORINTEREST>No</USEFORINTEREST>";
                Xml_Str += "  <USEFORGAINLOSS>No</USEFORGAINLOSS>";
                Xml_Str += "  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>";
                Xml_Str += "  <USEFORCOMPOUND>No</USEFORCOMPOUND>";
                Xml_Str += "  <ALTERID>1</ALTERID>";
                Xml_Str += "  <EXCISEOPENING>No</EXCISEOPENING>";
                Xml_Str += "  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ";

                Xml_Str += "  <ISCANCELLED>No</ISCANCELLED>";
                Xml_Str += "  <HASCASHFLOW>Yes</HASCASHFLOW>";
                Xml_Str += "  <ISPOSTDATED>No</ISPOSTDATED>";
                Xml_Str += "  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>";
                Xml_Str += "  <ISINVOICE>No</ISINVOICE>";
                Xml_Str += "  <MFGJOURNAL>No</MFGJOURNAL>";
                Xml_Str += "  <HASDISCOUNTS>No</HASDISCOUNTS>";
                Xml_Str += "  <ASPAYSLIP>No</ASPAYSLIP>";
                Xml_Str += "  <ISCOSTCENTRE>No</ISCOSTCENTRE> ";
                Xml_Str += "  <ISDELETED>No</ISDELETED>";
                Xml_Str += "  <ASORIGINAL>No</ASORIGINAL>";
                Xml_Str += "  <VCHISFROMSYNC>No</VCHISFROMSYNC>";
                Xml_Str += "  <MASTERID>1</MASTERID>";
                Xml_Str += "  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>";

                Xml_Str += "  <INVOICEINDENTLIST.LIST>";
                Xml_Str += "  </INVOICEINDENTLIST.LIST>";
                Xml_Str += "  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"');
                Xml_Str += "   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"');
                Xml_Str += "     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"');
                Xml_Str += "   </UDF:HARVCHSUBCLASS.LIST>";
                Xml_Str += "  </UDF:HARYANAVAT.LIST>";


                if (Vmode == 6)
                {
                    Dt2 = new System.Data.DataTable();
                    Load_Data("Select isnull(Invoice_No, '') invoice_No, INvoice_Date from voucher_Master where vcode = " + VchNo + " and company_Code = " + CompCode + " and year_Code = '" + year_Code + "'", ref Dt2);
                    if (Dt2.Rows.Count > 0)
                    {
                        Xml_Str += "<UDF:REFERENCEDATE.LIST DESC=!`ReferenceDate`! ISLIST=!YES! TYPE=!Date!>";
                        Xml_Str += "<UDF:REFERENCEDATE DESC=!`ReferenceDate`!>" + Tally_Date(Convert.ToDateTime(Dt2.Rows[0]["Invoice_Date"])) + "</UDF:REFERENCEDATE>";
                        Xml_Str += "</UDF:REFERENCEDATE.LIST>";
                    }
                }

                Xml_Str += ReceiptNew_ALLLEDGERENTRIES_Voucher(Vcode, Vdate, CompCode, year_Code);

                Xml_Str += " </VOUCHER>";
                Xml_Str += "  </TALLYMESSAGE>";
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        void Body_Contra_New(Int64 VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!Contra! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + 15000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Contra</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                ContraNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_Journal_New(Int64 VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!Journal! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo) + 18000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Journal</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                JournalNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_Sales_New(String VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!Sales! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(Vcode) + 20000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Sales</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                SalesNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Body_Purchase_New(String VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!Purchase! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(Vcode) + 25000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Purchase</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                PurcahseNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_DebitNote_New(String VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!DebitNote! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(Vcode) + 30000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>DebitNote</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                DebitNoteNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Body_CreditNote_New(String VchNo, String Date, String Ledger, String Narration, Int64 Vcode, DateTime Vdate)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = "<TALLYMESSAGE xmlns:UDF=!TallyUDF!>";
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Str = " <VOUCHER REMOTEID=!1574fab7-e28a-422e-b457-582a4b262a81-00000001! VCHKEY=!1574fab7-e28a-422e-b457-582a4b262a81-00009d4c:00000008! VCHTYPE=!CreditNote! ACTION=!Create! OBJVIEW=!Accounting Voucher View!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(Vcode) + 31000);
                Str = Str.Replace("00000001", StrNo);
                Str = Str.Replace("00000008", String.Format("{0:00000000}", Convert.ToDouble(StrNo) + 8));
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                //Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>1574fab7-e28a-422e-b457-582a4b262a81-00000001</GUID>";
                Str = Str.Replace("00000001", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>CreditNote</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <PERSISTEDVIEW>Accounting Voucher View</PERSISTEDVIEW>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>1</ALTERID>");
                Tally_Edit.WriteLine("  <EXCISEOPENING>No</EXCISEOPENING>");
                Tally_Edit.WriteLine("  <USEFORFINALPRODUCTION>No</USEFORFINALPRODUCTION> ");

                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISCOSTCENTRE>No</ISCOSTCENTRE> ");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <VCHISFROMSYNC>No</VCHISFROMSYNC>");
                Tally_Edit.WriteLine("  <MASTERID>1</MASTERID>");
                Tally_Edit.WriteLine("  <VOUCHERKEY>1729497430" + String.Format("{0:00000}", Convert.ToDouble(Vcode)) + "</VOUCHERKEY>");

                //Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                //Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                //Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                //Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                //Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                CreditNoteNew_ALLLEDGERENTRIES(Vcode, Vdate);
                Tally_Edit.WriteLine(" </VOUCHER>");
                Tally_Edit.WriteLine("  </TALLYMESSAGE>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        void ReceiptNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry ("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 2 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        String Get_Cheque_No(Int64 Vcode, DateTime VDate, int Company_Code, String Year_Code)
        {
            System.Data.DataTable TDt = new System.Data.DataTable();
            try
            {
                Load_Data("select Chq_NO, Chq_Date from Cheque_Details where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' order by slno", ref TDt);
                if (TDt.Rows.Count == 0)
                {
                    return String.Empty;
                }
                else
                {
                    return TDt.Rows[0]["Chq_No"].ToString();
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        DateTime Get_Cheque_Date(Int64 Vcode, DateTime VDate, int Company_Code, String Year_Code)
        {
            System.Data.DataTable TDt = new System.Data.DataTable();
            try
            {
                Load_Data("select Chq_NO, Chq_Date from Cheque_Details where vcode = " + Vcode + " and vdate = '" + String.Format("{0:dd-MMM-yyyy}", VDate) + "' and company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "' order by slno", ref TDt);
                if (TDt.Rows.Count == 0)
                {
                    return Convert.ToDateTime("01-Jan-1899");
                }
                else
                {
                    return Convert.ToDateTime(TDt.Rows[0]["Chq_Date"]);
                }
            }
            catch (Exception ex)
            {
                return Convert.ToDateTime("01-Jan-1899");
            }
        }
        String ReceiptNew_ALLLEDGERENTRIES_Voucher(Int64 VCHNO, DateTime vdate, int CompCode, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            String Xml_Str = String.Empty;
            String Chq_No = String.Empty;
            DateTime Chq_Date;
            try
            {

                Chq_Date = Get_Cheque_Date(VCHNO, vdate, CompCode, Year_Code);
                Chq_No = Get_Cheque_No(VCHNO, vdate, CompCode, Year_Code);

                Execute_Qry("select distinct v1.vmode, v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.ledger_Code, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate, v1.recon_Date from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Xml_Str += "  <ALLLEDGERENTRIES.LIST>";
                        Xml_Str += "   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>";
                        Xml_Str += "   <GSTCLASS/>";
                        Xml_Str += "   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>";
                        Xml_Str += "   <LEDGERFROMITEM>No</LEDGERFROMITEM>";
                        Xml_Str += "   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>";
                        Xml_Str += "   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>";
                        Xml_Str += "   <AMOUNT>" + Amount + "</AMOUNT>";
                        if (Convert.ToInt32(Dt.Rows[i]["vmode"]) == 6)
                        {
                            if (Get_RecordCount("Ledger_Master", "Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and ledger_Group_Code in (Select GroupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and groupreserved = 4100)") > 0)
                            {
                                if (Check_Table("Stock_TB"))
                                {
                                    System.Data.DataTable Dt1 = new System.Data.DataTable();
                                    Load_Data("Select * from Stock_TB", ref Dt1);
                                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                                    {
                                        Xml_Str += "<INVENTORYALLOCATIONS.LIST>";
                                        Xml_Str += "<STOCKITEMNAME>" + Dt1.Rows[j]["Name"].ToString() + "</STOCKITEMNAME>";
                                        Xml_Str += "<ISDEEMEDPOSITIVE>YES</ISDEEMEDPOSITIVE>";
                                        Xml_Str += "<ISAUTONEGATE>No</ISAUTONEGATE> ";
                                        Xml_Str += " <ISCUSTOMSCLEARANCE>No</ISCUSTOMSCLEARANCE>";
                                        Xml_Str += " <RATE>" + Dt1.Rows[j]["Rate"].ToString() + "</RATE>";
                                        Xml_Str += " <AMOUNT>-" + Dt1.Rows[j]["Amount"].ToString() + "</AMOUNT> ";
                                        Xml_Str += " <ACTUALQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</ACTUALQTY>";
                                        Xml_Str += " <BILLEDQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</BILLEDQTY> ";
                                        Xml_Str += " <BATCHALLOCATIONS.LIST>";
                                        Xml_Str += " <GODOWNNAME>Dummy</GODOWNNAME> ";
                                        Xml_Str += " <BATCHNAME>Primary Batch</BATCHNAME>";
                                        Xml_Str += " <INDENTNO />";
                                        Xml_Str += " <ORDERNO /> ";
                                        Xml_Str += " <TRACKINGNUMBER />";
                                        Xml_Str += " <AMOUNT>-" + Dt1.Rows[j]["Amount"].ToString() + "</AMOUNT>";
                                        Xml_Str += " <ACTUALQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</ACTUALQTY>";
                                        Xml_Str += " <BILLEDQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</BILLEDQTY>";
                                        Xml_Str += " </BATCHALLOCATIONS.LIST>";
                                        Xml_Str += " </INVENTORYALLOCATIONS.LIST>";
                                    }
                                }
                            }
                        }
                        else if (Convert.ToInt32(Dt.Rows[i]["vmode"]) == 2 || Convert.ToInt32(Dt.Rows[i]["vmode"]) == 3)
                        {
                            if (Get_RecordCount("Socks_Companymas", "((Compname like '%Avaneetha%') or (compname like '%Gainup%'))") > 0)
                            {
                                if (Get_RecordCount("Ledger_Master", "Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and ledger_Group_Code in (Select GroupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and groupreserved in (1600, 1650))") > 0)
                                {
                                    if (Convert.ToDateTime(Dt.Rows[0]["Recon_Date"]) == Convert.ToDateTime("01/01/1899"))
                                    {
                                        Xml_Str += "<BANKALLOCATIONS.LIST>";
                                        Xml_Str += "<DATE>" + Tally_Date(vdate) + "</DATE> ";
                                        if (Chq_Date == Convert.ToDateTime("01-Jan-1899"))
                                        {
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])) + "</INSTRUMENTDATE>";
                                        }
                                        else
                                        {
                                            Xml_Str += "<TRANSACTIONTYPE>Cheque</TRANSACTIONTYPE>";
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Chq_Date) + "</INSTRUMENTDATE>";
                                            Xml_Str += "<INSTRUMENTNUMBER>" + Chq_No + "</INSTRUMENTNUMBER>";
                                        }
                                        Xml_Str += "<STATUS>No</STATUS>";
                                        Xml_Str += "<AMOUNT>" + Amount + "</AMOUNT>";
                                        Xml_Str += "</BANKALLOCATIONS.LIST>";
                                    }
                                    else
                                    {
                                        Xml_Str += "<BANKALLOCATIONS.LIST>";
                                        Xml_Str += "<DATE>" + Tally_Date(vdate) + "</DATE> ";
                                        Xml_Str += "<BANKERSDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Recon_Date"])) + "</BANKERSDATE> ";
                                        if (Chq_Date == Convert.ToDateTime("01-Jan-1899"))
                                        {
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])) + "</INSTRUMENTDATE>";
                                        }
                                        else
                                        {
                                            Xml_Str += "<TRANSACTIONTYPE>Cheque</TRANSACTIONTYPE>";
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Chq_Date) + "</INSTRUMENTDATE>";
                                            Xml_Str += "<INSTRUMENTNUMBER>" + Chq_No + "</INSTRUMENTNUMBER>";
                                        }
                                        Xml_Str += "<STATUS>Yes</STATUS>";
                                        Xml_Str += "<AMOUNT>" + Amount + "</AMOUNT>";
                                        Xml_Str += "</BANKALLOCATIONS.LIST>";
                                    }
                                }
                            }
                        }
                        Xml_Str += Breakup_XML(VCHNO, vdate, Convert.ToInt32(Dt.Rows[i]["Ledger_Code"]), CompCode, Year_Code);
                        Xml_Str += "  </ALLLEDGERENTRIES.LIST>";
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Xml_Str += "  <ALLLEDGERENTRIES.LIST>";
                        Xml_Str += "   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>";
                        Xml_Str += "   <GSTCLASS/>";
                        Xml_Str += "   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>";
                        Xml_Str += "   <LEDGERFROMITEM>No</LEDGERFROMITEM>";
                        Xml_Str += "   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>";
                        Xml_Str += "   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>";
                        Xml_Str += "   <AMOUNT>" + Amount + "</AMOUNT>";
                        if (Convert.ToInt32(Dt.Rows[i]["vmode"]) == 5)
                        {
                            if (Get_RecordCount("Ledger_Master", "Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and ledger_Group_Code in (Select GroupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and groupreserved = 4400)") > 0)
                            {
                                if (Check_Table("STOCK_TB"))
                                {
                                    System.Data.DataTable Dt1 = new System.Data.DataTable();
                                    Load_Data("Select * from Stock_TB", ref Dt1);
                                    for (int j = 0; j <= Dt1.Rows.Count - 1; j++)
                                    {
                                        Xml_Str += "<INVENTORYALLOCATIONS.LIST>";
                                        Xml_Str += "<STOCKITEMNAME>" + Dt1.Rows[j]["Name"].ToString() + "</STOCKITEMNAME>";
                                        Xml_Str += "<ISDEEMEDPOSITIVE>NO</ISDEEMEDPOSITIVE>";
                                        Xml_Str += "<ISAUTONEGATE>No</ISAUTONEGATE> ";
                                        Xml_Str += " <ISCUSTOMSCLEARANCE>No</ISCUSTOMSCLEARANCE>";
                                        Xml_Str += " <RATE>" + Dt1.Rows[j]["Rate"].ToString() + "</RATE>";
                                        Xml_Str += " <AMOUNT>" + Dt1.Rows[j]["Amount"].ToString() + "</AMOUNT> ";
                                        Xml_Str += " <ACTUALQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</ACTUALQTY>";
                                        Xml_Str += " <BILLEDQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</BILLEDQTY> ";
                                        Xml_Str += " <BATCHALLOCATIONS.LIST>";
                                        Xml_Str += " <GODOWNNAME>Dummy</GODOWNNAME> ";
                                        Xml_Str += " <BATCHNAME>Primary Batch</BATCHNAME>";
                                        Xml_Str += " <INDENTNO />";
                                        Xml_Str += " <ORDERNO /> ";
                                        Xml_Str += " <TRACKINGNUMBER />";
                                        Xml_Str += " <AMOUNT>" + Dt1.Rows[j]["Amount"].ToString() + "</AMOUNT>";
                                        Xml_Str += " <ACTUALQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</ACTUALQTY>";
                                        Xml_Str += " <BILLEDQTY>" + Dt1.Rows[j]["Qty"].ToString() + "</BILLEDQTY>";
                                        Xml_Str += " </BATCHALLOCATIONS.LIST>";
                                        Xml_Str += " </INVENTORYALLOCATIONS.LIST>";
                                    }
                                }
                            }
                        }
                        else if (Convert.ToInt32(Dt.Rows[i]["vmode"]) == 1 || Convert.ToInt32(Dt.Rows[i]["vmode"]) == 3)
                        {
                            if (Get_RecordCount("Socks_Companymas", "((Compname like '%Avaneetha%') or (compname like '%Gainup%'))") > 0)
                            {
                                if (Get_RecordCount("Ledger_Master", "Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and ledger_Code = " + Dt.Rows[i]["ledger_Code"].ToString() + " and ledger_Group_Code in (Select GroupCode from groupmas where company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' and groupreserved in (1600, 1650))") > 0)
                                {
                                    if (Convert.ToDateTime(Dt.Rows[0]["Recon_Date"]) == Convert.ToDateTime("01/01/1899"))
                                    {
                                        Xml_Str += "<BANKALLOCATIONS.LIST>";
                                        Xml_Str += "<DATE>" + Tally_Date(vdate) + "</DATE> ";
                                        if (Chq_Date == Convert.ToDateTime("01-Jan-1899"))
                                        {
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])) + "</INSTRUMENTDATE>";
                                        }
                                        else
                                        {
                                            Xml_Str += "<TRANSACTIONTYPE>Cheque</TRANSACTIONTYPE>";
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Chq_Date) + "</INSTRUMENTDATE>";
                                            Xml_Str += "<INSTRUMENTNUMBER>" + Chq_No + "</INSTRUMENTNUMBER>";
                                        }
                                        Xml_Str += "<STATUS>No</STATUS>";
                                        Xml_Str += "<AMOUNT>" + Amount + "</AMOUNT>";
                                        Xml_Str += "</BANKALLOCATIONS.LIST>";
                                    }
                                    else
                                    {
                                        Xml_Str += "<BANKALLOCATIONS.LIST>";
                                        Xml_Str += "<DATE>" + Tally_Date(vdate) + "</DATE> ";
                                        Xml_Str += "<BANKERSDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Recon_Date"])) + "</BANKERSDATE> ";
                                        if (Chq_Date == Convert.ToDateTime("01-Jan-1899"))
                                        {
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Convert.ToDateTime(Dt.Rows[0]["Date"])) + "</INSTRUMENTDATE>";
                                        }
                                        else
                                        {
                                            Xml_Str += "<TRANSACTIONTYPE>Cheque</TRANSACTIONTYPE>";
                                            Xml_Str += "<INSTRUMENTDATE>" + Tally_Date(Chq_Date) + "</INSTRUMENTDATE>";
                                            Xml_Str += "<INSTRUMENTNUMBER>" + Chq_No + "</INSTRUMENTNUMBER>";
                                        }
                                        Xml_Str += "<STATUS>Yes</STATUS>";
                                        Xml_Str += "<AMOUNT>" + Amount + "</AMOUNT>";
                                        Xml_Str += "</BANKALLOCATIONS.LIST>";
                                    }
                                }
                            }
                        }
                        Xml_Str += Breakup_XML(VCHNO, vdate, Convert.ToInt32(Dt.Rows[i]["Ledger_Code"]), CompCode, Year_Code);
                        Xml_Str += "  </ALLLEDGERENTRIES.LIST>";
                    }
                }
                return Xml_Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Search_Folder(String Folder_Name)
        {
            try
            {
                String[] Drives = Environment.GetLogicalDrives();
                foreach (String S in Drives)
                {
                    if (Directory.Exists(S))
                    {
                        foreach (String S1 in Directory.GetDirectories(S))
                        {
                            if (S1.ToUpper().Replace(S, "") == Folder_Name.ToUpper())
                            {
                                return S1.ToUpper();
                            }
                        }
                    }
                }
                return String.Empty;
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }


        void ContraNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 3 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void JournalNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 4 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SalesNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 5 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void PurcahseNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 6 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        if (Convert.ToDouble(Dt.Rows[i]["DEBIT"]) < 0)
                        {
                            Amount = Convert.ToString(Convert.ToDouble(Dt.Rows[i]["DEBIT"]) * (-1));
                        }
                        else
                        {
                            Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        }
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void DebitNoteNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 7 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        if (Convert.ToDouble(Dt.Rows[i]["DEBIT"]) < 0)
                        {
                            Amount = Convert.ToString(Convert.ToDouble(Dt.Rows[i]["DEBIT"]) * (-1));
                        }
                        else
                        {
                            Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        }
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CreditNoteNew_ALLLEDGERENTRIES(Int64 VCHNO, DateTime vdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Execute_Qry("select distinct v1.user_Date Date, v1.vno, v2.byto toBy, l1.ledger_NAme Ledger, v2.debit, v2.slno, l1.ledger_Name Prtclr, v2.Credit, v2.narration, v1.vcode, v1.vdate from voucher_master v1 left join voucher_Details v2 on v1.vcode = v2.vcode and v1.vdate = v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v2.ledger_Code = l1.ledger_Code and v2.company_Code = l1.company_Code and v2.year_Code = l1.year_Code where v1.company_Code = 1 and v1.year_Code = '2010-2011' and v1.vmode= 8 and v1.vcode = " + VCHNO + " and v1.vdate = '" + String.Format("{0:dd-MMM-yyyy}", vdate) + "'", "Qry_Rec_Export");
                Load_Data("select * from Qry_Rec_Export order by Slno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "BY")
                    {
                        if (Convert.ToDouble(Dt.Rows[i]["DEBIT"]) < 0)
                        {
                            Amount = Convert.ToString(Convert.ToDouble(Dt.Rows[i]["DEBIT"]) * (-1));
                        }
                        else
                        {
                            Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        }
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Tally Receipt
        public void Write_XML_Receipt(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Receipt.txt");
                Header_XML(Company);
                Upload_Receipt_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Receipt.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Receipt.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Upload_Receipt()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                //Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr LEDGER, e1.debit, c2.prtclr, e1.credit, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 2 AND VNO = 2 and toby = 'BY'", ref Dt);
                Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 2 and toby = 'BY'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Receipt(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Body_Receipt(Int64 VchNo, String Date, String Ledger, String Narration)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = " <VOUCHER REMOTEID=!9d462c7c-3121-4bec-9e86-8a42d3f14004-00000005! VCHTYPE=!Receipt! ACTION=!Create!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo));
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                //Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>9d462c7c-3121-4bec-9e86-8a42d3f14004-00000005</GUID>";
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Receipt</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>3083</ALTERID>");
                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                Receipt_ALLLEDGERENTRIES(VchNo.ToString());
                Tally_Edit.WriteLine(" </VOUCHER>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Receipt_ALLLEDGERENTRIES(String VCHNO)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Load_Data("select e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 2 and e1.vno = " + VCHNO, ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "TO")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Tally Contra
        public void Write_XML_Contra(String Company)
        {
            try
            {
                //Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Contra.txt");
                //Header_XML(Company);
                //Upload_Contra();
                //Footer_XML();
                //Tally_Edit.Close();
                //FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Contra.txt");
                //F1.CopyTo("C:\\Vaahrep\\t11Contra.xml", true);
                //F1.Delete();
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Contra.txt");
                Header_XML(Company);
                Upload_Contra_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Contra.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Contra.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Upload_Contra()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                //Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr LEDGER, e1.debit, c2.prtclr, e1.credit, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 3 AND VNO = 79 and toby = 'BY'", ref Dt);
                Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 3 and toby = 'BY'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Contra(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Body_Contra(Int64 VchNo, String Date, String Ledger, String Narration)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = " <VOUCHER REMOTEID=!16339ca9-ad9b-42f4-8f80-c3addf82621d-00000005! VCHTYPE=!Contra! ACTION=!Create!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo));
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                //Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>16339ca9-ad9b-42f4-8f80-c3addf82621d-00000005</GUID>";
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Contra</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>3248</ALTERID>");
                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>Yes</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                Contra_ALLLEDGERENTRIES(VchNo.ToString());
                Tally_Edit.WriteLine(" </VOUCHER>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Contra_ALLLEDGERENTRIES(String VCHNO)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Load_Data("select e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 3 and e1.vno = " + VCHNO, ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "TO")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Tally Journal
        public void Write_XML_Journal(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Journal.txt");
                Header_XML(Company);
                Upload_Journal_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Journal.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Journal.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Upload_Journal()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            try
            {
                //Load_Data("select distinct E1.DATE, e1.vno from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where vno > 649 and mode = 4 and toby = 'BY'", ref Dt);
                Load_Data("select distinct E1.DATE, e1.vno from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 4 and toby = 'BY'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where vno = " + Dt.Rows[i]["vno"].ToString() + " and mode = 4 and toby = 'BY'", ref Dt1);
                    Body_Journal(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt1.Rows[0]["Ledger"].ToString(), Dt1.Rows[0]["Narration"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Body_Journal(Int64 VchNo, String Date, String Ledger, String Narration)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            Boolean Mul = false;
            try
            {
                Str = " <VOUCHER REMOTEID=!16339ca9-ad9b-42f4-8f81-c3addf82621d-00000005! VCHTYPE=!Journal! ACTION=!Create!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo));
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                //Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>16339ca9-ad9b-42f4-8f81-c3addf82621d-00000005</GUID>";
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Journal</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                if (Get_RecordCount("ENT0896", "mode = 4 and vno = " + VchNo.ToString() + " and toby = 'BY'") == 1)
                {
                    Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                    Mul = false;
                }
                else
                {
                    Mul = true;
                }
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>3360</ALTERID>");
                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>No</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                Journal_ALLLEDGERENTRIES(VchNo.ToString(), Mul);
                Tally_Edit.WriteLine(" </VOUCHER>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Journal_ALLLEDGERENTRIES(String VCHNO, Boolean Multiple)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Load_Data("select e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 4 and e1.vno = " + VCHNO, ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "TO")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        if (Multiple)
                        {
                            Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        }
                        else
                        {
                            Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        }
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Tally Sales
        public void Write_XML_Sales(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Sales.txt");
                Header_XML(Company);
                Upload_Sales_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Sales.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Sales.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Upload_Sales()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                //Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, e1.refDoc refe, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where vno = 72 and mode = 5 and toby = 'BY'", ref Dt);
                Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, e1.refDoc refe, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 5 and toby = 'BY'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Sales(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Dt.Rows[i]["Refe"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Body_Sales(Int64 VchNo, String Date, String Ledger, String Narration, String Refe)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = " <VOUCHER REMOTEID=!8732fa69-4200-442c-8c4d-f6dfa8e78e55-00000005! VCHTYPE=!Sales! ACTION=!Create!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo));
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                //Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>8732fa69-4200-442c-8c4d-f6dfa8e78e55-00000005</GUID>";
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Sales</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <REFERENCE>" + Refe + "</REFERENCE>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>3079</ALTERID>");
                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>No</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                Sales_ALLLEDGERENTRIES(VchNo.ToString());
                Tally_Edit.WriteLine(" </VOUCHER>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Sales_ALLLEDGERENTRIES(String VCHNO)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Load_Data("select e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 5 and e1.vno = " + VCHNO, ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "TO")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Tally Purchase
        public void Write_XML_Purchase(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11Purchase.txt");
                Header_XML(Company);
                Upload_Purchase_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11Purchase.txt");
                F1.CopyTo("C:\\Vaahrep\\t11Purchase.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write_XML_DebitNote(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11DebitNote.txt");
                Header_XML(Company);
                Upload_DebitNote_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11DebitNote.txt");
                F1.CopyTo("C:\\Vaahrep\\t11DebitNote.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Write_XML_CreditNote(String Company)
        {
            try
            {
                Tally_Edit = new StreamWriter("C:\\Vaahrep\\t11CreditNote.txt");
                Header_XML(Company);
                Upload_CreditNote_New();
                Footer_XML();
                Tally_Edit.Close();
                FileInfo F1 = new FileInfo("C:\\Vaahrep\\t11CreditNote.txt");
                F1.CopyTo("C:\\Vaahrep\\t11CreditNote.xml", true);
                F1.Delete();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Upload_Purchase()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                //Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, e1.refDoc refe, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 6 and vno = 1 and toby = 'BY'", ref Dt);
                Load_Data("select E1.DATE, e1.vno, e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit, e1.refDoc refe, (case when narr is null then '' else narr end) + (case when narr2 is null then '' else ' ' + narr2 end) as Narration from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 6 and toby = 'BY'", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Body_Purchase(Convert.ToInt64(Dt.Rows[i]["vno"]), Tally_Date(Convert.ToDateTime(Dt.Rows[i]["Date"])), Dt.Rows[i]["Ledger"].ToString(), Dt.Rows[i]["Narration"].ToString(), Dt.Rows[i]["Refe"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Body_Purchase(Int64 VchNo, String Date, String Ledger, String Narration, String Refe)
        {
            String Str = String.Empty;
            Int64 No = 0;
            String StrNo = String.Empty;
            try
            {
                Str = " <VOUCHER REMOTEID=!8732fa69-4201-442c-8c4d-f6dfa8e78e55-00000005! VCHTYPE=!Purchase! ACTION=!Create!>";
                StrNo = String.Format("{0:00000000}", Convert.ToDouble(VchNo));
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str.Replace('!', '"'));
                //Tally_Edit.WriteLine("  <DATE>" + Date + "</DATE>");
                Tally_Edit.WriteLine("  <DATE>20080401</DATE>");
                Str = "  <GUID>8732fa69-4201-442c-8c4d-f6dfa8e78e55-00000005</GUID>";
                Str = Str.Replace("00000005", StrNo);
                Tally_Edit.WriteLine(Str);
                if (Narration.Contains("&"))
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration.Replace("&", "&amp;") + "</NARRATION>");
                }
                else
                {
                    Tally_Edit.WriteLine("  <NARRATION>" + Narration + "</NARRATION>");
                }
                Tally_Edit.WriteLine("  <VOUCHERTYPENAME>Purchase</VOUCHERTYPENAME>");
                Tally_Edit.WriteLine("  <VOUCHERNUMBER>" + VchNo.ToString() + "</VOUCHERNUMBER>");
                Tally_Edit.WriteLine("  <REFERENCE>" + Refe + "</REFERENCE>");
                Tally_Edit.WriteLine("  <PARTYLEDGERNAME>" + Ledger.Replace("&", "&amp;") + "</PARTYLEDGERNAME>");
                Tally_Edit.WriteLine("  <CSTFORMISSUETYPE/>");
                Tally_Edit.WriteLine("  <CSTFORMRECVTYPE/>");
                Tally_Edit.WriteLine("  <FBTPAYMENTTYPE>Default</FBTPAYMENTTYPE>");
                Tally_Edit.WriteLine("  <VCHGSTCLASS/>");
                Tally_Edit.WriteLine("  <DIFFACTUALQTY>No</DIFFACTUALQTY>");
                Tally_Edit.WriteLine("  <AUDITED>No</AUDITED>");
                Tally_Edit.WriteLine("  <FORJOBCOSTING>No</FORJOBCOSTING>");
                Tally_Edit.WriteLine("  <ISOPTIONAL>No</ISOPTIONAL>");
                //Tally_Edit.WriteLine("  <EFFECTIVEDATE>" + Date + "</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <EFFECTIVEDATE>20080401</EFFECTIVEDATE>");
                Tally_Edit.WriteLine("  <USEFORINTEREST>No</USEFORINTEREST>");
                Tally_Edit.WriteLine("  <USEFORGAINLOSS>No</USEFORGAINLOSS>");
                Tally_Edit.WriteLine("  <USEFORGODOWNTRANSFER>No</USEFORGODOWNTRANSFER>");
                Tally_Edit.WriteLine("  <USEFORCOMPOUND>No</USEFORCOMPOUND>");
                Tally_Edit.WriteLine("  <ALTERID>3079</ALTERID>");
                Tally_Edit.WriteLine("  <ISCANCELLED>No</ISCANCELLED>");
                Tally_Edit.WriteLine("  <HASCASHFLOW>No</HASCASHFLOW>");
                Tally_Edit.WriteLine("  <ISPOSTDATED>No</ISPOSTDATED>");
                Tally_Edit.WriteLine("  <USETRACKINGNUMBER>No</USETRACKINGNUMBER>");
                Tally_Edit.WriteLine("  <ISINVOICE>No</ISINVOICE>");
                Tally_Edit.WriteLine("  <MFGJOURNAL>No</MFGJOURNAL>");
                Tally_Edit.WriteLine("  <HASDISCOUNTS>No</HASDISCOUNTS>");
                Tally_Edit.WriteLine("  <ASPAYSLIP>No</ASPAYSLIP>");
                Tally_Edit.WriteLine("  <ISDELETED>No</ISDELETED>");
                Tally_Edit.WriteLine("  <ASORIGINAL>No</ASORIGINAL>");
                Tally_Edit.WriteLine("  <INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  </INVOICEINDENTLIST.LIST>");
                Tally_Edit.WriteLine("  <UDF:HARYANAVAT.LIST DESC=!`HARYANAVAT`!>".Replace('!', '"'));
                Tally_Edit.WriteLine("   <UDF:HARVCHSUBCLASS.LIST DESC=!`HARVCHSUBCLASS`! ISLIST=!YES!>".Replace('!', '"'));
                Tally_Edit.WriteLine("     <UDF:HARVCHSUBCLASS DESC=!`HARVCHSUBCLASS`!>Others</UDF:HARVCHSUBCLASS>".Replace('!', '"'));
                Tally_Edit.WriteLine("   </UDF:HARVCHSUBCLASS.LIST>");
                Tally_Edit.WriteLine("  </UDF:HARYANAVAT.LIST>");
                Purchase_ALLLEDGERENTRIES(VchNo.ToString());
                Tally_Edit.WriteLine(" </VOUCHER>");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void Purchase_ALLLEDGERENTRIES(String VCHNO)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Amount = String.Empty;
            try
            {
                Load_Data("select e1.toby, c1.prtclr Ledger, e1.debit, c2.prtclr, e1.credit from ent0896 e1 left join ctb0896 c1 on e1.ledcode = c1.ledcode left join ctb0896 c2 on e1.ledger = c2.ledcode where mode = 6 and e1.vno = " + VCHNO, ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["toby"].ToString() == "TO")
                    {
                        Amount = "-" + Dt.Rows[i]["DEBIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>No</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                    else
                    {
                        Amount = Dt.Rows[i]["CREDIT"].ToString();
                        Tally_Edit.WriteLine("  <ALLLEDGERENTRIES.LIST>");
                        Tally_Edit.WriteLine("   <LEDGERNAME>" + Dt.Rows[i]["Ledger"].ToString().Replace("&", "&amp;") + "</LEDGERNAME>");
                        Tally_Edit.WriteLine("   <GSTCLASS/>");
                        Tally_Edit.WriteLine("   <ISDEEMEDPOSITIVE>No</ISDEEMEDPOSITIVE>");
                        Tally_Edit.WriteLine("   <LEDGERFROMITEM>No</LEDGERFROMITEM>");
                        Tally_Edit.WriteLine("   <REMOVEZEROENTRIES>No</REMOVEZEROENTRIES>");
                        Tally_Edit.WriteLine("   <ISPARTYLEDGER>Yes</ISPARTYLEDGER>");
                        Tally_Edit.WriteLine("   <AMOUNT>" + Amount + "</AMOUNT>");
                        Tally_Edit.WriteLine("  </ALLLEDGERENTRIES.LIST>");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        public void Tally_Upload_Voucher_Table(String FilePath)
        {
            String Str = String.Empty;
            Boolean Fini = false;
            String Tag = String.Empty;
            String Ledger = String.Empty;
            String Amount = String.Empty;
            try
            {
                FileInfo F1 = new FileInfo(FilePath);
                if (F1.Exists)
                {
                    if (F1.Extension.ToUpper() == ".XML")
                    {
                        if (Check_Table ("Voucher_XML") == false)
                        {
                            Execute("create table Voucher_XML(VchDate varchar(15), RemoteID Varchar(1000), VchType varchar(20), vchNumber varchar(10), PartyLedger Varchar(1000), Ledger varchar(1000), Amount varchar(15), Amount1 varchar(15))");
                        }
                        XmlTextReader X = new XmlTextReader(FilePath);
                        while (X.Read())
                        {
                            if (Tag == "DATE")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    Str = "Insert into Voucher_XML values ('" + X.Value + "',";                                    
                                }
                            }
                            else if (Tag == "GUID")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    Str += "'" + X.Value + "',";
                                }
                            }
                            else if (Tag == "VOUCHERNUMBER")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    Str += "'" + X.Value + "',";
                                }
                            }
                            else if (Tag == "VOUCHERTYPENAME")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    Str += "'" + X.Value + "',";
                                }
                            }
                            else if (Tag == "PARTYLEDGERNAME")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    Str += "'" + X.Value + "',";
                                }
                            }
                            else if (Tag == "LEDGERNAME")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    if (Ledger == String.Empty)
                                    {
                                        Ledger = X.Value;
                                        Str += "'" + X.Value + "',";
                                    }
                                    else
                                    {
                                        Ledger = string.Empty;
                                    }
                                }
                            }
                            else if (Tag == "AMOUNT")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    if (Amount == string.Empty)
                                    {
                                        Amount = X.Value;
                                        if (Convert.ToDouble(X.Value) > 0)
                                        {
                                            Str += "'" + X.Value + "','-" + X.Value + "')";
                                        }
                                        else
                                        {
                                            Str += "'" + X.Value + "','" + X.Value.Replace("-",String.Empty) + "')";
                                        }
                                    }
                                    else
                                    {
                                        Amount = String.Empty;
                                    }
                                }
                                Fini = true;
                            }
                            Tag = X.Name;
                            if (Fini)
                            {
                                if (Str != String.Empty)
                                {
                                    Execute(Str);
                                }
                                Fini = false;
                                Str = String.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Tally_Upload_Group_Table(String FilePath)
        {
            String Str = String.Empty;
            Boolean Fini = false;
            String Tag = String.Empty;
            String Ledger = String.Empty;
            String Amount = String.Empty;
            try
            {
                FileInfo F1 = new FileInfo(FilePath);
                if (F1.Exists)
                {
                    if (F1.Extension.ToUpper() == ".XML")
                    {
                        if (Check_Table("Group_XML") == false)
                        {
                            Execute("create table Group_XMl(parent varchar(100), GroupName varchar(100))");
                        }
                        XmlTextReader X = new XmlTextReader(FilePath);
                        while (X.Read())
                        {
                            if (Tag == "PARENT")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    Str = "Insert into GROUP_XML values ('" + X.Value + "',";
                                }
                                else
                                {
                                    if (Str == String.Empty)
                                    {
                                        Str = "Insert into GROUP_XML values (NULL,";
                                    }
                                }
                            }
                            else if (Tag == "NAME")
                            {
                                if (X.Value.Trim() != "\r\n  ".Trim())
                                {
                                    if (Str != String.Empty)
                                    {
                                        Str += "'" + X.Value + "')";
                                        Fini = true;
                                    }
                                }
                            }
                            Tag = X.Name;
                            if (Fini)
                            {
                                if (Str != String.Empty)
                                {
                                    Execute(Str);
                                }
                                Fini = false;
                                Str = String.Empty;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Datasource(Int32 Company_code, String Server_Address)
        {
            try
            {
                return String.Format("{0:000}", (Company_code + 28)) + Ascii(Server_Address);
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public Int32 Get_Company_Code_From_datasource(String Source)
        {
            try
            {
                return Convert.ToInt32(Source.Substring(0, 3)) - 28;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public String Get_Source_From_datasource(String Source)
        {
            try
            {
                return Ascii_Reverse (Source.Substring(3));
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public String Ascii(String Term)
        {
            try
            {
                String Str = String.Empty;
                foreach (Char C in Term)
                {
                    if (Str == String.Empty)
                    {
                        Str = String.Format("{0:000}", Convert.ToInt32(C));
                    }
                    else
                    {
                        Str += String.Format("{0:000}", Convert.ToInt32(C));
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Connection_Ascii(String Term)
        {
            try
            {
                String Str = String.Empty;
                foreach (Char C in Term)
                {
                    if (Str == String.Empty)
                    {
                        Str = String.Format("{0:000}", Convert.ToInt32(C) + 28);
                    }
                    else
                    {
                        Str += String.Format("{0:000}", Convert.ToInt32(C) + 28);
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean CheckInternetConnection()
        {
            try
            {
                String Str = Dns.GetHostByName("www.google.com").HostName;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean SendMail(String FromId, String SmtpAccountName, String smtpPwd, String FromName, String SMTPAddress, String SubJect, String Body, String CC, String Bcc, String ToID, String Term, Int64 Code, DateTime Date, params String[] AttachmentFilePath)
        {
            String A = "N";
            String Mode = string.Empty;
            try
            {
                System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage();
                Email.From = new MailAddress(FromId, FromName);
                Email.Sender = new MailAddress(FromId, FromName);
                Email.ReplyTo = new MailAddress(FromId, FromName);
                if (CC != String.Empty)
                {
                    MailAddress M1 = new MailAddress(CC, FromName);
                    Email.CC.Add(M1);
                }
                if (Bcc != String.Empty)
                {
                    MailAddress M2 = new MailAddress(Bcc, FromName);
                    Email.Bcc.Add(M2);
                }
                Email.To.Add(ToID);
                Email.Subject = SubJect;
                Email.Body = Body;
                foreach (String Str in AttachmentFilePath)
                {
                    if (File.Exists(Str))
                    {
                        Attachment Att = new Attachment(Str);
                        Email.Attachments.Add(Att);
                    }
                }
                SmtpClient Client = new SmtpClient();
                NetworkCredential Crd = new NetworkCredential();
                Crd.UserName = SmtpAccountName;
                Crd.Password = smtpPwd;
                Client.Credentials = Crd;
                Client.Port = 25;
                Client.Host = SMTPAddress;
                //Client.EnableSsl = true;
                Client.Send(Email);
                if (AttachmentFilePath.Length > 0)
                {
                    A = "Y";
                    if (AttachmentFilePath[0].ToString().ToUpper().Contains(".TXT"))
                    {
                        Mode = "N";
                    }
                    else
                    {
                        Mode = "P";
                    }
                }
                else
                {
                    Mode = "-"; 
                }
                Insert_Mail_Table(FromId, ToID, CC, Bcc, SubJect, A, Body, Term, Code, Date, Mode);
                Email = null;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public String Run_UpdateExe(String FileName)
        {
            String Command;
            try
            {
                FileInfo F1 = new FileInfo(FileName);
                if (F1.Exists == false)
                {
                    MessageBox.Show("Update Application Not Available", "Vaahini", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return "Exe";
                }
                Process.Start(FileName);
                return String.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_Exe(String FileName)
        {
            try
            {
                if (FileName.ToUpper().Contains(".EXE") == false)
                {
                    FileName = FileName + ".EXE";
                }
                DirectoryInfo DI = new DirectoryInfo(@"\\SERVER\VAAHINIEXE");
                if (DI.Exists == false)
                {
                    MessageBox.Show("Please Check SERVER Path ...!");
                    return false;
                }
                DirectoryInfo DI1 = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
                FileInfo[] FI = DI.GetFiles("*.EXE");
                FileInfo[] FI1 = DI1.GetFiles("*.EXE");
                DateTime CExe = DateTime.Now;
                foreach (FileInfo f in FI1)
                {
                    if (f.Name.ToUpper() == FileName.ToUpper())
                    {
                        CExe = Convert.ToDateTime(f.LastWriteTime);
                    }
                }
                foreach (FileInfo f in FI)
                {
                    if (f.Name.ToUpper() == FileName.ToUpper())
                    {
                        if (Convert.ToDateTime(f.LastWriteTime) <= CExe)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return true;
            }
        }

        public void Execute_Tbl(String Sql, String TblName)
        {
            try
            {
                Drop(TblName, "Table");
                Drop(TblName, "View");
                Cn_Open();
                if (Sql.Contains("From"))
                {
                    Sql = Sql.Replace("From", " into " + TblName + " From ");
                }
                else if (Sql.Contains("from"))
                {
                    Sql = Sql.Replace("from", " into " + TblName + " From ");
                }
                else if (Sql.Contains("FROM"))
                {
                    Sql = Sql.Replace("FROM", " into " + TblName + " From ");
                }
                OdbcCommand Cmd2 = new OdbcCommand(Sql, Cn);
                Cmd2.CommandTimeout = 800;
                Cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        String ValiDate_Filename(String Filename)
        {
            String T = string.Empty;
            try
            {
                T = Filename.Substring(Filename.Length - 1, 1);
                if (Convert.ToInt16(T) > 0)
                {
                    DBF_CCode = T;
                    DBF_Year = Filename.Substring(Filename.Length - 3, 2);
                    return Filename.Substring(0, Filename.Length - 3);
                }
                else
                {
                    return Filename;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Input string was not in a correct format.")
                {
                    return Filename;
                }
                else
                {
                    throw ex;
                }
            }
        }

        public Int64 Get_RecordCount_View(String ObjName, String Condition)
        {
            try
            {
                Int64 Count = 0;
                if (Check_View(ObjName) == true)
                {
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand();
                    if (Condition.Trim() == string.Empty)
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper(), Cn);
                    }
                    else
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper() + " where " + Condition, Cn);
                    }
                    Cmd.CommandTimeout = 600;
                    Count = Convert.ToInt64(Cmd.ExecuteScalar());
                }
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void UpdateSpecialFields(String Tblname)
        {
            try
            {
                if (Check_Table(Tblname))
                {
                    Add_NewField(Tblname, "New_empCode", "Numeric(4)");
                    Add_NewField(Tblname, "New_SysCode", "Numeric(4)");
                    Add_NewField(Tblname, "New_DateTime", "DateTime");
                    Add_NewField(Tblname, "Alter_empCode", "Numeric(4)");
                    Add_NewField(Tblname, "Alter_SysCode", "Numeric(4)");
                    Add_NewField(Tblname, "Alter_DateTime", "DateTime");
                    Add_NewField(Tblname, "Company_Code", "Numeric(2)");
                    Add_NewField(Tblname, "Year_Code", "Varchar(10)");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateSpecialFields_Inventory(String Tblname)
        {
            try
            {
                if (Check_Table(Tblname))
                {
                    Add_NewField(Tblname, "Alter_empCode", "Numeric(4)");
                    Add_NewField(Tblname, "Alter_SysCode", "Numeric(4)");
                    Add_NewField(Tblname, "Alter_DateTime", "DateTime");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void UpdateSpecialFields_Two(String Tblname)
        {
            try
            {
                if (Check_Table(Tblname))
                {
                    Add_NewField(Tblname, "Company_Code", "Numeric(2)");
                    Add_NewField(Tblname, "Year_Code", "Varchar(10)");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Boolean CreateTableFromDataTable(ref System.Data.DataTable Dt, String Filename, Boolean ABS)
        {
            Boolean Flag = false;
            String Str = string.Empty, Str1 = string.Empty;
            String ColName = string.Empty;
            try
            {
                foreach (DataColumn DC in Dt.Columns)
                {
                    ColName = DC.ColumnName;
                    if (ColName.ToUpper() == "GROUP" || ColName.ToUpper() == "UNIT" || ColName.ToUpper() == "DESC")
                    {
                        ColName = ColName + "1";
                    }
                    if (Str == string.Empty)
                    {
                        Str = ColName + " ";
                    }
                    else
                    {
                        Str += ", " + ColName + " ";
                    }
                    if (DC.DataType == Type.GetType("System.String"))
                    {
                        if (DC.ColumnName.ToUpper() == "ITEMDETL")
                        {
                            Str += "ntext";
                        }
                        else
                        {
                            Str += "Varchar(200)";
                        }   
                    }
                    else if (DC.DataType == Type.GetType("System.Boolean"))
                    {
                        Str += "Varchar(10)";
                    }
                    else if (DC.DataType == Type.GetType("System.Decimal"))
                    {
                        if (ABS == false)
                        {
                            Str += "Varchar(20)";
                        }
                        else
                        {
                            if (DC.ColumnName.ToUpper().Contains("CREDIT") || DC.ColumnName.ToUpper().Contains("DEBIT"))
                            {
                                Str += "Numeric(15,2)";
                            }
                            else
                            {
                                Str += "Numeric(10,2)";
                            }
                        }
                    }
                    else if (DC.DataType == Type.GetType("System.Int32"))
                    {
                        if (ABS == false)
                        {
                            Str += "Varchar(20)";
                        }
                        else
                        {
                            Str += "int";
                        }
                    }
                    else if (DC.DataType == Type.GetType("System.Int16"))
                    {
                        if (ABS == false)
                        {
                            Str += "Varchar(20)";
                        }
                        else
                        {
                            Str += "int";
                        }
                    }
                    else if (DC.DataType == Type.GetType("System.Int64"))
                    {
                        if (ABS == false)
                        {
                            Str += "Varchar(20)";
                        }
                        else
                        {
                            Str += "Numeric(10)";
                        }
                    }
                    else if (DC.DataType == Type.GetType("System.DateTime"))
                    {
                        Str += "Datetime";
                    }
                    else if (DC.DataType == Type.GetType("System.Double"))
                    {
                        if (ABS == false)
                        {
                            Str += "Varchar(20)";
                        }
                        else
                        {
                            Str += "NUmeric(10,2)";
                        }
                    }
                }
                Str1 = "create table " + Filename + " (" + Str + ")";
                Execute(Str1);
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void CreaTblStruct_InstRows_FromDBF(String Filename, String Tblname)
        {
            String[] Queries;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data_BackupCn("Select * from " + Filename, ref Dt);
                if (Check_Table(Tblname) == false)
                {
                    CreateTableFromDataTable(ref Dt, Tblname, true);
                }
                Insert_DBFRecords_SqlTable(out Queries, ref Dt, Tblname);
                Run(Queries);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
                
        public void UpdateDBF(String FileName)
        {
            String TblName = string.Empty;
            String OrgFilename = FileName;
            try
            {
                TblName = OrgFilename;
                if (TblName.ToUpper() == "UNIT")
                {
                    TblName = "UOM";
                }
                if (Check_Table(TblName) == false || Get_RecordCount(TblName, String.Empty) == 0)
                {
                    CreaTblStruct_InstRows_FromDBF(OrgFilename, TblName);
                    //Execute("Insert into " + OrgFilename + " select * from openrowset('MSDASQL','DRIVER=Microsoft Visual Foxpro Driver;sourcedb=" + System.Windows.Forms.Application.StartupPath + ";sourcetype=DBF','select * from " + OrgFilename + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadOraDBName()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn_Open();
                OdbcDataAdapter adp = new OdbcDataAdapter(new OdbcCommand ("Select * from report_Details",Cn));
                adp.Fill (Dt);
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show ("Check Report Details ...!","Vaahini");
                }
                else
                {
                    OraDBName = Dt.Rows[0]["DataBase"].ToString();
                    if (OraDBName.Trim() == String.Empty)
                    {
                        MessageBox.Show("Check Report Details ...!", "Vaahini");
                    }
                }
                Cn_Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Connection_Ascii_Reverse(String AsciiTerm)
        {
            String Str = String.Empty;
            try
            {
                for (int i = 0; i <= AsciiTerm.Length - 1; i += 3)
                {
                    if (Str == String.Empty)
                    {
                        Str = Convert.ToString(Convert.ToChar(Convert.ToInt32(AsciiTerm.Substring(i, 3)) - 28));
                    }
                    else
                    {
                        Str += Convert.ToString(Convert.ToChar(Convert.ToInt32(AsciiTerm.Substring(i, 3)) - 28));
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String Ascii_Reverse(String AsciiTerm)
        {
            String Str = String.Empty;
            try
            {
                for (int i = 0; i <= AsciiTerm.Length - 1; i += 3)
                {
                    if (Str == String.Empty)
                    {
                        Str = Convert.ToString(Convert.ToChar(Convert.ToInt32(AsciiTerm.Substring(i, 3))));
                    }
                    else
                    {
                        Str += Convert.ToString(Convert.ToChar(Convert.ToInt32(AsciiTerm.Substring(i, 3))));
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BackupCn_Execute_Statement(String Query)
        {
            try
            {
                BackupCn_Open();
                OdbcCommand Cmd = new OdbcCommand (Query, BackupCn);
                Cmd.ExecuteNonQuery();
                BackupCn_Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DBFCn_Execute_Statement(String Query)
        {
            try
            {
                DBFCn_Open();
                OdbcCommand Cmd = new OdbcCommand(Query, DBFCn);
                Cmd.ExecuteNonQuery();
                DBFCn_Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean DBFCn_Check_Table(String Tblname)
        {
            System.Data.DataTable Chk = new System.Data.DataTable();
            try
            {
                DBFCn_Open();
                OdbcDataAdapter adp = new OdbcDataAdapter(new OdbcCommand("Select * from sysobjects where name = '" + Tblname + "' and Xtype = 'U'", DBFCn));
                adp.Fill(Chk);
                if (Chk.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean BackupCn_Check_Table(String Tblname)
        {
            System.Data.DataTable Chk = new System.Data.DataTable();
            try
            {
                BackupCn_Open();
                OdbcDataAdapter adp = new OdbcDataAdapter(new OdbcCommand("Select * from sysobjects where name = '" + Tblname + "' and Xtype = 'U'", BackupCn));
                adp.Fill(Chk);
                if (Chk.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String View_Details(String TblName, String Condition)
        {
            String Val;
            String User;
            String System;
            DateTime Dat;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                //Val = "Select New_EmpCode, New_Syscode, New_Datetime from " + TblName + " where " + Condition;
                Val = "Select Alter_EmpCode, Alter_Syscode, Alter_Datetime from " + TblName + " where " + Condition;
                Load_Data(Val, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    User = Dt.Rows[0]["Alter_Empcode"].ToString();
                    System = Dt.Rows[0]["Alter_Syscode"].ToString();
                    if (Dt.Rows[0]["Alter_Datetime"] != DBNull.Value)
                    {
                        Dat = Convert.ToDateTime(Dt.Rows[0]["alter_Datetime"]);
                    }
                    else
                    {
                        Dat =Convert.ToDateTime("01/01/1899");
                    }
                    if (User != String.Empty)
                    {
                        User = GetData_InString("Socks_User_Master", "User_Code", User, "User_Name");
                    }
                    if (System != String.Empty)
                    {
                        System = GetData_InString("Sys_Master", "Sys_Code", System, "Sys_Name");
                    }
                }
                else
                {
                    User = "---";
                    System = "---";
                    Dat = Convert.ToDateTime("01/01/1899");
                }
                return " ("+ User + "@" + System + "  " + Dat + ")";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsRunningMaterialDir(String ItemCode)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn_Open();
                if (Check_Table("RM_IDs") == false)
                {
                    //Execute("Create table RM_IDs as select Item_ID from Item_Master where item_description like 'DRESS%'");
                    Execute("Create table RM_IDs as select Item_ID from Item_Master where Type = 'R'");
                }
                if (Check_Table("RM_IDsT") == false)
                {
                    Execute("Create table RM_IDsT as select * from RM_IDs");
                }
                OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand("select * from RM_IDsT where item_ID in (select item_id from GSN_Acceptance_Details where item_No = '" + ItemCode + "')", Cn));
                Adp.Fill(Dt);
                if (Dt.Rows.Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsRunningMaterial(String ItemCode)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn_Open();
                if (Check_Table("RM_IDs") == false)
                {
                    //Execute("Create table RM_IDs as select Item_ID from Item_Master where item_description like 'DRESS%'");
                    Execute("Create table RM_IDs as select Item_ID from Item_Master where type = 'R'");
                }
                OdbcDataAdapter Adp = new OdbcDataAdapter (new OdbcCommand ("select * from RM_Ids where item_ID in (Select item_ID from gsn_acceptance_details where item_No = '" + ItemCode + "')", Cn));
                Adp.Fill (Dt);
                if (Dt.Rows.Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsRunningMaterialItemID(String ItemID)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn_Open();
                if (Check_Table("RM_IDs") == false)
                {
                    Execute("Create table RM_IDs as select Item_ID from Item_Master where item_description like 'DRESS%'");
                }
                OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand("select * from RM_Ids where item_ID = '" + ItemID + "'", Cn));
                Adp.Fill(Dt);
                if (Dt.Rows.Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsRunningMaterialOld(String ItemCode)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn_Open();
                OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand("select * from RM_Ids where item_ID in (select itemId item_id from OldPurchaseItems where itemCode = '" + ItemCode + "')", Cn));
                Adp.Fill(Dt);
                if (Dt.Rows.Count == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_Exe(String FileName, String Source)
        {
            try
            {
                if (FileName.ToUpper().Contains(".EXE") == false)
                {
                    FileName = FileName + ".EXE";
                }
                DirectoryInfo DI = new DirectoryInfo(Source);
                if (DI.Exists == false)
                {
                    MessageBox.Show("Please Check SERVER Path ...!");
                    return false;
                }
                DirectoryInfo DI1 = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
                FileInfo[] FI = DI.GetFiles("*.EXE");
                FileInfo[] FI1 = DI1.GetFiles("*.EXE");
                DateTime CExe = DateTime.Now;
                foreach (FileInfo f in FI1)
                {
                    if (f.Name.ToUpper() == FileName.ToUpper())
                    {
                        CExe = Convert.ToDateTime(f.LastWriteTime);
                    }
                }
                foreach (FileInfo f in FI)
                {
                    if (f.Name.ToUpper() == FileName.ToUpper())
                    {
                        if (Convert.ToDateTime(f.LastWriteTime) <= CExe)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean Check_Table_TAB(String TblName)
        {
            try
            {
                Boolean Flag;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("select * from sysobjects where xtype = 'U' and name = '" + TblName.ToUpper() + "'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Check_Trigger(String TriggerName)
        {
            try
            {
                Boolean Flag;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("select * from sysobjects where xtype = 'TR' and name = '" + TriggerName.ToUpper() + "'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Check_Function(String Function_Name)
        {
            try
            {
                Boolean Flag;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("select * from sysobjects where ((xtype = 'FN') or (xtype = 'TF') or (xtype = 'IF')) and name = '" + Function_Name.ToUpper() + "'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public Boolean Check_Procedure(String Procedure_Name)
        {
            try
            {
                Boolean Flag;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("select * from sysobjects where xtype = 'P' and name = '" + Procedure_Name.ToUpper() + "'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        void Year()
        {
            try
            {
                if (Check_Table("Year_Master") == false)
                {
                    Execute("Create Table Year_Master (Year varchar(10))");
                }
                if (Convert.ToInt32(String.Format("{0:MM}", DateTime.Now)) < 4)
                {
                    YearCode = Convert.ToString(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)) - 1) + "-" + String.Format("{0:yyyy}", DateTime.Now);
                }
                else
                {
                    YearCode = String.Format("{0:yyyy}", DateTime.Now) + "-" + Convert.ToString(Convert.ToInt32(String.Format("{0:yyyy}", DateTime.Now)) + 1);
                }
                //YearCode = "2009-2010";
                if (Get_RecordCount("Year_master", "Year = '" + YearCode + "'") == 0)
                {
                    Execute("Insert into Year_Master values ('" + YearCode + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Double Max(String TblName, String FldName, String Condition, String Year_Code, Int32 ComPCode)
        {
            try
            {
                Double Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where Year_Code = '" + Year_Code + "' and Company_Code = " + ComPCode, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition + " And Year_Code = '" + Year_Code + "' and Company_Code = " + ComPCode, Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
                }
                else
                {
                    Value = 1;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Double MaxOnlyComp(String TblName, String FldName, String Condition, String Year_Code, Int32 ComPCode)
        {
            try
            {
                Double Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where Company_Code = " + ComPCode, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition + " And Company_Code = " + ComPCode, Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
                }
                else
                {
                    Value = 1;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Double MaxOnlyWithoutComp(String TblName, String FldName, String Condition, String Year_Code, Int32 ComPCode)
        {
            try
            {
                Double Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty && Year_Code.Trim() == String.Empty && Convert.ToInt16(ComPCode) == 0)
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition + " And Company_Code = " + ComPCode, Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
                }
                else
                {
                    Value = 1;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Rate_Details_Item(String ItemCode)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select p1.selling_rate rate, M1. discount from product_master p1 left join Manufacturer_master m1 on p1.Manuf_Code = m1.Manuf_code where item_code = '" + ItemCode + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    Rate_Item_Rate = (Convert.ToDouble(Dt.Rows[0]["rate"]) - (Convert.ToDouble(Dt.Rows[0]["rate"]) / 100 * Convert.ToDouble(Dt.Rows[0]["Discount"])));
                    return " *** [" + PadL(String.Format("{0:0.00}", Convert.ToDouble(Dt.Rows[0]["rate"])), 10) + "  /  " + PadR(string.Format("{0:0.00 %}", Convert.ToDouble(Dt.Rows[0]["Discount"])), 6) + "] ***";
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime MaxDate(String TblName, String FldName, String Condition)
        {
            try
            {
                DateTime Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition, Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDateTime(Cmd.ExecuteScalar());
                }
                else
                {
                    Value = Convert.ToDateTime("01/01/1899");
                }
                return Value.AddDays (-1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }
        
        public Boolean IsTableLocked(String TblName)
        {
            String ObjID;
            try
            {
                ObjID = GetData_InString("Dba_Objects", "Object_Name", TblName.ToUpper(), "Object_ID");
                if (ObjID == String.Empty)
                {
                    MessageBox.Show("Invalid Table Name on IsTableLocked ...");
                    return true;
                }
                else
                {
                    return false;
                }
                //if (Get_RecordCount ("VLock", "Name = '" + TblName.ToUpper() + "'") > 0)
                //if (GetData_InNumber("Vlock","Name",TblName.ToUpper(), "lock_Status") == 1)
                //{
                 //   return true;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Lock_Table(string TblName)
        {
            try
            {
                Execute("update vlock set lock_status = 1 where Name = '" + TblName.ToUpper() + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UNLock_Table(String TblName)
        {
            try
            {
                try
                {
                    Execute("update vlock set lock_status = 0 where Name = '" + TblName.ToUpper() + "'");
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Wait(String TblName)
        {
            try
            {
                while (IsTableLocked("Socks_User_Master") == true)
                {
                }
                return;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Double Min(String TblName, String FldName, String Condition)
        {
            try
            {
                Double Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Min(" + FldName + ") from " + TblName + " where Year_Code = '" + YearCode + "'", Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Min(" + FldName + ") from " + TblName + " where " + Condition + " And Year_Code = '" + YearCode + "'", Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDouble(Cmd.ExecuteScalar());
                    if (Value == 0 || Value == 1)
                    {
                        Value = -1;
                    }
                    else
                    {
                        Value = Value - 1;
                    }
                }
                else
                {
                    Value = -1;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Double MaxWOCC(String TblName, String FldName, String Condition)
        {
            try
            {
                Double Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " Where " + Condition, Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
                }
                else
                {
                    Value = 1;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Double Max(String TblName, String FldName, String Condition, Boolean Increment)
        {
            try
            {
                Double Value;
                Year();
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where Year_Code = '" + YearCode + "'", Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition + " And Year_Code = '" + YearCode + "'", Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    if (Increment == true)
                    {
                        Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
                    }
                    else
                    {
                        Value = Convert.ToDouble(Cmd.ExecuteScalar());
                    }
                }
                else
                {
                    if (Increment == true)
                    {
                        Value = 1;
                    }
                    else
                    {
                        Value = 0;
                    }
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public System.Data.DataTable Copy_DataTable(out System.Data.DataTable Dt1, ref System.Data.DataTable Org, params String[] WithoutFields)
        {
            try
            {
                Dt1 = new System.Data.DataTable();
                Dt1 = Org.Copy();
                foreach (String Str in WithoutFields)
                {
                    Dt1.Columns.Remove(Str);
                }
                return Dt1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ZExportToExcel(ref System.Data.DataTable Dt1, String FilePath, String ColA, String Colb, Boolean Suppress, params String[] WithoutFlds)
        {
            System.Data.DataTable Dt;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Add(Missing);
                WSheet = (Excel.Worksheet)WBook.ActiveSheet;

                Copy_DataTable(out Dt, ref Dt1, WithoutFlds);
                for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                {
                    WSheet.Cells[1, j + 1] = Dt.Columns[j].ColumnName;
                }
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                    {
                        if (Suppress == true)
                        {
                            if (Dt.Rows[i][j].ToString() == "0" || Dt.Rows[i][j].ToString() == "0.00" || Dt.Rows[i][j].ToString() == "0.0" || Dt.Rows[i][j].ToString() == "-")
                            {
                                WSheet.Cells[i + 3, j + 1] = String.Empty;
                            }
                            else
                            {
                                if (Dt.Rows[i][j].ToString().Contains("`"))
                                {
                                    WSheet.Cells[i + 3, j + 1] = Dt.Rows[i][j].ToString().Replace("`","'");
                                }
                                else
                                {
                                    WSheet.Cells[i + 3, j + 1] = Dt.Rows[i][j];
                                }
                            }
                        }
                        else
                        {
                            if (Dt.Rows[i][j].ToString().Contains("`"))
                            {
                                WSheet.Cells[i + 3, j + 1] = Dt.Rows[i][j].ToString().Replace("`","'");
                            }
                            else
                            {
                                WSheet.Cells[i + 3, j + 1] = Dt.Rows[i][j];
                            }
                        }
                    }
                }
                WSheet.get_Range(ColA + "1", Colb + "1").Font.Bold = true;
                WSheet.get_Range(ColA + "1", Colb + "1").EntireColumn.AutoFit();
                ColA = ColA + "1:" + ColA + Convert.ToInt32(Dt.Rows.Count + 2).ToString();
                Colb = Colb + "1:" + Colb + Convert.ToInt32(Dt.Rows.Count + 2).ToString();
                WSheet.get_Range(ColA, Colb).Borders.Value = 7;
                System.IO.File.Delete(FilePath);
                WBook.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing, Excel.XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                WBook.Close(true, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Read_TextFile(String FileName)
        {
            RichTextBox Rt = new RichTextBox();
            try
            {
                Rt.LoadFile(FileName, RichTextBoxStreamType.PlainText);
                return Rt.Text;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 Get_First_Visible_Column(ref DataGridView DGV)
        {
            try
            {
                for (int i = 0; i <= DGV.Columns.Count - 1; i++)
                {
                    if (DGV.Columns[i].Visible == true)
                    {
                        return i;
                    }
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Int32 Get_row(Char Ch, ref DataGridView DGV)
        {
            Int32 ColName = Get_First_Visible_Column(ref DGV);
            Int32 MinRow = -1;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (DGV[ColName, i].Value.ToString().Substring(0, 1).ToUpper() == Ch.ToString().ToUpper())
                    {
                        if (MinRow == -1)
                        {
                            MinRow = i;
                        }
                        if (DGV.CurrentCell.RowIndex < i)
                        {
                            return i;
                        }
                    }
                }
                return MinRow;
            }
            catch (Exception ex)
            {
                return MinRow;
            }
        }


        public void ExportToExcel(ref System.Data.DataTable Dt, String FilePath)
        {
            String Str = String.Empty;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Add(Missing);
                WSheet = (Excel.Worksheet)WBook.ActiveSheet;

                for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                {
                    WSheet.Cells[1, j + 1] = Dt.Columns[j].ColumnName;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                    {
                        if (Dt.Columns[j].DataType.ToString().ToUpper().Contains("DATE"))
                        {
                            if (Dt.Rows[i][j] == null || Dt.Rows[i][j] == DBNull.Value)
                            {
                                Str = "";
                            }
                            else
                            {
                                Str = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i][j]));
                            }
                        }
                        else
                        {
                            Str = Dt.Rows[i][j].ToString();
                        }
                        //WSheet.Cells[i + 3, j + 1] = Dt.Rows[i][j];
                        WSheet.Cells[i + 3, j + 1] = Str;
                    }
                }
                WSheet.get_Range("A1", "Z1").Font.Bold = true;
                WSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
                System.IO.File.Delete(FilePath);
                WBook.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing, Excel.XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                WBook.Close(true, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        System.Data.DataTable Copy_DT_Visible_Col_Only(ref DataGridView DGV, System.Data.DataTable Dt)
        {
            System.Data.DataTable Dt1 = Dt.Copy();
            try
            {
                for (int i = Dt.Columns.Count - 1; i >= 0; i--)
                {
                    if (DGV.Columns[i].Visible == false)
                    {
                        Dt1.Columns.RemoveAt(i);
                        //Dt1 = Copy_DT_Visible_Col_Only(ref DGV, Dt1);
                    }
                }
                return Dt1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ZExportToExcel_VisibleOnly(ref DataGridView DGV, ref System.Data.DataTable Dt1, String FilePath)
        {
            String Str = String.Empty;
            System.Data.DataTable Dt;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Add(Missing);
                WSheet = (Excel.Worksheet)WBook.ActiveSheet;

                Dt = Copy_DT_Visible_Col_Only(ref DGV, Dt1);

                for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                {
                    WSheet.Cells[1, j + 1] = Dt.Columns[j].ColumnName;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                    {

                        if (Dt.Columns[j].DataType.ToString().ToUpper().Contains("DATE"))
                        {
                            if (Dt.Rows[i][j] != null && Dt.Rows[i][j] != DBNull.Value)
                            {
                                Str = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i][j]));
                            }
                            else
                            {
                                Str = String.Empty;
                            }
                        }
                        else
                        {
                            Str = Dt.Rows[i][j].ToString();
                        }

                        WSheet.Cells[i + 3, j + 1] = Str;
                    }
                }
                WSheet.get_Range("A1", "Z1").Font.Bold = true;
                WSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();
                System.IO.File.Delete(FilePath);
                WBook.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing, Excel.XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                WBook.Close(true, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean IsGrid_VGrid(ref DataGridView DGV)
        {
            try
            {
                if (DGV.Rows.Count < 2)
                {
                    return false;
                }
                else
                {
                    if (DGV.Rows[DGV.Rows.Count - 1].DefaultCellStyle.BackColor == System.Drawing.Color.White && DGV.Rows[DGV.Rows.Count - 2].DefaultCellStyle.BackColor == SystemColors.Control)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.DataTable V_DataTable(ref System.Data.DataTable Dt)
        {
            Boolean Flag = false;
            System.Data.DataTable NewDt;
            try
            {
                NewDt = Dt.Copy();
                foreach (DataColumn DC in NewDt.Columns)
                {
                    //if (Convert.ToString(DC.DataType) == "System.Double" || Convert.ToString(DC.DataType) == "System.Int32" || Convert.ToString(DC.DataType) == "System.Decimal" && Dt.Columns[DC.ColumnName].Ordinal != 0)
                    if (Convert.ToString(DC.DataType) == "System.Double" || Convert.ToString(DC.DataType) == "System.Int32" || Convert.ToString(DC.DataType) == "System.Decimal")
                    {
                        //if (DC.ColumnName.ToUpper() != "BILLNO" && DC.ColumnName.ToUpper() != "MBILL" && DC.ColumnName.ToUpper() != "BILL_NO" && DC.ColumnName.ToUpper() != "LOCATION" && DC.ColumnName.ToUpper() != "MINNO" && DC.ColumnName.ToUpper() != "MAXNO" && DC.ColumnName.ToUpper() != "GSNNO" && DC.ColumnName.ToUpper() != "ACCNO" && DC.ColumnName.ToUpper() != "SLNO" && DC.ColumnName.ToUpper() != "TAX_PER" && DC.ColumnName.ToUpper() != "SRNO" && DC.ColumnName.ToUpper() != "DCNO" && DC.ColumnName.ToUpper() != "RECNO" && DC.ColumnName.ToUpper() != "EC" && DC.ColumnName.ToUpper() != "E_NO" && DC.ColumnName.ToUpper() != "SALESQTY %" && DC.ColumnName.ToUpper() != "SALESMTR %" && DC.ColumnName.ToUpper() != "GRN_NO")
                        if (DC.ColumnName.ToUpper() == "DEBIT" || DC.ColumnName.ToUpper() == "CREDIT" || DC.ColumnName.ToUpper() == "PAYMENT" || DC.ColumnName.ToUpper() == "RECEIPT" || DC.ColumnName.ToUpper() == "AMOUNT" || DC.ColumnName.ToUpper().Contains("AMOUNT") || DC.ColumnName.ToUpper().Contains("VALUE"))
                        {
                            Flag = true;
                            break;
                        }
                    }
                }
                if (Flag == true)
                {
                    DataRow Dr = NewDt.NewRow();
                    DataRow Empty = NewDt.NewRow();
                    foreach (DataColumn DC in NewDt.Columns)
                    {
                        Empty[DC] = DBNull.Value;
                    }
                    foreach (DataColumn DC in NewDt.Columns)
                    {
                        if (Convert.ToString(DC.DataType) == "System.Double")
                        {
                            if (DC.ColumnName.ToUpper() != "CLBAL" && DC.ColumnName.ToUpper() != "BILLNO" && DC.ColumnName.ToUpper() != "MBILL" && DC.ColumnName.ToUpper() != "BILL_NO" && DC.ColumnName.ToUpper() != "LOCATION" && DC.ColumnName.ToUpper() != "MINNO" && DC.ColumnName.ToUpper() != "MAXNO" && DC.ColumnName.ToUpper() != "GSNNO" && DC.ColumnName.ToUpper() != "ACCNO" && DC.ColumnName.ToUpper() != "SLNO" && DC.ColumnName.ToUpper() != "TAX_PER" && DC.ColumnName.ToUpper() != "SRNO" && DC.ColumnName.ToUpper() != "DCNO" && DC.ColumnName.ToUpper() != "RECNO" && DC.ColumnName.ToUpper() != "EC" && DC.ColumnName.ToUpper() != "E_NO" && DC.ColumnName.ToUpper() != "SALESQTY %" && DC.ColumnName.ToUpper() != "SALESMTR %" && DC.ColumnName.ToUpper() != "GRN_NO")
                            {
                                Dr[DC] = Convert.ToDouble(Sum(ref NewDt, DC.ColumnName, true));
                            }
                            if (Dr[DC] != DBNull.Value)
                            {
                                if (Convert.ToDouble(Dr[DC]) == 0)
                                {
                                    Dr[DC] = DBNull.Value;
                                }
                            }
                        }
                        else if ((Convert.ToString(DC.DataType) == "System.Decimal" && Dt.Columns[DC.ColumnName].Ordinal != 0) || (Convert.ToString(DC.DataType) == "System.Int32"))
                        {
                            if (DC.ColumnName.ToUpper() != "CLBAL" && DC.ColumnName.ToUpper() != "BILLNO" && DC.ColumnName.ToUpper() != "MBILL" && DC.ColumnName.ToUpper() != "BILL_NO" && DC.ColumnName.ToUpper() != "LOCATION" && DC.ColumnName.ToUpper() != "MINNO" && DC.ColumnName.ToUpper() != "MAXNO" && DC.ColumnName.ToUpper() != "GSNNO" && DC.ColumnName.ToUpper() != "ACCNO" && DC.ColumnName.ToUpper() != "SLNO" && DC.ColumnName.ToUpper() != "TAX_PER" && DC.ColumnName.ToUpper() != "SRNO" && DC.ColumnName.ToUpper() != "DCNO" && DC.ColumnName.ToUpper() != "RECNO" && DC.ColumnName.ToUpper() != "EC" && DC.ColumnName.ToUpper() != "E_NO" && DC.ColumnName.ToUpper() != "SALESQTY %" && DC.ColumnName.ToUpper() != "SALESMTR %" && DC.ColumnName.ToUpper() != "GRN_NO")
                            {
                                Dr[DC] = Convert.ToDouble(Sum(ref NewDt, DC.ColumnName, true));
                            }
                            if (Dr[DC] != DBNull.Value)
                            {
                                if (Convert.ToDouble(Dr[DC]) == 0)
                                {
                                    Dr[DC] = DBNull.Value;
                                }
                            }
                        }
                        else
                        {
                            Dr[DC] = DBNull.Value;
                        }
                    }
                    NewDt.Rows.Add(Empty);
                    NewDt.Rows.Add(Dr);
                }
                return NewDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public System.Data.DataTable V_DataTable(ref System.Data.DataTable Dt, int WithoutRow)
        {
            Boolean Flag = false;
            System.Data.DataTable NewDt;
            try
            {
                NewDt = Dt.Copy();
                foreach (DataColumn DC in NewDt.Columns)
                {
                    //if (Convert.ToString(DC.DataType) == "System.Double" || Convert.ToString(DC.DataType) == "System.Int32" || Convert.ToString(DC.DataType) == "System.Decimal" && Dt.Columns[DC.ColumnName].Ordinal != 0)
                    if (Convert.ToString(DC.DataType) == "System.Double" || Convert.ToString(DC.DataType) == "System.Int32" || Convert.ToString(DC.DataType) == "System.Decimal")
                    {
                        //if (DC.ColumnName.ToUpper() != "BILLNO" && DC.ColumnName.ToUpper() != "MBILL" && DC.ColumnName.ToUpper() != "BILL_NO" && DC.ColumnName.ToUpper() != "LOCATION" && DC.ColumnName.ToUpper() != "MINNO" && DC.ColumnName.ToUpper() != "MAXNO" && DC.ColumnName.ToUpper() != "GSNNO" && DC.ColumnName.ToUpper() != "ACCNO" && DC.ColumnName.ToUpper() != "SLNO" && DC.ColumnName.ToUpper() != "TAX_PER" && DC.ColumnName.ToUpper() != "SRNO" && DC.ColumnName.ToUpper() != "DCNO" && DC.ColumnName.ToUpper() != "RECNO" && DC.ColumnName.ToUpper() != "EC" && DC.ColumnName.ToUpper() != "E_NO" && DC.ColumnName.ToUpper() != "SALESQTY %" && DC.ColumnName.ToUpper() != "SALESMTR %" && DC.ColumnName.ToUpper() != "GRN_NO")
                        if (DC.ColumnName.ToUpper() == "DEBIT" || DC.ColumnName.ToUpper() == "CREDIT" || DC.ColumnName.ToUpper() == "PAYMENT" || DC.ColumnName.ToUpper() == "RECEIPT" || DC.ColumnName.ToUpper() == "AMOUNT" || DC.ColumnName.ToUpper().Contains("AMOUNT"))
                        {
                            Flag = true;
                            break;
                        }
                    }
                }
                if (Flag == true)
                {
                    DataRow Dr = NewDt.NewRow();
                    DataRow Empty = NewDt.NewRow();
                    foreach (DataColumn DC in NewDt.Columns)
                    {
                        Empty[DC] = DBNull.Value;
                    }
                    foreach (DataColumn DC in NewDt.Columns)
                    {
                        if (Convert.ToString(DC.DataType) == "System.Double")
                        {
                            if (DC.ColumnName.ToUpper() != "CLBAL" && DC.ColumnName.ToUpper() != "BILLNO" && DC.ColumnName.ToUpper() != "MBILL" && DC.ColumnName.ToUpper() != "BILL_NO" && DC.ColumnName.ToUpper() != "LOCATION" && DC.ColumnName.ToUpper() != "MINNO" && DC.ColumnName.ToUpper() != "MAXNO" && DC.ColumnName.ToUpper() != "GSNNO" && DC.ColumnName.ToUpper() != "ACCNO" && DC.ColumnName.ToUpper() != "SLNO" && DC.ColumnName.ToUpper() != "TAX_PER" && DC.ColumnName.ToUpper() != "SRNO" && DC.ColumnName.ToUpper() != "DCNO" && DC.ColumnName.ToUpper() != "RECNO" && DC.ColumnName.ToUpper() != "EC" && DC.ColumnName.ToUpper() != "E_NO" && DC.ColumnName.ToUpper() != "SALESQTY %" && DC.ColumnName.ToUpper() != "SALESMTR %" && DC.ColumnName.ToUpper() != "GRN_NO")
                            {
                                Dr[DC] = Convert.ToDouble(Sum(ref NewDt, DC.ColumnName, true, WithoutRow));
                            }
                            if (Dr[DC] != DBNull.Value)
                            {
                                if (Convert.ToDouble(Dr[DC]) == 0)
                                {
                                    Dr[DC] = DBNull.Value;
                                }
                            }
                        }
                        else if (Convert.ToString(DC.DataType) == "System.Decimal" && Dt.Columns[DC.ColumnName].Ordinal != 0)
                        {
                            if (DC.ColumnName.ToUpper() != "CLBAL" && DC.ColumnName.ToUpper() != "BILLNO" && DC.ColumnName.ToUpper() != "MBILL" && DC.ColumnName.ToUpper() != "BILL_NO" && DC.ColumnName.ToUpper() != "LOCATION" && DC.ColumnName.ToUpper() != "MINNO" && DC.ColumnName.ToUpper() != "MAXNO" && DC.ColumnName.ToUpper() != "GSNNO" && DC.ColumnName.ToUpper() != "ACCNO" && DC.ColumnName.ToUpper() != "SLNO" && DC.ColumnName.ToUpper() != "TAX_PER" && DC.ColumnName.ToUpper() != "SRNO" && DC.ColumnName.ToUpper() != "DCNO" && DC.ColumnName.ToUpper() != "RECNO" && DC.ColumnName.ToUpper() != "EC" && DC.ColumnName.ToUpper() != "E_NO" && DC.ColumnName.ToUpper() != "SALESQTY %" && DC.ColumnName.ToUpper() != "SALESMTR %" && DC.ColumnName.ToUpper() != "GRN_NO")
                            {
                                Dr[DC] = Convert.ToDouble(Sum(ref NewDt, DC.ColumnName, true, WithoutRow));
                            }
                            if (Dr[DC] != DBNull.Value)
                            {
                                if (Convert.ToDouble(Dr[DC]) == 0)
                                {
                                    Dr[DC] = DBNull.Value;
                                }
                            }
                        }
                        else
                        {
                            Dr[DC] = DBNull.Value;
                        }
                    }
                    NewDt.Rows.Add(Empty);
                    NewDt.Rows.Add(Dr);
                }
                return NewDt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void V_DataGridView(ref System.Windows.Forms.DataGridView DGV)
        {
            Boolean Flag = false;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.ValueType.ToString() == "System.Double" || Dc.ValueType.ToString() == "System.Int32")
                    {
                        if (Dc.Name.ToUpper() == "DEBIT" || Dc.Name.ToUpper() == "CREDIT" || Dc.Name.ToUpper() == "PAYMENT" || Dc.Name.ToUpper() == "RECEIPT" || Dc.Name.ToUpper() == "AMOUNT" || Dc.Name.ToUpper().Contains("AMOUNT"))
                        {
                            Flag = true;
                            break;
                        }
                    }
                }
                if (Flag == true)
                {
                    //foreach (DataGridViewColumn Dc in DGV.Columns)
                    //{
                        //DGV.Columns[Dc.Name].SortMode = DataGridViewColumnSortMode.Programmatic;
                    //}
                    DGV.Rows[DGV.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    DGV.Rows[DGV.Rows.Count - 2].DefaultCellStyle.BackColor = SystemColors.Control;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void V_DataGridView(ref DotnetVFGrid.MyDataGridView DGV)
        {
            Boolean Flag = false;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.ValueType.ToString() == "System.Double" || Dc.ValueType.ToString() == "System.Int32")
                    {
                        if (Dc.Name.ToUpper() == "DEBIT" || Dc.Name.ToUpper() == "CREDIT" || Dc.Name.ToUpper() == "PAYMENT" || Dc.Name.ToUpper() == "RECEIPT" || Dc.Name.ToUpper() == "AMOUNT" || Dc.Name.ToUpper().Contains("AMOUNT"))
                        {
                            Flag = true;
                            break;
                        }
                    }
                }
                if (Flag == true)
                {
                    //foreach (DataGridViewColumn Dc in DGV.Columns)
                    //{
                    //DGV.Columns[Dc.Name].SortMode = DataGridViewColumnSortMode.Programmatic;
                    //}
                    DGV.Rows[DGV.Rows.Count - 1].DefaultCellStyle.BackColor = System.Drawing.Color.White;
                    DGV.Rows[DGV.Rows.Count - 2].DefaultCellStyle.BackColor = SystemColors.Control;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void ExportToExcel(ref System.Windows.Forms.DataGridView Dt, String ExcTitle, String FilePath)
        {
            Int32 J=0;
            String Str = String.Empty;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Add(Missing);
                WSheet = (Excel.Worksheet)WBook.ActiveSheet;
                WSheet.Cells[1,1] = ExcTitle;
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count-1, "1")).Merge(Missing);
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count-1, "1")).Font.Bold = true;
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).HorizontalAlignment = Excel.Constants.xlCenter;

                // Header
                for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                {
                    WSheet.Cells[3, j + 1] = Dt.Columns[j].HeaderText;
                }
                WSheet.get_Range("A3", "BU3").Font.Bold = true;

                // Detail

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                    {
                        if (Dt.Columns[j].ValueType.ToString().ToUpper().Contains("DATE"))
                        {
                            if (Dt[j, i].Value == null || Dt[j, i].Value == DBNull.Value)
                            {
                                Str = "";
                            }
                            else
                            {
                                Str = String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt[j, i].Value));
                            }
                        }
                        else
                        {
                            Str = Dt[j, i].Value.ToString();
                        }
                        if (Str.Contains(" Dr"))
                        {
                            Str = Str.Replace(" Dr", "");
                            Str = Convert_ToDouble (Str).ToString();
                        }
                        else if (Str.Contains(" Cr"))
                        {
                            Str = Str.Replace(" Cr", "");
                            Str = "-" + Convert_ToDouble(Str).ToString();
                        }
                        else
                        {
                            if (Dt.Columns[j].ValueType != typeof(String) && Dt.Columns[j].ValueType != typeof(DateTime)) 
                            {
                                Str = Convert_ToDouble(Str).ToString();
                            }
                        }
                        WSheet.Cells[i + 5, j + 1] = Str;
                    }
                }
                //============================
                // Last Line Bold
                //============================
                //WSheet.get_Range(ExcelColumn(J + 1, Convert.ToString(Dt.Rows.Count + 4)), Missing).EntireRow.Font.Bold = true;
                //============================

                WSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();

                // Alignment
                if (Dt.Columns.Count > 26)
                {
                    WSheet.get_Range("B1", "BU1").EntireColumn.NumberFormat = "0.00";
                }
                else
                {
                    J = 0;
                    foreach (DataGridViewColumn Dc in Dt.Columns)
                    {
                        if (Dc.ValueType.ToString() == "System.Double")
                        {
                            WSheet.get_Range(ExcelColumn(J, "1"), Missing).EntireColumn.NumberFormat = "0.00";
                        }
                        else if (Dc.ValueType.ToString() == "System.String")
                        {
                            WSheet.get_Range(ExcelColumn(J, "1"), Missing).EntireColumn.HorizontalAlignment = Excel.Constants.xlLeft;
                        }
                        J += 1;
                    }
                }
                //
                System.IO.File.Delete(FilePath);
                WBook.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing, Excel.XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                WBook.Close(false, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Create_Excel(String FilePath)
        {
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;
                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Add(Missing);
                System.IO.File.Delete(FilePath);
                WBook.SaveAs(FilePath, Excel.XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing, Excel.XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void ZExportToExcel_WSheet_Name(ref System.Windows.Forms.DataGridView Dt, String ExcTitle, String FilePath, String WSheetName, Int16 SheetNo)
        {
            Int32 J = 0;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Open(FilePath, Missing, (object)false, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                WSheet = (Excel.Worksheet)WBook.Sheets[SheetNo];
                WSheet.Name = WSheetName;
                WSheet.Cells[1, 1] = ExcTitle;
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).Merge(Missing);
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).Font.Bold = true;
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).HorizontalAlignment = Excel.Constants.xlCenter;

                // Header
                for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                {
                    WSheet.Cells[3, j + 1] = Dt.Columns[j].HeaderText;
                }
                WSheet.get_Range("A3", "BU3").Font.Bold = true;

                // Detail

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                    {
                        WSheet.Cells[i + 5, j + 1] = Dt[j, i].Value;
                    }
                }
                //============================
                // Last Line Bold
                //============================
                //WSheet.get_Range(ExcelColumn(J + 1, Convert.ToString(Dt.Rows.Count + 4)), Missing).EntireRow.Font.Bold = true;
                //============================

                WSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();

                // Alignment
                if (Dt.Columns.Count > 26)
                {
                    WSheet.get_Range("B1", "BU1").EntireColumn.NumberFormat = "0.00";
                }
                else
                {
                    J = 0;
                    foreach (DataGridViewColumn Dc in Dt.Columns)
                    {
                        if (Dc.ValueType.ToString() == "System.Double")
                        {
                            WSheet.get_Range(ExcelColumn(J, "1"), Missing).EntireColumn.NumberFormat = "0.00";
                        }
                        else if (Dc.ValueType.ToString() == "System.String")
                        {
                            WSheet.get_Range(ExcelColumn(J, "1"), Missing).EntireColumn.HorizontalAlignment = Excel.Constants.xlLeft;
                        }
                        J += 1;
                    }
                }
                //WBook.SaveAs(FilePath, XlFileFormat.xlWorkbookNormal, Missing, Missing, (Object)false, Missing, XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                WBook.Save();
                WBook.Close(false, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void PrintExcel(ref System.Windows.Forms.DataGridView Dt, String ExcTitle, String FilePath)
        {
            Int32 J = 0;
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Add(Missing);
                WSheet = (Excel.Worksheet)WBook.ActiveSheet;
                WSheet.Cells[1, 1] = ExcTitle;
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).Merge(Missing);
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).Font.Bold = true;
                WSheet.get_Range("A1", ExcelColumn(Dt.Columns.Count - 1, "1")).HorizontalAlignment = Excel.Constants.xlCenter;

                // Header
                for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                {
                    WSheet.Cells[3, j + 1] = Dt.Columns[j].HeaderText;
                }
                WSheet.get_Range("A3", "BU3").Font.Bold = true;

                // Detail

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count - 1; j++)
                    {
                        WSheet.Cells[i + 5, j + 1] = Dt[j, i].Value;
                    }
                }
                WSheet.get_Range(ExcelColumn(J + 1, Convert.ToString(Dt.Rows.Count + 4)), Missing).EntireRow.Font.Bold = true;
                WSheet.get_Range("A1", "Z1").EntireColumn.AutoFit();

                // Alignment
                if (Dt.Columns.Count > 26)
                {
                    WSheet.get_Range("B1", "BU1").EntireColumn.NumberFormat = "0.00";
                }
                else
                {
                    J = 0;
                    foreach (DataGridViewColumn Dc in Dt.Columns)
                    {
                        if (Dc.ValueType.ToString() == "System.Double")
                        {
                            WSheet.get_Range(ExcelColumn(J, "1"), Missing).EntireColumn.NumberFormat = "0.00";
                        }
                        else if (Dc.ValueType.ToString() == "System.String")
                        {
                            WSheet.get_Range(ExcelColumn(J, "1"), Missing).EntireColumn.HorizontalAlignment = Excel.Constants.xlLeft;
                        }
                        J += 1;
                    }
                }
                //
                System.IO.File.Delete(FilePath);
                Exc.WindowState = Excel.XlWindowState.xlMaximized;
                Exc.Visible = true;
                WBook.PrintPreview(Missing);
                //WBook.SaveAs(FilePath, XlFileFormat.xlWorkbookNormal, Missing, Missing, Missing, Missing, XlSaveAsAccessMode.xlNoChange, Missing, Missing, Missing, Missing, Missing);
                WBook.Close(false, Missing, Missing);
                Exc.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void sendEMailThroughOUTLOOK(String toid, String ccid, String subject, String Body, params String[] FilePath)
        {
            try
            {
                Int32 ArrayIndex = 0;
                String AttachmentName = String.Empty;

                Outlook.Application oApp = new Outlook.Application();
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                oMsg.HTMLBody = Body;
                oMsg.Subject = subject;
                oMsg.To = toid;
                //Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                //Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add(toid);

                foreach (String Str in FilePath)
                {
                    ArrayIndex++;
                    AttachmentName = "Attament" + ArrayIndex;
                    oMsg.Attachments.Add(Str, Outlook.OlAttachmentType.olByValue, ArrayIndex, (Object)AttachmentName);
                }

                if (ccid.Trim() != String.Empty)
                {
                    oMsg.CC = ccid;
                }
                //oRecip.Resolve();
                oMsg.Display(false);
                //oRecip = null;
                //oRecips = null;
                oMsg = null;
                oApp = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        public String ShowSave(String DialogTitle, String FileName)
        {
            try
            {
                SaveFileDialog SFDialog = new SaveFileDialog();
                SFDialog.Title = DialogTitle;
                SFDialog.Filter = "(Microsoft Excel *.xls)|*.xls";
                if (System.IO.Directory.Exists("D:"))
                {
                    SFDialog.InitialDirectory = "D:";
                }
                else if (System.IO.Directory.Exists("E:"))
                {
                    SFDialog.InitialDirectory = "E:";
                }
                else if (System.IO.Directory.Exists("F:"))
                {
                    SFDialog.InitialDirectory = "F:";
                }
                else
                {
                    SFDialog.InitialDirectory = "C:";
                }
                SFDialog.FileName = FileName + String.Format("{0:dd_MM_yyyy}", DateTime.Now);
                if (SFDialog.ShowDialog() == DialogResult.OK)
                {
                    if (SFDialog.FileName.Trim() != String.Empty)
                    {
                        return SFDialog.FileName;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String ShowSave(String DialogTitle, String FileName, String Filter)
        {
            try
            {
                SaveFileDialog SFDialog = new SaveFileDialog();
                SFDialog.Title = DialogTitle;
                SFDialog.Filter = "(Microsoft Excel *.xls)|*.xls";
                //SFDialog.Filter = Filter;
                if (System.IO.Directory.Exists("D:"))
                {
                    SFDialog.InitialDirectory = "D:";
                }
                else if (System.IO.Directory.Exists("E:"))
                {
                    SFDialog.InitialDirectory = "E:";
                }
                else if (System.IO.Directory.Exists("F:"))
                {
                    SFDialog.InitialDirectory = "F:";
                }
                else
                {
                    SFDialog.InitialDirectory = "C:";
                }
                SFDialog.FileName = FileName + String.Format("{0:dd_MM_yyyy}", DateTime.Now);
                if (SFDialog.ShowDialog() == DialogResult.OK)
                {
                    if (SFDialog.FileName.Trim() != String.Empty)
                    {
                        return SFDialog.FileName;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ShowSave_FileName_Condition(String DialogTitle, String FileName, String Filter)
        {
            try
            {
                SaveFileDialog SFDialog = new SaveFileDialog();
                SFDialog.Title = DialogTitle;
                SFDialog.Filter = "(Microsoft Excel *.xls)|*.xls";
                //SFDialog.Filter = Filter;
                if (System.IO.Directory.Exists("D:"))
                {
                    SFDialog.InitialDirectory = "D:";
                }
                else if (System.IO.Directory.Exists("E:"))
                {
                    SFDialog.InitialDirectory = "E:";
                }
                else if (System.IO.Directory.Exists("F:"))
                {
                    SFDialog.InitialDirectory = "F:";
                }
                else
                {
                    SFDialog.InitialDirectory = "C:";
                }
                SFDialog.FileName = FileName;
                if (SFDialog.ShowDialog() == DialogResult.OK)
                {
                    if (SFDialog.FileName.Trim() != String.Empty)
                    {
                        return SFDialog.FileName;
                    }
                    else
                    {
                        return String.Empty;
                    }
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean Check_File(String Dir)
        {
            try
            {
                if (System.IO.File.Exists(Dir) == true)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void Delete_File(String Dir)
        {
            try
            {
                if (System.IO.File.Exists(Dir) == true)
                {
                    System.IO.File.Delete(Dir);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ExcelColumn(Int64 i, String Append)
        {
            Int64 Ascii;
            String Str = String.Empty;
            try
            {
                Ascii = Convert.ToInt64(65 + i);
                if (Append == String.Empty)
                {
                    Str = Convert.ToChar(Ascii) + "1";
                }
                else
                {
                    Str = Convert.ToChar(Ascii) + Append;
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Initialize_Report_Details()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("Select * from report_Details", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    Server_Name = Dt.Rows[0]["Server_Name"].ToString();
                    DB_Name = Dt.Rows[0]["Db_Name"].ToString();
                    UserName = Dt.Rows[0]["User_Name"].ToString();
                    Pwd = Dt.Rows[0]["P_Word"].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Tally_Voucher(int CompCode, String Year_Code, DateTime Sdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            Double GroupCode = 0;
            String Group;
            String Voucher_Type = String.Empty;
            Double VCode = 0;
            try
            {
                Excel.Application Exc1;
                Excel.Workbook WBook1;
                Excel.Worksheet WSheet1;
                Object Missing = System.Reflection.Missing.Value;
                DateTime Vdate = DateTime.Now;
                String Narration = String.Empty, Guid = String.Empty, Previous_Guid = String.Empty, Ledger = String.Empty;
                Double Amount = 0, Vmode = 0;
                String Breakup_Mode = String.Empty, Breakup_Bill = String.Empty, Str = String.Empty, Vtype = String.Empty;
                DateTime Breakup_Date = DateTime.Now;
                Double Breakup_Amount = 0;
                Int32 Slno = 0, Ledger_Code;
                Double Vno = 0;
                Int32 RowId = 1;
                String TempStr = String.Empty;
                Boolean Master_Flag = false;


                Exc1 = new Excel.Application();
                WBook1 = (Excel.Workbook)Exc1.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\Tally_Voucher.xls", Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                WSheet1 = (Excel.Worksheet)WBook1.Sheets[1];


                if (Check_Table("Voucher_Export"))
                {
                    Execute("Drop table Voucher_Export");
                }
                
                Execute("Create table Voucher_export (RowID Varchar(50))");

                for (int i = 1; i <= 250; i++)
                {
                    if (((Excel.Range)(WSheet1.Cells[1, i])).Value2 != null)
                    {
                        Execute("Alter Table Voucher_Export Add Col_" + ((Excel.Range)(WSheet1.Cells[1, i])).Value2.ToString().Replace(".", "").Replace(":", "") + " varchar(1000)");
                    }
                }

                for (int i = 2; i <= 500; i++)
                {
                    if (((Excel.Range)(WSheet1.Cells[i, 4])).Value2 != null && ((Excel.Range)(WSheet1.Cells[i, 48])).Value2 != null)
                    {
                        if (Get_RecordCount("Voucher_Export", "Col_RemoteID = '" + ((Excel.Range)(WSheet1.Cells[i, 4])).Value2.ToString() + "' and Col_LedgerName = '" + ((Excel.Range)(WSheet1.Cells[i, 48])).Value2.ToString() + "'") == 0)
                        {
                            Execute("insert into Voucher_Export(Col_RemoteID) Values ('" + ((Excel.Range)(WSheet1.Cells[i, 4])).Value2.ToString() + "')");
                        }
                    }
                }

                WBook1.Close(Missing, Missing, Missing);
                Exc1.Quit();
                return;

                for (int i = 2; i <= 60000; i++)
                {
                    if (((Excel.Range)(WSheet1.Cells[i, 4])).Value2 != null)
                    {
                        Guid = ((Excel.Range)(WSheet1.Cells[i, 4])).Value2.ToString();
                        Voucher_Type = ((Excel.Range)(WSheet1.Cells[i, 6])).Value2.ToString();

                        if (Get_RecordCount("Voucher_Export", "COl_RemoteID = '" + Guid + "'") == 0)
                        {
                            Execute("Insert into Voucher_Export(RowId, Col_RemoteID) values (" + RowId + ", '" + Guid + "')");
                        }
                        else
                        {
                            for (int j = 9; j <= 250; j++)
                            {
                                if (((Excel.Range)(WSheet1.Cells[1, j])).Value2 != null)
                                {
                                    if (Get_RecordCount("Voucher_Export", "RowID = " + RowId + " and Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace("'", "").Replace(":", "") + " <> ''") > 0)
                                    {
                                        RowId += 1;
                                        Execute("Insert into Voucher_Export(RowId, Col_RemoteID) values (" + RowId + ", '" + Guid + "')");
                                    }
                                    if (((Excel.Range)(WSheet1.Cells[i, j])).Value2 != null)
                                    {
                                        if (RowId > 1)
                                        {
                                            Str = "Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`");
                                            if (Get_RecordCount("Voucher_Export", Str + " = '' and rowid = " + Convert.ToInt32(RowId - 1)) > 0)
                                            {
                                                Execute("Update Voucher_Export Set Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`") + " = '" + ((Excel.Range)(WSheet1.Cells[i, j])).Value2.ToString().Replace("'", "`") + "' where RowId = " + (RowId - 1));
                                            }
                                            else
                                            {
                                                Execute("Update Voucher_Export Set Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`") + " = '" + ((Excel.Range)(WSheet1.Cells[i, j])).Value2.ToString().Replace("'", "`") + "' where RowId = " + RowId);
                                            }
                                        }
                                        else
                                        {
                                            Execute("Update Voucher_Export Set Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`") + " = '" + ((Excel.Range)(WSheet1.Cells[i, j])).Value2.ToString().Replace("'", "`") + "' where RowId = " + RowId);
                                        }
                                    }
                                    else
                                    {
                                        if (RowId > 1)
                                        {
                                            Str = "Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`");
                                            if (Get_RecordCount("Voucher_Export", Str + " = '' and rowid = " + Convert.ToInt32(RowId - 1)) == 0)
                                            {
                                                Execute("Update Voucher_Export Set Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`") + " = '' where RowId = " + (RowId - 1));
                                            }
                                            else
                                            {
                                                Execute("Update Voucher_Export Set Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`") + " = '' where RowId = " + RowId);
                                            }
                                        }
                                        else
                                        {
                                            Execute("Update Voucher_Export Set Col_" + ((Excel.Range)(WSheet1.Cells[1, j])).Value2.ToString().Replace(".", "").Replace(":", "").Replace("'", "`") + " = '' where RowId = " + RowId);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }




                //VCode = Max("Voucher_master", "Vcode", "Company_Code = " + CompCode, true);

                //for (int i = 2; i <= 20000; i++)
                //{
                //    if (((Excel.Range)(WSheet1.Cells[i, 5])).Value2 != null)
                //    {
                //        Voucher_Type = ((Excel.Range)(WSheet1.Cells[i, 5])).Value2.ToString();

                //        if (Voucher_Type.ToUpper().Contains("PAYMENT"))
                //        {
                //            Vmode = 1;
                //        }
                //        else if (Voucher_Type.ToUpper().Contains("RECEIPT"))
                //        {
                //            Vmode = 2;
                //        }
                //        else if (Voucher_Type.ToUpper() == "CONTRA")
                //        {
                //            Vmode = 3;
                //        }
                //        else if (Voucher_Type.ToUpper() == "JOURNAL")
                //        {
                //            Vmode = 4;
                //        }
                //        else if (Voucher_Type.ToUpper() == "SALES")
                //        {
                //            Vmode = 5;
                //        }
                //        else if (Voucher_Type.ToUpper() == "PURCHASE")
                //        {
                //            Vmode = 6;
                //        }
                //        else if (Voucher_Type.ToUpper() == "DEBITNOTE")
                //        {
                //            Vmode = 7;
                //        }
                //        else if (Voucher_Type.ToUpper() == "CREDITNOTE")
                //        {
                //            Vmode = 8;
                //        }
                //        else
                //        {
                //            Vmode = 9;
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 7])).Value2 != null)
                //        {
                //            TempStr = ((Excel.Range)(WSheet1.Cells[i, 7])).Value2.ToString();
                //            Vdate = Convert.ToDateTime(TempStr.Substring(0, 4) + "/" + TempStr.Substring(4, 2) + "/" + TempStr.Substring(6, 2));
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 9])).Value2 != null)
                //        {
                //            Narration = ((Excel.Range)(WSheet1.Cells[i, 9])).Value2.ToString();
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 8])).Value2 != null)
                //        {
                //            Guid = ((Excel.Range)(WSheet1.Cells[i, 8])).Value2.ToString();
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 43])).Value2 != null)
                //        {
                //            Ledger = ((Excel.Range)(WSheet1.Cells[i, 43])).Value2.ToString();
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 49])).Value2 != null)
                //        {
                //            Amount = Convert.ToDouble(((Excel.Range)(WSheet1.Cells[i, 49])).Value2);
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 50])).Value2 != null)
                //        {
                //            Breakup_Bill = ((Excel.Range)(WSheet1.Cells[i, 50])).Value2.ToString();
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 52])).Value2 != null)
                //        {
                //            Breakup_Mode = ((Excel.Range)(WSheet1.Cells[i, 52])).Value2.ToString();
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 53])).Value2 != null)
                //        {
                //            Breakup_Amount = Convert.ToDouble(((Excel.Range)(WSheet1.Cells[i, 53])).Value2);
                //        }

                //        if (((Excel.Range)(WSheet1.Cells[i, 51])).Value2 != null)
                //        {
                //            Breakup_Date = Convert.ToDateTime(((Excel.Range)(WSheet1.Cells[i, 51])).Value2);
                //        }

                //        if (Guid == Previous_Guid)
                //        {
                //            Slno += 1;
                //        }
                //        else
                //        {
                //            Slno = 1;
                //        }

                //        Ledger_Code = Convert.ToInt32(GetData_InNumberWC ("Ledger_master", "Ledger_Name", Ledger, "Ledger_Code", Year_Code, CompCode));

                //        //Voucher Details
                //        if (Amount < 0)
                //        {
                //            Str = "Insert into Voucher_Details Values (" + VCode + ", " + String.Format("{0:dd-MMM-yyyy}",  Vdate) + ", " + Slno + ", 'BY', " + Ledger_Code + ", " + (-1) * (Amount) + ", 0, '" + Narration + "', " + CompCode + ", '" + Year_Code + "', 0)";
                //        }
                //        else
                //        {
                //            Str = "Insert into Voucher_Details Values (" + VCode + ", " + String.Format("{0:dd-MMM-yyyy}",  Vdate) + ", " + Slno + ", 'BY', " + Ledger_Code + ", " + Amount + ", 0, '" + Narration + "', " + CompCode + ", '" + Year_Code + "', 0)";
                //        }
                //        Delete(Str);


                //        if (Master_Flag == false)
                //        {
                //            // Voucher Master
                //            Vno = Max("Voucher_master", "Vno", "Company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", true);
                //            Str = "Insert into voucher_master values (" + VCode + ", " + Vmode + ", " + Vno + ", ' ',  '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', '" + Narration + "', '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', null, null, null, null, null, null, " + CompCode + ", '" + Year_Code + "', Null, '01-Jan-1899')";
                //            Delete(Str);
                //            Master_Flag = true;
                //        }

                //        //Voucher Breakup Bills
                //        if (Amount < 0)
                //        {
                //            if (Breakup_Amount > 0)
                //            {
                //                Str = "Insert into voucher_breakup_bills (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + Ledger_Code + ", " + Slno + ", '" + Breakup_Mode + "', '" + Breakup_Bill + "', '" + String.Format("{0:dd-MMM-yyyy}", Breakup_Date) + "', " + Breakup_Amount + ", 0, 0, " + VCode + ", 'CR', " + CompCode + ", '" + Year_Code + "', Null)";
                //            }
                //        }
                //        else
                //        {
                //            if (Breakup_Amount > 0)
                //            {
                //                Str = "Insert into voucher_breakup_bills (" + VCode + ", '" + String.Format("{0:dd-MMM-yyyy}", Vdate) + "', " + Ledger_Code + ", " + Slno + ", '" + Breakup_Mode + "', '" + Breakup_Bill + "', '" + String.Format("{0:dd-MMM-yyyy}", Breakup_Date) + "', 0, " + Breakup_Amount + ", 0, " + VCode + ", 'CR', " + CompCode + ", '" + Year_Code + "', Null)";
                //            }
                //        }
                //        Delete(Str);

                //        Previous_Guid = Guid;
                //    }
                //}

                WBook1.Close(Missing, Missing, Missing);
                Exc1.Quit();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        Int32 Get_Excel_Col_Code(String ColName)
        {
            Int32 Code = 1;
            try
            {
                Excel.Application Exc1;
                Excel.Workbook WBook1;
                Excel.Worksheet WSheet1;
                Object Missing = System.Reflection.Missing.Value;

                Exc1 = new Excel.Application();
                WBook1 = (Excel.Workbook)Exc1.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\New Acc.xls", Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                WSheet1 = (Excel.Worksheet)WBook1.Sheets[1];

                for (int i = 1; i <= 200; i++)
                {
                    if (((Excel.Range)(WSheet1.Cells[1, i])).Value2.ToString().ToUpper() == ColName.ToUpper())
                    {
                        WBook1.Close(Missing, Missing, Missing);
                        Exc1.Quit();
                        return i;
                    }
                }
                WBook1.Close(Missing, Missing, Missing);
                Exc1.Quit();
                return Code;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Tally_Connection(int CompCode, String Year_Code, DateTime Sdate)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            OdbcCommand Cmd;
            Double Code = 0;
            Double GroupCode = 0;
            String Group;
            Int32 Cell_Code = 1;
            try
            {
                // Group

                Excel.Application Exc1;
                Excel.Workbook WBook1;
                Excel.Worksheet WSheet1;
                Object Missing = System.Reflection.Missing.Value;

                Exc1 = new Excel.Application();
                WBook1 = (Excel.Workbook)Exc1.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\New Acc.xls", Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                WSheet1 = (Excel.Worksheet)WBook1.Sheets[1];

                Code = Max("GroupMas", "GroupCode", "Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'", true);
                Code += 1;

                Cell_Code = Get_Excel_Col_Code("Parent7");
                for (int i = 2; i <= 3000; i++)
                {
                    if (((Excel.Range)(WSheet1.Cells[i, Cell_Code])).Value2 != null)
                    {
                        Group = ((Excel.Range)(WSheet1.Cells[i, Cell_Code])).Value2.ToString();
                        if (Get_RecordCount("GroupMas", "Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "' and GroupName = '" + Group.Replace("'", "`") + "'") == 0)
                        {
                            Execute("Insert into GroupMas values (" + Code + ", '" + Group.Replace("'", "`") + "', 4800, 4800, null, null, null, null, null, null, null, null, null, null, null, null, null, null, " + CompCode + ", '" + Year_Code + "', 'Y', null, null)");
                            Code += 1;
                        }
                    }
                }
                WBook1.Close(Missing, Missing, Missing);
                Exc1.Quit();

                String Name = String.Empty;
                String Parent = String.Empty, Address = String.Empty, VatTinNUmber = String.Empty, OpenningBalance = String.Empty;
                String Bill = String.Empty, TDSApplicable = String.Empty, PanNo = String.Empty, TdsDedutType = String.Empty;
                Int32 Tds_Code = 0;
                DateTime Dtp = DateTime.Now;
                Object Ledger_Cell_Code = 1, Group_Cell_Code = 1, Tds_Cell_Code = 1, Tds_Deduct_Cell_Code = 1, Pan_Cell_Code = 1, Address_Cell_Code = 1, Vat_Cell_Code = 1, OpBal_Cell_Code = 1, Br_Bill_Cell_Code = 1, BR_Date_Cell_Code = 1;


                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing1 = System.Reflection.Missing.Value;


                Ledger_Cell_Code = (Object)Get_Excel_Col_Code("Name5");
                Group_Cell_Code = (Object)Get_Excel_Col_Code("parent7");
                Tds_Cell_Code = (Object)Get_Excel_Col_Code("ISTDSAPPLICABLE");
                Tds_Deduct_Cell_Code = (Object)Get_Excel_Col_Code("TDSDEDUCTEETYPE");
                Pan_Cell_Code = (Object)Get_Excel_Col_Code("INCOMETAXNUMBER");
                Address_Cell_Code = (Object)Get_Excel_Col_Code("ADDRESS");
                Vat_Cell_Code = (Object)Get_Excel_Col_Code("VATTINNUMBER");
                OpBal_Cell_Code = (Object)Get_Excel_Col_Code("OPENINGBALANCE");
                Br_Bill_Cell_Code = (Object)Get_Excel_Col_Code("NAME19");
                BR_Date_Cell_Code = (Object)Get_Excel_Col_Code("BILLCREDITPERIOD");
    

                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Open(System.Windows.Forms.Application.StartupPath + "\\New Acc.xls", Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1, Missing1);
                WSheet = (Excel.Worksheet)WBook.Sheets[1];

                for (int i = 2; i <= 3000; i++)
                {
                    if (((Excel.Range)(WSheet.Cells[i, Ledger_Cell_Code])).Value2 != null && ((Excel.Range)(WSheet.Cells[i, Group_Cell_Code])).Value2 != null)
                    {
                        Name = ((Excel.Range)(WSheet.Cells[i, Ledger_Cell_Code])).Value2.ToString();
                        Parent = ((Excel.Range)(WSheet.Cells[i, Group_Cell_Code])).Value2.ToString();
                        if (((Excel.Range)(WSheet.Cells[i, Tds_Cell_Code])).Value2 == null)
                        {
                            TDSApplicable = "N";
                        }
                        else
                        {
                            TDSApplicable = ((Excel.Range)(WSheet.Cells[i, Tds_Cell_Code])).Value2.ToString().Substring(0, 1);
                        }
                        if (((Excel.Range)(WSheet.Cells[i, Tds_Deduct_Cell_Code])).Value2 == null)
                        {
                            TdsDedutType = String.Empty;
                        }
                        else
                        {
                            TdsDedutType = ((Excel.Range)(WSheet.Cells[i, Tds_Deduct_Cell_Code])).Value2.ToString();
                            if (Get_RecordCount("Tds_Type", "Name = '" + TdsDedutType + "'") > 0)
                            {
                                Tds_Code = Convert.ToInt32(GetData_InNumber("Tds_type", "Name", TdsDedutType, "No"));
                            }
                            else
                            {
                                Tds_Code = Convert.ToInt32(MaxWOCC("tds_type", "No", String.Empty));
                            }
                            Execute("Insert into Tds_type values (" + Tds_Code + ", '" + TdsDedutType + "')");
                        }
                        if (((Excel.Range)(WSheet.Cells[i, Pan_Cell_Code])).Value2 == null)
                        {
                            PanNo = String.Empty;
                        }
                        else
                        {
                            PanNo = ((Excel.Range)(WSheet.Cells[i, Pan_Cell_Code])).Value2.ToString();
                        }

                        if (((Excel.Range)(WSheet.Cells[i, Address_Cell_Code])).Value2 == null)
                        {
                            Address = String.Empty;
                        }
                        else
                        {
                            Address = ((Excel.Range)(WSheet.Cells[i, Address_Cell_Code])).Value2.ToString();
                        }
                        if (((Excel.Range)(WSheet.Cells[i, Vat_Cell_Code])).Value2 == null)
                        {
                            VatTinNUmber = String.Empty;
                        }
                        else
                        {
                            VatTinNUmber = ((Excel.Range)(WSheet.Cells[i, Vat_Cell_Code])).Value2.ToString();
                        }
                        if (((Excel.Range)(WSheet.Cells[i, OpBal_Cell_Code])).Value2 == null)
                        {
                            OpenningBalance = "0";
                        }
                        else
                        {
                            OpenningBalance = ((Excel.Range)(WSheet.Cells[i, OpBal_Cell_Code])).Value2.ToString();
                            if (((Excel.Range)(WSheet.Cells[i, Br_Bill_Cell_Code])).Value2 == null)
                            {
                                Bill = String.Empty;
                            }
                            else
                            {
                                Bill = ((Excel.Range)(WSheet.Cells[i, Br_Bill_Cell_Code])).Value2.ToString();
                            }
                            if (((Excel.Range)(WSheet.Cells[i, BR_Date_Cell_Code])).Value2 == null)
                            {
                                Dtp = Sdate;
                            }
                            else
                            {
                                Dtp = Convert.ToDateTime(((Excel.Range)(WSheet.Cells[i, BR_Date_Cell_Code])).Value2);
                            }
                        }

                        if (Get_RecordCount("Ledger_master", "Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "' and Ledger_Name = '" + Name.Replace("'", "`") + "'") == 0)
                        {
                            GroupCode = GetData_InNumberWC("GroupMas", "GroupName", Parent.Replace("'", "`"), "GroupCOde", Year_Code, CompCode);
                            if (OpenningBalance != String.Empty && Convert.ToDouble(OpenningBalance) < 0)
                            {
                                Execute("Insert into Ledger_master(Ledger_Code, Ledger_Name, Ledger_title, Ledger_InPrint, Ledger_group_Code, ledger_Odebit, Ledger_OCredit, Ledger_Address, Ledger_Tin, company_Code, year_Code, Breakup, PanNo, TDsApplicable, TDsType) values (" + Code + ", '" + Name.Replace("'", "`") + "', 'M/S.', '" + Name.Replace("'", "`") + "', " + GroupCode + ", " + (-1) * Convert.ToDouble(OpenningBalance) + ", 0, '" + Address.Replace("'", "`") + "', '" + VatTinNUmber.Replace("'", "`") + "', " + CompCode + ", '" + Year_Code + "', 'Y', '" + PanNo + "', '" + TDSApplicable + "', '" + Tds_Code + "')");
                                Execute("Insert into Ledger_Breakup values (" + Code + ", 'LEDGER', 1, 'N', '" + Bill + "', '" + String.Format("{0:dd-MMM-yyyy}", Dtp) + "', 0, " + (-1) * Convert.ToDouble(OpenningBalance) + ", 0, 4, 0, 0, 'L1', " + CompCode + ", '" + Year_Code + "', null)");
                                Code += 1;
                            }
                            else if (OpenningBalance != String.Empty && Convert.ToDouble(OpenningBalance) > 0)
                            {
                                Execute("Insert into Ledger_master(Ledger_Code, Ledger_Name, Ledger_title, Ledger_InPrint, Ledger_group_Code, ledger_Odebit, Ledger_OCredit, Ledger_Address, Ledger_Tin, company_Code, year_Code, Breakup, PanNo, TDsApplicable, TDsType) values (" + Code + ", '" + Name.Replace("'", "`") + "', 'M/S.', '" + Name.Replace("'", "`") + "', " + GroupCode + ", 0, " + Convert.ToDouble(OpenningBalance) + ", '" + Address.Replace("'", "`") + "', '" + VatTinNUmber.Replace("'", "`") + "', " + CompCode + ", '" + Year_Code + "', 'Y', '" + PanNo + "', '" + TDSApplicable + "', '" + Tds_Code + "')");
                                Execute("Insert into Ledger_Breakup values (" + Code + ", 'LEDGER', 1, 'N', '" + Bill + "', '" + String.Format("{0:dd-MMM-yyyy}", Dtp) + "', " + Convert.ToDouble(OpenningBalance) + ", 0, 0, 4, 0, 0, 'L1', " + CompCode + ", '" + Year_Code + "', null)");
                                Code += 1;
                            }
                            else
                            {
                                Execute("Insert into Ledger_master(Ledger_Code, Ledger_Name, Ledger_title, Ledger_InPrint, Ledger_group_Code, ledger_Odebit, Ledger_OCredit, Ledger_Address, Ledger_Tin, company_Code, year_Code, Breakup, PanNo, TDsApplicable, TDsType) values (" + Code + ", '" + Name.Replace("'", "`") + "', 'M/S.', '" + Name.Replace("'", "`") + "', " + GroupCode + ", 0, 0, '" + Address.Replace("'", "`") + "', '" + VatTinNUmber.Replace("'", "`") + "', " + CompCode + ", '" + Year_Code + "', 'Y', '" + PanNo + "', '" + TDSApplicable + "', '" + Tds_Code + "')");
                                Code += 1;
                            }
                        }
                        else
                        {
                            Execute("Update Ledger_master set Ledger_address = Ledger_Address + Char(13) + Char(10) + '" + Address.Replace("'", "`") + "' where ledger_Name = '" + Name.Replace("'", "`") + "'");
                        }
                    }
                }

                WBook.Close(Missing1, Missing1, Missing1);
                Exc.Quit();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Get_Ledger_Similar(String Like_Ledger, Int32 CompCode, String Year_Code, out String Ledger, out Int32 Ledger_Code)
        {
            try
            {
                System.Data.DataTable Dt = new System.Data.DataTable();
                Load_Data("Select Ledger_name Ledger, Ledger_Code from ledger_Master where Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "' and Ledger_name = '" + Like_Ledger + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    Ledger = Dt.Rows[0]["Ledger"].ToString();
                    Ledger_Code = Convert.ToInt32(Dt.Rows[0]["Ledger_Code"]);
                }
                else
                {
                    Ledger = String.Empty;
                    Ledger_Code = 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DosPrint(String FileName)
        {
            try
            {
                string cmd = "/c Type " + FileName + ">Prn"; 
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "cmd.exe";
                proc.StartInfo.Arguments = cmd; 
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true; 
                proc.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool Connection_Initialize()
        {
            try
            {
                bool Flag;
                String ConnStr;
                StreamReader SR;
                if (System.IO.File.Exists("C:\\Vaahrep\\VSocks.txt") == true)
                {
                    SR = new StreamReader("C:\\Vaahrep\\VSocks.txt");
                    ConnStr = Connection_Ascii_Reverse(SR.ReadToEnd().Trim());
                    SR.Close();
                }
                else
                {
                    ConnStr = String.Empty;
                }

                if (ConnStr == String.Empty)
                {
                    Flag = false;
                }
                else
                {
                    Cn.ConnectionString = ConnStr;
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        bool SqlConnection_Initialize()
        {
            try
            {
                bool Flag;
                String ConnStr;
                StreamReader SR;

                if (System.IO.File.Exists("C:\\Vaahrep\\VSockssql.txt") == true)
                {
                    SR = new StreamReader("C:\\Vaahrep\\VSockssql.txt");
                    ConnStr = Connection_Ascii_Reverse(SR.ReadToEnd().Trim());
                    ConnStr += " ; Max Pool Size =2000;";
                    SR.Close();
                }
                else
                {
                    ConnStr = String.Empty;
                }

                if (ConnStr == String.Empty)
                {
                    Flag = false;
                }
                else
                {
                    SqlCn.ConnectionString = ConnStr;
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CmbSelection(ref CheckedListBox ChkSelect, Boolean All)
        {
            try
            {
                for (int i = 0; i <= ChkSelect.Items.Count - 1; i++)
                {
                    if (All == true)
                    {
                        ChkSelect.SetItemChecked(i, true);
                    }
                    else
                    {
                        ChkSelect.SetItemChecked(i, false);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Rupee(Double Amount)
        {
            try
            {
                String[] one = new String[20];
                String[] ty = new String[10];
                String[] Div = new String[5];
                String Pais = String.Empty, Rup = String.Empty, Rupees = String.Empty, Value = String.Empty, Temp = String.Empty;

                one[1] = "One ";
                one[2] = " Two ";
                one[3] = " Three ";
                one[4] = " Four ";
                one[5] = " Five ";
                one[6] = " Six ";
                one[7] = " Seven ";
                one[8] = " Eight ";
                one[9] = " Nine ";
                one[10] = " Ten ";
                one[11] = "Eleven ";
                one[12] = "Twelve ";
                one[13] = "Thirteen ";
                one[14] = "Fourteen ";
                one[15] = "Fifteen ";
                one[16] = "Sixteen ";
                one[17] = "Seventeen ";
                one[18] = "Eighteen ";
                one[19] = "Ninteen ";

                ty[1] = "";
                ty[2] = "Twenty ";
                ty[3] = "Thirty ";
                ty[4] = "Fourty ";
                ty[5] = "Fifty ";
                ty[6] = "Sixty ";
                ty[7] = "Seventy ";
                ty[8] = "Eighty ";
                ty[9] = "Ninety ";


                if (Convert.ToString(Amount).Contains("."))
                {
                    Value = String.Format("{0:0.00}", Amount);
                    Pais = Value.Substring(Value.IndexOf('.') + 1, 2);
                    //Rup = Value.Substring(1, Value.IndexOf('.') - 1);
                    Rup = Value.Substring(0, Value.IndexOf('.'));
                }
                else
                {
                    Value = Convert.ToString(Amount);
                    Rup = Value;
                }

                if (Rup.Length >= 2)
                {
                    Div[4] = Rup.Substring(Rup.Length - 2, 2);
                }
                else
                {
                    Div[4] = Rup.Substring(Rup.Length - 1, 1);
                }
                if (Rup.Length >= 3)
                {
                    Div[3] = Rup.Substring(Rup.Length - 3, 1);
                }
                if (Rup.Length >= 5)
                {
                    Div[2] = Rup.Substring(Rup.Length - 5, 2);
                }
                else if (Rup.Length >= 4)
                {
                    Div[2] = Rup.Substring(Rup.Length - 4, 1);
                }
                if (Rup.Length >= 7)
                {
                    Div[1] = Rup.Substring(Rup.Length - 7, 2);
                }
                else if (Rup.Length >= 6)
                {
                    Div[1] = Rup.Substring(Rup.Length - 6, 1);
                }
                if (Rup.Length > 7)
                {
                    Div[0] = Rup.Substring(0, Rup.Length - 7);
                }
                for (int j = Div.Length - 1; j >= 0; j--)
                {
                    Temp = String.Empty;
                    if (Div[j] != null)
                    {
                        if (Div[j].Length >= 2)
                        {
                            if (Convert.ToInt32(Div[j].Substring(0, 1)) >= 2)
                            {
                                Temp = ty[Convert.ToInt32(Div[j].Substring(0, 1))] + one[Convert.ToInt32(Div[j].Substring(1, 1))];
                            }
                            else
                            {
                                Temp = one[Convert.ToInt32(Div[j])];
                            }
                        }
                        else
                        {
                            Temp = one[Convert.ToInt32(Div[j])];
                        }
                        if (j == 0)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Crore[s] ";
                            }
                        }
                        else if (j == 1)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Lakh[s] ";
                            }
                        }
                        else if (j == 2)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Thousand ";
                            }
                        }
                        else if (j == 3)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Hundred ";
                            }
                        }
                        else if (j == 4)
                        {
                            if (Value.Contains(".") == false)
                            {
                                //Temp = "and " + Temp + " Only";
                                Temp = Temp + " Only";
                            }
                        }
                        Rupees = Temp + Rupees;
                    }
                }
                if (Pais.Length == 2)
                {
                    Temp = String.Empty;
                    if (Convert.ToInt32(Pais.Substring(0, 1)) >= 2)
                    {
                        Temp = ty[Convert.ToInt32(Pais.Substring(0, 1))] + one[Convert.ToInt32(Pais.Substring(1, 1))];
                    }
                    else
                    {
                        Temp = one[Convert.ToInt32(Pais)];
                    }
                    if (Temp.Length > 0)
                    {
                        Temp = " AND " + Temp;
                    }
                    Temp = Temp + "Paise Only";
                    Rupees = Rupees + Temp;
                }
                return Rupees;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Rupee(Double Amount, String Paise_Term)
        {
            try
            {
                String[] one = new String[20];
                String[] ty = new String[10];
                String[] Div = new String[5];
                String Pais = String.Empty, Rup = String.Empty, Rupees = String.Empty, Value = String.Empty, Temp = String.Empty;

                one[1] = "One ";
                one[2] = " Two ";
                one[3] = " Three ";
                one[4] = " Four ";
                one[5] = " Five ";
                one[6] = " Six ";
                one[7] = " Seven ";
                one[8] = " Eight ";
                one[9] = " Nine ";
                one[10] = " Ten ";
                one[11] = "Eleven ";
                one[12] = "Twelve ";
                one[13] = "Thirteen ";
                one[14] = "Fourteen ";
                one[15] = "Fifteen ";
                one[16] = "Sixteen ";
                one[17] = "Seventeen ";
                one[18] = "Eighteen ";
                one[19] = "Ninteen ";

                ty[1] = "";
                ty[2] = "Twenty ";
                ty[3] = "Thirty ";
                ty[4] = "Fourty ";
                ty[5] = "Fifty ";
                ty[6] = "Sixty ";
                ty[7] = "Seventy ";
                ty[8] = "Eighty ";
                ty[9] = "Ninety ";


                if (Convert.ToString(Amount).Contains("."))
                {
                    Value = String.Format("{0:0.00}", Amount);
                    Pais = Value.Substring(Value.IndexOf('.') + 1, 2);
                    //Rup = Value.Substring(1, Value.IndexOf('.') - 1);
                    Rup = Value.Substring(0, Value.IndexOf('.'));
                }
                else
                {
                    Value = Convert.ToString(Amount);
                    Rup = Value;
                }

                if (Rup.Length >= 2)
                {
                    Div[4] = Rup.Substring(Rup.Length - 2, 2);
                }
                else
                {
                    Div[4] = Rup.Substring(Rup.Length - 1, 1);
                }
                if (Rup.Length >= 3)
                {
                    Div[3] = Rup.Substring(Rup.Length - 3, 1);
                }
                if (Rup.Length >= 5)
                {
                    Div[2] = Rup.Substring(Rup.Length - 5, 2);
                }
                else if (Rup.Length >= 4)
                {
                    Div[2] = Rup.Substring(Rup.Length - 4, 1);
                }
                if (Rup.Length >= 7)
                {
                    Div[1] = Rup.Substring(Rup.Length - 7, 2);
                }
                else if (Rup.Length >= 6)
                {
                    Div[1] = Rup.Substring(Rup.Length - 6, 1);
                }
                if (Rup.Length > 7)
                {
                    Div[0] = Rup.Substring(0, Rup.Length - 7);
                }
                for (int j = Div.Length - 1; j >= 0; j--)
                {
                    Temp = String.Empty;
                    if (Div[j] != null)
                    {
                        if (Div[j].Length >= 2)
                        {
                            if (Convert.ToInt32(Div[j].Substring(0, 1)) >= 2)
                            {
                                Temp = ty[Convert.ToInt32(Div[j].Substring(0, 1))] + one[Convert.ToInt32(Div[j].Substring(1, 1))];
                            }
                            else
                            {
                                Temp = one[Convert.ToInt32(Div[j])];
                            }
                        }
                        else
                        {
                            Temp = one[Convert.ToInt32(Div[j])];
                        }
                        if (j == 0)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Crore[s] ";
                            }
                        }
                        else if (j == 1)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Lakh[s] ";
                            }
                        }
                        else if (j == 2)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Thousand ";
                            }
                        }
                        else if (j == 3)
                        {
                            if (Temp != null)
                            {
                                Temp = Temp + "Hundred ";
                            }
                        }
                        else if (j == 4)
                        {
                            if (Value.Contains(".") == false)
                            {
                                //Temp = "and " + Temp + " Only";
                                Temp = Temp + " Only";
                            }
                        }
                        Rupees = Temp + Rupees;
                    }
                }
                if (Pais.Length == 2)
                {
                    Temp = String.Empty;
                    if (Convert.ToInt32(Pais.Substring(0, 1)) >= 2)
                    {
                        Temp = ty[Convert.ToInt32(Pais.Substring(0, 1))] + one[Convert.ToInt32(Pais.Substring(1, 1))];
                    }
                    else
                    {
                        Temp = one[Convert.ToInt32(Pais)];
                    }
                    if (Temp.Length > 0)
                    {
                        Temp = " AND " + Temp;
                    }
                    Temp = Temp + "" + Paise_Term + " Only";
                    Rupees = Rupees + Temp;
                }
                return Rupees;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Grid_Value(ref MyDataGridView DGV, int RowIndex, DataRow Dr)
        {
            try
            {
                for (int i = 0; i <= DGV.Columns.Count - 1; i++)
                {
                    DGV[i, RowIndex].Value = Convert.ToString(Dr[i]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_DataGridValueAvailable(ref System.Data.DataTable Dt, String ColName, String ConditionValue)
        {
            try
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    if (Convert.ToString(Dr[ColName]) == ConditionValue)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetServerName()
        {
            String Str = String.Empty;
            try
            {
                Str = GetData_InString("Report_Details", "Database", OraDBName, "Server");
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String GetSystemNameForTable()
        {
            String Str = String.Empty;
            try
            {
                Str = Environment.MachineName.Replace("-", String.Empty);
                Str = Str.Replace(".", String.Empty);
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String GetServerUserName()
        {
            String Str = String.Empty;
            try
            {
                Str = GetData_InString("Report_Details", "Database", OraDBName, "UserID");
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetServerPass()
        {
            String Str = String.Empty;
            try
            {
                Str = GetData_InString("Report_Details", "Database", OraDBName, "PWd");
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public string ReturnWithSeperator(String TblName, String GetFldName, String Condition)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String Str = string.Empty;
            try
            {
                if (Condition.Trim() != String.Empty)
                {
                    Load_Data("Select " + GetFldName + " from " + TblName + " where " + Condition, ref Dt);
                }
                else
                {
                    Load_Data("Select " + GetFldName + " from " + TblName, ref Dt);
                }
                Str = ReturnWithSeperator(ref Dt, GetFldName);
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Int32 Return_Array_value(Int32[] Arr, Int32 Position)
        {
            try
            {
                return Arr[Position];
            }
            catch (Exception ex)
            {
                return Arr[Arr.Length - 1];
            }
        }

        Int32[] Return_Array (Int32[] Arr)
        {
            Int32[] output;
            try
            {
                output = new Int32[50];
                for (int i = 0; i <= 49; i++)
                {
                    if (i == 0)
                    {
                        output[i] = Return_Array_value(Arr, i);
                    }
                    else
                    {
                        output[i] = Return_Array_value(output, i - 1) + Return_Array_value(Arr, i);
                    }
                }
                return output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void Print_PageDetails(Int16 FromPage, Int16 toPage, String FilePath, params Int32[] Pagelen)
        {
            Int32 Line = 0;
            Int32 FromLine = 0;
            Int32 ToLine = 0;
            String Str;
            Int32[] NewArr = Return_Array(Pagelen);
            Int32 Frompage_1 = FromPage - 1;
            Int32 ToPage_1 = toPage - 1;
            try
            {
                if (FromPage == 1)
                {
                    FromLine += 1;
                }
                else
                {
                    FromLine = Return_Array_value(NewArr, Frompage_1 - 1) + 1;
                }
                //for (int i = Frompage_1; i <= ToPage_1; i++)
                //{
                    ToLine = Return_Array_value(NewArr, ToPage_1);
                //}
                StreamReader Rd = new StreamReader(FilePath);
                StreamWriter Wr = new StreamWriter(Base_Dir + "\\PPr.txt");
                while (Rd.EndOfStream == false)
                {
                    Line += 1;
                    if (Line >= FromLine && Line <= ToLine)
                    {
                        Str = Rd.ReadLine();
                    }
                    else
                    {
                        Str = Rd.ReadLine();
                    }
                    if (Line >= FromLine && Line <= ToLine)
                    {
                        Wr.WriteLine(Str);
                    }
                }
                Rd.Close();
                Wr.Close();
                Print(Base_Dir + "\\PPr.txt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Print_PageByPage(String FilePath, Int16 PageTotalLine, Int16 FromPage, Int16 toPage)
        {
            Int32 Line = 0;
            Int32 FromLine = 0;
            Int32 ToLine = 0;
            String Str;
            try
            {
                FromLine = Convert.ToInt32(Convert.ToInt32(FromPage * PageTotalLine) - PageTotalLine);
                FromLine += 1;
                ToLine = Convert.ToInt32(toPage * PageTotalLine);
                //ToLine -= 1;
                StreamReader Rd = new StreamReader(FilePath);
                StreamWriter Wr = new StreamWriter(Base_Dir + "\\PPr.txt");
                while (Rd.EndOfStream == false)
                {
                    Line += 1;
                    if (Line >= FromLine && Line <= ToLine)
                    {
                        Str = Rd.ReadLine();
                    }
                    else
                    {
                        Str = Rd.ReadLine();
                    }
                    if (Line >= FromLine && Line <= ToLine)
                    {
                        Wr.WriteLine(Str);
                    }
                }
                Rd.Close();
                Wr.Close();
                Print(Base_Dir + "\\PPr.txt");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert_Mail_Table(String FromID, String Toid, String CC, String Bcc, String Subject, String IsAttached,  String Body, String term, Int64 Code, DateTime Date, String Mode)
        {
            try
            {
                CreateTable_Mail();
                Execute("Insert into Mail values ('" + String.Format("{0:dd-MMM-yyyy} {0:T}", DateTime.Now) + "', '" + FromID + "', '" + Toid + "', '" + CC + "', '" + Bcc + "', '" + Subject + "', '" + IsAttached + "', '" + Body + "', '" + term + "', " + Code + ", '" + String.Format("{0:dd-MMM-yyyy}", Date) + "','" + Mode + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateTable_Mail()
        {
            try
            {
                if (Check_Table("Mail") == false)
                {
                    Execute("Create table Mail (OnDate datetime, FromId Varchar(50), ToId Varchar(150), CC varchar(150), Bcc varchar(150), Subject Varchar(150), Attac varchar(1), Body Varchar(4000), Term varchar(100), Code Bigint, Date_ Datetime)");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public Boolean Make_PDF(String FilePath, String OutPutFilePath)
        //{
        //    Object Missing = System.Reflection.Missing.Value;
        //    try
        //    {
        //        PDFMAKERAPILib.PDFMakerApp PDf = new PDFMAKERAPILib.PDFMakerApp();
        //        PDf.CreatePDF(FilePath, OutPutFilePath, Missing, true, false, Missing, Missing);
        //        PDf = null;
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        public void Open_NotePad(String Filename)
        {
            String Command = String.Empty;
            String Output = String.Empty;
            try
            {
                if (System.IO.File.Exists(Filename))
                {
                    Command = "/C START NOTEPAD.EXE " + Filename;
                    ProcessStartInfo PSI = new ProcessStartInfo("Cmd.exe", Command);
                    PSI.WindowStyle = ProcessWindowStyle.Hidden;
                    Process PC = new Process();
                    PC.StartInfo = PSI;
                    PC.Start();
                    PC.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void Open_PDf(String Filename)
        {
            String Command = String.Empty;
            String Output = String.Empty;
            try
            {
                if (System.IO.File.Exists(Filename))
                {
                    Command = "/C START ACROBAT.EXE " + Filename;
                    ProcessStartInfo PSI = new ProcessStartInfo("Cmd.exe", Command);
                    PSI.WindowStyle = ProcessWindowStyle.Hidden;
                    Process PC = new Process();
                    PC.StartInfo = PSI;
                    PC.Start();
                    PC.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Open_Word(Object FileName)
        {
            String Command = String.Empty;
            String Output = String.Empty;
            try
            {
                Command = "/C START Winword.EXE " + FileName;
                ProcessStartInfo PSI = new ProcessStartInfo("Cmd.exe", Command);
                PSI.WindowStyle = ProcessWindowStyle.Hidden;
                Process PC = new Process();
                PC.StartInfo = PSI;
                PC.Start();
                PC.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Double Department_Code(String Department)
        {
            Double Code = 0;
            try
            {
                Code = GetData_InNumber("Department_Master", "Department_Name", Department, "Department_Code");
                if (Code > 0)
                {
                    return Code;
                }
                else
                {
                    Code = MaxWOCC("Department_Master", "Department_Code", String.Empty);
                    Execute("Insert into Department_Master(Department_Code, Department_Name) values (" + Code + ", '" + Department + "')");
                    return Code;
                }
            }
            catch (Exception ex)
            {
                return Code;
            }
        }

        public bool DBFConnection_Initialize(Boolean Restore)
        {
            try
            {
                bool Flag = false;
                String ConnStr;
                ConnStr = "Driver={Sql Server};Server=" + DBF_SQL_SERVER + ";Uid=sa;Pwd=pscsa;Database=" + DBF_SQL_DB + ";";
                //ConnStr = "Driver={Sql Server};Server=" + DBF_SQL_SERVER + ";Uid=;Pwd=;Database=" + DBF_SQL_DB + ";";
                if (DBFCn.State == ConnectionState.Open)
                {
                    DBFCn_Close();
                }
                DBFCn.ConnectionString = ConnStr;
                Flag = true;
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool BackupConnection_Initialize(Boolean Restore)
        {
            try
            {
                bool Flag=false;
                String ConnStr;
                ConnStr = "Driver={Microsoft Visual Foxpro Driver};Sourcetype=DBF;SourceDb=" + System.Windows.Forms.Application.StartupPath + ";Exclusive=no";
                if (BackupCn.State == ConnectionState.Open)
                {
                    BackupCn_Close();
                }
                BackupCn.ConnectionString = ConnStr;
                Flag = true;
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ReturnWithSeperator(ref System.Data.DataTable Dt, String FieldName)
        {
            String Str = String.Empty;
            try
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    if (Str.Trim() != String.Empty)
                    {
                        Str = Str + "," + Convert.ToString(Dr[FieldName]);
                    }
                    else
                    {
                        Str = Convert.ToString(Dr[FieldName]);
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ReturnWithSeperator(ref CheckedListBox Chk)
        {
            String Str = String.Empty;
            try
            {
                foreach (Object Itch in Chk.CheckedItems)
                {
                    if (Str != String.Empty)
                    {
                        Str = Str + ",'" + Convert.ToString(Itch) + "'";
                    }
                    else
                    {
                        Str = "'" + Convert.ToString(Itch) +"'";
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String ReturnWithSeperator_WithIN_Space(ref CheckedListBox Chk, int WithInSpace)
        {
            String Str = String.Empty;
            try
            {
                foreach (Object Itch in Chk.CheckedItems)
                {
                    if (Str != String.Empty)
                    {
                        Str = Str + ",'" + Convert.ToString(Itch).Substring (1, WithInSpace) + "'";
                    }
                    else
                    {
                        Str = "'" + Convert.ToString(Itch).Substring(1, WithInSpace) + "'";
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public String ReturnWithSeperator(ref CheckedListBox Chk, Int16 StringSize)
        {
            String Str = String.Empty;
            try
            {
                foreach (Object Itch in Chk.CheckedItems)
                {
                    if (Str != String.Empty)
                    {
                        Str = Str + ",'" + Convert.ToString(Itch.ToString().Substring(0, StringSize).Trim()) + "'";
                    }
                    else
                    {
                        Str = "'" + Convert.ToString(Itch.ToString().Substring(0, StringSize).Trim()) + "'";
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String ReturnWithSeperator(ref System.Data.DataTable Dt, String FieldName, String ConditionColName, String Value)
        {
            String Str = String.Empty;
            try
            {
                foreach (DataRow Dr in Dt.Rows)
                {
                    if (Convert.ToString(Dr[ConditionColName]) == Value)
                    {
                        if (Str.Trim() != String.Empty)
                        {
                            Str = Str + ",'" + Convert.ToString(Dr[FieldName]) + "'";
                        }
                        else
                        {
                            Str = "'" + Convert.ToString(Dr[FieldName]) + "'";
                        }
                    }
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Cn_Open()
        {
            try
            {
                if (Cn.State == ConnectionState.Closed)
                {
                    Connection_Initialize(); 
                    Cn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SqlCn_Open()
        {
            try
            {
                if (SqlCn.State == ConnectionState.Closed)
                {
                    SqlConnection_Initialize();
                    SqlCn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SizingCn_Open()
        {
            try
            {
                if (SizingCn.State == ConnectionState.Closed)
                {
                    if (SizingCn.ConnectionString.Trim() == String.Empty)
                    {
                        SizingCn.ConnectionString = "Server=SIZING;Uid=sa;pwd=;Database=SMART_SIZING_NT9_5_2;";
                    }
                    SizingCn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void SizingCn_Close()
        {
            try
            {
                if (SizingCn.State == ConnectionState.Open)
                {
                    SizingCn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Cn_Close()
        {
            try
            {
                if (Cn.State == ConnectionState.Open)
                {
                    Cn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SqlCn_Close()
        {
            try
            {
                if (SqlCn.State == ConnectionState.Open)
                {
                    SqlCn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Sum_With_Three_Digits(ref MyDataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[ColumnName, i].Value).Trim() != String.Empty)
                        {
                            SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                        }
                    }
                }
                return String.Format("{0:0.000}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Width(ref MyDataGridView DGV, params int[] ColumnWidth)
        {
            int i = 0;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        if (i < ColumnWidth.Length)
                        {
                            Dc.Width = ColumnWidth[i];
                            i += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Width(Int32 ActGridWidth, ref MyDataGridView DGV, params int[] ColumnWidth)
        {
            int i = 0;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        if (i < ColumnWidth.Length)
                        {
                            Dc.Width = Convert.ToInt32(Convert.ToDecimal(ColumnWidth[i] / Convert.ToDecimal(ActGridWidth)) * Convert.ToDecimal(DGV.Width));
                            i += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Width(Int32 ActGridWidth, ref System.Windows.Forms.DataGridView DGV, params int[] ColumnWidth)
        {
            int i = 0;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        if (i < ColumnWidth.Length)
                        {
                            Dc.Width = Convert.ToInt32(Convert.ToDecimal(ColumnWidth[i] / Convert.ToDecimal(ActGridWidth)) * Convert.ToDecimal(DGV.Width));
                            i += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_WidthPercent(ref MyDataGridView DGV, params int[] ColumnWidthPercent)
        {
            int i = 0;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        if (i < ColumnWidthPercent.Length)
                        {
                            Dc.Width = Convert.ToInt32(Convert.ToDecimal(ColumnWidthPercent[i]) / Convert.ToDecimal(100) * Convert.ToInt32(DGV.Width - 8));
                            i += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Width(ref DataGridView DGV, params int[] ColumnWidth)
        {
            int i = 0;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        if (i < ColumnWidth.Length)
                        {
                            Dc.Width = ColumnWidth[i];
                            i += 1;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DDGrid_Width(ref DataGridView DGV, params int[] ColumnWidth)
        {
            int i = 0;
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    if (Dc.Visible == true)
                    {
                        if (i < ColumnWidth.Length)
                        {
                            if (ColumnWidth[i] != 0)
                            {
                                Dc.Width = ColumnWidth[i];
                                i += 1;
                            }
                            else
                            {
                                Dc.Visible = false;
                                i += 1;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DBFCn_Open()
        {
            try
            {
                if (DBFCn.State == ConnectionState.Closed)
                {
                    if (DBFCn.ConnectionString.Trim() == String.Empty)
                    {
                        DBFConnection_Initialize(false);
                    }
                    DBFCn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void BackupCn_Open()
        {
            try
            {
                if (BackupCn.State == ConnectionState.Closed)
                {
                    if (BackupCn.ConnectionString.Trim() == String.Empty)
                    {
                        BackupConnection_Initialize(false); 
                    }
                    BackupCn.Open();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Boolean IsAllNullInDatatableArray(DataTable[] Dt_Array)
        {
            try
            {
                for (int i = 0; i <= Dt_Array.Length - 1; i++)
                {
                    if (Dt_Array[i] != null)
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

        public Boolean ReArrange_Datatable_Array(DataTable[] Dt_Array)
        {
            Boolean IsAllNull = true;
            try
            {
                if (IsAllNullInDatatableArray(Dt_Array))
                {
                    return true;
                }
                else
                {
                    for (int i = 0; i <= Dt_Array.Length - 2; i++)
                    {
                        if (Dt_Array[i] == null && Dt_Array[i + 1] != null)
                        {
                            Dt_Array[i] = Dt_Array[i + 1].Copy();
                            Dt_Array[i + 1] = null;
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

        public void Row_Number(ref DataGridView DGV)
        {
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    DGV[0, i].Value = i + 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Report_Row_Number(ref DataGridView DGV)
        {
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 3; i++)
                {
                    DGV[0, i].Value = i + 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String MyName()
        {
            try
            {
                return Environment.MachineName.Replace("-", String.Empty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void PrintPreview(String Title, String Filepath, Int32 PageLength, Boolean Auto, params Int32[] PagesLen)
        {
            try
            {
                FrmPrintPreview Frm = new FrmPrintPreview();
                Frm.PrintMode (Title, Filepath, PageLength, Auto, PagesLen);
                Frm.StartPosition = FormStartPosition.CenterScreen;
                Frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Backup(String FileName)
        {
            String Command = String.Empty;
            String Output = String.Empty;
            StreamReader PCReader;
            try
            {
                LoadOraDBName();
                if (System.IO.Directory.Exists("D:"))
                {
                    if (System.IO.Directory.Exists("D:\\BACKUP") == false)
                    {
                        System.IO.Directory.CreateDirectory("D:\\BACKUP");
                    }
                    Command = "/C Exp Branch/Branch@" + OraDBName + " FILE = D:\\BACKUP\\" + FileName + ".DMP GRANTS=Y COMPRESS=Y FULL=Y";
                }
                else
                {
                    if (System.IO.Directory.Exists("C:\\BACKUP") == false)
                    {
                        System.IO.Directory.CreateDirectory("C:\\BACKUP");
                    }
                    Command = "/C Exp Branch/Branch@" + OraDBName + " FILE = D:\\BACKUP\\" + FileName + ".DMP GRANTS=Y COMPRESS=Y FULL=Y";
                }
                ProcessStartInfo PSI = new ProcessStartInfo("Cmd.exe", Command);
                //PSI.WindowStyle = ProcessWindowStyle.Hidden;
                PSI.UseShellExecute = false;
                PSI.RedirectStandardOutput = true;
                Process PC = new Process();
                PC.StartInfo = PSI;
                PC.Start();
                PCReader = PC.StandardOutput;
                Output = PCReader.ReadToEnd();
                PC.Close();
                return Output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Restore(String FileName)
        {
            String Command = String.Empty;
            String Output = String.Empty;
            StreamReader PCReader;
            try
            {
                if (System.IO.File.Exists(FileName))
                {
                    Command = "/C Imp USERID=Branch/Branch@PSR FILE="+ FileName + " FROMUSER=BRANCH TOUSER=BRANCH ROWS=Y IGNORE=Y GRANTS=Y";
                    ProcessStartInfo PSI = new ProcessStartInfo("Cmd.exe", Command);
                    //PSI.WindowStyle = ProcessWindowStyle.Hidden;
                    PSI.UseShellExecute = false;
                    PSI.RedirectStandardOutput = true;
                    Process PC = new Process();
                    PC.StartInfo = PSI;
                    PC.Start();
                    PCReader = PC.StandardOutput;
                    Output = PCReader.ReadToEnd();
                    PC.Close();
                }
                return Output;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Process_Name(String ProcessName, String MachineName)
        {
            int I = 0;
            try
            {
                foreach (Process P in Process.GetProcesses(MachineName))
                {
                    if (P.ProcessName.ToUpper() == ProcessName.ToUpper())
                    {
                        I += 1;
                    }
                }
                if (I == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Validate_tally_voucher(Int64 vcode, DateTime vdate, int compcode, string year_Code)
        {
            System.Data.DataTable Dt;
            try
            {
                Dt = new System.Data.DataTable();
                Load_Data(" select cast(sum(Debit) as Numeric(20)) Debit, cast(Sum(Credit) as Numeric(20)) Credit from voucher_Details where vcode = " + vcode + " and company_Code = " + compcode + " and yeaR_cODE = '" + year_Code + "'", ref Dt);
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Credit Not Equal to Debit ...!", "Vaahini");
                    return false;
                }
                else
                {
                    if (Convert.ToDouble(Dt.Rows[0]["Credit"]) != Convert.ToDouble(Dt.Rows[0]["Debit"]))
                    {
                        MessageBox.Show("Credit Not Equal to Debit ...!", "Vaahini");
                        return false;
                    }
                }

                Dt = new System.Data.DataTable();
                Load_Data("Select ledger_Name Ledger from voucher_details v1 left join ledger_Master l1 on v1.ledger_Code = l1.ledger_Code and v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code where v1.vcode = " + vcode + "  and v1.company_Code = " + compcode + " and v1.year_Code = '" + year_Code + "'", ref Dt);
                if (Dt.Rows.Count == 0)
                {
                    MessageBox.Show("Ledgers Not Available ...!", "Vaahini");
                    return false;
                }


                if (Dt.Rows.Count < 2)
                {
                    MessageBox.Show("Ledgers Not Available ...!", "Vaahini");
                    return false;
                }

                if (Get_RecordCount("Socks_Companymas", "compname like '%DHANALAKSHMI%'") > 0)
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Check_Ledger_Available_in_tally_7(Dt.Rows[i]["ledger"].ToString()) == false)
                        {
                            MessageBox.Show("*** " + Dt.Rows[i]["Ledger"].ToString() + " *** Not Avaialable in Tally ....!", "Vaahini");
                            return false;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Check_Ledger_Available_in_tally(Dt.Rows[i]["ledger"].ToString()) == false)
                        {
                            MessageBox.Show("*** " + Dt.Rows[i]["Ledger"].ToString() + " *** Not Avaialable in Tally ....!", "Vaahini");
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int32 Date_Difference_In_Days(DateTime Sdt, DateTime EDt)
        {
            try
            {
                TimeSpan Res = EDt.Subtract(Sdt);
                return Convert.ToInt32(Res.Days);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Boolean Check_Tally_Item_Name(ref MyDataGridView DGV)
        {
            try
            {
                if (DGV.Rows.Count == 0)
                {
                    Execute("Delete from Stock_TB");
                    return true;
                }

                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (DGV["CntName", i].Value == null || DGV["CntName", i].Value == DBNull.Value)
                    {
                        MessageBox.Show("Stock Item Not Available in Tally ...!", "Vaahini");
                        return false;
                    }
                    if (Check_ItemName_Available_in_tally(DGV["CntName", i].Value.ToString()) == false)
                    {
                        MessageBox.Show("*** " + DGV["CntName", i].Value.ToString() + " *** Item Not Available in Tally ...!", "Vaahini");
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Ledger_Address_verification_From_Tally()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$OpeningBalance from ledger order by Ledger.`$Name` ", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);

                if (Check_Table("Tally_Address_Verification") == false)
                {
                    Execute("Create table Tally_Address_Verification (Name varchar(150), Address varchar(2000))");
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("Insert into Tally_Address_Verification values ('" + Dt.Rows[i][0].ToString() + "', '" + Dt.Rows[i][2].ToString() + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public System.Data.DataTable Return_Tally_Ledger()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$OpeningBalance from ledger order by Ledger.`$Name` ", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                return Dt;
            }
        }


        public Boolean Get_Ledger_Balance_From_Tally(String CompName, int CompCode, String Year_Code)
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                if (Check_Table("Ledger_Balance_From_Tally") == false)
                {
                    Execute("Create table Ledger_Balance_From_Tally (Ledger Varchar(1000), Amount Numeric(25,2))");
                }
                else
                {
                    Execute("Delete from Ledger_balance_From_tally");
                }
                if (CompName.ToUpper().Contains("DHANA"))
                {
                    TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                }
                else
                {
                    TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                }
                TallyCn.Open();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$ClosingBalance from ledger order by Ledger.`$Name` ", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i][0] != null && Dt.Rows[i][0] != DBNull.Value && Dt.Rows[i][0].ToString() != String.Empty)
                    {
                        if (Dt.Rows[i][1] != null && Dt.Rows[i][1] != DBNull.Value && Dt.Rows[i][1].ToString() != String.Empty)
                        {
                            Execute("Insert into Ledger_Balance_From_Tally values ('" + Dt.Rows[i][0].ToString().Replace("'", "") + "', " + Dt.Rows[i][1].ToString() + ")");
                        }
                        else
                        {
                            Execute("Insert into Ledger_Balance_From_Tally values ('" + Dt.Rows[i][0].ToString().Replace("'", "") + "', null)");
                        }
                    }
                }
                Update_Profit_Loss(CompCode, Year_Code);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void Update_Profit_Loss(int CompCode, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("select isnull(Amount, 0) Amount from ledger_balance_From_tally where ledger like 'Profit & Loss A/c%'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    if (Convert.ToDouble(Dt.Rows[0]["Amount"]) < 0)
                    {
                        Execute("Update ledger_Master set Ledger_Odebit = " + (Convert.ToDouble(Dt.Rows[0]["Amount"]) * (-1)) + ", Ledger_Ocredit = 0 where ledger_Name = 'Profit & Loss A/c' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                    }
                    else
                    {
                        Execute("Update ledger_Master set Ledger_OCredit = " + Dt.Rows[0]["Amount"].ToString() + ", Ledger_ODebit = 0 where ledger_Name = 'Profit & Loss A/c' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'");
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public Boolean Check_Ledger_Available_in_tally(String Ledger)
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$OpeningBalance from ledger where Ledger.`$Name` = '" + Ledger.Replace(".", "").Replace(",", "") + "'", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Boolean Check_Ledger_Available_in_tally_7(String Ledger)
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$OpeningBalance from ledger where Ledger.`$Name` = '" + Ledger.Replace(".", "").Replace(",", "") + "'", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }


        public void Load_Tally_Item_Name()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct StockItem.`$Name` from StockItem ", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Check_Table("Tally_Item_Name"))
                {
                    Execute("Drop table tally_Item_Name");
                }
                Execute("Create table Tally_Item_Name (Name varchar(50))");
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("Insert into Tally_Item_Name values ('" + Dt.Rows[i][0].ToString() + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_Tally_Group_Name()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Groups.`$Name` from Groups ", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Check_Table("Tally_Group_Name"))
                {
                    Execute("Drop table tally_Group_Name");
                }
                Execute("Create table Tally_Group_Name (Name varchar(50))");
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("Insert into Tally_Group_Name values ('" + Dt.Rows[i][0].ToString() + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_Tally_Group_Name_7()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Groups.`$Name` from Groups ", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Check_Table("Tally_Group_Name"))
                {
                    Execute("Drop table tally_Group_Name");
                }
                Execute("Create table Tally_Group_Name (Name varchar(50))");
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("Insert into Tally_Group_Name values ('" + Dt.Rows[i][0].ToString() + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean Check_ItemName_Available_in_tally(String Ledger)
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct StockItem.`$Name` from StockItem where StockItem.`$Name` = '" + Ledger.Replace(".", "").Replace(",", "") + "'", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void Ledger_Address_Update_from_Tally()
        {
            OdbcConnection TallyCn;
            System.Data.DataTable Dt = new System.Data.DataTable();
            OdbcCommand Cmd;
            String Address = String.Empty;
            String IncomeTax = String.Empty;
            String PanNo = String.Empty;
            String Phone = String.Empty;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$_Address1, Ledger.$_Address2, Ledger.$_Address3, Ledger.$_Address4, Ledger.$_Address5, Ledger.$_IncomeTaxNumber, Ledger.$_SalesTaxNumber, Ledger.$_LedgerPhone from ledger where Ledger.`$Name` is not null and Ledger.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i][1] != DBNull.Value)
                    {
                        if (Dt.Rows[i][2] != DBNull.Value)
                        {
                            if (Dt.Rows[i][3] != DBNull.Value)
                            {
                                if (Dt.Rows[i][4] != DBNull.Value)
                                {
                                    if (Dt.Rows[i][5] != DBNull.Value)
                                    {
                                        Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][3].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][4].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][5].ToString();
                                        Address = Address.Replace("'", "`");
                                        PanNo = Dt.Rows[i][6].ToString();
                                        IncomeTax = Dt.Rows[i][7].ToString();
                                        Phone = Dt.Rows[i][8].ToString();
                                        Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                                    }
                                    else
                                    {
                                        Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][3].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][4].ToString();
                                        Address = Address.Replace("'", "`");
                                        Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                                    }
                                }
                                else
                                {
                                    Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][3].ToString();
                                    Address = Address.Replace("'", "`");
                                    Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                                }
                            }
                            else
                            {
                                Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString();
                                Address = Address.Replace("'", "`");
                                Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                            }
                        }
                        else
                        {
                            Address = Dt.Rows[i][1].ToString();
                            Address = Address.Replace("'", "`");
                            Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Ledger_TinNo_Update_from_Tally7()
        {
            OdbcConnection TallyCn;
            System.Data.DataTable Dt = new System.Data.DataTable();
            OdbcCommand Cmd;
            String Address = String.Empty;
            String IncomeTax = String.Empty;
            String PanNo = String.Empty;
            String Phone = String.Empty;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.`$INCOMETAXNUMBER`, Ledger.`$SALESTAXNUMBER` from ledger where Ledger.`$Name` is not null and Ledger.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    IncomeTax = Dt.Rows[i][1].ToString();
                    if (IncomeTax.Trim() == String.Empty || IncomeTax.Trim() == "-")
                    {
                        IncomeTax = Dt.Rows[i][2].ToString();
                    }
                    Execute("UPdate Ledger_Master set Ledger_Tin = '" + IncomeTax + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Ledger_TinNo_Update_from_Tally()
        {
            OdbcConnection TallyCn;
            System.Data.DataTable Dt = new System.Data.DataTable();
            OdbcCommand Cmd;
            String Address = String.Empty;
            String IncomeTax = String.Empty;
            String PanNo = String.Empty;
            String Phone = String.Empty;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.`$INCOMETAXNUMBER`, Ledger.`$VATTINNUMBER` from ledger where Ledger.`$Name` is not null and Ledger.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    IncomeTax = Dt.Rows[i][1].ToString();
                    if (IncomeTax.Trim() == String.Empty || IncomeTax.Trim() == "-")
                    {
                        IncomeTax = Dt.Rows[i][2].ToString();
                    }
                    Execute("UPdate Ledger_Master set Ledger_Tin = '" + IncomeTax + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void Ledger_Address_Update_from_Tally7()
        {
            OdbcConnection TallyCn;
            System.Data.DataTable Dt = new System.Data.DataTable();
            OdbcCommand Cmd;
            String Address = String.Empty;
            String IncomeTax = String.Empty;
            String PanNo = String.Empty;
            String Phone = String.Empty;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                TallyCn.Open();
                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$_Address1, Ledger.$_Address2, Ledger.$_Address3, Ledger.$_Address4, Ledger.$_Address5, Ledger.$_IncomeTaxNumber, Ledger.$_SalesTaxNumber, Ledger.$_LedgerPhone from ledger where Ledger.`$Name` is not null and Ledger.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i][1] != DBNull.Value)
                    {
                        if (Dt.Rows[i][2] != DBNull.Value)
                        {
                            if (Dt.Rows[i][3] != DBNull.Value)
                            {
                                if (Dt.Rows[i][4] != DBNull.Value)
                                {
                                    if (Dt.Rows[i][5] != DBNull.Value)
                                    {
                                        Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][3].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][4].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][5].ToString();
                                        Address = Address.Replace("'", "`");
                                        PanNo = Dt.Rows[i][6].ToString();
                                        IncomeTax = Dt.Rows[i][7].ToString();
                                        Phone = Dt.Rows[i][8].ToString();
                                        Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                                    }
                                    else
                                    {
                                        Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][3].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][4].ToString();
                                        Address = Address.Replace("'", "`");
                                        Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                                    }
                                }
                                else
                                {
                                    Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][3].ToString();
                                    Address = Address.Replace("'", "`");
                                    Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                                }
                            }
                            else
                            {
                                Address = Dt.Rows[i][1].ToString() + Chr(13) + Chr(10) + Dt.Rows[i][2].ToString();
                                Address = Address.Replace("'", "`");
                                Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                            }
                        }
                        else
                        {
                            Address = Dt.Rows[i][1].ToString();
                            Address = Address.Replace("'", "`");
                            Execute("UPdate Ledger_Master set ledger_Address = '" + Address + "', PanNo = '" + PanNo + "', Ledger_Tin = '" + IncomeTax + "', Ledger_Phone = '" + Phone + "' where ledger_Name = '" + Dt.Rows[i][0].ToString().Replace("'", "`") + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        
        public void Save_Ledger_From_Tally_72()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            System.Data.DataTable Dt1;
            System.Data.DataTable Dt2;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                TallyCn.Open();
                Dt1 = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Company.`$Name` from Company where Company.`$Name` is not null ", TallyCn);
                OdbcDataAdapter Adp1 = new OdbcDataAdapter(Cmd);
                Adp1.Fill(Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    if (MessageBox.Show("Current Company is - '" + Dt1.Rows[0][0].ToString() + "'", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                /// IF ledger parent is null
                /// 

                    //Dt = new System.Data.DataTable();
                    //Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$SalesTaxNumber, Ledger.$IncomeTaxNumber, Ledger.$InterStateNumber, Ledger.$OpeningBalance from ledger ", TallyCn);
                    //OdbcDataAdapter Adp28 = new OdbcDataAdapter(Cmd);
                    //Adp28.Fill(Dt);
                    //if (Check_Table("Tally_Ledger_Null"))
                    //{
                    //    Execute("Drop table Tally_ledger_Null");
                    //}
                    //Execute("Create table Tally_ledger_Null (Name varchar(500), Group_Name varchar(500), Address varchar(2000), Op_Bal varchar(20), Tin varchar(500), Tin1 varchar(500), Pan varchar(500), CST varchar(500))");
                    //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    //{
                    //    Execute("Insert into tally_ledger_Null values ('" + Dt.Rows[i][0].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][1].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][2].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][7].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][3].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][4].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][5].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][6].ToString().Replace("'", "`") + "')");
                    //}

                ///// Else 
                
                    Dt = new System.Data.DataTable();
                    Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$SalesTaxNumber, Ledger.$IncomeTaxNumber, Ledger.$InterStateNumber, Ledger.$OpeningBalance from ledger ", TallyCn);
                    OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                    Adp2.Fill(Dt);
                    if (Check_Table("Tally_Ledger"))
                    {
                        Execute("Drop table Tally_ledger");
                    }
                    Execute("Create table Tally_ledger (Name varchar(500), Group_Name varchar(500), Address varchar(2000), Op_Bal varchar(20), Tin varchar(500), Tin1 varchar(500), Pan varchar(500), CST varchar(500))");
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        Execute("Insert into tally_ledger values ('" + Dt.Rows[i][0].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][1].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][2].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][7].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][3].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][4].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][5].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][6].ToString().Replace("'", "`") + "')");
                    }

                Dt2 = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Groups.`$Name`, Groups.$Parent from Groups where Group.`$Name` is not null and Group.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp3 = new OdbcDataAdapter(Cmd);
                Adp3.Fill(Dt2);
                if (Check_Table("Tally_Groups"))
                {
                    Execute("Drop table Tally_Groups");
                }
                Execute("Create table Tally_Groups (Name varchar(500), Parent varchar(500))");
                for (int i = 0; i <= Dt2.Rows.Count - 1; i++)
                {
                    Execute("Insert into tally_Groups values ('" + Dt2.Rows[i][0].ToString().Replace("'", "`") + "', '" + Dt2.Rows[i][1].ToString().Replace("'", "`") + "')");
                }

                Ledger_Address_Update_from_Tally();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Tally_Current_Company()
        {
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            OdbcCommand Cmd;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt1 = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Company.`$Name` from Company where Company.`$Name` is not null ", TallyCn);
                OdbcDataAdapter Adp1 = new OdbcDataAdapter(Cmd);
                Adp1.Fill(Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    return Dt1.Rows[0][0].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }


        public String Tally_Current_Company_7()
        {
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            OdbcCommand Cmd;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc;uid=;pwd=;");
                TallyCn.Open();
                Dt1 = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Company.`$Name` from Company where Company.`$Name` is not null ", TallyCn);
                OdbcDataAdapter Adp1 = new OdbcDataAdapter(Cmd);
                Adp1.Fill(Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    return Dt1.Rows[0][0].ToString();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public void Save_Ledger_From_Tally()
        {
            OdbcConnection TallyCn;
            OdbcCommand Cmd;
            System.Data.DataTable Dt;
            System.Data.DataTable Dt1;
            System.Data.DataTable Dt2;
            try
            {
                TallyCn = new OdbcConnection("DSN=TallyOdbc_9000;uid=;pwd=;");
                TallyCn.Open();
                Dt1 = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Company.`$Name` from Company where Company.`$Name` is not null ", TallyCn);
                OdbcDataAdapter Adp1 = new OdbcDataAdapter(Cmd);
                Adp1.Fill(Dt1);
                if (Dt1.Rows.Count > 0)
                {
                    if (MessageBox.Show("Current Company is - '" + Dt1.Rows[0][0].ToString() + "'", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                Dt = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Ledger.`$Name`, Ledger.$Parent, Ledger.$Address, Ledger.$VatTinNumber, Ledger.$OpeningBalance from ledger where Ledger.`$Name` is not null and Ledger.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp2 = new OdbcDataAdapter(Cmd);
                Adp2.Fill(Dt);
                if (Check_Table("Tally_Ledger"))
                {
                    Execute("Drop table Tally_ledger");
                }
                Execute("Create table Tally_ledger (Name varchar(500), Group_Name varchar(500), Address varchar(2000), Op_Bal varchar(20))");
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("Insert into tally_ledger values ('" + Dt.Rows[i][0].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][1].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][2].ToString().Replace("'", "`") + "', '" + Dt.Rows[i][4].ToString().Replace("'", "`") + "')");
                }

                Dt2 = new System.Data.DataTable();
                Cmd = new OdbcCommand("Select Distinct Groups.`$Name`, Groups.$Parent from Groups where Group.`$Name` is not null and Group.$Parent is not null", TallyCn);
                OdbcDataAdapter Adp3 = new OdbcDataAdapter(Cmd);
                Adp3.Fill(Dt2);
                if (Check_Table("Tally_Groups"))
                {
                    Execute("Drop table Tally_Groups");
                }
                Execute("Create table Tally_Groups (Name varchar(500), Parent varchar(500))");
                for (int i = 0; i <= Dt2.Rows.Count - 1; i++)
                {
                    Execute("Insert into tally_Groups values ('" + Dt2.Rows[i][0].ToString().Replace("'", "`") + "', '" + Dt2.Rows[i][1].ToString().Replace("'", "`") + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Boolean Process_Name_For_Tally(String MachineName)
        {
            int I = 0;
            try
            {
                foreach (Process P in Process.GetProcesses(MachineName))
                {
                    if (P.ProcessName.ToUpper() == "TALLY" || P.ProcessName.ToUpper() == "TALLY9" || P.ProcessName.ToUpper() == "TALLY8" || P.ProcessName.ToUpper() == "TALLY72")
                    {
                        I += 1;
                    }
                }
                if (I == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int16 HowMany_Times_Exe_Running(String ExeName, String MachineName)
        {
            Int16 I = 0;
            try
            {
                foreach (Process P in Process.GetProcesses(MachineName))
                {
                    if (P.ProcessName.ToUpper().Contains(ExeName.ToUpper()))
                    {
                        if (P.ProcessName.ToUpper() != "TALLYLICSERVER")
                        {
                            I += 1;
                        }
                    }
                }

                return I;
            }
            catch (Exception ex)
            {
                return I;
            }
        }



        public void Print(String FilePath)
        {
            String Command;
            try
            {
                Command = "/C Type " + FilePath + ">Prn";
                ProcessStartInfo PSI = new ProcessStartInfo("Cmd.exe",Command );
                //PSI.WindowStyle = ProcessWindowStyle.Hidden;
                Process PC = new Process();
                PC.StartInfo = PSI;
                PC.Start();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public String Space(int Size)
        {
            String Spc = String.Empty;
            try
            {
                for (int i = 0; i <= Size - 1; i++)
                {
                    Spc = Spc + " ";
                }
                return Spc;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Fill_Char(int Length, Char Ch)
        {
            String Str= String.Empty;
            try
            {
                for (int i=0;i< Length;i++)
                {
                    Str = Str + Convert.ToString(Ch);
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Customer_Address(int CustomerCode)
        {
            String Area = string.Empty;
            String Tr = string.Empty;
            String Nam = string.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            StreamWriter SW = new StreamWriter(Base_Dir + "\\CusArea.txt");
            int i;
            try
            {
                CusAddress = new String[6];
                Load_Data("Select l1.Ledger_Name Name, l1.ledger_inprint, l1.Ledger_Address Address, A1.Area_Name Area, A1.Area_Std Std from Ledger_Master l1 left join area_master a1 on l1.Ledger_Area_Code = a1.Area_Code where l1.Ledger_Code = " + CustomerCode, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    Tr = GetData_InString("Ledger_Master", "Ledger_Code", CustomerCode.ToString(), "Ledger_title");
                    Nam = Tr + " " + Convert.ToString(Dt.Rows[0]["ledger_inprint"]);
                    SW.WriteLine(Nam.TrimStart(' ')); 
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Address"]));
                }
                else
                {
                    SW.WriteLine(" " + Space(29));
                    SW.WriteLine(" " + Space(29));
                    SW.WriteLine(Space(29));
                }
                SW.Close();

                for (i = 0; i < 6; i++)
                {
                    CusAddress[i] = String.Empty;
                }

                StreamReader SR = new StreamReader(Base_Dir + "\\CusArea.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 6)
                    {
                        CusAddress[i] = SR.ReadLine().Replace("`","'");
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                //CusAddress[i] = Area;
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Ledger_Address(int CustomerCode, int CompCode, String Year_Code)
        {
            String Area = string.Empty;
            String Tr = string.Empty;
            String Nam = string.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            StreamWriter SW = new StreamWriter(Base_Dir + "\\CusArea.txt");
            int i;
            try
            {
                CusAddress = new String[6];
                Load_Data("Select l1.Ledger_Name Name, l1.ledger_inprint, l1.Ledger_Address Address, A1.Area_Name Area, A1.Area_Std Std from Ledger_Master l1 left join area_master a1 on l1.Ledger_Area_Code = a1.Area_Code where l1.Ledger_Code = " + CustomerCode + " and l1.company_Code = " + CompCode + " and l1.year_Code = '" + Year_Code + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Address"]));
                }
                else
                {
                    SW.WriteLine("");
                    SW.WriteLine("");
                    SW.WriteLine("");
                }
                SW.Close();

                for (i = 0; i < 6; i++)
                {
                    CusAddress[i] = String.Empty;
                }

                StreamReader SR = new StreamReader(Base_Dir + "\\CusArea.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 6)
                    {
                        CusAddress[i] = SR.ReadLine().Replace("`", "'");
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                //CusAddress[i] = Area;
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Above_Address(String Above)
        {
            StreamWriter SW = new StreamWriter(Base_Dir + "\\CusAbove.txt");
            int i;
            try
            {
                AboveAddress = new String[6];
                SW.WriteLine(Above.Trim());
                SW.Close();

                for (i = 0; i < 6; i++)
                {
                    AboveAddress[i] = String.Empty;
                }

                StreamReader SR = new StreamReader(Base_Dir + "\\CusABove.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 6)
                    {
                        AboveAddress[i] = SR.ReadLine().Replace("`", "'");
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Terms(String TblName, String Condition)
        {
            String Area = string.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            StreamWriter SW = new StreamWriter(Base_Dir + "\\Terms.txt");
            int i;
            try
            {
                TermsArr = new String[25];
                Load_Data("Select Terms from " + TblName + " where " + Condition, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Terms"]));
                }
                else
                {
                    SW.WriteLine(" " + Space(29));
                }
                SW.Close();

                for (i = 0; i < 25; i++)
                {
                    TermsArr[i] = String.Empty;
                }
                StreamReader SR = new StreamReader(Base_Dir + "\\Terms.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 24)
                    {
                        TermsArr[i] = SR.ReadLine();
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                SR.Close();
                TermsCount = i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Narration(String TblName, String Condition)
        {
            String Area = string.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            StreamWriter SW = new StreamWriter(Base_Dir + "\\Terms.txt");
            int i;
            try
            {
                TermsArr = new String[25];
                Load_Data("Select Narration from " + TblName + " where " + Condition, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Narration"]));
                }
                else
                {
                    SW.WriteLine(" " + Space(29));
                }
                SW.Close();

                for (i = 0; i < 25; i++)
                {
                    TermsArr[i] = String.Empty;
                }
                StreamReader SR = new StreamReader(Base_Dir + "\\Terms.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 24)
                    {
                        TermsArr[i] = SR.ReadLine();
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                SR.Close();
                TermsCount = i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ItemDetails(String TblName, String Condition)
        {
            String Area = string.Empty; String Str = String.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            StreamWriter SW = new StreamWriter(Base_Dir + "\\ItemDetails.txt");
            int i;
            try
            {
                ItemDetailsArr = new String[250];
                if (TblName.ToUpper() == "DC_DETAILS" || TblName.ToUpper() == "INVOICE_DETAILS" || TblName.ToUpper() == "INVOICE_CASH_DETAILS" || TblName.ToUpper() == "INVOICE_PROFORMA_DETAILS")
                {
                    Load_Data("Select Item_Desc from " + TblName + " where " + Condition, ref Dt);
                }
                else
                {
                    Load_Data("Select Item_Details from " + TblName + " where " + Condition, ref Dt);
                }
                if (Dt.Rows.Count > 0)
                {
                    if (TblName.ToUpper() == "DC_DETAILS" || TblName.ToUpper() == "INVOICE_DETAILS" || TblName.ToUpper() == "INVOICE_CASH_DETAILS" || TblName.ToUpper() == "INVOICE_PROFORMA_DETAILS")
                    {
                        SW.WriteLine(Convert.ToString(Dt.Rows[0]["Item_Desc"]).Trim().Replace("`", "'"));
                    }
                    else
                    {
                        SW.WriteLine(Convert.ToString(Dt.Rows[0]["Item_Details"]).Trim().Replace("`", "'"));
                    }
                }
                else
                {
                    SW.WriteLine(" " + Space(29));
                }
                SW.Close();

                for (i = 0; i < 249; i++)
                {
                    ItemDetailsArr[i] = String.Empty;
                }
                StreamReader SR = new StreamReader(Base_Dir + "\\ItemDetails.txt");
                i = 0; 
                while (SR.EndOfStream == false)
                {
                    if (i < 249)
                    {
                        Str = SR.ReadLine();
                        Str = Str.Replace("", String.Empty);
                        Str = Str.Replace("ÿ", String.Empty);
                        Str = Str.Replace("Ÿ", String.Empty);
                        if (Str.Length == 1)
                        {
                            Str = Str.Replace("X", String.Empty);
                        }
                        ItemDetailsArr[i] = Str;
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                SR.Close();
                ItemDetailsCount = i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Ledger_Address(int LedgerCode)
        {
            String Area = string.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            //StreamWriter SW = new StreamWriter(System.Windows.Forms.Application.StartupPath + "\\CusArea.txt");
            StreamWriter SW = new StreamWriter(Base_Dir + "\\LedArea.txt");
            int i;
            try
            {
                CusAddress = new String[5];
                Load_Data("Select C1.LedgerName Name, C3.LAddress Address, c2.Area_Name Area  from LedgerMas C1 left join Ledaddress c3 on c1.ledgerCode = c3.ledgercode left join Area_Master c2 on c3.AreaCode = C2.Area_Code where c1.LedgerCode = " + LedgerCode, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Name"]));
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Address"]));
                    Area = Convert.ToString(Dt.Rows[0]["Area"]);
                    //SW.WriteLine();
                }
                else
                {
                    SW.WriteLine(" " + Space(29));
                    SW.WriteLine(" " + Space(29));
                    SW.WriteLine(Space(29));
                }
                SW.Close();

                for (i = 0; i < 5; i++)
                {
                    CusAddress[i] = String.Empty;
                }

                StreamReader SR = new StreamReader(Base_Dir + "\\LedArea.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 4)
                    {
                        CusAddress[i] = SR.ReadLine();
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                CusAddress[i] = Area;
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void JobWorker_Address(int LedgerCode)
        {
            String Area = string.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            StreamWriter SW = new StreamWriter(Base_Dir + "\\JoBwArea.txt");
            int i;
            try
            {
                CusAddress = new String[5];
                Load_Data("Select C1.Supplier_Name Name, concat(c1.supplier_Address1, c1.Supplier_Address2) Address, c2.Area_Name Area  from JobWorker_Master C1 left join Area_Master c2 on c1.Supplier_City_Code = C2.Area_Code where c1.Supplier_Code = " + LedgerCode, ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Name"]));
                    SW.WriteLine(Convert.ToString(Dt.Rows[0]["Address"]));
                    Area = Convert.ToString(Dt.Rows[0]["Area"]);
                    //SW.WriteLine();
                }
                else
                {
                    SW.WriteLine(" " + Space(29));
                    SW.WriteLine(" " + Space(29));
                    SW.WriteLine(Space(29));
                }
                SW.Close();

                for (i = 0; i < 5; i++)
                {
                    CusAddress[i] = String.Empty;
                }

                StreamReader SR = new StreamReader(Base_Dir + "\\JoBwArea.txt");
                i = 0;
                while (SR.EndOfStream == false)
                {
                    if (i < 4)
                    {
                        CusAddress[i] = SR.ReadLine();
                        i += 1;
                    }
                    else
                    {
                        break;
                    }
                }
                CusAddress[i] = Area;
                SR.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Amount_Format(Double Number)
        {
            return String.Format("{0:##,##,##,###.00}", Number);
        }

        public void Row_Number(ref MyDataGridView DGV)
        {
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    DGV[0, i].Value = i + 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DBFCn_Close()
        {
            try
            {
                if (DBFCn.State == ConnectionState.Open)
                {
                    DBFCn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void BackupCn_Close()
        {
            try
            {
                if (BackupCn.State == ConnectionState.Open)
                {
                    BackupCn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Set_Print_Flag(String TBLName, String Condition)
        {
            try
            {
                if (Check_Table(TBLName))
                {
                    if (Check_TableField(TBLName, "Print_Flag") == false)
                    {
                        Execute("Alter table " + TBLName + " add Print_Flag int Null");
                    }

                    Execute("Update " + TBLName + " Set Print_Flag = Isnull(Print_Flag, 0) + 1 where " + Condition);
                }
                else
                {
                    MessageBox.Show("Invalid Table Name ...!", "Print");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Get_Print_Flag(String TBLName, String Condition)
        {
            try
            {
                if (Check_Table(TBLName))
                {
                    if (Check_TableField(TBLName, "Print_Flag") == false)
                    {
                        Execute("Alter table " + TBLName + " add Print_Flag int Null");
                    }
                    SqlCn_Open();
                    SqlCommand Cmd = new SqlCommand("Select (Isnull(Print_Flag, 0) + 1) Flag from " + TBLName + " where " + Condition, SqlCn);
                    return Convert.ToInt32(Cmd.ExecuteScalar());
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                SqlCn_Close();
            }
        }


        public System.Data.DataTable Load_Data(String Sql, ref System.Data.DataTable Dt)
        {
            try
            {
                Cn_Open();
                Dt.Clear();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, Cn);
                SqlCmd.CommandTimeout = 800;
                OdbcDataAdapter ADP = new OdbcDataAdapter(SqlCmd);
                ADP.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public System.Data.DataTable Load_Data_BackupCn(String Sql, ref System.Data.DataTable Dt)
        {
            try
            {
                BackupCn_Open();
                Dt.Clear();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, BackupCn);
                OdbcDataAdapter ADP = new OdbcDataAdapter(SqlCmd);
                ADP.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                BackupCn_Close(); 
            }
        }

        public String Grid_Max(ref MyDataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            Decimal MaxValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (DGV[ColumnName, i].Value == DBNull.Value)
                        {
                            MaxValue = MaxValue + 0;
                        }
                        else
                        {
                            if (Convert.ToDecimal(DGV[ColumnName, i].Value) > MaxValue)
                            {
                                MaxValue = Convert.ToDecimal(DGV[ColumnName, i].Value);
                            }
                        }
                    }

                }
                return String.Format("{0:0.00}", MaxValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String Get_Date_Format(String Field)
        {
            try
            {
                return String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Field));
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public DataView Load_Data(String Sql, out DataView Dv)
        {
            try
            {
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, Cn);
                OdbcDataAdapter ADP = new OdbcDataAdapter(SqlCmd);
                ADP.Fill(Dt);
                Dv = new DataView(Dt);
                return Dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Chr(int CharCode)
        {
            try
            {
                return Convert.ToString(Convert.ToChar(CharCode));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView Load_DataWithAuto(String Sql, out DataView Dv)
        {
            try
            {
                Cn_Open();
                System.Data.DataTable TmpDt = new System.Data.DataTable();
                DataColumn Dc = new DataColumn("Id", Type.GetType("System.Int32"));
                Dc.AutoIncrement = true;
                Dc.AutoIncrementSeed = 0;
                Dc.AutoIncrementStep = 1;
                TmpDt.Columns.Add(Dc);
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, Cn);
                SqlCmd.CommandTimeout = 800;
                OdbcDataAdapter ADP = new OdbcDataAdapter(SqlCmd);
                ADP.Fill(Dt);
                TmpDt.Merge(Dt);
                Dv = new DataView(TmpDt);
                return Dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Table_Alias(String Query, String FieldName)
        {
            String Str = string.Empty;
            String Query1 = Query.ToUpper();
            String Query2 = String.Empty;
            String Query3 = String.Empty;
            String Alias1 = FieldName.ToUpper();
            try
            {
                Query1 = Query1.ToUpper().Replace("SELECT ", "");
                Query1 = Query1.ToUpper().Replace(" ,", ",");
                Query1 = Query1.ToUpper().Replace(", ", ",");
                if (Query1.Contains(Alias1))
                {
                    Query1 = Query1.Remove(Query1.IndexOf(Alias1) + Alias1.Length);
                    if (Query1.Contains(","))
                    {
                        Query1 = Query3.Replace(",", "").Trim();
                    }
                    Str = Query1;
                }
                else
                {
                    Str = FieldName;
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Org_Column_Name(string Query, String Alias)
        {
            String Str = string.Empty;
            String Query1 = Query.ToUpper();
            String Query2 = String.Empty;
            String Query3 = String.Empty;
            String Alias1 = " " + Alias.ToUpper();
            try
            {
                Query1 = Query1.ToUpper().Replace("SELECT ", "");
                Query1 = Query1.ToUpper().Replace(" ,", ",");
                Query1 = Query1.ToUpper().Replace(", ", ",");
                Query1 = Query1.ToUpper().Replace(" AS ", " ");
                if (Query1.Contains(Alias1))
                {
                    Query1 = Query1.Remove(Query1.IndexOf(Alias1));
                    if (Query1.Contains(","))
                    {
                        Query2 = Query1.Remove(Query1.LastIndexOf(","));
                        Query3 = Query1.Replace(Query2, " ");
                        Query1 = Query3.Replace(",", "").Trim();
                    }
                    Str = Query1;
                }
                else
                {
                    Str = Alias;
                }
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataView Load_DataWithAuto_SqlCn(String Sql, out DataView Dv)
        {
            String Str = string.Empty;
            try
            {
                SqlCn_Open();
                System.Data.DataTable TmpDt = new System.Data.DataTable();
                DataColumn Dc = new DataColumn("Id", Type.GetType("System.Int32"));
                Dc.AutoIncrement = true;
                Dc.AutoIncrementSeed = 0;
                Dc.AutoIncrementStep = 1;
                TmpDt.Columns.Add(Dc);
                System.Data.DataTable Dt = new System.Data.DataTable();
                SqlCommand SqlCmd = new SqlCommand(Sql, SqlCn);
                SqlCmd.CommandTimeout = 800;
                //SqlDataAdapter ADP = new SqlDataAdapter(SqlCmd);
                //ADP.Fill(Dt);
                SqlDataReader ADP = SqlCmd.ExecuteReader();
                Dt.Load(ADP);
                TmpDt.Merge(Dt);
                Dv = new DataView(TmpDt);
                return Dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public DataView Load_DataWithAuto_SizingCn(String Sql, out DataView Dv)
        {
            String Str = string.Empty;
            try
            {
                SizingCn_Open();
                System.Data.DataTable TmpDt = new System.Data.DataTable();
                DataColumn Dc = new DataColumn("Id", Type.GetType("System.Int32"));
                Dc.AutoIncrement = true;
                Dc.AutoIncrementSeed = 0;
                Dc.AutoIncrementStep = 1;
                TmpDt.Columns.Add(Dc);
                System.Data.DataTable Dt = new System.Data.DataTable();
                SqlCommand SqlCmd = new SqlCommand(Sql, SizingCn);
                SqlCmd.CommandTimeout = 800;
                //SqlDataAdapter ADP = new SqlDataAdapter(SqlCmd);
                //ADP.Fill(Dt);
                SqlDataReader ADP = SqlCmd.ExecuteReader();
                Dt.Load(ADP);
                TmpDt.Merge(Dt);
                Dv = new DataView(TmpDt);
                return Dv;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public DataSet Load_Data(String Sql, ref DataSet Ds)
        {
            try
            {
                Cn_Open();
                Ds.Clear();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, Cn);
                OdbcDataAdapter ADP = new OdbcDataAdapter(SqlCmd);
                ADP.Fill(Ds);
                return Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public System.Data.DataTable Load_Data(String Sql, System.Data.DataTable Dt, out OdbcDataAdapter adp)
        {
            try
            {
                Cn_Open();
                Dt.Clear();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, Cn);
                adp = new OdbcDataAdapter(SqlCmd);
                adp.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public DataSet Load_Data(String Sql, DataSet Ds, out OdbcDataAdapter adp)
        {
            try
            {
                Cn_Open();
                Ds.Clear();
                OdbcCommand SqlCmd = new OdbcCommand(Sql, Cn);
                adp = new OdbcDataAdapter(SqlCmd);
                adp.Fill(Ds);
                return Ds;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Save(String Sql)
        {
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand(Sql, Cn);
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        //public void Save(String Sql, String MaxQry)
        //{
        //    OdbcTransaction Trans;
        //    OdbcCommand Cmd = new OdbcCommand();
        //    Cn_Open();
        //    Trans = Cn.BeginTransaction();
        //    try
        //    {
        //        foreach (String Sql in Queries)
        //        {
        //            Cmd.Connection = Cn;
        //            Cmd.Transaction = Trans;
        //            Cmd.CommandText = Sql;
        //            Cmd.ExecuteNonQuery();
        //        }
        //        Trans.Commit();
        //        Cn_Close();
        //    }
        //    catch (Exception ex)
        //    {
        //        Trans.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Cn_Close();
        //    }
        //}

        public System.Data.DataTable Load_DataTableBackupCN(String Sql, out System.Data.DataTable Dt, String DataTableName)
        {
            try
            {
                BackupConnection_Initialize(false);
                BackupCn_Open();
                Dt = new System.Data.DataTable(DataTableName);
                OdbcCommand Cmd = new OdbcCommand(Sql, BackupCn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                BackupCn_Close();
            }
        }

        public System.Data.DataTable Load_DataTable(String Sql, out System.Data.DataTable Dt, String DataTableName)
        {
            try
            {
                Cn_Open();
                Dt = new System.Data.DataTable(DataTableName);
                OdbcCommand Cmd = new OdbcCommand(Sql, Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                return Dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close(); 
            }
        }

        public void Execute(String Sql)
        {
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand(Sql, Cn);
                Cmd.CommandTimeout = 800;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void DBFCN_Delete(String Sql)
        {
            try
            {
                DBFCn_Open();
                OdbcCommand Cmd = new OdbcCommand(Sql, DBFCn);
                Cmd.CommandTimeout = 800;
                Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DBFCn_Close();
            }
        }

        public int NoOfTimes_Available(String TblName, String FldName, String Value, Char strSnumNdatD)
        {
            try
            {
                int Count;
                Cn_Open();
                OdbcCommand Cmd;
                if (strSnumNdatD == 'N')
                {
                    Cmd = new OdbcCommand("Select Count(*) from " + TblName + " where " + FldName + " = " + Value, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Count(*) from " + TblName + " where " + FldName + " = '" + Value + "'", Cn);
                }
                Count = Convert.ToInt32(Cmd.ExecuteScalar());
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Double GetData_InNumber(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName)
        {
            try
            {
                Int64 Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    if (Dr[0] != DBNull.Value)
                    {
                        Value = Convert.ToInt64(Dr[0]);
                    }
                    else
                    {
                        Value = 0;
                    }
                }
                else
                {
                    Value = 0;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void validate_Grids(ref System.Data.DataTable Dt, ref DotnetVFGrid.MyDataGridView Grid)
        {
            try
            {
                if (Dt.Rows.Count == Grid.Rows.Count)
                {
                    Dt.Rows.RemoveAt(Dt.Rows.Count);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Double GetData_InNumberWC(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName, String Year_Code, Int32 CompCode)
        {
            try
            {
                Double Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "' and Company_COde = " + CompCode + " and Year_Code = '" + Year_Code + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToDouble(Dr[0]);
                }
                else
                {
                    Value = 0;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public Double GetData_InDecimal(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName, String CondtionCol, String ConditionVal)
        {
            try
            {
                Int64 Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "' and " + CondtionCol +" = '" + ConditionVal + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToInt64(Dr[0]);
                }
                else
                {
                    Value = 0;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Decimal GetData_InDecimal(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName)
        {
            try
            {
                Decimal Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToDecimal(Dr[0]);
                }
                else
                {
                    Value = 0;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Decimal GetData_InDecimal_WC(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName, int CompCode, String Year_Code)
        {
            try
            {
                Decimal Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "' and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToDecimal(Dr[0]);
                }
                else
                {
                    Value = 0;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        
        public String GetData_InString(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName)
        {
            try
            {
                String Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToString(Dr[0]);
                }
                else
                {
                    Value = String.Empty;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String GetData_InStringWC(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName, Int32 CompCode, String Year_Code)
        {
            try
            {
                String Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "' and Company_Code = " + CompCode + " and Year_Code = '" + Year_Code + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToString(Dr[0]);
                }
                else
                {
                    Value = String.Empty;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public DateTime GetData_InDate(String TblName, String Criteria_FldName, String Criteria_Value, String Get_FldName)
        {
            try
            {
                DateTime Value;
                DataRow Dr;
                Cn_Open();
                System.Data.DataTable Dt = new System.Data.DataTable();
                OdbcCommand Cmd = new OdbcCommand("Select " + Get_FldName + " from " + TblName + " where " + Criteria_FldName + " = '" + Criteria_Value + "'", Cn);
                OdbcDataAdapter Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Dt);
                if (Dt.Rows.Count >= 1)
                {
                    Dr = Dt.Rows[0];
                    Value = Convert.ToDateTime(Dr[0]);
                }
                else
                {
                    Value = Convert.ToDateTime("01/01/1900");
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Sum(ref System.Data.DataTable Dt, String ColName, Boolean DoubleDigit, int WithoutRow)
        {
            Decimal Val = 0;
            int i = 0;
            try
            {
                if (Dt != null)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        if (i != WithoutRow)
                        {
                            if (Dr[ColName] == DBNull.Value)
                            {
                                Val = Val + 0;
                            }
                            else
                            {
                                if (DoubleDigit == false)
                                {
                                    Val = Val + Math.Round(Convert.ToDecimal(Dr[ColName]), 2);
                                }
                                else
                                {
                                    Val = Val + Convert.ToDecimal(Dr[ColName]);
                                }
                            }
                        }
                        i += 1;
                    }
                }
                if (DoubleDigit == true)
                {
                    return String.Format("{0:0.00}", Val);
                }
                else
                {
                    return String.Format("{0:0}", Val);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Sum_Trible(ref System.Data.DataTable Dt, String ColName, Boolean TribleDigit)
        {
            Decimal Val = 0;
            try
            {
                if (Dt != null)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        if (Dr[ColName] == DBNull.Value)
                        {
                            Val = Val + 0;
                        }
                        else
                        {
                            if (TribleDigit == false)
                            {
                                Val = Val + Math.Round(Convert.ToDecimal(Dr[ColName]), 3);
                            }
                            else
                            {
                                Val = Val + Convert.ToDecimal(Dr[ColName]);
                            }
                        }
                    }
                }
                if (TribleDigit == true)
                {
                    return String.Format("{0:0.000}", Val);
                }
                else
                {
                    return String.Format("{0:0}", Val);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String Sum(ref System.Data.DataTable Dt, String ColName, Boolean DoubleDigit)
        {
            Decimal Val = 0;
            try
            {
                if (Dt != null)
                {
                    foreach (DataRow Dr in Dt.Rows)
                    {
                        if (Dr[ColName] == DBNull.Value)
                        {
                            Val = Val + 0;
                        }
                        else
                        {
                            if (DoubleDigit == false)
                            {
                                Val = Val + Math.Round(Convert.ToDecimal(Dr[ColName]), 2);
                            }
                            else
                            {
                                Val = Val + Convert.ToDecimal(Dr[ColName]);
                            }
                        }
                    }
                }
                if (DoubleDigit == true)
                {
                    return String.Format("{0:0.00}", Val);
                }
                else
                {
                    return String.Format("{0:0}", Val);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Sum(ref DataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue=0;
            try
            {
                for (int i=0;i<=DGV.Rows.Count-1;i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (DGV[ColumnName, i].Value == DBNull.Value)
                        {
                            SumValue = SumValue + 0;
                        }
                        else
                        {
                            SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                        }
                    }
                }
                return String.Format("{0:0.00}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Sum(ref MyDataGridView DGV, String ColumnName, String ConditionField, String ConditionValues, Boolean EqualOrNot, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[ColumnName, i].Value).Trim() != String.Empty)
                        {
                            if (EqualOrNot == true)
                            {
                                if (Convert.ToString(DGV[ConditionField, i].Value).Trim() == ConditionValues)
                                {
                                    SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                                }
                            }
                            else
                            {
                                if (Convert.ToString(DGV[ConditionField, i].Value).Trim() != ConditionValues)
                                {
                                    SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                                }
                            }
                        }
                    }
                }
                return String.Format("{0:0.00}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public String Sum(ref MyDataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[ColumnName, i].Value).Trim() != String.Empty)
                        {
                            SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                        }
                    }
                }
                return String.Format("{0:0.00}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String SumWithCondtion(ref MyDataGridView DGV, String ColumnName, String CheckColName, String Value, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[CheckColName, i].Value).Trim().ToUpper() == Value.ToUpper())
                        {
                            SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                        }
                    }
                }
                return String.Format("{0:0.00}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String CountWithCondtion(ref MyDataGridView DGV, String ColumnName, String CheckColName, String Value, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[CheckColName, i].Value).Trim().ToUpper() == Value.ToUpper())
                        {
                            SumValue = SumValue + 1;
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


        public String Count(ref DataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            int SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        SumValue += 1;
                    }
                }
                return String.Format("{0:0}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 PrintOptions()
        {
            try
            {
                FrmPrintOPtions Frm = new FrmPrintOPtions();
                Frm.StartPosition = FormStartPosition.CenterScreen;
                Frm.ShowDialog();
                return Frm.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Int32 Insert_Delete()
        {
            try
            {
                FrmInsert_Delete Frm = new FrmInsert_Delete();
                Frm.StartPosition = FormStartPosition.CenterScreen;
                Frm.ShowDialog();
                return Frm.Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Mail_Options(String FromID, String ToId, String Subject, params String[] Attach)
        {
            try
            {
                FrmMail Frm = new FrmMail();
                Frm.StartPosition = FormStartPosition.CenterScreen;
                Frm.Mail_Initialize(FromID, ToId, Subject, Attach);
                Frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Count(ref MyDataGridView DGV, String Condition_ColumnName, String NotEqualTo)
        {
            int SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (DGV[Condition_ColumnName, i].Value != null )
                    {
                        if (DGV[Condition_ColumnName, i].Value.ToString() != String.Empty)
                        {
                            if (DGV[Condition_ColumnName, i].Value.ToString() != NotEqualTo)
                            {
                                SumValue += 1;
                            }
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

        public String Count(ref MyDataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            int SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        SumValue += 1;
                    }
                }
                return String.Format("{0:0}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public Boolean Check_EmptyinDataGridView(ref DataGridView DGV, int RowIndex, params String[] ColumnNames)
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
        
        public void ReadOnly_Grid(ref MyDataGridView DGV, params String[] ColumnNames)
        {
            try
            {
                foreach (String Sql in ColumnNames)
                {
                    DGV.Columns[Sql].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Current_Balance_InGrid(ref DataGridView DGV)
        {
            Double Credit = 0, Debit = 0;
            try
            {
                if (DGV["Debit", DGV.Rows.Count - 1].Value != DBNull.Value)
                {
                    Debit = Convert.ToDouble(DGV["Debit", DGV.Rows.Count - 1].Value);
                }
                else
                {
                    Debit = 0;
                }
                if (DGV["Credit", DGV.Rows.Count - 1].Value != DBNull.Value)
                {
                    Credit = Convert.ToDouble(DGV["Credit", DGV.Rows.Count - 1].Value);
                }
                else
                {
                    Credit = 0;
                }
                if (Debit > Credit)
                {
                    if (Convert.ToDouble(Debit - Credit) >= 1000)
                    {
                        return string.Format("{0:0,000.00}", Debit - Credit) + " Dr";
                    }
                    else
                    {
                        return string.Format("{0:0.00}", Debit - Credit) + " Dr";
                    }
                }
                else
                {
                    if (Convert.ToDouble(Credit - Debit) >= 1000)
                    {
                        return string.Format("{0:0,000.00}", Credit - Debit) + " Cr";
                    }
                    else
                    {
                        return string.Format("{0:0.00}", Credit - Debit) + " Cr";
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReadOnly_Grid_Without(ref MyDataGridView DGV, params String[] ColumnNames)
        {
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    Dc.ReadOnly = true;
                }
                foreach (String Sql in ColumnNames)
                {
                    DGV.Columns[Sql].ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void ReadOnly_Grid(ref DataGridView DGV, params String[] ColumnNames)
        {
            try
            {
                foreach (String Sql in ColumnNames)
                {
                    DGV.Columns[Sql].ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean DataAvailable(ref System.Data.DataTable Dt, String ColName, String Value)
        {
            try
            {
                for (int i=0;i<=Dt.Rows.Count-1;i++)
                {
                    if (Convert.ToString(Dt.Rows[i][ColName]) == Value)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean DataAvailable(ref System.Data.DataTable Dt, String ColName1, String Value1, String ColName2, String Value2)
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Convert.ToString(Dt.Rows[i][ColName1]) == Value1 && Convert.ToString(Dt.Rows[i][ColName1]) == Value1)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReadOnly_Grid_Without(ref DataGridView DGV, params String[] ColumnNames)
        {
            try
            {
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    Dc.ReadOnly = true;
                }
                foreach (String Sql in ColumnNames)
                {
                    DGV.Columns[Sql].ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Double Sum(String TblName, String FldName, String Condition)
        {
            try
            {
                Double Value;
                Cn_Open();
                OdbcCommand Cmd;
                if (Condition.Trim() == String.Empty)
                {
                    Cmd = new OdbcCommand("Select Sum(" + FldName + ") from " + TblName, Cn);
                }
                else
                {
                    Cmd = new OdbcCommand("Select Sum(" + FldName + ") from " + TblName + " where " + Condition, Cn);
                }
                if (Cmd.ExecuteScalar() != DBNull.Value)
                {
                    Value = Convert.ToDouble(Cmd.ExecuteScalar());
                }
                else
                {
                    Value = 0;
                }
                return Value;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        //public Double  Max(String TblName, String FldName, String Condition)
        //{
        //    try
        //    {
        //        Double Value;
        //        Cn_Open();
        //        OdbcCommand Cmd;
        //        if (Condition.Trim() == String.Empty)
        //        {
        //            Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName, Cn);
        //        }
        //        else
        //        {
        //            Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition, Cn);
        //        }
        //        if (Cmd.ExecuteScalar() != DBNull.Value)
        //        {
        //            Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
        //        }
        //        else
        //        {
        //            Value = 1;
        //        }
        //        return Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Cn_Close();
        //    }
        //}

        //public Double Max(String TblName, String FldName, String Condition, Boolean Increment)
        //{
        //    try
        //    {
        //        Double Value;
        //        Cn_Open();
        //        OdbcCommand Cmd;
        //        if (Condition.Trim() == String.Empty)
        //        {
        //            Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName, Cn);
        //        }
        //        else
        //        {
        //            Cmd = new OdbcCommand("Select Max(" + FldName + ") from " + TblName + " where " + Condition, Cn);
        //        }
        //        if (Cmd.ExecuteScalar() != DBNull.Value)
        //        {
        //            if (Increment == true)
        //            {
        //                Value = Convert.ToDouble(Cmd.ExecuteScalar()) + 1;
        //            }
        //            else
        //            {
        //                Value = Convert.ToDouble(Cmd.ExecuteScalar());
        //            }
        //        }
        //        else
        //        {
        //            if (Increment == true)
        //            {
        //                Value = 1;
        //            }
        //            else
        //            {
        //                Value = 0;
        //            }
        //        }
        //        return Value;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        Cn_Close();
        //    }
        //}
                
        public Boolean CheckItem_AlreadyAvailable(ref DataGridView DGV, String Item, String ColumnName, int RowIndex)
        {
            Boolean Flag=false;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Convert.ToString(DGV[ColumnName, i].Value).ToUpper() == Item.ToUpper() && i != RowIndex)
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
        
        public Boolean CheckItem_AlreadyAvailable(ref MyDataGridView DGV, String Item, String ColumnName, int RowIndex)
        {
            Boolean Flag = false;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Convert.ToString(DGV[ColumnName, i].Value).ToUpper() == Item.ToUpper() && i != RowIndex)
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

        public Boolean CheckItem_AlreadyAvailableSR(ref MyDataGridView DGV, String Item, String ColumnName, int RowIndex, String ConditionCol1, String ConditionVal1, String ConditionCol2, String ConditionVal2)
        {
            Boolean Flag = false;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Convert.ToString(DGV[ColumnName, i].Value).ToUpper() == Item.ToUpper() && i != RowIndex)
                    {
                        if (Convert.ToString(DGV[ConditionCol1, i].Value).ToUpper() == ConditionVal1.ToUpper() && Convert.ToString(DGV[ConditionCol2, i].Value).ToUpper() == ConditionVal2.ToUpper())
                        {
                            Flag = true;
                            break;
                        }
                    }
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Check_Stock(String ItemCode)
        {
            try
            {
                String Qty;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select Qty from Stock where item_Code = '" + ItemCode + "'", Cn);
                if (Cmd.ExecuteScalar() == null)
                {
                    Qty = "0";
                }
                else
                {
                    Qty = Convert.ToString(Cmd.ExecuteScalar());
                }
                return Qty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Check_GRNReturnStock(String ItemCode)
        {
            try
            {
                String Qty;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select Qty from GSNReturnStock where item_Code = '" + ItemCode + "'", Cn);
                if (Cmd.ExecuteScalar() == null)
                {
                    Qty = "0";
                }
                else
                {
                    Qty = Convert.ToString(Cmd.ExecuteScalar());
                }
                return Qty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String[] UpdatePrice(out String[] ReturnQueries, ref System.Data.DataTable Dt, String ItemColumnName, String NewPriceColumn)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update GSN_ACCeptance_Details set S_Price = " + Dt.Rows[i][NewPriceColumn] + " where item_No = '" + Dt.Rows[i][ItemColumnName] + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public Boolean Fill_RowWithTemp_LR(ref MyDataGridView DGV, int RowIndex, String LR_No, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            double Discount = 0;
            Boolean Flag = false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from Temp_LR where LR_NO = '" + LR_No + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = "0";
                    DGV[1, RowIndex].Value = LR_No;
                    i = 2;
                    foreach (String Sql in OrderByCols)
                    {
                        DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        // Not Necessary have to Remove
        
        public Boolean Fill_RowWithItem(ref DataGridView DGV, int RowIndex, int Slno, String ItemCode, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            double Discount = 0;
            Boolean Flag=false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from Stock where Item_Code = '" + ItemCode + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = Slno;
                    DGV[1, RowIndex].Value = ItemCode;
                    i = 2;
                    foreach (String Sql in OrderByCols)
                    {
                        if (Sql.ToUpper() == "DIS_PER")
                        {
                            Discount = GetData_InNumber("Item_Discount_Master", "ItemCode", ItemCode, "Discount_Percentage");
                            if (Discount == 0)
                            {
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            }
                            else
                            {
                                DGV[i, RowIndex].Value = Discount;
                            }
                        }
                        else
                        {
                            DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                        }
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Fill_RowWithItem(ref DotnetVFGrid.MyDataGridView DGV, int RowIndex, int Slno, String ItemCode, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            Boolean Flag = false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from Stock where Item_Code = '" + ItemCode + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = Slno;
                    DGV[1, RowIndex].Value = ItemCode;
                    i = 2;
                    foreach (String Sql in OrderByCols)
                    {
                        DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Fill_RowWithGSNReturnItem(ref DotnetVFGrid.MyDataGridView DGV, int RowIndex, int Slno, String ItemCode, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            Boolean Flag = false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from GSNReturnStock1 where Item_Code = '" + ItemCode + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = Slno;
                    DGV[1, RowIndex].Value = Dt.Rows[0]["GSN_Slno"];
                    DGV[2, RowIndex].Value = ItemCode;
                    i = 3;
                    foreach (String Sql in OrderByCols)
                    {
                        if (Sql.ToUpper().Contains("EMPTY0"))
                        {
                            DGV[i, RowIndex].Value = 0;
                        }
                        else if (Sql.ToUpper().Contains("EMPTY-"))
                        {
                            DGV[i, RowIndex].Value = "-";
                        }
                        else
                        {
                            DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                        }
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Fill_RowWithItemJO(ref MyDataGridView DGV, int RowIndex, String Ec, int Slno, String ItemCode, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            Boolean Flag = false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from Stock1 where Item_Code = '" + ItemCode + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = Slno;
                    DGV[3, RowIndex].Value = ItemCode;
                    i = 4;
                    foreach (String Sql in OrderByCols)
                    {
                        if (Sql.Trim().ToUpper() == "EC")
                        {
                            DGV[i, RowIndex].Value = Ec;
                        }
                        else if (Sql.ToUpper() == "PRICE")
                        {
                            //if (Get_RecordCount("Item_Price_Modification_Log", "Item_Code = '" + ItemCode + "' and Modification_Date = (Select Max(Modification_Date) from Item_Price_Modifcation_Log where item_Code = '" + ItemCode + "')") > 0)
                            //{
                            //    //DGV[i, RowIndex].Value = GetData_InDecimal("Item_Price_Modification_Log", "Item_Code", Convert.ToString(ItemCode), "New_Value");
                            //    DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            //}
                            //else
                            //{
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            //}
                        }
                        else if (Sql.ToUpper() == "DIS_PER")
                        {
                            Load_Data("select i1.itemCode, i1.Discount_Code, i1.discount_percentage, d1.Discount_From, d1.discount_to from item_discount_master i1 left join discount_master d1 on  d1.discount_Code = i1.discount_Code where i1.itemCode = '" + ItemCode + "' order by d1.discount_Code desc", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                if (Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Dt1.Rows[0]["Discount_From"])) <= Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", DateTime.Now)) && Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Dt1.Rows[0]["Discount_To"])) >= Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", DateTime.Now)))
                                {
                                    DGV[i, RowIndex].Value = Dt1.Rows[0]["Discount_Percentage"];
                                }
                                else
                                {
                                    DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                                }
                            }
                            else
                            {
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            }
                        }
                        else
                        {
                            if (Sql.ToUpper() == "QMT")
                            {
                                if (Convert.ToDecimal(Dt.Rows[0][Sql]) > 1)
                                {
                                    DGV[i, RowIndex].Value = "1";
                                }
                                else
                                {
                                    DGV[i, RowIndex].Value = Convert.ToDecimal(Dt.Rows[0][Sql]);
                                }
                            }
                            else
                            {
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            }
                        }
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Fill_RowWithItem(ref MyDataGridView DGV, int RowIndex, String Ec, int Slno, String ItemCode, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            Boolean Flag = false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from Stock1 where Item_Code = '" + ItemCode + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = Slno;
                    DGV[1, RowIndex].Value = ItemCode;
                    i = 2;
                    foreach (String Sql in OrderByCols)
                    {
                        if (Sql.Trim().ToUpper() == "EC")
                        {
                            DGV[i, RowIndex].Value = Ec;
                        }
                        else if (Sql.ToUpper() == "PRICE")
                        {
                            //if (Get_RecordCount("Item_Price_Modification_Log", "Item_Code = '" + ItemCode + "' and Modification_Date = (Select Max(Modification_Date) from Item_Price_Modifcation_Log where item_Code = '" + ItemCode + "')") > 0)
                            //{
                            //    //DGV[i, RowIndex].Value = GetData_InDecimal("Item_Price_Modification_Log", "Item_Code", Convert.ToString(ItemCode), "New_Value");
                            //    DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            //}
                            //else
                            //{
                            DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            //}
                        }
                        else if (Sql.ToUpper() == "DIS_PER")
                        {
                            
                            Load_Data("select i1.itemCode, i1.Discount_Code, i1.discount_percentage, d1.Discount_From, d1.discount_to from item_discount_master i1 left join discount_master d1 on d1.discount_Code = i1.discount_Code where i1.itemCode = '" + ItemCode + "' order by d1.discount_Code desc", ref Dt1);
                            if (Dt1.Rows.Count > 0)
                            {
                                if (Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Dt1.Rows[0]["Discount_From"])) <= Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", DateTime.Now)) && Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Dt1.Rows[0]["Discount_To"])) >= Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", DateTime.Now)))
                                {
                                    DGV[i, RowIndex].Value = Dt1.Rows[0]["Discount_Percentage"];
                                }
                                else
                                {
                                    DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                                }
                            }
                            else
                            {
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            }
                        }
                        else
                        {
                            if (Sql.ToUpper() == "QMT")
                            {
                                if (Convert.ToDecimal(Dt.Rows[0][Sql]) > 1)
                                {
                                    DGV[i, RowIndex].Value = "1";
                                }
                                else
                                {
                                    DGV[i, RowIndex].Value = Convert.ToDecimal(Dt.Rows[0][Sql]);
                                }
                            }
                            else
                            {
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            }
                        }
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean isStockUpdate()
        {
            double Qty_V, Qty_T;
            try
            {
                if (Check_Table("Stock1") == false)
                {
                    return false;
                }
                Qty_V = Get_RecordCount("Stock", "");
                Qty_T = Get_RecordCount("Stock1", "");
                if (Qty_T == Qty_V)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void stockRole()
        {
            try
            {
                string Sql = string.Empty;
                Sql = "Select a1.Item_Code, A1.Qty, g1.item_no, g1.item_Style, g1.item_Size, A1.Qty as QMT, g1.PCI, g1.S_Price as Price,g1.S_Price as Amount,g1.DIS_Per,g1.DIS_amount,'-' as EC,g1.TAx_Desc,g1.tax_per,g1.Supplier_Code, S1.Style_Name, S2.Size_name, t1.Tax_Name, A1.LOCATION_CODE AS LOCATION, g1.item_ID from acc_Stock a1 left join GSn_Acceptance_details g1 on a1.Item_Code = g1.item_No left join Style_Master s1 on g1.Item_Style = s1.style_Code left join Size_master s2 on g1.Item_Size = S2.Size_Code left join Tax_master t1 on g1.tax_Desc = T1.tax_Code where a1.Qty > 0 and g1.S_Price>0 ";
                Execute_Qry(Sql, "Stock");
                if (Check_Table("Stock1"))
                {
                    Execute("Drop table stock1");
                }
                Execute("Create table stock1 as select * from stock");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Fill_RowWithItemOB(ref MyDataGridView DGV, int RowIndex, String Ec, int Slno, String ItemCode, params String[] OrderByCols)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            int i = 0;
            System.Data.DataTable Dt1 = new System.Data.DataTable();
            Boolean Flag = false;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("Select * from Stock1 where Item_Code = '" + ItemCode + "'", Cn);
                OdbcDataAdapter adp = new OdbcDataAdapter(Cmd);
                adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    Flag = false;
                }
                else
                {
                    DGV[0, RowIndex].Value = Slno;
                    DGV[1, RowIndex].Value = ItemCode;
                    i = 2;
                    foreach (String Sql in OrderByCols)
                    {
                        if (Sql.Trim().ToUpper() == "EC")
                        {
                            DGV[i, RowIndex].Value = Ec;
                        }
                        //else if (Sql.ToUpper() == "DIS_PER")
                        //{
                        //    Load_Data("select i1.itemCode, i1.Discount_Code, i1.discount_percentage, d1.Discount_From, d1.discount_to from item_discount_master i1 left join discount_master d1 on  d1.discount_Code = i1.discount_Code where i1.itemCode = '" + ItemCode + "' order by d1.discount_Code desc", ref Dt1);
                        //    if (Dt1.Rows.Count > 0)
                        //    {
                        //        if (Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Dt1.Rows[0]["Discount_From"])) <= Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", DateTime.Now)) && Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", Dt1.Rows[0]["Discount_To"])) >= Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy}", DateTime.Now)))
                        //        {
                        //            DGV[i, RowIndex].Value = Dt1.Rows[0]["Discount_Percentage"];
                        //        }
                        //        else
                        //        {
                        //            DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                        //        }
                        //    }
                        //    else
                        //    {
                        //        DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                        //    }
                        //}
                        else
                        {
                            if (Sql.ToUpper() == "QMT")
                            {
                                if (Convert.ToDecimal(Dt.Rows[0][Sql]) > 1)
                                {
                                    DGV[i, RowIndex].Value = "1";
                                }
                                else
                                {
                                    DGV[i, RowIndex].Value = Convert.ToDecimal(Dt.Rows[0][Sql]);
                                }
                            }
                            else
                            {
                                DGV[i, RowIndex].Value = Dt.Rows[0][Sql];
                            }
                        }
                        i += 1;
                    }
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                Flag = false;
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Fill_RowWithDCItem(ref MyDataGridView DGV, int RowIndex, String Ec, int Slno, String ItemCode, params String[] OrderByCols)
        {
            int i = 0;
            try
            {
                DGV[0, RowIndex].Value = Slno;
                DGV[1, RowIndex].Value = ItemCode;
                i = 2;
                foreach (String Sql in OrderByCols)
                {
                    if (Sql.Trim().ToUpper() == "EC")
                    {
                        DGV[i, RowIndex].Value = Ec;
                    }
                    else
                    {
                        DGV[i, RowIndex].Value = Sql;
                    }
                    i += 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        // upto
        
        public void Fill_RowWithJOItem(ref MyDataGridView DGV, int RowIndex, params String[] OrderByCols)
        {
            int i = 0;
            try
            {
                foreach (String Sql in OrderByCols)
                {
                    DGV[i, RowIndex].Value = Sql;
                    i += 1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Company_Address(int CompCode)
        {
            String Str = String.Empty;
            try
            {
                StreamWriter SW = new StreamWriter(Base_Dir + "\\CusArea.txt");
                SW.WriteLine (GetData_InString("Socks_Companymas", "CompCode", CompCode.ToString(), "Compaddress"));
                SW.Close();

                StreamReader Read = new StreamReader(Base_Dir + "\\CusArea.txt");
                while (Read.EndOfStream == false)
                {
                    if (Str == String.Empty)
                    {
                        Str = Read.ReadLine();
                    }
                    else
                    {
                        Str += Read.ReadLine();
                    }
                }
                Read.Close();
                return Str;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double Get_Balance(DateTime UpTodate, int Ledger_Code, int CompCode, String Year_Code)
        {
            String Str = String.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Str = "select null VDate, Null Voucher, null vcode, null vno, null Mode, null Vmode, null Ledger, Ledger_Ocredit Credit, Ledger_Odebit Debit, ' ' as Narration from ledger_Master where ledger_Code = " + Ledger_Code + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' union ";
                Str += " select v2.User_Date VDate, null Voucher, v1.vcode, v2.vno, v1.Byto Mode, v2.Vmode, l2.ledger_Name Ledger, v1.credit, v1.debit, v1.Narration from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate=v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and  v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join ledger_Master l2 on v1.rev_ledCode = l2.ledger_Code and v1.company_Code = l2.company_code and v1.year_Code = l2.year_Code where v2.User_Date <= '" + String.Format("{0:dd-MMM-yyyy}", UpTodate.AddDays(-1)) + "' and v1.ledger_Code = " + Ledger_Code + " and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' ";
                Execute_Qry(Str, "XLed");
                Load_Data("select (case when sum(Credit) > Sum(Debit) then sum(Debit) - sum(Credit) else sum(Debit) - sum(Credit) end) Amount from xled", ref Dt);
                if (Dt.Rows[0]["Amount"] == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDouble(Dt.Rows[0]["Amount"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public double Get_Balance_WO_OpBal(DateTime UpTodate, int Ledger_Code, int CompCode, String Year_Code)
        {
            String Str = String.Empty;
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                //Str = "select null VDate, Null Voucher, null vcode, null vno, null Mode, null Vmode, null Ledger, Ledger_Ocredit Credit, Ledger_Odebit Debit, ' ' as Narration from ledger_Master where ledger_Code = " + Ledger_Code + " and company_Code = " + CompCode + " and year_Code = '" + Year_Code + "' union ";
                Str = " select v2.User_Date VDate, null Voucher, v1.vcode, v2.vno, v1.Byto Mode, v2.Vmode, l2.ledger_Name Ledger, v1.credit, v1.debit, v1.Narration from voucher_details v1 left join voucher_master v2 on v1.vcode = v2.vcode and v1.vdate=v2.vdate and v1.company_Code = v2.company_Code and v1.year_Code = v2.year_Code left join ledger_master l1 on v1.ledger_Code = l1.ledger_Code and  v1.company_Code = l1.company_Code and v1.year_Code = l1.year_Code left join ledger_Master l2 on v1.rev_ledCode = l2.ledger_Code and v1.company_Code = l2.company_code and v1.year_Code = l2.year_Code where v2.User_Date <= '" + String.Format("{0:dd-MMM-yyyy}", UpTodate.AddDays(-1)) + "' and v1.ledger_Code = " + Ledger_Code + " and v1.company_Code = " + CompCode + " and v1.year_Code = '" + Year_Code + "' ";
                Execute_Qry(Str, "XLed");
                Load_Data("select (case when sum(Credit) > Sum(Debit) then sum(Debit) - sum(Credit) else sum(Debit) - sum(Credit) end) Amount from xled", ref Dt);
                if (Dt.Rows[0]["Amount"] == null)
                {
                    return 0;
                }
                else
                {
                    return Convert.ToDouble(Dt.Rows[0]["Amount"]);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        public void Execute_Qry(String Sql, String QryName)
        {
            try
            {
                Drop(QryName, "View");
                Drop(QryName, "Table");
                Cn_Open();
                OdbcCommand Cmd2 = new OdbcCommand("Create View " + QryName.ToUpper() + " as " + Sql, Cn);
                Cmd2.CommandTimeout = 800;
                Cmd2.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public string PadL(String Str, Int32 Size)
        {
            if (Str.Length > Size)
            {
                return Str.Substring(0, Size);
            }
            else
            {
                return Space(Size - Str.Length) + Str;
            }
        }

        public String Fill(Char C, int Size)
        {
            String Str = String.Empty;
            for (int i = 0; i < Size - 1; i++)
            {
                Str += C;
            }
            return Str;
        }

        public string PadM(String Str, Int32 Size)
        {
            int y;
            if (Str.Length > Size)
            {
                return Str.Substring(0, Size);
            }
            else
            {
                y = (Size/2) - (Str.Length/2);
                Str = Space(y) + Str + Space(y);
                if (Str.Length > Size)
                {
                    return Str.Substring(0, Size);
                }
                else
                {
                    return Str;
                }
            }
        }
        
        public string PadR(String Str, Int32 Size)
        {
            if (Str.Length > Size)
            {
                return Str.Substring(0, Size);
            }
            else
            {
                return Str + Space(Size - Str.Length);
            }
        }
        
        public void Add_NewField(String TblName, String FldName, String DataType)
        {
            try
            {
                if (Check_Table(TblName) == true)
                {
                    Cn_Open();
                    //OdbcCommand Cmd = new OdbcCommand("select * from user_tab_Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcCommand Cmd = new OdbcCommand("select * from information_Schema.Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcDataReader Rd = Cmd.ExecuteReader();
                    if (Rd.HasRows == false)
                    {
                        String Sql = @"Alter table " + TblName.ToUpper() + " add " + FldName.ToUpper() + " " + DataType;
                        OdbcCommand cmd = new OdbcCommand(Sql, Cn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }
        
        public Boolean Check_TableField(String TblName, String FldName)
        {
            try
            {
                if (Check_Table(TblName) == true)
                {
                    Cn_Open();
                    //OdbcCommand Cmd = new OdbcCommand("select * from user_tab_Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcCommand Cmd = new OdbcCommand("select * from Information_Schema.Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcDataReader Rd = Cmd.ExecuteReader();
                    if (Rd.HasRows == false)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public Boolean Check_TableField_OtherDB(String DBName, String TblName, String FldName)
        {
            try
            {
                if (Check_Table_OtherDb(DBName, TblName) == true)
                {
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand("select * from  " + DBName + ".Information_Schema.Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcDataReader Rd = Cmd.ExecuteReader();
                    if (Rd.HasRows == false)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

       
        public void Drop(String ObjName, String Type)
        {
            try
            {
                Cn_Open();
                if (Type.ToUpper() == "TABLE")
                {
                    ////OdbcCommand Cmd = new OdbcCommand("Select * from Tab where tabType = 'TABLE' and TName = '" + ObjName.ToUpper() + "'", Cn);
                    //OdbcCommand Cmd = new OdbcCommand("Select * from Sysobjects where Xtype = 'U' and Name = '" + ObjName.ToUpper() + "'", Cn);
                    //OdbcDataReader Rd = Cmd.ExecuteReader();
                    //if (Rd.HasRows == true)
                    //{
                    //    OdbcCommand Cmd1 = new OdbcCommand("Drop Table " + ObjName.ToUpper(), Cn);
                    //    Cmd1.ExecuteNonQuery();
                    //}
                    if (Check_Table (ObjName))
                    {
                        Execute("Drop table " + ObjName);
                    }
                }
                else if (Type.ToUpper() == "VIEW")
                {
                    ////OdbcCommand Cmd = new OdbcCommand("Select * from Tab where tabType = 'VIEW' and TName = '" + ObjName.ToUpper() + "'", Cn);
                    //Check_Table
                    //OdbcCommand Cmd = new OdbcCommand("Select * from Sysobjects where XType = 'V' and Name = '" + ObjName.ToUpper() + "'", Cn);
                    //OdbcDataReader Rd = Cmd.ExecuteReader();
                    //if (Rd.HasRows == true)
                    //{
                    //    Execute("Drop view " + ObjName);
                    //}
                    if (Check_View(ObjName))
                    {
                        Execute("Drop view " + ObjName);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Remove_Field(String TblName, String FldName)
        {
            try
            {
                if (Check_Table(TblName) == true)
                {
                    Cn_Open();
                    //OdbcCommand Cmd = new OdbcCommand("select * from user_tab_Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcCommand Cmd = new OdbcCommand("select * from Information_Schema.Columns where Table_name = '" + TblName.ToUpper() + "' and Column_name = '" + FldName.ToUpper() + "'", Cn);
                    OdbcDataReader Rd = Cmd.ExecuteReader();
                    if (Rd.HasRows == true)
                    {
                        OdbcCommand cmd = new OdbcCommand("Alter table " + TblName.ToUpper() + " Drop Column " + FldName.ToUpper(), Cn);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Update_Unique_Code_in_ENT()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                if (Check_Table("Ent077T") == false)
                {
                    Execute("Select 1 as Vcode, * into Ent077t from ent077");
                }
                Load_Data("Select distinct date, vno, mode from ENT077T order by date, Vno", ref Dt);
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Execute("Update ENt077t set Vcode = " + Convert.ToInt32(i + 1).ToString() + " where date = '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i]["date"])) + "' and vno = '" + Dt.Rows[i]["Vno"].ToString() + "' and mode = " + Dt.Rows[i]["mode"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_Table(String TblName)
        {
            try
            {
                Boolean Flag;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("select * from SysObjects where Name = '" + TblName.ToUpper() + "' and Xtype = 'U'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Check_Table_OtherDb (String DBName, String TblName)
        {
            try
            {
                Boolean Flag;
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand("select * from " + DBName + ".dbo.SysObjects where Name = '" + TblName.ToUpper() + "' and Xtype = 'U'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public Boolean Check_View(String TblName)
        {
            try
            {
                Boolean Flag;
                //Execute("Commit");
                Cn_Open();
                //OdbcCommand Cmd = new OdbcCommand("select * from user_tab_Columns where Table_name = '" + TblName.ToUpper() + "'", Cn);
                OdbcCommand Cmd = new OdbcCommand("select * from SysObjects where Name = '" + TblName.ToUpper() + "' and Xtype = 'V'", Cn);
                OdbcDataReader Rd = Cmd.ExecuteReader();
                if (Rd.HasRows == true)
                {
                    Flag = true;
                }
                else
                {
                    Flag = false;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Copy_Table_WithDatas(String SourceTblName, String NewTableName, String Condition)
        {
            try
            {
                if (Check_Table(SourceTblName) == true)
                {
                    if (Check_Table(NewTableName) == true)
                    {
                        Drop(NewTableName, "Table");
                    }
                    Cn_Open();
                    OdbcCommand Cmd;
                    if (Condition.Trim() == String.Empty)
                    {
                        Cmd = new OdbcCommand("Create Table " + NewTableName + " as Select * from " + SourceTblName, Cn);
                    }
                    else
                    {
                        Cmd = new OdbcCommand("Create Table " + NewTableName + " as Select * from " + SourceTblName + " where " + Condition, Cn);
                    }
                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Fetch_DataFromTemp(String SourceTblName, String TempTblName, String Cond_Field, Int16 FromShopCode)
        {
            String Sql;
            try
            {
                if (Cond_Field.Trim() != String.Empty)
                {
                    Execute_Qry("Select * from " + SourceTblName + " where gsn_From_ShopCode = " + FromShopCode, "View_Source");
                    Cn_Open();
                    Sql = "Insert into " + SourceTblName + " Select * from " + TempTblName + " where " + Cond_Field + " NOT IN (Select " + Cond_Field + " From View_Source)";
                    OdbcCommand Cmd = new OdbcCommand(Sql, Cn);
                    Cmd.CommandTimeout = 800;
                    Cmd.ExecuteNonQuery();
                    Cn_Close();
                }
                else
                {
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand("Drop Table " + SourceTblName, Cn);
                    Cmd.ExecuteNonQuery();
                    //OdbcCommand Cmd1 = new OdbcCommand("Select * into " + SourceTblName + " from " + TempTblName, Cn);
                    OdbcCommand Cmd1 = new OdbcCommand("CREATE TABLE " + SourceTblName + " AS SELECT * from " + TempTblName, Cn);
                    Cmd1.ExecuteNonQuery();
                    Cn_Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Move_DatasToOracle(System.Data.DataTable Dt, String TblName)
        {
            String Str = "Insert into " + TblName + " values (",CDtime=String.Empty;
            DateTime Dtime = DateTime.Now;
            OdbcCommand Cmd;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count-1; i++)
                {
                    Cn_Open();
                    for (int j = 0; j <= Dt.Columns.Count; j++)
                    {
                        if (j == Dt.Columns.Count)
                        {
                            Str = Str.Substring(0, Str.Length - 1);
                            Str += ")";
                        }
                        else
                        {
                            if (Dt.Columns[j].DataType == System.Type.GetType("System.DateTime"))
                            {
                                Dtime = Convert.ToDateTime(Dt.Rows[i][j].ToString());
                                CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                                Str += "'" + CDtime + "',";
                            }
                            else
                            {
                                Str += "'" + Dt.Rows[i][j].ToString() + "',";
                            }
                        }
                    }
                    Cmd = new OdbcCommand(Str,Cn);
                    Cmd.ExecuteNonQuery();
                    Str = "Insert into " + TblName + " values (";
                    Cn_Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Move_DatasToSqlServer(System.Data.DataTable Dt, String TblName)
        {
            String Str = "Insert into " + TblName + " values (", CDtime = String.Empty;
            DateTime Dtime = DateTime.Now;
            OdbcCommand Cmd;
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    BackupCn_Open();
                    for (int j = 0; j <= Dt.Columns.Count; j++)
                    {
                        if (j == Dt.Columns.Count)
                        {
                            Str = Str.Substring(0, Str.Length - 1);
                            Str += ")";
                        }
                        else
                        {
                            if (Dt.Columns[j].DataType == System.Type.GetType("System.DateTime"))
                            {
                                Dtime = Convert.ToDateTime(Dt.Rows[i][j].ToString());
                                CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                                Str += "'" + CDtime + "',";
                            }
                            else
                            {
                                Str += "'" + Dt.Rows[i][j].ToString() + "',";
                            }
                        }
                    }
                    Cmd = new OdbcCommand(Str, BackupCn);
                    Cmd.ExecuteNonQuery();
                    Str = "Insert into " + TblName + " values (";
                    BackupCn_Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void SaveDataTbl_Table(System.Data.DataTable Dt, String TblName)
        {
            String Str = "Insert into " + TblName + " values (", CDtime = String.Empty;
            DateTime Dtime = DateTime.Now;
            OdbcCommand Cmd;
            try
            {
                Cn_Open();
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count; j++)
                    {
                        if (j == Dt.Columns.Count)
                        {
                            Str = Str.Substring(0, Str.Length - 1);
                            Str += ")";
                        }
                        else
                        {
                            if (Dt.Columns[j].DataType == System.Type.GetType("System.DateTime"))
                            {
                                Dtime = Convert.ToDateTime(Dt.Rows[i][j].ToString());
                                CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                                Str += "'" + CDtime + "',";
                            }
                            else
                            {
                                Str += "'" + Dt.Rows[i][j].ToString() + "',";
                            }
                        }
                    }
                    Cmd = new OdbcCommand(Str, Cn);
                    Cmd.ExecuteNonQuery();
                    Str = "Insert into " + TblName + " values (";
                }
                Cn_Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Insert_DBFRecords_SqlTable(out String[] ReturnQueries, ref System.Data.DataTable Dt, String TblName)
        {
            String Str = "Insert into " + TblName + " values (", CDtime = String.Empty;
            DateTime Dtime = DateTime.Now;
            String Val = string.Empty;
            OdbcCommand Cmd;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Dt.Columns.Count; j++)
                    {
                        if (j == Dt.Columns.Count)
                        {
                            Str = Str.Substring(0, Str.Length - 1);
                            Str += ")";
                        }
                        else
                        {
                            if (Dt.Columns[j].DataType == System.Type.GetType("System.DateTime"))
                            {
                                if (Dt.Rows[i][j] == DBNull.Value)
                                {
                                    Str += "'01-01-1899',";
                                }
                                else
                                {
                                    Dtime = Convert.ToDateTime(Dt.Rows[i][j].ToString().Trim());
                                    CDtime = String.Format("{0:dd/MMM/yy}", Dtime);
                                    Str += "'" + CDtime + "',";
                                }
                            }
                            else
                            {
                                Val = Dt.Rows[i][j].ToString().TrimEnd();
                                Val = Val.Replace("'", "`");
                                Val = Val.Replace("\0", "\\0");
                                if (Val.Trim() == String.Empty)
                                {
                                    Str += "Null,";
                                }
                                else
                                {
                                    Str += "'" + Val + "',";
                                }
                            }
                        }
                    }
                    ReturnQueries[i] = Str;
                    Str = "Insert into " + TblName + " values (";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Copy_Table_WithOutDatas(String SourceTblName, String NewTableName)
        {
            try
            {
                if (Check_Table(SourceTblName) == true)
                {
                    if (Check_Table(NewTableName) == true)
                    {
                        Drop(NewTableName, "Table");
                    }
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand("Create Table " + NewTableName + " as Select * from " + SourceTblName + " where 1=2", Cn);
                    Cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Boolean Check_Column_name(String Colname, System.Data.DataTable Dt)
        {
            String Col = String.Empty;
            try
            {
                Col = Dt.Columns[Colname].ColumnName;
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Int64 Get_RecordCountView(String ObjName, String Condition)
        {
            try
            {
                Int64 Count = 0;
                if (Check_View(ObjName) == true)
                {
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand();
                    if (Condition.Trim() == string.Empty)
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper(), Cn);
                    }
                    else
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper() + " where " + Condition, Cn);
                    }
                    Cmd.CommandTimeout = 600;
                    Count = Convert.ToInt64(Cmd.ExecuteScalar());
                }
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Int64 Get_RecordCount_OtherDB(String DBName, String ObjName, String Condition)
        {
            try
            {
                Int64 Count = 0;
                if (Check_Table_OtherDb(DBName, ObjName) == true)
                {
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand();
                    if (Condition.Trim() == string.Empty)
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + DBName + ".dbo." + ObjName.ToUpper(), Cn);
                    }
                    else
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + DBName + ".dbo." + ObjName.ToUpper() + " where " + Condition, Cn);
                    }
                    Cmd.CommandTimeout = 600;
                    Count = Convert.ToInt64(Cmd.ExecuteScalar());
                }
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Int32 Get_RoundedOff_Ledger(int COmpcode, String Year_Code)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Load_Data("Select Ledger_Code, ledger_Name from ledger_master where ledger_NAme like '%Round%' and company_Code = " + COmpcode + " and year_Code = '" + Year_Code + "'", ref Dt);
                if (Dt.Rows.Count > 0)
                {
                    return Convert.ToInt32(Dt.Rows[0]["Ledger_Code"]);
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public Int64 RowCount(String Str)
        {
            Int64 Count = 0;
            try
            {
                Cn_Open();
                OdbcCommand Cmd = new OdbcCommand();
                Cmd = new OdbcCommand(Str, Cn);
                Cmd.CommandTimeout = 600;
                Count = Convert.ToInt64(Cmd.ExecuteScalar());
                return Count;
            }
            catch (Exception ex)
            {
                return Count;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Int64 Get_RecordCount(String ObjName, String Condition)
        {
            try
            {
                Int64 Count = 0;
                if (Check_Table(ObjName) == true)
                {
                    Cn_Open();
                    OdbcCommand Cmd = new OdbcCommand();
                    if (Condition.Trim() == string.Empty)
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper(), Cn);
                    }
                    else
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper() + " where " + Condition, Cn);
                    }
                    Cmd.CommandTimeout = 600;
                    Count = Convert.ToInt64(Cmd.ExecuteScalar());
                }
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public Int64 DBFCn_Get_RecordCount(String ObjName, String Condition)
        {
            try
            {
                Int64 Count = 0;
                if (DBFCn_Check_Table(ObjName) == true)
                {
                    DBFCn_Open();
                    OdbcCommand Cmd = new OdbcCommand();
                    if (Condition.Trim() == string.Empty)
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper(), DBFCn);
                    }
                    else
                    {
                        Cmd = new OdbcCommand("Select Count(*) from " + ObjName.ToUpper() + " where " + Condition, DBFCn);
                    }
                    Cmd.CommandTimeout = 600;
                    Count = Convert.ToInt64(Cmd.ExecuteScalar());
                }
                return Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DBFCn_Close();
            }
        }
        
        public String ItemDescription(String ItemCode)
        {
            String ItemID = string.Empty;
            String IName = string.Empty;
            try
            {
                ItemID = GetData_InString("Gsn_Acceptance_details", "Item_No", ItemCode, "Item_ID");
                if (ItemID == String.Empty)
                {
                    IName = String.Empty;
                }
                else
                {
                    IName = GetData_InString("item_MasteR", "Item_ID", ItemID, "Item_Description");
                }
                return IName;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetEntryDetails_Sys(String TableName, String Condition)
        {
            try
            {
                String Str;
                System.Data.DataTable Dt = new System.Data.DataTable();
                if (Check_Table(TableName))
                {
                    if (Condition.Trim() == String.Empty)
                    {
                        Str = "Select New_Syscode from " + TableName;
                    }
                    else
                    {
                        Str = "Select New_Syscode from " + TableName + " where " + Condition;
                    }
                    Cn_Open();
                    OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand(Str, Cn));
                    Adp.Fill(Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0]["New_Syscode"] == DBNull.Value)
                        {
                            return "0";
                        }
                        else
                        {
                            return Dt.Rows[0]["New_Syscode"].ToString();
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String GetEntryDetails_User(String TableName, String Condition)
        {
            try
            {
                String Str;
                System.Data.DataTable Dt = new System.Data.DataTable();
                if (Check_Table(TableName))
                {
                    if (Condition.Trim() == String.Empty)
                    {
                        Str = "Select New_Empcode from " + TableName;
                    }
                    else
                    {
                        Str = "Select New_Empcode from " + TableName + " where " + Condition;
                    }
                    Cn_Open();
                    OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand(Str, Cn));
                    Adp.Fill(Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0]["New_EmpCode"] == DBNull.Value)
                        {
                            return "0";
                        }
                        else
                        {
                            return Dt.Rows[0]["New_EmpCode"].ToString();
                        }
                    }
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "0";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String GetEntryDetails_Date(String TableName, String Condition)
        {
            try
            {
                String Str;
                System.Data.DataTable Dt = new System.Data.DataTable();
                if (Check_Table(TableName))
                {
                    if (Condition.Trim() == String.Empty)
                    {
                        Str = "Select New_Datetime from " + TableName;
                    }
                    else
                    {
                        Str = "Select New_Datetime from " + TableName + " where " + Condition;
                    }
                    Cn_Open();
                    OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand(Str, Cn));
                    Adp.Fill(Dt);
                    if (Dt.Rows.Count > 0)
                    {
                        if (Dt.Rows[0]["New_Datetime"] == DBNull.Value)
                        {
                            return "'" + "01-Jan-1899" + "'";
                        }
                        else
                        {
                            return "'" + String.Format("{0:dd-MMM-yyyy} {0:T}", Dt.Rows[0]["New_Datetime"]) + "'";
                        }
                    }
                    else
                    {
                        return "'"+"01-Jan-1899" + "'";
                    }
                }
                else
                {
                    return "'" + "01-Jan-1899" + "'";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        //public String[] ReturnQueries_FromDataTable (out String[] ReturnQueries, ref System.Data.DataTable Dt, String Before, String After, params String[] ColumnNamesInOrder)
        //{
        //    String Str=String.Empty, CDtime = String.Empty;
        //    int Array_Size = 0;
        //    DateTime Dtime = DateTime.Now;
        //    try
        //    {
        //        Array_Size = Dt.Rows.Count;
        //        ReturnQueries = new String[Array_Size];
        //        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
        //        {
        //            Str = Before;

        //            foreach (String Sql in ColumnNamesInOrder)
        //            {
        //                if (Dt.Columns[Sql].DataType == System.Type.GetType("System.DateTime"))
        //                {
        //                    Dtime = Convert.ToDateTime(Dt.Rows[i][Sql].ToString());
        //                    CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
        //                    Str += "'" + CDtime + "',";
        //                }
        //                else
        //                {
        //                    if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
        //                    {
        //                        if (After.Trim() != String.Empty)
        //                        {
        //                            Str += "'" + Dt.Rows[i][Sql].ToString() + "'," + After;
        //                        }
        //                        else
        //                        {
        //                            Str += "'" + Dt.Rows[i][Sql].ToString() + "')";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Str += "'" + Dt.Rows[i][Sql].ToString() + "',";
        //                    }
        //                }
        //            }
 
        //            ReturnQueries[i] = Str;
        //        }
        //        return ReturnQueries;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public Boolean IsConstraintsAvailable (String TblName, String const_Name)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn_Open();
                OdbcDataAdapter Adp = new OdbcDataAdapter(new OdbcCommand("Select * from all_constraints where table_name = '" + TblName.ToUpper() + "' and Constraint_Name = '" + const_Name.ToUpper() + "'", Cn));
                Adp.Fill(Dt);
                if (Dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }
        
        public String[] ReturnQueries_FromDataTableforSR(out String[] ReturnQueries, ref System.Data.DataTable Dt, String Before, String After, params String[] ColumnNamesInOrder)
        {
            String Str = String.Empty, CDtime = String.Empty, TempStr = String.Empty;
            String AfterVal = String.Empty;
            int Array_Size = 0;
            DateTime Dtime = DateTime.Now;
            try
            {
                Array_Size = Dt.Rows.Count;
                ReturnQueries = new String[Array_Size];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = Before;

                    foreach (String Sql in ColumnNamesInOrder)
                    {
                        if (Dt.Columns[Sql].DataType == System.Type.GetType("System.DateTime"))
                        {
                            Dtime = Convert.ToDateTime(Dt.Rows[i][Sql].ToString());
                            CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                            Str += "'" + CDtime + "',";
                        }
                        else
                        {
                            if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
                            {
                                if (After.Trim() != String.Empty)
                                {
                                    AfterVal = After;
                                    if (After.ToUpper().Contains("@LOCATION@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@LOCATION@", Dt.Rows[i]["LOCATION"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@DATE@", "'" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "'");
                                    }
                                    if (After.ToUpper().Contains("@BILL_DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@BILL_DATE@", "'" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["BILL_DATE"]) + "'");
                                    }
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "'," + AfterVal;
                                }
                                else
                                {
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "')";
                                }
                            }
                            else
                            {
                                Str += "'" + Dt.Rows[i][Sql].ToString() + "',";
                            }
                        }
                    }
                    //TempStr = Str.ToUpper().Replace("FALSE", "False");
                    //TempStr = TempStr.ToUpper().Replace("CANCEL", "Cancel");
                    //TempStr = TempStr.ToUpper().Replace("TRUE", "True");
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] DiscountOLD_Update(ref System.Data.DataTable Dt, out String[] ReturnQueries, String ItemCodeColumn, String DiscountCode, String DiscountPer, String EmpCode, String Syscode)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    if (Get_RecordCount("Item_Discount_masteR", "itemCode = '" + Dt.Rows[i][ItemCodeColumn] + "'") > 0)
                    {
                        Str = "Update Item_Discount_master set Discount_Code = " + DiscountCode + ", Discount_Percentage = " + DiscountPer + " where itemCode = '" + Dt.Rows[i][ItemCodeColumn] + "'";
                    }
                    else
                    {
                        Str = "Insert into item_Discount_Master values ('" + Dt.Rows[i][ItemCodeColumn] + "', " + DiscountCode + ", " + DiscountPer + ", " + EmpCode + ", " + Syscode + ")";
                    }
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public String[] GSNAcc_Update(ref System.Data.DataTable Dt, out String[] ReturnQueries, String ItemCodeColumn, String ItemIDColumn, String NewPriceColumn)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update GSN_Acceptance_Details set Item_Id = '" + Dt.Rows[i][ItemIDColumn] + "', S_Price = " + Dt.Rows[i][NewPriceColumn] + " where item_No = '" + Dt.Rows[i][ItemCodeColumn] + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Location_Update(ref System.Data.DataTable Dt, out String[] ReturnQueries, String ItemCodeColumn, String Location_Code)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update Acc_Stock set Location_Code = " + Location_Code + " where item_Code = '" + Dt.Rows[i][ItemCodeColumn] + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public String[] ReturnQueries_FromDataTable(out String[] ReturnQueries, ref System.Data.DataTable Dt, String Before, String After, params String[] ColumnNamesInOrder)
        {
            String Str = String.Empty, CDtime = String.Empty, TempStr=String.Empty;
            String AfterVal = String.Empty;
            int Array_Size = 0;
            DateTime Dtime = DateTime.Now;
            try
            {
                Array_Size = Dt.Rows.Count;
                ReturnQueries = new String[Array_Size];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = Before;

                    foreach (String Sql in ColumnNamesInOrder)
                    {
                        if (Dt.Columns[Sql].DataType == System.Type.GetType("System.DateTime"))
                        {
                            if (Dt.Rows[i][Sql] == DBNull.Value)
                            {
                                if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
                                {
                                    if (After.Trim() != String.Empty)
                                    {
                                        Str += "null," + After;
                                    }
                                    else
                                    {
                                        Str += "null)";
                                    }
                                }
                                else
                                {
                                    Str += "null,";
                                }
                            }
                            else
                            {
                                Dtime = Convert.ToDateTime(Dt.Rows[i][Sql].ToString());
                                CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                                if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
                                {
                                    if (After.Trim() != String.Empty)
                                    {
                                        Str += "'" + CDtime + "'," + After;
                                    }
                                    else
                                    {
                                        Str += "'" + CDtime + "')";
                                    }
                                }
                                else
                                {
                                    Str += "'" + CDtime + "',";
                                }
                            }
                        }
                        else
                        {
                            if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
                            {
                                if (After.Trim() != String.Empty)
                                {
                                    AfterVal = After;
                                    if (After.ToUpper().Contains("@LOCATION@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@LOCATION@", Dt.Rows[i]["LOCATION"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@REV_LEDCODE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@REV_LEDCODE@", Dt.Rows[i]["REV_LEDCODE"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@LEDGER_CODE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@LEDGER_CODE@", Dt.Rows[i]["LEDGER_CODE"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@AGENT_CODE@"))
                                    {
                                        if (Dt.Rows[i]["AGENT_CODE"].ToString() == String.Empty)
                                        {
                                            AfterVal = AfterVal.ToUpper().Replace("@AGENT_CODE@", "NULL");
                                        }
                                        else
                                        {
                                            AfterVal = AfterVal.ToUpper().Replace("@AGENT_CODE@", Dt.Rows[i]["AGENT_CODE"].ToString());
                                        }
                                    }
                                    if (After.ToUpper().Contains("@AGENT_NAME@"))
                                    {
                                        if (Dt.Rows[i]["AGENT"].ToString() == String.Empty)
                                        {
                                            AfterVal = AfterVal.ToUpper().Replace("@AGENT_NAME@", "NULL");
                                        }
                                        else
                                        {
                                            AfterVal = AfterVal.ToUpper().Replace("@AGENT_NAME@", "'" + Dt.Rows[i]["AGENT"].ToString() + "'");
                                        }
                                    }
                                    if (After.ToUpper().Contains("@TAXACHEAD@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@TAXACHEAD@", Dt.Rows[i]["TAXACHEAD"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@CHARGE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@CHARGE@", Dt.Rows[i]["CHARGE"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@P_C@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@P_C@", "'" + Dt.Rows[i]["P_C"].ToString() + "'");
                                    }
                                    if (After.ToUpper().Contains("@MATCH@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@MATCH@", Dt.Rows[i]["MATCH"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@NEW_PRICE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@NEW_PRICE@", Dt.Rows[i]["NEW_PRICE"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@NEW_ITEMID@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@NEW_ITEMID@", "'" + Dt.Rows[i]["NEW_ITEMID"].ToString() + "'");
                                    }
                                    if (After.ToUpper().Contains("@JOB_CODE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@JOB_CODE@", "'" + Dt.Rows[i]["JOB_CODE"].ToString() + "'");
                                    }
                                    if (After.ToUpper().Contains("@UOM@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@UOM@", "'" + Dt.Rows[i]["UOM"].ToString() + "'");
                                    }
                                    if (After.ToUpper().Contains("@DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@DATE@", "'" + String.Format("{0:dd-MMM-yyyy}",DateTime.Now) + "'");
                                    }
                                    if (After.ToUpper().Contains("@FROM@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@FROM@", "0");
                                    }
                                    if (After.ToUpper().Contains("@JOB_CODE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@JOB_CODE@", Dt.Rows[i]["JOB_CODE"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@TO@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@TO@", "0");
                                    }
                                    if (After.ToUpper().Contains("@BILL_DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@BILL_DATE@", "'" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["BILL_DATE"]) + "'");
                                    }
                                    if (After.ToUpper().Contains("@GRN_DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@GRN_DATE@", "'" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["GRN_DATE"]) + "'");
                                    }
                                    if (After.ToUpper().Contains("@GRN_NO@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@GRN_NO@", Dt.Rows[i]["GRN_NO"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@LEDGER@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@LEDGER@", "'" + Dt.Rows[i]["LEDGER"].ToString() + "'");
                                    }
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "'," + AfterVal;
                                }
                                else
                                {
                                    if (Dt.Rows[i][Sql] == DBNull.Value)
                                    {
                                        Str += "Null)";
                                    }
                                    else
                                    {
                                        Str += "'" + Dt.Rows[i][Sql].ToString() + "')";
                                    }
                                }
                            }
                            else
                            {
                                if (Dt.Rows[i][Sql] == DBNull.Value)
                                {
                                    Str += "Null,";
                                }
                                else
                                {
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "',";
                                }
                            }
                        }
                    }
                    //TempStr = Str.ToUpper().Replace("FALSE", "False");
                    //TempStr = TempStr.ToUpper().Replace("CANCEL", "Cancel");
                    //TempStr = TempStr.ToUpper().Replace("TRUE", "True");
                    //TempStr = TempStr.ToUpper().Replace("CARD", "Card");
                    //TempStr = TempStr.ToUpper().Replace("CHEQUE", "Cheque");
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] ReturnQueries_FromDataTableDis(out String[] ReturnQueries, ref System.Data.DataTable Dt, String Before, String After, params String[] ColumnNamesInOrder)
        {
            String Str = String.Empty, CDtime = String.Empty, TempStr = String.Empty;
            String AfterVal = String.Empty;
            int Array_Size = 0;
            DateTime Dtime = DateTime.Now;
            try
            {
                Array_Size = Dt.Rows.Count;
                ReturnQueries = new String[Array_Size];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = Before;

                    foreach (String Sql in ColumnNamesInOrder)
                    {
                        if (Dt.Columns[Sql].DataType == System.Type.GetType("System.DateTime"))
                        {
                            Dtime = Convert.ToDateTime(Dt.Rows[i][Sql].ToString());
                            CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                            Str += "'" + CDtime + "',";
                        }
                        else
                        {
                            if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
                            {
                                if (After.Trim() != String.Empty)
                                {
                                    AfterVal = After;
                                    if (After.ToUpper().Contains("@LOCATION@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@LOCATION@", Dt.Rows[i]["LOCATION"].ToString());
                                    }
                                    if (After.ToUpper().Contains("@DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@DATE@", "'" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "'");
                                    }
                                    if (After.ToUpper().Contains("@FROM@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@FROM@", "0");
                                    }
                                    if (After.ToUpper().Contains("@TO@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@TO@", "0");
                                    }
                                    if (After.ToUpper().Contains("@BILL_DATE@"))
                                    {
                                        AfterVal = AfterVal.ToUpper().Replace("@BILL_DATE@", "'" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i]["BILL_DATE"]) + "'");
                                    }
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "'," + AfterVal;
                                }
                                else
                                {
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "')";
                                }
                            }
                            else
                            {
                                Str += "'" + Dt.Rows[i][Sql].ToString() + "',";
                            }
                        }
                    }
                    //TempStr = Str.ToUpper().Replace("FALSE", "False");
                    //TempStr = TempStr.ToUpper().Replace("CANCEL", "Cancel");
                    //TempStr = TempStr.ToUpper().Replace("TRUE", "1");
                    //TempStr = TempStr.ToUpper().Replace("CARD", "Card");
                    //TempStr = TempStr.ToUpper().Replace("CHEQUE", "Cheque");
                    ReturnQueries[i] = TempStr;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void Make_Excel_Chart(string FilePath)
        {
            try
            {
                Excel.Application Exc;
                Excel.Workbook WBook;
                Excel.Worksheet WSheet;
                Object Missing = System.Reflection.Missing.Value;
                Exc = new Excel.Application();
                WBook = (Excel.Workbook)Exc.Workbooks.Open(FilePath, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing, Missing);
                WSheet = (Excel.Worksheet)WBook.ActiveSheet;
                Exc.Visible = true;
                SendKeys.Send("{Down 2}"); SendKeys.Send("+^{RIGHT} +^{DOWN}"); SendKeys.Send("^z"); SendKeys.Send("+^{Down}"); SendKeys.Send("%i h"); SendKeys.Send("%n"); SendKeys.Send("%n"); SendKeys.Send("%n"); SendKeys.Send("%s"); SendKeys.Send("%f");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public void DBFCN_Run(params String[] Queries)
        {
            String Sql1;
            Double I = 0;
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            DBFCn_Open();
            Trans = DBFCn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    I += 1;
                    Sql1 = Sql;
                    Cmd.Connection = DBFCn;
                    Cmd.Transaction = Trans;
                    Cmd.CommandText = Sql;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                DBFCn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                DBFCn_Close();
            }
        }

        public void Run_Without_Error_Message(params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public void Run(params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                 {
                     if (Sql != null && Sql != String.Empty)
                     {
                         Cmd.Connection = Cn;
                         Cmd.Transaction = Trans;
                         Cmd.CommandText = Sql;
                         Cmd.ExecuteNonQuery();
                     }
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Sum_With_Four_Digits(ref MyDataGridView DGV, String ColumnName, params String[] Condition_NotNullColumns)
        {
            Decimal SumValue = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    if (Check_EmptyinDataGridView(ref DGV, i, Condition_NotNullColumns) != true)
                    {
                        if (Convert.ToString(DGV[ColumnName, i].Value).Trim() != String.Empty)
                        {
                            SumValue = SumValue + Convert.ToDecimal(DGV[ColumnName, i].Value);
                        }
                    }
                }
                return String.Format("{0:0.0000}", SumValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Run_Identity(Boolean Edit, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            Double Code = 0;
            Boolean First_Qry = true;
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        if (First_Qry == false)
                        {
                            if (Edit)
                            {
                                Cmd.CommandText = Sql;
                            }
                            else
                            {
                                Cmd.CommandText = Sql.Replace("@@IDENTITY", Code.ToString());
                            }
                        }
                        else
                        {
                            Cmd.CommandText = Sql;
                        }
                        if (First_Qry)
                        {
                            if (Edit)
                            {
                                Cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                Code = Convert.ToDouble(Cmd.ExecuteScalar());
                                First_Qry = false;
                            }
                        }
                        else
                        {
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public Int64 Run_Identity_Return(Boolean Edit, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            Int64 Code = 0;
            Boolean First_Qry = true;
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        if (First_Qry == false)
                        {
                            if (Edit)
                            {
                                Cmd.CommandText = Sql;
                            }
                            else
                            {
                                Cmd.CommandText = Sql.Replace("@@IDENTITY", Code.ToString());
                            }
                        }
                        else
                        {
                            Cmd.CommandText = Sql;
                        }
                        if (First_Qry)
                        {
                            if (Edit)
                            {
                                Cmd.ExecuteNonQuery();
                            }
                            else
                            {
                                Code = Convert.ToInt64(Cmd.ExecuteScalar());
                                First_Qry = false;
                            }
                        }
                        else
                        {
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }
                Trans.Commit();
                Cn_Close();
                return Code;
            }
            catch (Exception ex)
            {
                return Code;
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void RunWithMax(String TblName, String FldName, String Condition,  params String[] Queries)
        {
            String Code = String.Empty, SqlQ= String.Empty;
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    Cmd.Connection = Cn;
                    Cmd.Transaction = Trans;
                    if (Code == String.Empty)
                    {
                        if (Condition == String.Empty)
                        {
                            Cmd.CommandText = "Select Max(" + FldName + ") from " + TblName + " for update";
                        }
                        else
                        {
                            Cmd.CommandText = "Select Max(" + FldName + ") from " + TblName + " where " + Condition + " for update";
                        }
                        if (Cmd.ExecuteScalar() != DBNull.Value)
                        {
                            Code = Convert.ToString(Convert.ToDouble(Cmd.ExecuteScalar()) + 1);
                        }
                        else
                        {
                            Code = "1";
                        }
                    }
                    if (Sql.Contains("@MAX"))
                    {
                        SqlQ = Sql.Replace("@MAX", Code);
                    }
                    Cmd.CommandText = SqlQ;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Run(String[] Detail_Queries2, String[] Detail_Queries3, String[] Detail_Queries4, String[] Detail_Queries5, String[] Detail_Queries6, String[] Detail_Queries7, params String[] Queries1)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries1)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries2)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries3)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries4)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries5)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries6)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }


                foreach (String Sql in Detail_Queries7)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public void Run(String[] Detail_Queries, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Run(String[] Detail_Queries, String[] Detail_Queries1, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Run(String[] Detail_Queries, String[] Detail_Queries1, String[] Detail_Queries2, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Cmd.CommandTimeout = 600;
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                if (Detail_Queries2 != null)
                {
                    foreach (String Sql in Detail_Queries2)
                    {
                        if (Sql != null && Sql != string.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Sql;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                if (Detail_Queries != null)
                {
                    foreach (String Sql in Detail_Queries)
                    {
                        if (Sql != null && Sql != String.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Sql;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String User_Address()
        {
            return "User1_Address14";
        }

        public String[] Update_LREntry(ref System.Data.DataTable Dt, out String[] ReturnQueries, String Transport_Code, String LR_NOColummn, String LR_DateColumn)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update LR_Entry_Master set R_Status = 'LRIss' where LRNo = '" + Dt.Rows[i][LR_NOColummn].ToString() + "' and LRDate = '" + String.Format ("{0:dd-MMM-yyyy}", Dt.Rows[i][LR_DateColumn]) + "' and transport_Code = " + Transport_Code + " and R_Status = 'False'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_LREntryRev(ref System.Data.DataTable Dt, out String[] ReturnQueries, String Transport_Code, String LR_NOColummn, String LR_DateColumn)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update LR_Entry_Master set R_Status = 'False' where LRNo = '" + Dt.Rows[i][LR_NOColummn].ToString() + "' and LRDate = '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][LR_DateColumn]) + "' and transport_Code = " + Transport_Code + " and R_Status = 'LRIss'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_FlagSR(String TblName, String FldName, ref System.Data.DataTable Dt, out String[] ReturnQueries, Boolean Flag, String BillNo, String DateColumn, String ItemColumnName, int CompCode, String YearCode)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update " + TblName + " set " + FldName + " = '" + Flag + "' where item_No = '" + Dt.Rows[i][ItemColumnName] + "' and cashbill_Slno = '" + Dt.Rows[i][BillNo] + "' and cashbill_Slno in (Select cashbill_Slno from cashbill_master where cashbill_Date = '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][DateColumn]) + "')";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public String[] Update_Conversion(ref System.Data.DataTable Dt, out String[] ReturnQueries, String ItemColumnName, String StockColumn)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 1; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update Acc_Stock set Qty = qty -  " + Dt.Rows[i][StockColumn] + " where item_Code = '" + Dt.Rows[i][ItemColumnName] + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_Location(ref System.Data.DataTable Dt, out String[] ReturnQueries, String ItemColumnName, Int32 NewLocatioNCode, String StockColumn, String CommentsColumn)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    if (Dt.Rows[i][CommentsColumn].ToString() != "GSN Required ...!")
                    {
                        //Str = "Update Acc_Stock set Location_Code = " + NewLocatioNCode + ", Qty = " + Dt.Rows[i][StockColumn].ToString() + "  where item_Code = '" + Dt.Rows[i][ItemColumnName] + "'";
                        Str = "Update Acc_Stock set Location_Code = " + NewLocatioNCode + " where item_Code = '" + Dt.Rows[i][ItemColumnName] + "'";
                    }
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_LRFlag(ref System.Data.DataTable Dt, out String[] ReturnQueries, String SupplierCodeColumn, String LRNoColumns, String LRDateColumns, String Transport_Code)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["R"].ToString() == "Y")
                    {
                        Str = "Update LR_Entry_Master set R_Status = 'False' where Supplier_Code = " + Dt.Rows[i][SupplierCodeColumn] + " and LRNo = '" + Dt.Rows[i][LRNoColumns] + "' and LRDate = '" + string.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][LRDateColumns]) + "' and Transport_Code = " + Transport_Code;
                    }
                    else
                    {
                        Str = "Update LR_Entry_Master set R_Status = 'LRIss' where Supplier_Code = " + Dt.Rows[i][SupplierCodeColumn] + " and LRNo = '" + Dt.Rows[i][LRNoColumns] + "' and LRDate = '" + string.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][LRDateColumns]) + "' and Transport_Code = " + Transport_Code;
                    }
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_LRIssueFlag(ref System.Data.DataTable Dt, out String[] ReturnQueries, String SupplierCodeColumn, String LRNoColumns, String LRDateColumns, String Transport_Code)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (Dt.Rows[i]["R"].ToString() == "Y")
                    {
                        Str = "Update LRIssue_Entry_Details set R_Status = 'Ret' where Supplier_Code = " + Dt.Rows[i][SupplierCodeColumn] + " and LRNo = '" + Dt.Rows[i][LRNoColumns] + "' and LRDate = '" + string.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][LRDateColumns]) + "' and Transport_Code = " + Transport_Code;
                    }
                    else
                    {
                        Str = "Update LRIssue_Entry_Details set R_Status = 'False' where Supplier_Code = " + Dt.Rows[i][SupplierCodeColumn] + " and LRNo = '" + Dt.Rows[i][LRNoColumns] + "' and LRDate = '" + string.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][LRDateColumns]) + "' and Transport_Code = " + Transport_Code;
                    }
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_Flag (String TblName, String FldName, ref System.Data.DataTable Dt, out String[] ReturnQueries, Boolean Flag, String ItemColumnName, int CompCode, String YearCode)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i=0;i<= Dt.Rows.Count - 1;i++)
                {
                    Str = String.Empty;
                    Str = "Update " + TblName +" set " + FldName + " = '" + Flag + "' where item_No = '" + Dt.Rows[i][ItemColumnName] + "' and Comp_Code = " + CompCode + " and year_Code = '" + YearCode + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_FlagCancel(String TblName, String FldName, ref System.Data.DataTable Dt, out String[] ReturnQueries, string Flag, String ItemColumnName, String ConditionFieldName, String cashbill_Slno, int CompCode, String YearCode)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update " + TblName + " set " + FldName + " = '" + Flag + "' where item_No = '" + Dt.Rows[i][ItemColumnName] + "' and " + ConditionFieldName + " = '" + cashbill_Slno + "' and Comp_Code = " + CompCode + " and year_Code = '" + YearCode + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] Update_Flag(String TblName, String FldName, ref System.Data.DataTable Dt, out String[] ReturnQueries, Boolean Flag, String ItemColumnName, String ConditionFieldName, String ConditionFieldColumnName, int CompCode, String YearCode)
        {
            String Str;
            try
            {
                ReturnQueries = new string[Convert.ToInt32(Dt.Rows.Count)];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = String.Empty;
                    Str = "Update " + TblName + " set " + FldName + " = '" + Flag + "' where item_No = '" + Dt.Rows[i][ItemColumnName] + "' and " + ConditionFieldName + " = '" + Dt.Rows[i][ConditionFieldColumnName] + "' and Company_Code = " + CompCode + " and year_Code = '" + YearCode + "'";
                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Run(String[] Detail_QueriesX, String[] Detail_Queries, String[] Detail_Queries1,String[] Detail_Queries2, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    Cmd.Connection = Cn;
                    Cmd.Transaction = Trans;
                    Cmd.CommandText = Sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (String Sql in Detail_Queries2)
                {
                    Cmd.Connection = Cn;
                    Cmd.Transaction = Trans;
                    Cmd.CommandText = Sql;
                    Cmd.ExecuteNonQuery();
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql.Trim() != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql.Trim() != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX)
                {
                    if (Sql != null && Sql.Trim() != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String[] OldStockSR(ref System.Data.DataTable Dt, out String[] Update_Commands, String BillNoCol, String BillDateCol, String ItemCol, String BooleanFlag)
        {
            String Sql;
            try
            {
                Update_Commands = new string[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    if (Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Dt.Rows[i][BillDateCol])) < Convert.ToDateTime("30/03/2009"))
                    {
                        //Sql = "Update OldSalesSR set Sale_Return = '" + BooleanFlag + "' where CashBill_Slno = '" + Dt.Rows[i][BillNoCol] + "' and item_no = '" + Dt.Rows[i][ItemCol] + "' and cashbill_date = to_date('" + String.Format("{0:dd-MMM-yyyy} {0:T}", Dt.Rows[i][BillDateCol]) + "','dd-Mon-yyyy hh:mi:ss PM')";
                        Sql = "Update OldSalesSR set Sale_Return = '" + BooleanFlag + "' where CashBill_Slno = '" + Dt.Rows[i][BillNoCol] + "' and item_no = '" + Dt.Rows[i][ItemCol] + "' and cashbill_date >= '" + String.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][BillDateCol]) + "' and cashbill_date <= '" + String.Format("{0:dd-MMM-yyyy}", Convert.ToDateTime(Dt.Rows[i][BillDateCol]).AddDays(1)) + "'";
                    }
                    else
                    {
                        Sql = String.Empty;
                    }
                    
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Run(String[] Detail_QueriesY, String[] Detail_QueriesX, String[] Detail_Queries, String[] Detail_Queries1, String[] Detail_Queries2, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Detail_QueriesY)
                {
                    if (Sql != null)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries2)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }


        public void Run_Stores(String[] Detail_QueriesY, String[] Detail_QueriesX, String[] Detail_Queries, String[] Detail_Queries1, String[] Detail_Queries2, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {

                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != string.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries2)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesY)
                {
                    if (Sql != null)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Run(String[] Detail_Queriesx1, String[] Detail_QueriesY, String[] Detail_QueriesX, String[] Detail_Queries, String[] Detail_Queries1, String[] Detail_Queries2, String[] Detail_QueriesX2, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queriesx1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries2)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX2)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesY)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Run(String[] Detail_Queriesx1, String[] Detail_QueriesY, String[] Detail_QueriesX, String[] Detail_Queries, String[] Detail_Queries1, String[] Detail_Queries2, String[] Detail_QueriesX2, String[] Detail_QueriesX3, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                foreach (String Sql in Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queriesx1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries2)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX2)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesX3)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_QueriesY)
                {
                    if (Sql != null && Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                MessageBox.Show(Cmd.CommandText);
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }



        public void RunSR(String[] Detail_QueriesY, String[] Detail_QueriesX, String[] Detail_Queries, String[] OldStockFalse, String[] OldStockTrue, String[] Detail_Queries1, String[] Detail_Queries2, params String[] Queries)
        {
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {
                if (OldStockFalse != null)
                {
                    foreach (String Sql in OldStockFalse)
                    {
                        if (Sql != String.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Sql;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                foreach (String Sql in Detail_Queries2)
                {
                    if (Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries1)
                {
                    if (Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Queries)
                {
                    if (Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                foreach (String Sql in Detail_Queries)
                {
                    if (Sql != String.Empty)
                    {
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.CommandText = Sql;
                        Cmd.ExecuteNonQuery();
                    }
                }

                if (Detail_QueriesX != null)
                {
                    foreach (String Sql in Detail_QueriesX)
                    {
                        if (Sql != String.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Sql;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }
                if (Detail_QueriesY != null)
                {
                    foreach (String Sql in Detail_QueriesY)
                    {
                        if (Sql != String.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Sql;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                if (OldStockTrue != null)
                {
                    foreach (String Sql in OldStockTrue)
                    {
                        if (Sql != String.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Sql;
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        //String Menu_underName(String Under)
        //{
        //    String Code;
        //    try
        //    {
        //        Code = GetData_InString("Menu_Master", "Menu_name", Under.ToUpper(), "Menu_Code");
        //        if (Code == string.Empty)
        //        {
        //            Code = "Main";
        //        }
        //        return Code;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        String Menu_underName(String Under)
        {
            String Code;
            try
            {
                Code = GetData_InString("Socks_Menu_Master_New", "Menu_name", Under.ToUpper(), "Menu_CName");
                if (Code == string.Empty)
                {
                    Code = "Main";
                }
                return Code;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Menu_save(String MenuName, String MenuCName, String Under)
        {
            try
            {
                Execute("Insert into Socks_Menu_Master_New values ('" + MenuName.ToUpper() + "','" + MenuCName.ToUpper() + "', '" + Menu_underName(Under) + "')");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Code(String TblName, String Condition_Field, String ToUpdateColumn)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                if (Check_TableField(TblName, ToUpdateColumn))
                {
                    if (Get_RecordCount(TblName, ToUpdateColumn + " is null ") > 0)
                    {
                        Load_Data("Select distinct " + Condition_Field + " from " + TblName, ref Dt);
                        for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                        {
                            Execute("Update " + TblName + " set " + ToUpdateColumn + " = " + Convert.ToInt32(i + 1) + " where " + Condition_Field + " = '" + Dt.Rows[i][Condition_Field].ToString() + "'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public void Menu_save(String MenuName, String MenuCName, String Under)
        //{
        //    try
        //    {
        //        Execute("Insert into Menu_master values (" + MaxWOCC("Menu_Master", "Menu_Code", "") + ", '" + MenuName.ToUpper() + "','" + MenuCName.ToUpper() + "', '" + Menu_underName(Under) + "')");
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Clear1(ContainerControl Cr)
        {
            DateTime ServerDateTime = GetServerDateTime();
            try
            {
                Cr.BackColor = System.Drawing.Color.Tan;
                foreach (Control ct in Cr.Controls)
                {
                    if (ct is System.Windows.Forms.GroupBox || ct is Panel || ct is FlowLayoutPanel || ct is TabControl)
                    {
                        if (ct.Name.ToUpper().Contains("SPECIAL") == false)
                        {
                            ct.BackColor = System.Drawing.Color.Wheat;
                        }
                        ct.Font = new System.Drawing.Font(ct.Font, FontStyle.Bold);

                        //ct.Font = new System.Drawing.Font(ct.Font, FontStyle.Bold);
                        foreach (Control Co in ct.Controls)
                        {
                            if (Co is System.Windows.Forms.TextBox)
                            {
                                Co.Text = String.Empty;
                                Co.Tag = String.Empty;
                                if (Co.BackColor == System.Drawing.Color.LightCyan)
                                {
                                    Co.BackColor = System.Drawing.Color.White;
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                }
                                else if (Co.BackColor == System.Drawing.Color.LightGreen)
                                {
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                }
                                else
                                {
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                }
                            }
                            else if (Co is System.Windows.Forms.Label)
                            {
                                Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                            }
                            else if (Co is System.Windows.Forms.Button)
                            {
                                Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                            }
                            else if (Co is DataGridView)
                            {
                                DataGridView Obj;
                                Obj = (DataGridView)Co;
                                Obj.DataSource = null;
                            }
                            else if (Co is System.Windows.Forms.GroupBox)
                            {
                                foreach (Control Co1 in Co.Controls)
                                {
                                    if (Co1 is System.Windows.Forms.TextBox)
                                    {
                                        Co1.Text = String.Empty;
                                        Co1.Tag = String.Empty;
                                    }
                                }
                            }
                            else if (Co is TabControl)
                            {
                                foreach (Control Co1 in Co.Controls)
                                {
                                    if (Co1 is TabPage)
                                    {
                                        foreach (Control Co2 in Co1.Controls)
                                        {
                                            if (Co2 is System.Windows.Forms.TextBox)
                                            {
                                                Co2.Text = String.Empty;
                                                Co2.Tag = String.Empty;
                                                if (Co2.BackColor == System.Drawing.Color.LightCyan)
                                                {
                                                    Co2.BackColor = System.Drawing.Color.White;
                                                    Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
                                                }
                                                else if (Co2.BackColor == System.Drawing.Color.LightGreen)
                                                {
                                                    Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
                                                }
                                                else
                                                {
                                                    Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
                                                }
                                            }
                                            else if (Co2 is RadioButton)
                                            {
                                                RadioButton Obj;
                                                Obj = (RadioButton)Co2;
                                                Obj.Checked = false;
                                            }
                                            else if (Co2 is DataGridView)
                                            {
                                                DataGridView Obj;
                                                Obj = (DataGridView)Co2;
                                                Obj.DataSource = null;
                                            }
                                        }
                                    }
                                    else if (Co1 is System.Windows.Forms.TextBox)
                                    {
                                        Co1.Text = String.Empty;
                                        Co1.Tag = String.Empty;
                                        if (Co1.BackColor == System.Drawing.Color.LightCyan)
                                        {
                                            Co1.BackColor = System.Drawing.Color.White;
                                            Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
                                        }
                                        else if (Co1.BackColor == System.Drawing.Color.LightGreen)
                                        {
                                            Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
                                        }
                                        else
                                        {
                                            Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
                                        }
                                    }
                                    else if (Co1 is RadioButton)
                                    {
                                        RadioButton Obj;
                                        Obj = (RadioButton)Co1;
                                        Obj.Checked = false;
                                    }
                                    else if (Co1 is DataGridView)
                                    {
                                        DataGridView Obj;
                                        Obj = (DataGridView)Co1;
                                        Obj.DataSource = null;
                                    }
                                }
                            }
                            else if (Co is RadioButton)
                            {
                                RadioButton Obj;
                                Obj = (RadioButton)Co;
                                Obj.Checked = false;
                            }
                            else if (Co is DateTimePicker)
                            {
                                DateTimePicker Dt;
                                Dt = (DateTimePicker)Co;
                                Dt.MinDate = Convert.ToDateTime("01/01/1899");
                                Dt.MaxDate = Convert.ToDateTime("01/01/3999");
                                Dt.Value = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", ServerDateTime));
                                if (Dt.Name == "DtpDate")
                                {
                                    if (Cr.Name == "FrmCashierDayClosingEntry" || Cr.Name == "FrmVoucherEntry")
                                    {
                                        Dt.Enabled = true;
                                    }
                                    else
                                    {
                                        Dt.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (ct is System.Windows.Forms.TextBox)
                    {
                        ct.Text = String.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Tax_Adjustment_Save(DateTime DtpFrom, DateTime DtpTo, int Company_Code, String Year_Code, ref System.Data.DataTable Input_Dt, ref DotnetVFGrid.MyDataGridView Input_Grid, ref System.Data.DataTable Output_Dt, ref DataGridView Output_Grid, ref System.Data.DataTable Reposting_Dt, ref DataGridView Reposting_Grid, ref System.Data.DataTable Assining_Dt, ref DataGridView Assining_Grid, ref System.Data.DataTable Payment_Dt, ref MyDataGridView Payment_Grid, String Narration, String ChqBookNo, String ChqNo, String Reposting, Double FinalAmount, int EmpCode, int SysCode, String E_Datetime)
        {
            Double Tax_Payment_No = 0;
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Int32 Rev_ledger_Code_BY = 0;
            Int32 Rev_ledger_Code_TO = 0;
            Cn_Open();
            Double Reposting_Code = 0;
            Double Assining_Code = 0;
            Double Payment_Code = 0;
            Trans = Cn.BeginTransaction();
            try
            {
                // Tax Adjustment Max - Code

                Cmd.CommandText = "Select isnull(Max(Eno), 0) from Tax_Adjustment_Input";
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                Tax_Payment_No = Convert.ToDouble(Convert.ToDouble(Cmd.ExecuteScalar()) + 1);


                // Tax Adjustment Input

                    for (int i = 0; i <= Input_Dt.Rows.Count - 1; i++)
                    {
                        Cmd.CommandText = "insert into Tax_Adjustment_Input values (" + Tax_Payment_No + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + Input_Grid["Code", i].Value.ToString() + ", " + Input_Grid["P_Value", i].Value.ToString() + ", " + Input_Grid["Amount", i].Value.ToString() + ", " + Input_Grid["Adj_Amount", i].Value.ToString() + ", " + Input_Grid["Output_Ledger_Code", i].Value.ToString() + ", " + Company_Code + ", '" + Year_Code + "')";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();
                    }

                // Tax Adjustment Output

                    for (int i = 0; i <= Output_Dt.Rows.Count - 1; i++)
                    {
                        Cmd.CommandText = "insert into Tax_Adjustment_Out values (" + Tax_Payment_No + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + Output_Grid["Code", i].Value.ToString() + ", " + Output_Grid["S_Value", i].Value.ToString() + ", " + Output_Grid["Amount", i].Value.ToString() + ", " + Company_Code + ", '" + Year_Code + "')";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();
                    }

                // Reposting Journal - 

                    if (Reposting_Dt != null && Reposting_Dt.Rows.Count > 0)
                    {

                        for (int i = 0; i <= Reposting_Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Reposting_Dt.Rows[i]["debit"]) > 0)
                            {
                                Rev_ledger_Code_TO = Convert.ToInt32(Reposting_Dt.Rows[i]["Ledger_Code"]);
                            }
                            else
                            {
                                Rev_ledger_Code_BY = Convert.ToInt32(Reposting_Dt.Rows[i]["Ledger_Code"]);
                            }
                        }

                        // Max Code
                        Cmd.CommandText = "Select isnull(Max(vcode), 0) from voucher_Master where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Reposting_Code = Convert.ToDouble(Convert.ToDouble(Cmd.ExecuteScalar()) + 1);

                        // Voucher Master

                        Cmd.CommandText = "insert into Voucher_master values (" + Reposting_Code + ", 4, " + Reposting_Code + ", 'Others', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', '" + Narration + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + EmpCode + ", " + SysCode + ", " + E_Datetime + "," + EmpCode + ", " + SysCode + ", " + E_Datetime + ", " + Company_Code + ", '" + Year_Code + "', 1, null, null, null, null, null, null, null)";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();

                        // Voucher Details 

                        for (int i = 0; i <= Reposting_Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Reposting_Dt.Rows[i]["debit"]) > 0)
                            {
                                Cmd.CommandText = "insert into voucher_Details values (" + Reposting_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToInt32(i + 1) + ", 'BY', " + Reposting_Dt.Rows[i]["Ledger_Code"].ToString() + ", " + Reposting_Dt.Rows[i]["Debit"].ToString() + ", 0, '" + Narration + "', " + Company_Code + ", '" + Year_Code + "', " + Rev_ledger_Code_TO + ", 'True', 'True', 'True')";
                            }
                            else
                            {
                                Cmd.CommandText = "insert into voucher_Details values (" + Reposting_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToInt32(i + 1) + ", 'TO', " + Reposting_Dt.Rows[i]["Ledger_Code"].ToString() + ", 0, " + Reposting_Dt.Rows[i]["Credit"].ToString() + ", '" + Narration + "', " + Company_Code + ", '" + Year_Code + "', " + Rev_ledger_Code_BY + ", 'True', 'True', 'True')";
                            }
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.ExecuteNonQuery();
                        }

                        Cmd.CommandText = "insert into Tax_Adjustment_Reposting values (" + Tax_Payment_No + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + Reposting_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Company_Code + ", '" + Year_Code + "')";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();

                    }

                    // Assining Journal - 

                    Rev_ledger_Code_BY = 0;
                    Rev_ledger_Code_TO = 0;
                    
                    if (Assining_Dt != null && Assining_Dt.Rows.Count > 0)
                    {

                        for (int i = 0; i <= Assining_Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Assining_Dt.Rows[i]["debit"]) > 0)
                            {
                                Rev_ledger_Code_TO = Convert.ToInt32(Assining_Dt.Rows[i]["Ledger_Code"]);
                            }
                            else
                            {
                                Rev_ledger_Code_BY = Convert.ToInt32(Assining_Dt.Rows[i]["Ledger_Code"]);
                            }
                        }

                        // Max Code
                        Cmd.CommandText = "Select isnull(Max(vcode), 0) from voucher_Master where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Assining_Code = Convert.ToDouble(Convert.ToDouble(Cmd.ExecuteScalar()) + 1);

                        // Voucher Master

                        Cmd.CommandText = "insert into Voucher_master values (" + Assining_Code + ", 4, " + Assining_Code + ", 'Others', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', '" + Narration + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + EmpCode + ", " + SysCode + ", " + E_Datetime + "," + EmpCode + ", " + SysCode + ", " + E_Datetime + ", " + Company_Code + ", '" + Year_Code + "', 1, null, null, null, null, null, null, null)";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();

                        // Voucher Details 

                        for (int i = 0; i <= Assining_Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Assining_Dt.Rows[i]["debit"]) > 0)
                            {
                                Cmd.CommandText = "insert into voucher_Details values (" + Assining_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToInt32(i + 1) + ", 'BY', " + Assining_Dt.Rows[i]["Ledger_Code"].ToString() + ", " + Assining_Dt.Rows[i]["Debit"].ToString() + ", 0, '" + Narration + "', " + Company_Code + ", '" + Year_Code + "', " + Rev_ledger_Code_TO + ", 'True', 'True', 'True')";
                            }
                            else
                            {
                                Cmd.CommandText = "insert into voucher_Details values (" + Assining_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToInt32(i + 1) + ", 'TO', " + Assining_Dt.Rows[i]["Ledger_Code"].ToString() + ", 0, " + Assining_Dt.Rows[i]["Credit"].ToString() + ", '" + Narration + "', " + Company_Code + ", '" + Year_Code + "', " + Rev_ledger_Code_BY + ", 'True', 'True', 'True')";
                            }
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.ExecuteNonQuery();
                        }

                        Cmd.CommandText = "insert into Tax_Adjustment_Assining values (" + Tax_Payment_No + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpFrom) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + Assining_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Company_Code + ", '" + Year_Code + "')";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();

                    }

                // Payment Entry

                    Rev_ledger_Code_BY = 0;
                    Rev_ledger_Code_TO = 0;

                    if (Payment_Dt != null && Payment_Dt.Rows.Count > 0)
                    {

                        for (int i = 0; i <= Payment_Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Payment_Dt.Rows[i]["debit"]) > 0)
                            {
                                Rev_ledger_Code_TO = Convert.ToInt32(Payment_Dt.Rows[i]["Ledger_Code"]);
                            }
                            else
                            {
                                Rev_ledger_Code_BY = Convert.ToInt32(Payment_Dt.Rows[i]["Ledger_Code"]);
                            }
                        }

                        // Max Code
                        Cmd.CommandText = "Select isnull(Max(vcode), 0) from voucher_Master where company_Code = " + Company_Code + " and year_Code = '" + Year_Code + "'";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Payment_Code = Convert.ToDouble(Convert.ToDouble(Cmd.ExecuteScalar()) + 1);

                        // Voucher Master

                        Cmd.CommandText = "insert into Voucher_master values (" + Payment_Code + ", 1, " + Payment_Code + ", 'Bank', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', '" + Narration + "', '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + EmpCode + ", " + SysCode + ", " + E_Datetime + "," + EmpCode + ", " + SysCode + ", " + E_Datetime + ", " + Company_Code + ", '" + Year_Code + "', 1, null, null, null, null, null, null, null)";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();

                        // Voucher Details 

                        for (int i = 0; i <= Payment_Dt.Rows.Count - 1; i++)
                        {
                            if (Convert.ToDouble(Payment_Dt.Rows[i]["debit"]) > 0)
                            {
                                Cmd.CommandText = "insert into voucher_Details values (" + Payment_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToInt32(i + 1) + ", 'BY', " + Payment_Dt.Rows[i]["Ledger_Code"].ToString() + ", " + Payment_Dt.Rows[i]["Debit"].ToString() + ", 0, '" + Narration + "', " + Company_Code + ", '" + Year_Code + "', " + Rev_ledger_Code_TO + ", 'True', 'True', 'True')";
                            }
                            else
                            {
                                Cmd.CommandText = "insert into voucher_Details values (" + Payment_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Convert.ToInt32(i + 1) + ", 'TO', " + Payment_Dt.Rows[i]["Ledger_Code"].ToString() + ", 0, " + Payment_Dt.Rows[i]["Credit"].ToString() + ", '" + Narration + "', " + Company_Code + ", '" + Year_Code + "', " + Rev_ledger_Code_BY + ", 'True', 'True', 'True')";
                            }
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.ExecuteNonQuery();
                        }

                        Cmd.CommandText = "insert into Cheque_Details values (" + Payment_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Rev_ledger_Code_TO + ", " + Rev_ledger_Code_BY + ", 1, " + ChqBookNo + ", " + ChqNo + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + FinalAmount + ", " + Company_Code + ", '" + Year_Code + "', 'TRUE', '', NULL)";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();


                        Cmd.CommandText = "insert into tax_adjustment_payment values (" + Tax_Payment_No + ",'" + String.Format("{0:dd-MMM-yyyy}", DtpFrom) + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpTo) + "', " + Payment_Code + ", '" + String.Format("{0:dd-MMM-yyyy}", DateTime.Now) + "', " + Company_Code + ", '" + Year_Code + "', '" + Narration + "', '" + ChqBookNo + "', '" + ChqNo + "', " + FinalAmount + ", '" + Reposting + "')";
                        Cmd.Connection = Cn;
                        Cmd.Transaction = Trans;
                        Cmd.ExecuteNonQuery();
                    }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                Cn_Close();
                throw ex;
            }
        }

        public Boolean IS_Multiple_Company()
        {
            Boolean Flag = false;
            System.Data.DataTable TmpDt = new System.Data.DataTable();
            try
            {
                Load_Data("Select * from STOCK_SETTINGS where MULTIPLE_COMPANY = 'Y'", ref TmpDt);
                if (TmpDt.Rows.Count > 0)
                {
                    Flag = true;
                }
                return Flag;
            }
            catch (Exception ex)
            {
                return Flag;
            }
        }

        public String Fetch_Company_Code(String Company_Code_String, String Var, int Company_Code)
        {
            String Sql = String.Empty;
            try
            {
                Sql = Var.Replace(Company_Code_String, Company_Code.ToString());
                return Sql;
            }
            catch (Exception ex)
            {
                return Sql;
            }
        }

        public Boolean Check_Instance_Running(String InstanceName)
        {
            SqlDataAdapter Adp;
            SqlCommand Cmd;
            System.Data.DataTable Tdt = new System.Data.DataTable();
            Boolean Flag = false;
            try
            {
                SqlCn_Open();
                Cmd = new SqlCommand("Select * from " + InstanceName + "Socks_Companymas", SqlCn);
                Cmd.CommandTimeout = 5;
                Adp = new SqlDataAdapter(Cmd);
                Adp.Fill(Tdt);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                SqlCn_Close();
            }
        }

        public Boolean Check_Instance_Running_ODBC(String InstanceName)
        {
            OdbcDataAdapter Adp;
            OdbcCommand Cmd;
            System.Data.DataTable Tdt = new System.Data.DataTable();
            Boolean Flag = false;
            try
            {
                Cn_Open();
                Cmd = new OdbcCommand("Select * from " + InstanceName + "Socks_Companymas", Cn);
                Cmd.CommandTimeout = 5;
                Adp = new OdbcDataAdapter(Cmd);
                Adp.Fill(Tdt);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void ShowImage(Bitmap Bmp)
        {
            try
            {
                Frm_Image_Viewer Frm = new Frm_Image_Viewer();
                Frm.Bmp = Bmp;
                Frm.StartPosition = FormStartPosition.CenterScreen;
                Frm.ShowDialog();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Qty_Format_Grid(Int16 Decimal)
        {
            String Form = String.Empty;
            try
            {
                if (Decimal == 1)
                {
                    Form = "0";
                }
                else
                {
                    Form = "0.";
                    for (Int16 i = 0; i <= Decimal - 1; i++)
                    {
                        Form = Form + "0";
                    }
                }

                return Form;
            }
            catch (Exception ex)
            {
                return Form;
            }
        }


        public String SqlServer_InstanceName()
        {
            System.Data.DataTable TDt = new System.Data.DataTable();
            try
            {
                Load_Data("select cast(@@serverName as varchar(25)) + '\\' + cast(SERVERPROPERTY ('instanceName') as varchar(25)) as Name ", ref TDt);
                if (TDt.Rows.Count > 0)
                {
                    return TDt.Rows[0]["Name"].ToString().ToUpper();
                }
                else
                {
                    return String.Empty;
                }
            }
            catch (Exception ex)
            {
                return String.Empty;
            }
        }

        public String Qty_Format(Int16 Decimal)
        {
            String Form = String.Empty;
            try
            {
                if (Decimal == 1)
                {
                    Form = "{0:0}";
                }
                else
                {
                    Form = "{0:0.";
                    for (Int16 i = 0; i <= Decimal - 1; i++)
                    {
                        Form = Form + "0";
                    }
                    Form = Form + "}";
                }
                return Form;
            }
            catch (Exception ex)
            {
                return Form;
            }
        }



        public void Run_Multiple_Company(Boolean Ledger_Edit, String CompCode_String, int[] Multiple_Company_Code, String Company_Address_String, String[] Multiple_Company_Address, int Cur_Company_Code, String Cur_Year_Code, Int32 Ledger_Code, String[] Detail_QueriesX, String[] Detail_Queries, String[] Detail_Queries1, String[] Detail_Queries2, params String[] Queries)
        {
            System.Data.DataTable TDt = new System.Data.DataTable();
            String SqlInstanceName = SqlServer_InstanceName();
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {

                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                Cmd.CommandText = "SET XACT_ABORT ON";
                Cmd.ExecuteNonQuery();

                for (int i = 0; i <= Multiple_Company_Code.Length - 1; i++)
                {
                    if (Check_Instance_Running(Multiple_Company_Address[i]))
                    {
                        foreach (String Sql in Queries)
                        {
                            if (Sql != null && Sql.Trim() != String.Empty)
                            {
                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                if (Ledger_Edit)
                                {
                                    if (Cmd.CommandText.ToUpper().Contains("LEDGER_BREAKUP") && Cmd.CommandText.ToUpper().Contains("DELETE"))
                                    {
                                        if (Multiple_Company_Code[i] == Cur_Company_Code)
                                        {
                                            Cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        Cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    Cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        foreach (String Sql in Detail_Queries2)
                        {
                            if (Sql != null && Sql.Trim() != String.Empty)
                            {
                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                if (Ledger_Edit)
                                {
                                    if (Cmd.CommandText.ToUpper().Contains("INSERT INTO") == true && Cmd.CommandText.ToUpper().Contains("LEDGER_BREAKUP") == true)
                                    {
                                        if (Multiple_Company_Code[i] == Cur_Company_Code)
                                        {
                                            Cmd.ExecuteNonQuery();
                                        }
                                    }
                                    else
                                    {
                                        Cmd.ExecuteNonQuery();
                                    }
                                }
                                else
                                {
                                    Cmd.ExecuteNonQuery();
                                }
                            }
                        }

                        foreach (String Sql in Detail_Queries1)
                        {
                            if (Sql != null && Sql.Trim() != String.Empty)
                            {
                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                Cmd.ExecuteNonQuery();
                            }
                        }

                        foreach (String Sql in Detail_Queries)
                        {
                            if (Sql != null && Sql.Trim() != String.Empty)
                            {
                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                Cmd.ExecuteNonQuery();
                            }
                        }

                        foreach (String Sql in Detail_QueriesX)
                        {
                            if (Sql != null && Sql.Trim() != String.Empty)
                            {
                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                Cmd.ExecuteNonQuery();
                            }
                        }

                        if (Ledger_Edit)
                        {
                            if (Multiple_Company_Code[i] != Cur_Company_Code)
                            {
                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, "Update " + Company_Address_String + "Ledger_Master set Ledger_Odebit = 0, ledger_Ocredit = 0 where ledger_Code = " + Ledger_Code + " and Company_Code = " + Multiple_Company_Code[i] + " and Year_Code = '" + Cur_Year_Code + "'", Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                Cmd.ExecuteNonQuery();

                                Cmd.Connection = Cn;
                                Cmd.Transaction = Trans;
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, "Update L1 set Ledger_Odebit = l2.Ledger_Odebit, Ledger_Ocredit = l2.ledger_OCredit from " + Company_Address_String + "Ledger_Master l1 inner join " + Company_Address_String + "Ledger_Previous_Balance l2 on l1.Ledger_Code = l2.Ledger_Code and l1.COMPANY_CODE = l2.Company_Code and l1.year_Code = l2.year_Code where l1.company_Code = " + Multiple_Company_Code[i] + " and L1.ledger_Code = " + Ledger_Code, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                Cmd.ExecuteNonQuery();
                            }

                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, "Delete L2 from " + Company_Address_String + "Ledger_Master l1 inner join " + Company_Address_String + "Ledger_Previous_Balance l2 on l1.Ledger_Code = l2.Ledger_Code and l1.COMPANY_CODE = l2.Company_Code and l1.year_Code = l2.year_Code where l1.ledger_Code = " + Ledger_Code + " and L1.company_Code = " + Multiple_Company_Code[i], Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                            Cmd.ExecuteNonQuery();
                        }
                    }
                }

                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public void Run_Multiple_Company(String CompCode_String, int[] Multiple_Company_Code, String Company_Address_String, String[] Multiple_Company_Address, params String[] Queries)
        {
            String SqlInstanceName = SqlServer_InstanceName();
            OdbcTransaction Trans;
            OdbcCommand Cmd = new OdbcCommand();
            Cn_Open();
            Trans = Cn.BeginTransaction();
            try
            {

                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                Cmd.CommandText = "SET XACT_ABORT ON";
                Cmd.ExecuteNonQuery();

                for (int i = 0; i <= Multiple_Company_Code.Length - 1; i++)
                {
                    foreach (String Sql in Queries)
                    {
                        if (Sql != null && Sql != String.Empty)
                        {
                            Cmd.Connection = Cn;
                            Cmd.Transaction = Trans;
                            if (Check_Instance_Running(Multiple_Company_Address[i]))
                            {
                                Cmd.CommandText = Fetch_Company_Code(SqlInstanceName, CompCode_String, Sql, Company_Address_String, Multiple_Company_Address[i], Multiple_Company_Code[i]);
                                Cmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
                Trans.Commit();
                Cn_Close();
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                throw ex;
            }
            finally
            {
                Cn_Close();
            }
        }

        public String Fetch_Company_Code(String SqlInstanceName, String Company_Code_String, String Var, String Company_Address_String, String Cur_Company_Address, int Company_Code)
        {
            String Sql = String.Empty;
            try
            {
                Sql = Var.Replace(Company_Code_String, Company_Code.ToString());
                if (Cur_Company_Address.ToUpper().Contains(SqlInstanceName))
                {
                    Sql = Sql.Replace(Company_Address_String, "dbo.");
                }
                else
                {
                    Sql = Sql.Replace(Company_Address_String, Cur_Company_Address.ToString());
                }
                return Sql;
            }
            catch (Exception ex)
            {
                return Sql;
            }
        }





        //public void Clear(ContainerControl Cr)
        //{
        //    float CurrentSize;
        //    try
        //    {
        //        Cr.BackColor = System.Drawing.Color.Tan;
        //        foreach (Control ct in Cr.Controls)
        //        {
        //            if (ct is System.Windows.Forms.GroupBox || ct is Panel || ct is FlowLayoutPanel || ct is TabControl)
        //            {
        //                if (ct.Name.ToUpper().Contains("SPECIAL") == false)
        //                {
        //                    ct.BackColor = System.Drawing.Color.Wheat;
        //                }
        //                ct.Font = new System.Drawing.Font(ct.Font, FontStyle.Bold);
        //                //CurrentSize = ct.Font.SizeInPoints;
        //                //CurrentSize += 2.0F;

        //                //ct.Font = new System.Drawing.Font(ct.Font.Name, CurrentSize);
        //                foreach (Control Co in ct.Controls)
        //                {
        //                    if (Co is System.Windows.Forms.TextBox)
        //                    {
        //                        Co.Text = String.Empty;
        //                        Co.Tag = String.Empty;
        //                        if (Co.BackColor == System.Drawing.Color.LightCyan)
        //                        {
        //                            Co.BackColor = System.Drawing.Color.White;
        //                            Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
        //                        }
        //                        else if (Co.BackColor == System.Drawing.Color.LightGreen)
        //                        {
        //                            Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
        //                        }
        //                        else
        //                        {
        //                            Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
        //                        }
        //                    }
        //                    else if (Co is System.Windows.Forms.Label)
        //                    {
        //                        Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
        //                        Co.Text = Co.Text.Replace(":", "");
        //                    }
        //                    else if (Co is System.Windows.Forms.Button)
        //                    {
        //                        Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
        //                    }
        //                    else if (Co is DataGridView)
        //                    {
        //                        DataGridView Obj;
        //                        Obj = (DataGridView)Co;
        //                        Obj.AllowUserToDeleteRows = false;
        //                        Obj.DataSource = null;
        //                    }
        //                    else if (Co is System.Windows.Forms.GroupBox)
        //                    {
        //                        foreach (Control Co1 in Co.Controls)
        //                        {
        //                            if (Co1 is System.Windows.Forms.TextBox)
        //                            {
        //                                Co1.Text = String.Empty;
        //                                Co1.Tag = String.Empty;
        //                            }
        //                        }
        //                    }
        //                    else if (Co is TabControl)
        //                    {
        //                        foreach (Control Co1 in Co.Controls)
        //                        {
        //                            if (Co1 is TabPage)
        //                            {
        //                                foreach (Control Co2 in Co1.Controls)
        //                                {
        //                                    if (Co2 is System.Windows.Forms.TextBox)
        //                                    {
        //                                        Co2.Text = String.Empty;
        //                                        Co2.Tag = String.Empty;
        //                                        if (Co2.BackColor == System.Drawing.Color.LightCyan)
        //                                        {
        //                                            Co2.BackColor = System.Drawing.Color.White;
        //                                            Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
        //                                        }
        //                                        else if (Co2.BackColor == System.Drawing.Color.LightGreen)
        //                                        {
        //                                            Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
        //                                        }
        //                                        else
        //                                        {
        //                                            Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
        //                                        }
        //                                    }
        //                                    else if (Co2 is RadioButton)
        //                                    {
        //                                        RadioButton Obj;
        //                                        Obj = (RadioButton)Co2;
        //                                        Obj.Checked = false;
        //                                    }
        //                                    else if (Co2 is DataGridView)
        //                                    {
        //                                        DataGridView Obj;
        //                                        Obj = (DataGridView)Co2;
        //                                        Obj.AllowUserToDeleteRows = false;
        //                                        Obj.DataSource = null;
        //                                    }
        //                                }
        //                            }
        //                            else if (Co1 is System.Windows.Forms.TextBox)
        //                            {
        //                                Co1.Text = String.Empty;
        //                                Co1.Tag = String.Empty;
        //                                if (Co1.BackColor == System.Drawing.Color.LightCyan)
        //                                {
        //                                    Co1.BackColor = System.Drawing.Color.White;
        //                                    Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
        //                                }
        //                                else if (Co1.BackColor == System.Drawing.Color.LightGreen)
        //                                {
        //                                    Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
        //                                }
        //                                else
        //                                {
        //                                    Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
        //                                }
        //                            }
        //                            else if (Co1 is RadioButton)
        //                            {
        //                                RadioButton Obj;
        //                                Obj = (RadioButton)Co1;
        //                                Obj.Checked = false;
        //                            }
        //                            else if (Co1 is DataGridView)
        //                            {
        //                                DataGridView Obj;
        //                                Obj = (DataGridView)Co1;
        //                                Obj.DataSource = null;
        //                                Obj.AllowUserToDeleteRows = false;
        //                            }
        //                        }
        //                    }
        //                    else if (Co is RadioButton)
        //                    {
        //                        RadioButton Obj;
        //                        Obj = (RadioButton)Co;
        //                        Obj.Checked = false;
        //                    }
        //                    else if (Co is DateTimePicker)
        //                    {
        //                        DateTimePicker Dt;
        //                        Dt = (DateTimePicker)Co;
        //                        Dt.MinDate = Convert.ToDateTime("01/01/1899");
        //                        Dt.MaxDate = Convert.ToDateTime("01/01/3999");
        //                        Dt.Value = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", DateTime.Now));
        //                        if (Dt.Name == "DtpDate")
        //                        {
        //                            if (Cr.Name == "FrmCashierDayClosingEntry" || Cr.Name == "FrmVoucherEntry")
        //                            {
        //                                Dt.Enabled = true;
        //                            }
        //                            else
        //                            {
        //                                Dt.Enabled = false;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            else if (ct is System.Windows.Forms.TextBox)
        //            {
        //                ct.Text = String.Empty;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public void Disable_Cut_Copy(System.Windows.Forms.GroupBox A)
        {
            try
            {
                foreach (Control Ct in A.Controls)
                {
                    if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Disable_Cut_Copy(System.Windows.Forms.TabPage A)
        {
            try
            {
                foreach (Control Ct in A.Controls)
                {
                    if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Clear(ContainerControl Cr)
        {
            DateTime ServerDateTime = GetServerDateTime();
            float CurrentSize;
            try
            {
                Cr.BackColor = System.Drawing.Color.Tan;
                Make_Context_Menu_Form((Form)Cr);
                foreach (Control ct in Cr.Controls)
                {
                    if (ct is System.Windows.Forms.GroupBox || ct is Panel || ct is FlowLayoutPanel || ct is TabControl)
                    {
                        if (ct.Name.ToUpper().Contains("SPECIAL") == true)
                        {
                            ct.BackColor = System.Drawing.Color.Silver;
                        }
                        else
                        {
                            ct.BackColor = System.Drawing.Color.Wheat;
                        }
                        ct.Font = new System.Drawing.Font(ct.Font, FontStyle.Bold);

                        foreach (Control Co in ct.Controls)
                        {
                            if (Co is System.Windows.Forms.TextBox)
                            {
                                Co.Text = String.Empty;
                                Co.Tag = String.Empty;
                                if (Co.BackColor == System.Drawing.Color.LightCyan)
                                {
                                    Co.BackColor = System.Drawing.Color.White;
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                }
                                else if (Co.BackColor == System.Drawing.Color.LightGreen)
                                {
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                }
                                else
                                {
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                }
                            }
                            else if (Co is System.Windows.Forms.PictureBox)
                            {
                                if (Co.Name.ToUpper().Contains("ARROW") == false)
                                {
                                    PictureBox Pct = (PictureBox)Co;
                                    Pct.Image = null;
                                }
                            }
                            else if (Co is System.Windows.Forms.Label)
                            {
                                if (Co.Name.ToUpper().Contains("SPECIAL"))
                                {
                                }
                                else
                                {
                                    Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                                    Co.Text = Co.Text.Replace(":", "");
                                }
                            }
                            else if (Co is System.Windows.Forms.Button)
                            {
                                Co.Font = new System.Drawing.Font(Co.Font, FontStyle.Bold);
                            }
                            else if (Co is DataGridView)
                            {
                                DataGridView Obj;
                                Obj = (DataGridView)Co;
                                Obj.AllowUserToDeleteRows = false;
                                Obj.DataSource = null;
                            }
                            else if (Co is System.Windows.Forms.GroupBox)
                            {
                                foreach (Control Co1 in Co.Controls)
                                {
                                    if (Co1 is System.Windows.Forms.TextBox)
                                    {
                                        Co1.Text = String.Empty;
                                        Co1.Tag = String.Empty;
                                    }
                                    else if (Co1 is PictureBox)
                                    {
                                        PictureBox Pct = (PictureBox)Co1;
                                        Pct.Image = null;
                                    }
                                }
                            }
                            else if (Co is TabControl || Co is Panel || Co is System.Windows.Forms.GroupBox || Co is FlowLayoutPanel)
                            {
                                if (Co.Name.ToUpper().Contains("SPECIAL") == true)
                                {
                                    Co.BackColor = System.Drawing.Color.Silver;
                                }
                                else
                                {
                                    Co.BackColor = System.Drawing.Color.Wheat;
                                }
                                foreach (Control Co1 in Co.Controls)
                                {
                                    if (Co1 is TabPage || Co1 is Panel || Co1 is System.Windows.Forms.GroupBox || Co1 is FlowLayoutPanel)
                                    {
                                        if (Co1.Name.ToUpper().Contains("SPECIAL") == true)
                                        {
                                            Co1.BackColor = System.Drawing.Color.Silver;
                                        }
                                        else
                                        {
                                            Co1.BackColor = System.Drawing.Color.Wheat;
                                        }

                                        foreach (Control Co2 in Co1.Controls)
                                        {
                                            if (Co2 is System.Windows.Forms.TextBox)
                                            {
                                                Co2.Text = String.Empty;
                                                Co2.Tag = String.Empty;
                                                if (Co2.BackColor == System.Drawing.Color.LightCyan)
                                                {
                                                    Co2.BackColor = System.Drawing.Color.White;
                                                    Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
                                                }
                                                else if (Co2.BackColor == System.Drawing.Color.LightGreen)
                                                {
                                                    Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
                                                }
                                                else
                                                {
                                                    Co2.Font = new System.Drawing.Font(Co2.Font, FontStyle.Bold);
                                                }
                                            }
                                            else if (Co2 is PictureBox)
                                            {
                                                if (Co2.Name.ToUpper().Contains("ARROW") == false)
                                                {
                                                    PictureBox P = (PictureBox)Co2;
                                                    P.Image = null;
                                                }
                                            }
                                            else if (Co2 is RadioButton)
                                            {
                                                RadioButton Obj;
                                                Obj = (RadioButton)Co2;
                                                Obj.Checked = false;
                                            }
                                            else if (Co2 is DataGridView)
                                            {
                                                DataGridView Obj;
                                                Obj = (DataGridView)Co2;
                                                Obj.AllowUserToDeleteRows = false;
                                                Obj.DataSource = null;
                                            }
                                        }
                                    }
                                    else if (Co1 is System.Windows.Forms.TextBox)
                                    {
                                        Co1.Text = String.Empty;
                                        Co1.Tag = String.Empty;
                                        if (Co1.BackColor == System.Drawing.Color.LightCyan)
                                        {
                                            Co1.BackColor = System.Drawing.Color.White;
                                            Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
                                        }
                                        else if (Co1.BackColor == System.Drawing.Color.LightGreen)
                                        {
                                            Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
                                        }
                                        else
                                        {
                                            Co1.Font = new System.Drawing.Font(Co1.Font, FontStyle.Bold);
                                        }
                                    }
                                    else if (Co1 is RadioButton)
                                    {
                                        RadioButton Obj;
                                        Obj = (RadioButton)Co1;
                                        Obj.Checked = false;
                                    }
                                    else if (Co1 is DataGridView)
                                    {
                                        DataGridView Obj;
                                        Obj = (DataGridView)Co1;
                                        Obj.DataSource = null;
                                        Obj.AllowUserToDeleteRows = false;
                                    }
                                }
                            }
                            else if (Co is RadioButton)
                            {
                                RadioButton Obj;
                                Obj = (RadioButton)Co;
                                Obj.Checked = false;
                            }
                            else if (Co is PictureBox)
                            {
                                if (Co.Name.ToUpper().Contains("ARROW") == false)
                                {
                                    PictureBox P = (PictureBox)Co;
                                    P.Image = null;
                                }
                            }
                            else if (Co is DateTimePicker)
                            {
                                DateTimePicker Dt;
                                Dt = (DateTimePicker)Co;
                                Dt.MinDate = Convert.ToDateTime("01/01/1899");
                                Dt.MaxDate = Convert.ToDateTime("01/01/3999");
                                Dt.Value = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", ServerDateTime));
                                if (Dt.Name == "DtpDate")
                                {
                                    if (Cr.Name == "FrmCashierDayClosingEntry" || Cr.Name == "FrmVoucherEntry")
                                    {
                                        Dt.Enabled = true;
                                    }
                                    else
                                    {
                                        Dt.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                    else if (ct is System.Windows.Forms.TextBox)
                    {
                        ct.Text = String.Empty;
                    }
                    else if (ct is PictureBox)
                    {
                        if (ct.Name.ToUpper().Contains("ARROW") == false)
                        {
                            PictureBox P = (PictureBox)ct;
                            P.Image = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Make_Context_Menu_Form(Form Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.GroupBox)
                    {
                        Make_Context_Menu((System.Windows.Forms.GroupBox)Ct);
                    }
                    else if (Ct is System.Windows.Forms.Panel)
                    {
                        Make_Context_Menu((System.Windows.Forms.Panel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabPage)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabPage)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabControl)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabControl)Ct);
                    }
                    else if (Ct is System.Windows.Forms.FlowLayoutPanel)
                    {
                        Make_Context_Menu((System.Windows.Forms.FlowLayoutPanel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Make_Context_Menu(System.Windows.Forms.GroupBox Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.TabControl)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabControl)Ct);
                    }
                    else if (Ct is System.Windows.Forms.GroupBox)
                    {
                        Make_Context_Menu((System.Windows.Forms.GroupBox)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabPage)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabPage)Ct);
                    }
                    else if (Ct is System.Windows.Forms.Panel)
                    {
                        Make_Context_Menu((System.Windows.Forms.Panel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.FlowLayoutPanel)
                    {
                        Make_Context_Menu((System.Windows.Forms.FlowLayoutPanel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Make_Context_Menu(System.Windows.Forms.TabPage Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.TabControl)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabControl)Ct);
                    }
                    else if (Ct is System.Windows.Forms.GroupBox)
                    {
                        Make_Context_Menu((System.Windows.Forms.GroupBox)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabPage)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabPage)Ct);
                    }
                    else if (Ct is System.Windows.Forms.Panel)
                    {
                        Make_Context_Menu((System.Windows.Forms.Panel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.FlowLayoutPanel)
                    {
                        Make_Context_Menu((System.Windows.Forms.FlowLayoutPanel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Make_Context_Menu(System.Windows.Forms.TabControl Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.TabControl)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabControl)Ct);
                    }
                    else if (Ct is System.Windows.Forms.GroupBox)
                    {
                        Make_Context_Menu((System.Windows.Forms.GroupBox)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabPage)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabPage)Ct);
                    }
                    else if (Ct is System.Windows.Forms.Panel)
                    {
                        Make_Context_Menu((System.Windows.Forms.Panel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.FlowLayoutPanel)
                    {
                        Make_Context_Menu((System.Windows.Forms.FlowLayoutPanel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Make_Context_Menu(System.Windows.Forms.FlowLayoutPanel Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.TabControl)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabControl)Ct);
                    }
                    else if (Ct is System.Windows.Forms.GroupBox)
                    {
                        Make_Context_Menu((System.Windows.Forms.GroupBox)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabPage)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabPage)Ct);
                    }
                    else if (Ct is System.Windows.Forms.Panel)
                    {
                        Make_Context_Menu((System.Windows.Forms.Panel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.FlowLayoutPanel)
                    {
                        Make_Context_Menu((System.Windows.Forms.FlowLayoutPanel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Make_Context_Menu(System.Windows.Forms.Panel Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.TabControl)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabControl)Ct);
                    }
                    else if (Ct is System.Windows.Forms.GroupBox)
                    {
                        Make_Context_Menu((System.Windows.Forms.GroupBox)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TabPage)
                    {
                        Make_Context_Menu((System.Windows.Forms.TabPage)Ct);
                    }
                    else if (Ct is System.Windows.Forms.Panel)
                    {
                        Make_Context_Menu((System.Windows.Forms.Panel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.FlowLayoutPanel)
                    {
                        Make_Context_Menu((System.Windows.Forms.FlowLayoutPanel)Ct);
                    }
                    else if (Ct is System.Windows.Forms.TextBox)
                    {
                        Ct.ContextMenu = new ContextMenu();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        
        public void Color(ContainerControl Cr)
        {
            try
            {
                if (Cr is Form)
                {
                    //Cr.BackColor = System.Drawing.Color.w  
                }
                foreach (Control Ct in Cr.Controls)
                {
                    if (Ct is System.Windows.Forms.GroupBox || Ct is Panel || Ct is FlowLayoutPanel || Ct is TabControl)
                    {
                        foreach (Control Co in Ct.Controls)
                        {
                            if (Co is System.Windows.Forms.TextBox)
                            {
                                if (Convert.ToString(Co.Tag).Trim() != String.Empty && Convert.ToString(Co.Tag).Trim() ==   "1")
                                {
                                    Co.BackColor = System.Drawing.Color.LightBlue; 
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Decimal(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 58 || Convert.ToInt16(e.KeyChar) == 46 || Convert.ToInt16(e.KeyChar) == 8)
                {
                    if (Convert.ToInt16(e.KeyChar) == 46)
                    {
                        if (txt.Text.Contains(".") == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (txt.Text.Length > 0) //.Trim() != String.Empty) 
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Valid_Semicolon(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 59 || Convert.ToInt16(e.KeyChar) == 8)
                {
                    if (Convert.ToInt16(e.KeyChar) == 58)
                    {
                        if (txt.Text.Contains(":") == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (txt.Text.Length > 0) //.Trim() != String.Empty) 
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Semicolon_Decimal(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 59 || Convert.ToInt16(e.KeyChar) == 46 || Convert.ToInt16(e.KeyChar) == 8)
                {
                    if (Convert.ToInt16(e.KeyChar) == 58)
                    {
                        if (txt.Text.Contains(":") == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (txt.Text.Length > 0) //.Trim() != String.Empty) 
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    else if (Convert.ToInt16(e.KeyChar) == 46)
                    {
                        if (txt.Text.Contains(".") == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (txt.Text.Length > 0) //.Trim() != String.Empty) 
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Valid_DecimalPlusMinus(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 58 || Convert.ToInt16(e.KeyChar) == 46 || Convert.ToInt16(e.KeyChar) == 8)
                {
                    if (Convert.ToInt16(e.KeyChar) == 46)
                    {
                        if (txt.Text.Contains(".") == true)
                        {
                            e.Handled = true;
                        }
                        else
                        {
                            if (txt.Text.Length > 0) //.Trim() != String.Empty) 
                            {
                                e.Handled = false;
                            }
                            else
                            {
                                e.Handled = true;
                            }
                        }
                    }
                    else
                    {
                        e.Handled = false;
                    }
                }
                else
                {
                    if (Convert.ToInt16(e.KeyChar) == 43 || Convert.ToInt16(e.KeyChar) == 45)
                    {
                        if (Convert.ToInt32(txt.Text.Length) > 0)
                        {
                            if (txt.Text.Contains("+") || txt.Text.Contains("-"))
                            {
                                txt.Text = txt.Text.Replace("+", Convert.ToString(e.KeyChar));
                                txt.Text = txt.Text.Replace("-", Convert.ToString(e.KeyChar));
                            }
                            else
                            {
                                txt.Text = Convert.ToString(e.KeyChar) + txt.Text;
                            }
                            txt.Select(1, txt.Text.Length - 1);
                            e.Handled = true;
                        }
                        else
                        {
                            e.Handled = false;
                        }
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Number(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (char.IsDigit(e.KeyChar) == false && e.KeyChar != Convert.ToChar(Keys.Back))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Alpha_Numeric(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) > 47 && Convert.ToInt16(e.KeyChar) < 59 || Convert.ToInt16(e.KeyChar) > 64 && Convert.ToInt16(e.KeyChar) < 91 || Convert.ToInt16(e.KeyChar) > 96 && Convert.ToInt16(e.KeyChar) < 123 || Convert.ToInt16(e.KeyChar) == 8)
                {
                    e.Handled = false;
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Phone(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (char.IsDigit(e.KeyChar) == false && e.KeyChar != Convert.ToChar(Keys.Back))
                {
                    if (e.KeyChar == Convert.ToInt32('+') || e.KeyChar == Convert.ToInt32(' ') || e.KeyChar == Convert.ToInt32('-') || e.KeyChar == Convert.ToInt32(',') || e.KeyChar == Convert.ToInt32('/'))
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_P_OR_B(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToInt32('P') || e.KeyChar == Convert.ToInt32('p'))
                {
                    e.Handled = true;
                    txt.Text = "P";
                }
                else if (e.KeyChar == Convert.ToInt32('B') || e.KeyChar == Convert.ToInt32('b'))
                {
                    e.Handled = true;
                    txt.Text = "B";
                }
                else if (e.KeyChar == Convert.ToInt32('F') || e.KeyChar == Convert.ToInt32('f'))
                {
                    e.Handled = true;
                    txt.Text = "F";
                }
                else if (e.KeyChar == Convert.ToInt32('K') || e.KeyChar == Convert.ToInt32('k'))
                {
                    e.Handled = true;
                    txt.Text = "K";
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Yes_OR_No(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToInt32('Y') || e.KeyChar == Convert.ToInt32('y'))
                {
                    e.Handled = true;
                    txt.Text = "Y";
                }
                else if (e.KeyChar == Convert.ToInt32('N') || e.KeyChar == Convert.ToInt32('n'))
                {
                    e.Handled = true;
                    txt.Text = "N";
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Fill_Null(ContainerControl Ct)
        {
            try
            {
                foreach (Control Cr in Ct.Controls)
                {
                    if (Cr is System.Windows.Forms.GroupBox || Cr is Panel || Cr is FlowLayoutPanel || Cr is TabControl)
                    {
                        foreach (Control Co in Cr.Controls)
                        {
                            if (Co is System.Windows.Forms.TextBox)
                            {
                                if (Co.Text.Trim() == String.Empty)
                                {
                                    Co.Text = "-";
                                }
                            }
                        }
                    }
                    else if (Cr is System.Windows.Forms.TextBox)
                    {
                        if (Cr.Text.Trim() == String.Empty)
                        {
                            Cr.Text = "-";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Enable_Controls(ContainerControl Ct, Boolean Flag)
        {
            try
            {
                foreach (Control Cr in Ct.Controls)
                {
                    if (Cr is System.Windows.Forms.GroupBox || Cr is Panel || Cr is FlowLayoutPanel || Cr is TabControl)
                    {
                        foreach (Control Co in Cr.Controls)
                        {
                            if (Co is System.Windows.Forms.TextBox)
                            {
                                Co.Enabled = Flag;
                            }
                        }
                    }
                    else if (Cr is System.Windows.Forms.TextBox)
                    {
                        Ct.Enabled = Flag;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Null(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Valid_Date(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyPressEventArgs e)
        {
            try
            {
                if (char.IsDigit(e.KeyChar) == false && e.KeyChar != Convert.ToChar(Keys.Back))
                {
                    if (e.KeyChar == Convert.ToChar(47) || e.KeyChar == Convert.ToChar(46))
                    {

                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Handle_Delete(System.Windows.Forms.TextBox txt, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete) 
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Return_Ucase(System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsLower(e.KeyChar))
            {
                e.KeyChar = char.ToUpper(e.KeyChar);
            }
        }

        public void EnableContainer12(ContainerControl Frm)
        {
            try
            {
                foreach (Control Ct in Frm.Controls)
                {
                    if (Ct is System.Windows.Forms.GroupBox || Ct is FlowLayoutPanel || Ct is Panel || Ct is TabControl)
                    {
                        Ct.Enabled = true; 
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ActiveForm_Close(Form Frm, MDIMain Par)
        {
            try
            {
                if (Frm.ActiveControl is System.Windows.Forms.TextBox)
                {
                    if (Frm.ActiveControl.Text == String.Empty)
                    {
                        if (Par._Form == true)
                        {
                            if (MessageBox.Show("Sure to Close ...!", "Close ?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                            {
                                Frm.Close();
                            }
                        }
                        else
                        {
                            if (MessageBox.Show("Sure to Clear All Controls ...!", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                            {
                                Par.MenuButton_Status("Form");
                                Par.Replace_Caption();
                                Clear(Frm);
                                Frm.MdiParent.Focus();
                                //Frm.Focus();
                            }
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Sure to Clear ...!", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Frm.ActiveControl.Text = String.Empty;
                        }
                    }
                }
                else if (Frm.ActiveControl is DataGridView)
                {
                    //DataGridView Obj;
                    //Obj = (DataGridView)Frm.ActiveControl;
                    //Obj.DataSource = null;
                }
                else
                {
                    if (Par._Form == true)
                    {
                        if (MessageBox.Show("Sure to Close ...!", "Close ?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                        {
                            Frm.Close();
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("Sure to Clear All Controls ...!", "Clear", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                        {
                            Par.MenuButton_Status("Form");
                            Par.Replace_Caption();
                            Clear(Frm);
                            //Frm.Focus();
                            Frm.MdiParent.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Make_Beep()
        {
            try
            {
                Beep(4000, 100);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Check_Directory(String Dir)
        {
            try
            {
                if (System.IO.Directory.Exists (Dir) == true)
                {
                    return true;
                }
                else
                {
                    return false; 
                }
            }
            catch (Exception ex)
            {
                throw ex; 
            }
        }

        public void Get_Import(Form Frm)
        {
            try
            {
                if (Check_Directory(System.Windows.Forms.Application.StartupPath + "\\Import"))
                {
                    DirectoryInfo Dr = new DirectoryInfo(System.Windows.Forms.Application.StartupPath + "\\Import");
                    FileInfo[] Fl = Dr.GetFiles("*.   ");
                    if (Fl.Length > 0)
                    {
                        foreach (FileInfo Fi in Fl)
                        {
                            Frm.Controls["Tool_Box"].Controls["TxtFpath"].Text = "Import\\"+ Fi.Name ;
                            Frm.Controls["Tool_Box"].Controls["TxtLTime"].Text = Convert.ToString(Fi.LastWriteTime); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Get_Restore(Form Frm)
        {
            try
            {
                if (Check_Directory("C:\\vaahrep\\Restore"))
                {
                    DirectoryInfo Dr = new DirectoryInfo("C:\\vaahrep\\Restore");
                    FileInfo[] Fl = Dr.GetFiles("*.DMP");
                    if (Fl.Length > 0)
                    {
                        foreach (FileInfo Fi in Fl)
                        {
                            Frm.Controls["Tool_Box"].Controls["TxtFpath"].Text = "Restore\\" + Fi.Name;
                            Frm.Controls["Tool_Box"].Controls["TxtLTime"].Text = Convert.ToString(Fi.LastWriteTime);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Boolean Restore_Sql (String DBName, String FPath)
        {
            try
            {
                String Sql;
                BackupCn_Open();
                Sql = @"Restore DataBase " + DBName + " from Disk = '" + FPath + "'";
                OdbcCommand Cmd = new OdbcCommand(Sql, BackupCn);
                Cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                BackupCn_Close();
            }
        }

        public void ReSize_Form1(Form Frm, Boolean MaxOrNot, int DefWidth, int DefHeight)
        {
            Decimal Font_Size;
            Decimal DGVWidth=0;
            try
            {
                Def_Height = DefHeight;
                Def_Width = DefWidth;

                foreach (Control Cr in Frm.Controls)
                {
                    if (Cr is System.Windows.Forms.GroupBox || Cr is Panel || Cr is FlowLayoutPanel || Cr is TabControl)
                    {
                        if (MaxOrNot == true)
                        {
                            Def_MaxHeight = Frm.Height;
                            Def_MaxWidth = Frm.Width;
                            Font_Size = 13;
                            //Cr.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                            Cr.Width = Convert.ToInt32(Convert.ToDecimal(Cr.Width) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                            Cr.Left = Convert.ToInt32(Convert.ToDecimal(Cr.Left) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                            Cr.Height = Convert.ToInt32(Convert.ToDecimal(Cr.Height) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                            Cr.Top = Convert.ToInt32(Convert.ToDecimal(Cr.Top) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                            foreach (Control Ct in Cr.Controls)
                            {
                                if (Ct is System.Windows.Forms.Label || Ct is System.Windows.Forms.TextBox)
                                {
                                    //Font_Size = 13;
                                    //Ct.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                }
                                else if (Ct is System.Windows.Forms.DataGridView)
                                {
                                    //Font_Size = 13;
                                    //Ct.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    DGVWidth = Ct.Width;
                                }
                                Ct.Width = Convert.ToInt32(Convert.ToDecimal(Ct.Width) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                                Ct.Left = Convert.ToInt32(Convert.ToDecimal(Ct.Left) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                                Ct.Height = Convert.ToInt32(Convert.ToDecimal(Ct.Height) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                                Ct.Top = Convert.ToInt32(Convert.ToDecimal(Ct.Top) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                                if (Ct is System.Windows.Forms.DataGridView)
                                {
                                    DataGridView D1;
                                    D1 = (DataGridView)Ct;
                                    foreach (DataGridViewColumn Dc in D1.Columns)
                                    {
                                        //Dc.Width = Convert.ToInt32((Convert.ToDecimal(Dc.Width) / Convert.ToDecimal(DGVWidth)) * Ct.Width);
                                    }
                                    D1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                                }
                            }
                        }
                        else
                        {
                            if (Def_MaxHeight != 0)
                            {
                                Font_Size = Convert.ToInt32(Convert.ToDecimal(Cr.Font.Size) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                //Cr.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                Cr.Width = Convert.ToInt32(Convert.ToDecimal(Cr.Width) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                Cr.Left = Convert.ToInt32(Convert.ToDecimal(Cr.Left) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                Cr.Height = Convert.ToInt32(Convert.ToDecimal(Cr.Height) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                Cr.Top = Convert.ToInt32(Convert.ToDecimal(Cr.Top) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                foreach (Control Ct in Cr.Controls)
                                {
                                    if (Ct is System.Windows.Forms.Label || Ct is System.Windows.Forms.TextBox)
                                    {
                                        //Font_Size = 8;
                                        //Ct.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    }
                                    else if (Ct is System.Windows.Forms.DataGridView)
                                    {
                                        DGVWidth = Ct.Width;
                                        //Font_Size = 8;
                                        //Ct.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    }
                                    Ct.Width = Convert.ToInt32(Convert.ToDecimal(Ct.Width) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                    Ct.Left = Convert.ToInt32(Convert.ToDecimal(Ct.Left) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                    Ct.Height = Convert.ToInt32(Convert.ToDecimal(Ct.Height) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                    Ct.Top = Convert.ToInt32(Convert.ToDecimal(Ct.Top) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                    if (Ct is System.Windows.Forms.DataGridView)
                                    {
                                        DataGridView D1;
                                        D1 = (DataGridView)Ct;
                                        foreach (DataGridViewColumn Dc in D1.Columns)
                                        {
                                            //Dc.Width = Convert.ToInt32((Convert.ToDecimal(Dc.Width) / Convert.ToDecimal(DGVWidth)) * Ct.Width);
                                        }
                                        D1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Resize_Frame(Control Ct, int DefWidth, int DefHeight)
        {
            Decimal Font_Size;
            Decimal DGVWidth = 0;
            String FontName = "Courier New";
            try
            {
                Def_Height = DefHeight;
                Def_Width = DefWidth;

                foreach (Control Ct1 in Ct.Controls)
                {
                    if (Screen.PrimaryScreen.Bounds.Width >= 1024)
                    {
                        if (Ct1 is System.Windows.Forms.Label || Ct1 is System.Windows.Forms.TextBox || Ct1 is System.Windows.Forms.CheckBox || Ct1 is System.Windows.Forms.GroupBox || Ct1 is System.Windows.Forms.CheckedListBox)
                        {
                            Font_Size = 11;
                            Ct1.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                        }
                        if (Ct1 is DateTimePicker)
                        {
                            Font_Size = 9;
                            Ct1.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                        }
                    }
                    Ct1.Width = Convert.ToInt32(Convert.ToDecimal(Ct1.Width) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                    Ct1.Left = Convert.ToInt32(Convert.ToDecimal(Ct1.Left) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                    Ct1.Height = Convert.ToInt32(Convert.ToDecimal(Ct1.Height) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                    Ct1.Top = Convert.ToInt32(Convert.ToDecimal(Ct1.Top) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                    if (Ct1 is System.Windows.Forms.DataGridView)
                    {
                        if (Screen.PrimaryScreen.Bounds.Width >= 1024)
                        {
                            DataGridView D1;
                            D1 = (DataGridView)Ct1;
                            D1.RowTemplate.Height += 5;
                            D1.DefaultCellStyle.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToDecimal(Ct.Font.Size + 1).ToString()), FontStyle.Bold);
                        }
                    }
                    if (Ct1 is System.Windows.Forms.DateTimePicker)
                    {
                        if (Screen.PrimaryScreen.Bounds.Width < 1024)
                        {
                            Ct1.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToDecimal(Ct1.Font.Size - 2).ToString()), FontStyle.Bold);
                        }
                    }
                    if (Ct1 is System.Windows.Forms.GroupBox || Ct1 is Panel || Ct1 is FlowLayoutPanel || Ct1 is TabControl)
                    {
                        Font_Size = 8;
                        Ct1.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                        if (Ct1.Name == "GBDailyBalance")
                        {
                            Ct1.BackColor = System.Drawing.Color.MediumSeaGreen;
                        }
                        else
                        {
                            Ct1.BackColor = System.Drawing.Color.Wheat;
                        }
                        Ct1.Font = new System.Drawing.Font(Ct1.Font, FontStyle.Bold);
                        Resize_Frame(Ct1, Def_Width, Def_Height);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ReSize_Form(Form Frm, Boolean MaxOrNot, int DefWidth, int DefHeight)
        {
            Decimal Font_Size;
            Decimal DGVWidth = 0;
            String FontName = "Courier New";
            try
            {
                Def_Height = DefHeight;
                Def_Width = DefWidth;

                foreach (Control Cr in Frm.Controls)
                {
                    if (Cr is System.Windows.Forms.GroupBox || Cr is Panel || Cr is FlowLayoutPanel || Cr is TabControl)
                    {
                        if (MaxOrNot == true)
                        {
                            Def_MaxHeight = Frm.Height;
                            Def_MaxWidth = Frm.Width;
                            Cr.Width = Convert.ToInt32(Convert.ToDecimal(Cr.Width) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                            Cr.Left = Convert.ToInt32(Convert.ToDecimal(Cr.Left) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                            Cr.Height = Convert.ToInt32(Convert.ToDecimal(Cr.Height) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                            Cr.Top = Convert.ToInt32(Convert.ToDecimal(Cr.Top) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                            foreach (Control Ct in Cr.Controls)
                            {
                                if (Screen.PrimaryScreen.Bounds.Width >= 1024)
                                {
                                    if (Ct is System.Windows.Forms.Label || Ct is System.Windows.Forms.TextBox || Ct is System.Windows.Forms.CheckBox || Ct is System.Windows.Forms.GroupBox || Ct is System.Windows.Forms.CheckedListBox)
                                    {
                                        Font_Size = 11;
                                        Ct.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    }
                                    if (Ct is DateTimePicker)
                                    {
                                        Font_Size = 9;
                                        Ct.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    }
                                }
                                Ct.Width = Convert.ToInt32(Convert.ToDecimal(Ct.Width) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                                Ct.Left = Convert.ToInt32(Convert.ToDecimal(Ct.Left) / Convert.ToDecimal(Def_Width) * Def_MaxWidth);
                                Ct.Height = Convert.ToInt32(Convert.ToDecimal(Ct.Height) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                                Ct.Top = Convert.ToInt32(Convert.ToDecimal(Ct.Top) / Convert.ToDecimal(Def_Height) * Def_MaxHeight);
                                if (Ct is System.Windows.Forms.DataGridView)
                                {
                                    if (Screen.PrimaryScreen.Bounds.Width >= 1024)
                                    {
                                        DataGridView D1;
                                        D1 = (DataGridView)Ct;
                                        D1.RowTemplate.Height += 5;
                                        D1.DefaultCellStyle.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToDecimal(Ct.Font.Size + 1).ToString()), FontStyle.Bold);
                                    }
                                }
                                if (Ct is System.Windows.Forms.DateTimePicker)
                                {
                                    if (Screen.PrimaryScreen.Bounds.Width < 1024)
                                    {
                                        //Ct.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToDecimal(Ct.Font.Size - 2).ToString()), FontStyle.Bold);
                                        Ct.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToDecimal(Ct.Font.Size).ToString()), FontStyle.Bold);
                                    }
                                }
                                if (Ct is System.Windows.Forms.GroupBox || Ct is Panel || Ct is FlowLayoutPanel || Ct is TabControl)
                                {
                                    Font_Size = 8;
                                    Ct.Font = new System.Drawing.Font(FontName, float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    if (Ct.Name == "GBDailyBalance")
                                    {
                                        Ct.BackColor = System.Drawing.Color.MediumSeaGreen;
                                    }
                                    else
                                    {
                                        Ct.BackColor = System.Drawing.Color.Wheat;
                                    }
                                    Ct.Font = new System.Drawing.Font(Ct.Font, FontStyle.Bold);
                                    Resize_Frame(Ct, Def_Width, Def_Height);
                                }
                            }
                        }
                        else
                        {
                            if (Def_MaxHeight != 0)
                            {
                                //Font_Size = Convert.ToInt32(Convert.ToDecimal(Cr.Font.Size) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                //Cr.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                Cr.Width = Convert.ToInt32(Convert.ToDecimal(Cr.Width) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                Cr.Left = Convert.ToInt32(Convert.ToDecimal(Cr.Left) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                Cr.Height = Convert.ToInt32(Convert.ToDecimal(Cr.Height) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                Cr.Top = Convert.ToInt32(Convert.ToDecimal(Cr.Top) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                foreach (Control Ct in Cr.Controls)
                                {
                                    if (Ct is System.Windows.Forms.Label || Ct is System.Windows.Forms.TextBox)
                                    {
                                        //Font_Size = 8;
                                        //Ct.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    }
                                    else if (Ct is System.Windows.Forms.DataGridView)
                                    {
                                        DGVWidth = Ct.Width;
                                        //Font_Size = 8;
                                        //Ct.Font = new System.Drawing.Font("Verdana", float.Parse(Convert.ToString(Font_Size)), FontStyle.Bold);
                                    }
                                    Ct.Width = Convert.ToInt32(Convert.ToDecimal(Ct.Width) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                    Ct.Left = Convert.ToInt32(Convert.ToDecimal(Ct.Left) / Convert.ToDecimal(Def_MaxWidth) * Frm.Width);
                                    Ct.Height = Convert.ToInt32(Convert.ToDecimal(Ct.Height) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                    Ct.Top = Convert.ToInt32(Convert.ToDecimal(Ct.Top) / Convert.ToDecimal(Def_MaxHeight) * Frm.Height);
                                    if (Ct is System.Windows.Forms.DataGridView)
                                    {
                                        DataGridView D1;
                                        D1 = (DataGridView)Ct;
                                        foreach (DataGridViewColumn Dc in D1.Columns)
                                        {
                                            //Dc.Width = Convert.ToInt32((Convert.ToDecimal(Dc.Width) / Convert.ToDecimal(DGVWidth)) * Ct.Width);
                                        }
                                        D1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Grid_ParticularRowsColouring(ref DataGridView Dgv, params int[] Rows)
        {
            try
            {
                foreach (int i in Rows)
                {
                    Dgv.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_ParticularColsColouring(ref DataGridView Dgv, params String[] ColNames)
        {
            try
            {
                foreach (String Col in ColNames)
                {
                    Dgv.Columns[Col].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_ParticularColsColouring(ref MyDataGridView Dgv, params String[] ColName)
        {
            try
            {
                foreach (String Col in ColName)
                {
                    //Dgv.Columns[Col].DefaultCellStyle.BackColor = System.Drawing.Color.LightYellow;
                    Dgv.Columns[Col].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Delete(ref DotnetVFGrid.MyDataGridView DGV, ref System.Data.DataTable Dt, int Position)
        {
            try
            {
                if (Position <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Delete this ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Position);
                        Dt.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Insert(ref DotnetVFGrid.MyDataGridView DGV, ref System.Data.DataTable Dt, int Position, String ColumnToBeFocused)
        {
            try
            {
                if (Position <= Dt.Rows.Count)
                {
                    if (MessageBox.Show("Sure to Insert here ?", "Vaahini", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.InsertAt(Dt.NewRow(), Position);

                        DGV.CurrentCell = DGV[ColumnToBeFocused, Position];
                        DGV.Focus();
                        DGV.BeginEdit(true);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Freeze(ref DataGridView DGV, FreezeBY By, int RowColPosition)
        {
            try
            {
                if (By == FreezeBY.Column_Wise)
                {
                    DGV.Columns[RowColPosition].Frozen = true;
                }
                else
                {
                    DGV.Rows[RowColPosition].Frozen = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Freeze(ref MyDataGridView DGV, FreezeBY By, int RowColPosition)
        {
            try
            {
                if (By == FreezeBY.Column_Wise)
                {
                    DGV.Columns[RowColPosition].Frozen = true;
                }
                else
                {
                    DGV.Rows[RowColPosition].Frozen = true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_UnFreeze(ref DataGridView DGV, FreezeBY By)
        {
            try
            {
                if (By == FreezeBY.Column_Wise)
                {
                    DGV.Columns[0].Frozen = false;
                    OdbcCommand Cmd = new OdbcCommand();
                }
                else
                {
                    DGV.Rows[0].Frozen = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

        public void Grid_Designing(ref MyDataGridView DGV, ref System.Data.DataTable Dt, params String[] ToHideColumnsName)
        {
            try
            {
                for (int i = 0; i <= Dt.Columns.Count - 1; i++)
                {
                    if (Dt.Columns[i].ColumnName.ToUpper().Contains("RECEIPT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PAYMENT") || Dt.Columns[i].ColumnName.ToUpper().Contains("AMOUNT") || Dt.Columns[i].ColumnName.ToUpper().Contains("TO_PAY") || Dt.Columns[i].ColumnName.ToUpper().Contains("CHARGE") || Dt.Columns[i].ColumnName.ToUpper().Contains("PRICE") || Dt.Columns[i].ColumnName.ToUpper().Contains("RATE") || Dt.Columns[i].ColumnName.ToUpper().Contains("_PER") || Dt.Columns[i].ColumnName.ToUpper().Contains("DENOMINATION") || Dt.Columns[i].ColumnName.ToUpper().Contains("VALUE") || Dt.Columns[i].ColumnName.ToUpper().Contains("DEBIT") || Dt.Columns[i].ColumnName.ToUpper().Contains("CREDIT"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Dt.Columns[i].ColumnName.ToUpper() != "RATE_DETAILS")
                            {
                                DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                                //DGV.Columns[i].DefaultCellStyle.Format = "0.00";
                                DGV.Columns[i].DefaultCellStyle.Format = "n";
                                DGV.Columns[i].Width = 100;
                            }
                        }
                    }
                    else if (Dt.Columns[i].ColumnName.ToUpper().Contains("BALANCE") || Dt.Columns[i].ColumnName.ToUpper().Contains("CURBAL") || Dt.Columns[i].ColumnName.ToUpper().Contains("OPBAL"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            DGV.Columns[i].Width = 150;
                        }
                    }
                    else if (Dt.Columns[i].ColumnName.ToUpper().Contains("MTR"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            DGV.Columns[i].DefaultCellStyle.Format = "0.000";
                            DGV.Columns[i].Width = 100;
                        }
                    }
                    else if (Dt.Columns[i].ColumnName.ToUpper().Contains("SLNO") || Dt.Columns[i].ColumnName.ToUpper().Contains("QMT") || Dt.Columns[i].ColumnName.ToUpper().Contains("QTY") || Dt.Columns[i].ColumnName.ToUpper().Contains("PCI") || Dt.Columns[i].ColumnName.ToUpper().Contains("NO_QTY") || Dt.Columns[i].ColumnName.ToUpper().Contains("PAIRS"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Dt.Columns[i].ColumnName.ToUpper().Contains("QMT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PCI") || Dt.Columns[i].ColumnName.ToUpper().Contains("NO_QTY") || Dt.Columns[i].ColumnName.ToUpper().Contains("QTY") || Dt.Columns[i].ColumnName.ToUpper().Contains("PAIRS"))
                            {
                                DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                            DGV.Columns[i].Width = 60;
                        }
                    }
                }
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    DGV.Columns[Dc.Name].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                foreach (String Sql in ToHideColumnsName)
                {
                    DGV.Columns[Sql].Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String Separate_Amount(Double Amount)
        {
            String Am; String Am1; String PaisAm = string.Empty;
            String IntAm = string.Empty, AgC = String.Empty;
            Int32 Pos = 0;
            String FAm = string.Empty;
            try
            {
                Am = Amount.ToString();
                if (Am.Contains ("."))
                {
                    IntAm = Am.Substring (0, Am.IndexOf("."));
                    PaisAm = Am.Substring (Am.IndexOf(".") + 1, (Am.Length - (Am.IndexOf(".")+ 1)));
                }
                else
                {
                    IntAm = Am;
                }
                if (IntAm.Length > 9)
                {
                    AgC = IntAm.Substring(0, IntAm.Length - 9);
                    IntAm = IntAm.Substring(AgC.Length, IntAm.Length - AgC.Length);
                }
                while (IntAm.Length > 3)
                {
                    if (FAm.Length > 0)
                    {
                        if (IntAm.Length % 2 == 1)
                        {
                            FAm += "," + IntAm.Substring(0, 2);
                        }
                        else
                        {
                            FAm += "," + IntAm.Substring(0, 1);
                        }
                    }
                    else
                    {
                        if (IntAm.Length % 2 == 1)
                        {
                            FAm = IntAm.Substring(0, 2);
                        }
                        else
                        {
                            FAm = IntAm.Substring(0, 1);
                        }
                    }
                    if (IntAm.Length % 2 == 1)
                    {
                        IntAm = IntAm.Substring(2, (IntAm.Length - 2));
                    }
                    else
                    {
                        IntAm = IntAm.Substring(1, (IntAm.Length - 1));
                    }
                }
                if (AgC.Length > 0)
                {
                    FAm = AgC + FAm;
                }
                if (FAm.Length > 0)
                {
                    FAm += "," + IntAm.Substring(0, 3);
                }
                else
                {
                    if (IntAm.Length == 3)
                    {
                        FAm = IntAm.Substring(0, 3);
                    }
                    else
                    {
                        FAm = IntAm;
                    }
                }
                if (PaisAm.Length > 0)
                {
                    if (PaisAm.Length == 1)
                    {
                        PaisAm = PaisAm + "0";
                    }
                    PaisAm = "." + PaisAm;
                }
                else
                {
                    PaisAm = ".00";
                }
                return FAm + PaisAm;
            }
            catch (Exception ex)
            {
                return Amount.ToString();
            }
        }

        public void Make_Indian_Rupee_Format(ref DataGridView DGV, ref System.Data.DataTable Dt)
        {
            Double Amount = 0;
            try
            {
                for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= DGV.Columns.Count - 1; j++)
                    {
                        if (Dt.Columns[j].ColumnName.ToUpper().Contains("RECEIPT") || Dt.Columns[j].ColumnName.ToUpper().Contains("PAYMENT") || Dt.Columns[j].ColumnName.ToUpper().Contains("CREDIT") || Dt.Columns[j].ColumnName.ToUpper().Contains("CLBAL") || Dt.Columns[j].ColumnName.ToUpper().Contains("DEBIT") || Dt.Columns[j].ColumnName.ToUpper().Contains("AMOUNT") || Dt.Columns[j].ColumnName.ToUpper().Contains("TO_PAY") || Dt.Columns[j].ColumnName.ToUpper().Contains("CHARGE") || Dt.Columns[j].ColumnName.ToUpper().Contains("PRICE") || Dt.Columns[j].ColumnName.ToUpper().Contains("S_DISC") || Dt.Columns[j].ColumnName.ToUpper().Contains("DISC") || Dt.Columns[j].ColumnName.ToUpper().Contains("TAX") || Dt.Columns[j].ColumnName.ToUpper().Contains("CASH") || Dt.Columns[j].ColumnName.ToUpper().Contains("CARD") || Dt.Columns[j].ColumnName.ToUpper().Contains("RATE") || Dt.Columns[j].ColumnName.ToUpper().Contains("_PER") || Dt.Columns[j].ColumnName.ToUpper().Contains("VALUE") || Dt.Columns[j].ColumnName.ToUpper().Contains("R_OFF") || Dt.Columns[j].ColumnName.ToUpper().Contains("OLD_QTY") || Dt.Columns[j].ColumnName.ToUpper().Contains("NEW_QTY") || Dt.Columns[j].ColumnName.ToUpper().Contains("MTR") || Dt.Columns[j].ColumnName.ToUpper().Contains("SALESQTY %"))
                        {
                            if (Dt.Rows[i][j] != DBNull.Value)
                            {
                                Amount = Convert.ToDouble(Dt.Rows[i][j]);
                                if (Amount >= 100000000)
                                {
                                    DGV[j, i].Style.Format = "##,##,##,000.00";
                                }
                                else if (Amount >= 10000000)
                                {
                                    DGV[j, i].Style.Format = "#,##,##,000.00";
                                }
                                else if (Amount >= 1000000)
                                {
                                    DGV[j, i].Style.Format = "##,##,000.00";
                                }
                                else if (Amount >= 100000)
                                {
                                    DGV[j, i].Style.Format = "#,##,000.00";
                                }
                                else if (Amount >= 10000)
                                {
                                    DGV[j, i].Style.Format = "##,000.00";
                                }
                                else if (Amount >= 1000)
                                {
                                    DGV[j, i].Style.Format = "#,000.00";
                                }
                                else
                                {
                                    DGV[j, i].Style.Format = "000.00";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Designing(ref DataGridView DGV, ref System.Data.DataTable Dt, params String[] ToHideColumnsName)
        {
            try
            {
                for (int i = 0; i <= Dt.Columns.Count - 1; i++)
                {
                    if (Dt.Columns[i].ColumnName.ToUpper().Contains("RECEIPT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PAYMENT") || Dt.Columns[i].ColumnName.ToUpper().Contains("CREDIT") || Dt.Columns[i].ColumnName.ToUpper().Contains("OPBAL") || Dt.Columns[i].ColumnName.ToUpper().Contains("CLBAL") || Dt.Columns[i].ColumnName.ToUpper().Contains("DEBIT") || Dt.Columns[i].ColumnName.ToUpper().Contains("AMOUNT") || Dt.Columns[i].ColumnName.ToUpper().Contains("TO_PAY") || Dt.Columns[i].ColumnName.ToUpper().Contains("CHARGE") || Dt.Columns[i].ColumnName.ToUpper().Contains("PRICE") || Dt.Columns[i].ColumnName.ToUpper().Contains("S_DISC") || Dt.Columns[i].ColumnName.ToUpper().Contains("DISC") || Dt.Columns[i].ColumnName.ToUpper().Contains("TAX") || Dt.Columns[i].ColumnName.ToUpper().Contains("CASH") || Dt.Columns[i].ColumnName.ToUpper().Contains("CARD") || Dt.Columns[i].ColumnName.ToUpper().Contains("RATE") || Dt.Columns[i].ColumnName.ToUpper().Contains("_PER") || Dt.Columns[i].ColumnName.ToUpper().Contains("VALUE") || Dt.Columns[i].ColumnName.ToUpper().Contains("R_OFF") || Dt.Columns[i].ColumnName.ToUpper().Contains("OLD_QTY") || Dt.Columns[i].ColumnName.ToUpper().Contains("NEW_QTY") || Dt.Columns[i].ColumnName.ToUpper().Contains("MTR") || Dt.Columns[i].ColumnName.ToUpper().Contains("SALESQTY %"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            //DGV.Columns[i].DefaultCellStyle.Format = "0.00";
                            //DGV.Columns[i].DefaultCellStyle.Format = "#,##0.00";
                            DGV.Columns[i].DefaultCellStyle.Format = "n";
                            DGV.Columns[i].Width = 100;
                        }
                    }
                    else if (Dt.Columns[i].ColumnName.ToUpper().Contains("BALANCE") || Dt.Columns[i].ColumnName.ToUpper().Contains("CURBAL") || Dt.Columns[i].ColumnName.ToUpper().Contains("OPBAL"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                            DGV.Columns[i].Width = 150;
                        }
                    }
                    else if (Dt.Columns[i].ColumnName.ToUpper().Contains("SLNO") || Dt.Columns[i].ColumnName.ToUpper().Contains("QMT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PCI") || Dt.Columns[i].ColumnName.ToUpper().Contains("NO_QTY"))
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Dt.Columns[i].ColumnName.ToUpper().Contains("QMT") || Dt.Columns[i].ColumnName.ToUpper().Contains("PCI") || Dt.Columns[i].ColumnName.ToUpper().Contains("NO_QTY"))
                            {
                                DGV.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                            }
                            DGV.Columns[i].Width = 60;
                        }
                    }
                }
                foreach (DataGridViewColumn Dc in DGV.Columns)
                {
                    DGV.Columns[Dc.Name].SortMode = DataGridViewColumnSortMode.NotSortable; 

                }
                foreach (String Sql in ToHideColumnsName)
                {
                    DGV.Columns[Sql].Visible = false;
                }
                //Make_Indian_Rupee_Format(ref DGV, ref Dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] BillUpdate_FromDataTable(ref System.Data.DataTable Dt, out String[] Update_Commands, String BillnoColumnName, Boolean Flag)
        {
            String Sql;
            try
            {
                Year();
                Update_Commands = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    Sql = "Update CashBill_Master set Bill_Paid = '" + Flag + "' where CashBill_Slno = '" + Dt.Rows[i][BillnoColumnName] + "' and year_Code = '" + YearCode + "'";
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] BillUpdate_FromDataTableCashier(ref System.Data.DataTable Dt, out String[] Update_Commands, String BillnoColumnName, String TypeColumn, Boolean Flag)
        {
            String Sql;
            try
            {
                Year();
                Update_Commands = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    if (Dt.Rows[i][TypeColumn].ToString() == "CB")
                    {
                        Sql = "Update CashBill_Master set Bill_Paid = '" + Flag + "' where CashBill_Slno = '" + Dt.Rows[i][BillnoColumnName] + "' and year_Code = '" + YearCode + "'";
                    }
                    else if (Dt.Rows[i][TypeColumn].ToString() == "SR")
                    {
                        Sql = "Update SaleSReturn_Master set Bill_Status = '" + Flag + "' where SR_Slno = '" + Dt.Rows[i][BillnoColumnName] + "' and year_Code = '" + YearCode + "'";
                    }
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public String[] StockUpdate_FromDataTable (ref System.Data.DataTable Dt, out String[] Update_Commands, String ItemCodeColumnName, String  ItemQtyColumnName, String LocationColumnName, StockUpdate Symbol)        
        {
            String Sql;
            try
            {
                Update_Commands = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    if (Symbol == StockUpdate.Add)
                    {
                        if (Get_RecordCount("Acc_Stock", "Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName]) > 0)
                        {
                            Sql = "Update Acc_Stock Set Qty = Qty + " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName];
                        }
                        else
                        {
                            Sql = "Insert into Acc_Stock values ('" + Dt.Rows[i][ItemCodeColumnName] + "'," + Dt.Rows[i][ItemQtyColumnName] + ", " + Dt.Rows[i][LocationColumnName] + ")";
                        }
                    }
                    else
                    {
                        Sql = "Update Acc_Stock Set Qty = Qty - " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName];
                    }
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DateTime CashBillDate(String Cashbill_Slno)
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            try
            {
                Cn.Open();
                OdbcDataAdapter adp = new OdbcDataAdapter ( new OdbcCommand ("Select cashbill_Date from cashbill_master where cashbill_Slno = " + Cashbill_Slno, Cn));
                adp.Fill (Dt);
                if (Dt.Rows.Count == 0 )
                {
                    return Convert.ToDateTime ("01/04/2008");
                }
                else
                {
                    return Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Dt.Rows[0]["cashbill_Date"]));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] ReturnQueries_FromDataTableSR(out String[] ReturnQueries, ref System.Data.DataTable Dt, String Before, String After, params String[] ColumnNamesInOrder)
        {
            String Str = String.Empty, CDtime = String.Empty;
            int Array_Size = 0;
            DateTime Dtime = DateTime.Now;
            try
            {
                Array_Size = Dt.Rows.Count;
                ReturnQueries = new String[Array_Size];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Str = Before;

                    foreach (String Sql in ColumnNamesInOrder)
                    {
                        if (Dt.Columns[Sql].DataType == System.Type.GetType("System.DateTime"))
                        {
                            Dtime = Convert.ToDateTime(Dt.Rows[i][Sql].ToString());
                            CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                            Str += "'" + CDtime + "',";
                        }
                        else
                        {
                            if (Sql == ColumnNamesInOrder[ColumnNamesInOrder.Length - 1])
                            {
                                if (After.Trim() != String.Empty)
                                {
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "'," + After;
                                    Dtime = Convert.ToDateTime(Dt.Rows[i]["Bill_date"].ToString());
                                    CDtime = String.Format("{0:dd/MMM/yyyy}", Dtime);
                                    Str += ",'" + CDtime + "')";
                                }
                                else
                                {
                                    Str += "'" + Dt.Rows[i][Sql].ToString() + "')";
                                }
                            }
                            else
                            {
                                Str += "'" + Dt.Rows[i][Sql].ToString() + "',";
                            }
                        }
                    }

                    ReturnQueries[i] = Str;
                }
                return ReturnQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] StockUpdate_FromDataTableSR (ref System.Data.DataTable Dt, out String[] Update_Commands, String DateColumnName, String ItemCodeColumnName, String ItemQtyColumnName, String LocationColumnName, StockUpdate Symbol)
        {
            String Sql;
            try
            {
                //for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                //{
                //    if (Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}",Dt.Rows[i][DateColumnName])) < Convert.ToDateTime ("30/03/2009"))
                //    {
                //        Dt.Rows.RemoveAt (i);
                //    }
                //}
                Update_Commands = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    if (Symbol == StockUpdate.Add)
                    {
                        if (Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Dt.Rows[i][DateColumnName])) < Convert.ToDateTime("30/03/2009"))
                        {
                            Sql = String.Empty;
                        }
                        else
                        {
                            if (Get_RecordCount("Acc_Stock", "Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName]) > 0)
                            {
                                Sql = "Update Acc_Stock Set Qty = Qty + " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and location_Code = " + Dt.Rows[i][LocationColumnName];
                            }
                            else
                            {
                                Sql = "Insert into Acc_Stock values ('" + Dt.Rows[i][ItemCodeColumnName] + "'," + Dt.Rows[i][ItemQtyColumnName] + ", " + Dt.Rows[i][LocationColumnName] + ")";
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(String.Format("{0:dd/MM/yyyy}", Dt.Rows[i][DateColumnName])) < Convert.ToDateTime("30/03/2009"))
                        {
                            Sql = String.Empty;
                        }
                        else
                        {
                            Sql = "Update Acc_Stock Set Qty = Qty - " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName];
                        }
                    }
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] StockUpdate_FromDataTableSRAccOld(ref System.Data.DataTable Dt, out String[] Update_Commands, String DateColumnName, String ItemCodeColumnName, String ItemQtyColumnName, String LocationColumnName, StockUpdate Symbol)
        {
            String Sql;
            try
            {
                Update_Commands = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    if (Symbol == StockUpdate.Add)
                    {
                        if (Get_RecordCount("Acc_Stock", "Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName]) > 0)
                        {
                            Sql = "Update Acc_Stock Set Qty = Qty + " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName];
                        }
                        else
                        {
                            Sql = "Insert into Acc_Stock values ('" + Dt.Rows[i][ItemCodeColumnName] + "'," + Dt.Rows[i][ItemQtyColumnName] + ", " + Dt.Rows[i][LocationColumnName] + ")";
                        }
                    }
                    else
                    {
                        Sql = "Update Acc_Stock Set Qty = Qty - " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "' and Location_Code = " + Dt.Rows[i][LocationColumnName];
                    }
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] StockUpdate_FromDataTableSRAcc(ref System.Data.DataTable Dt, out String[] Update_Commands, String DateColumnName, String ItemCodeColumnName, String ItemQtyColumnName, String LocationColumnName, StockUpdate Symbol)
        {
            String Sql;
            try
            {
                Update_Commands = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = String.Empty;
                    if (Symbol == StockUpdate.Add)
                    {
                        if (Get_RecordCount("Acc_Stock", "Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "'") > 0)
                        {
                            Sql = "Update Acc_Stock Set Qty = Qty + " + Dt.Rows[i][ItemQtyColumnName] + ", Location_Code = " + Dt.Rows[i][LocationColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "'";
                        }
                        else
                        {
                            Sql = "Insert into Acc_Stock values ('" + Dt.Rows[i][ItemCodeColumnName] + "'," + Dt.Rows[i][ItemQtyColumnName] + ", " + Dt.Rows[i][LocationColumnName] + ")";
                        }
                    }
                    else
                    {
                        Sql = "Update Acc_Stock Set Qty = Qty - " + Dt.Rows[i][ItemQtyColumnName] + " where Item_Code = '" + Dt.Rows[i][ItemCodeColumnName] + "'";
                    }
                    Update_Commands[i] = Sql;
                }
                return Update_Commands;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String[] UpdateAuthorizeSr(ref System.Data.DataTable Dt, out String[] UpdateQueries, String SrNoColumn, String SrDateColum, String EmpCode, String Today)
        {
            String Sql = String.Empty;
            try
            {
                UpdateQueries = new String[Dt.Rows.Count];
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    Sql = "Update Salesreturn_master set Authorized_Code = " + EmpCode + ", Done_At = " + Today + " where Sr_Slno  = " + Dt.Rows[i][SrNoColumn] + " and Sr_Date = '" + string.Format("{0:dd-MMM-yyyy}", Dt.Rows[i][SrDateColum]) + "'";
                    UpdateQueries[i] = Sql;
                }
                return UpdateQueries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public System.Data.DataTable Copy_DataTableWihtoutColumns(ref System.Data.DataTable Source, out System.Data.DataTable Dest, params String[] ColNames)
        {
            try
            {
                Dest = Source.Copy();
                foreach (String Col in ColNames)
                {
                    Dest.Columns.Remove(Col);
                }
                return Dest;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Colouring(ref DataGridView DGV, Grid_Design_Mode Mode)
        {
            int Col=0;
            try
            {
                if (Mode == Grid_Design_Mode.Row_Wise)
                {
                    for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                    {
                        if (i % 2 == 0)
                        {
                            DGV.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen; 
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= DGV.Columns.Count- 1; i++)
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Col % 2 == 0)
                            {
                                DGV.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            }
                            Col += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Colouring(ref DataGridView DGV, Grid_Design_Mode Mode, Boolean V_DataGrid)
        {
            int Col = 0;
            try
            {
                if (Mode == Grid_Design_Mode.Row_Wise)
                {
                    if (V_DataGrid == true)
                    {
                        for (int i = 0; i <= DGV.Rows.Count - 3; i++)
                        {
                            if (i % 2 == 0)
                            {
                                DGV.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                        {
                            if (i % 2 == 0)
                            {
                                DGV.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= DGV.Columns.Count - 1; i++)
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Col % 2 == 0)
                            {
                                DGV.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            }
                            Col += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Grid_Colouring(ref MyDataGridView DGV, Grid_Design_Mode Mode)
        {
            int Col = 0;
            try
            {
                if (Mode == Grid_Design_Mode.Row_Wise)
                {
                    for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                    {
                        if (i % 2 == 0)
                        {
                            DGV.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= DGV.Columns.Count - 1; i++)
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Col % 2 == 0)
                            {
                                DGV.Columns[i].DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
                            }
                            Col += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Grid_Colouring_With_Form_Color(Color Colour, ref MyDataGridView DGV, Grid_Design_Mode Mode)
        {
            int Col = 0;
            try
            {
                if (Mode == Grid_Design_Mode.Row_Wise)
                {
                    for (int i = 0; i <= DGV.Rows.Count - 1; i++)
                    {
                        if (i % 2 == 0)
                        {
                            DGV.Rows[i].DefaultCellStyle.BackColor = Colour;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i <= DGV.Columns.Count - 1; i++)
                    {
                        if (DGV.Columns[i].Visible == true)
                        {
                            if (Col % 2 == 0)
                            {
                                DGV.Columns[i].DefaultCellStyle.BackColor = Colour;
                            }
                            Col += 1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        DateTime GRNDate()
        {
            try
            {
                if (Check_Table("GRNDate") == false)
                {
                    Execute("Create table GRNDate (no number(2), Dat Date)");
                    Execute("insert into GRNDate values (1,'27-Mar-2009')");
                }
                return GetData_InDate("GRNDate", "no", "1", "dat");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void GRNDate_Update(DateTime Date)
        {
            try
            {
                if (Check_Table("GRNDate"))
                {
                    Execute("Delete from GRNDate");
                    Execute("insert into GRNDate values (1,'" + String.Format("{0:dd-MMM-yyyy}", Date) + "')");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_PurchaseMaster()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String[] OutQueries;
            try
            {
                //Load_Data_BackupCn("select Purchase_GRNNumber GRNNumber,Purchase_GRNDate GRNDate,purchase_CompanyCode CompanyCode,Purchase_AgentCode AgentCode,Purchase_TransportCode TransportCode,Supplier_Code,Purchase_Type,Purchase_InvoiceNumber InvoiceNumber,Purchase_InvoiceDate InvoiceDate,Purchase_DCNumber DCNumber,Purchase_DcDate DcDate,Purchase_DueDate DueDate,Purchase_DiscountPercentage Dp,Purchase_InterestPercentage IP,Purchase_GrossAmount GrossAmount,Purchase_FreightAmount FAmount,Purchase_TaxPercentage TP,Purchase_TaxAmount TaxAmount,Purchase_LessAmount  LessAmount,Purchase_DiscountAmount DM,Purchase_NettAmount NettAmount,Purchase_Bundles Bundles,Purchase_TaxType TaxType,Purchase_OtherAmount OtherAmount,Employee_Code,System_Code,Purchase_Narration Narration from vasthrapsrdb.dbo.purchase_master where purchase_grndate > '03-27-2009'", ref Dt);
                Load_Data_BackupCn("select Purchase_GRNNumber GRNNumber,Purchase_GRNDate GRNDate,purchase_CompanyCode CompanyCode,Purchase_AgentCode AgentCode,Purchase_TransportCode TransportCode,Supplier_Code,Purchase_Type,Purchase_InvoiceNumber InvoiceNumber,Purchase_InvoiceDate InvoiceDate,Purchase_DCNumber DCNumber,Purchase_DcDate DcDate,Purchase_DueDate DueDate,Purchase_DiscountPercentage Dp,Purchase_InterestPercentage IP,Purchase_GrossAmount GrossAmount,Purchase_FreightAmount FAmount,Purchase_TaxPercentage TP,Purchase_TaxAmount TaxAmount,Purchase_LessAmount  LessAmount,Purchase_DiscountAmount DM,Purchase_NettAmount NettAmount,Purchase_Bundles Bundles,Purchase_TaxType TaxType,Purchase_OtherAmount OtherAmount,Employee_Code,System_Code,Purchase_Narration Narration from vasthrapsrdb.dbo.purchase_master where purchase_grndate > '" + string.Format("{0:MM-dd-yyyy}",GRNDate()) + "'", ref Dt);
                if (Check_Table("TempPurchaseMaster") == false)
                {
                    Execute("Create table TempPurchaseMaster as Select * from oldpurchaseMaster");
                }
                if (BackupCn_Check_Table("Latest_GRN") == true)
                {
                    BackupCn_Execute_Statement("Drop table Latest_GRN");
                }
                BackupCn_Execute_Statement("Select Purchase_GRNNUmber into Latest_GRN from vasthrapsrdb.dbo.purchase_master where purchase_grndate > '" + string.Format("{0:MM-dd-yyyy}", GRNDate()) + "'");
                Execute("delete from TempPurchaseMaster where grndate>'" + string.Format("{0:dd-MMM-yyyy}", GRNDate()) + "'");
                ReturnQueries_FromDataTable(out OutQueries, ref Dt, "Insert into TempPurchaseMaster values (", String.Empty, "GRNNumber", "GRNDate", "CompanyCode", "AgentCode", "TransportCode", "Supplier_Code", "Purchase_Type", "InvoiceNumber", "InvoiceDate", "DCNumber", "DcDate", "DueDate", "Dp", "IP", "GrossAmount", "FAmount", "TP", "TaxAmount", "LessAmount", "DM", "NettAmount", "Bundles", "TaxType", "OtherAmount", "Employee_Code", "System_Code", "Narration");
                Run(OutQueries);
                if (Check_Table("OldTempPurchaseMaster"))
                {
                    Execute("Drop table OldTempPurchaseMaster");
                }

                Execute("Alter table OldpurchaseMaster rename to OldTempPurchaseMaster");
                Execute("Alter table TempPurchaseMaster rename to OldpurchaseMaster");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_PurchaseDetails()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String[] OutQueries;
            try
            {
                //Load_Data_BackupCn("select Purchase_GrnNumber GrnNumber,Purchase_SerialNumber SerialNumber,Purchase_GrnDate GrnDate,Purchase_ItemCode ItemCode,Purchase_ItemID ItemID,Purchase_BrandCode BrandCode,Purchase_DesignCode DesignCode,Purchase_SizeCode SizeCode,Purchase_ColorCode ColorCode,Purchase_Qty Qty,Purchase_ExcessQty ExcessQty,Purchase_ShortageQty ShortageQty,Purchase_price price,Purchase_SalePrice SalePrice,Purchase_SalesmanCommissionPercent SMCp,Purchase_SalesmanCommission SMC,Purchase_ProfitPercent pp,Purchase_SaleDiscountPercent sdp,Purchase_ItemwiseDiscountPercent IDp,Tax_Code,Purchase_WholeSalePrice WSP,Invoice_SerialNo ISN from vasthrapsrdb.dbo.purchase_items where purchase_GrnNumber in (Select * from Latest_GRN)", ref Dt);
                Load_Data_BackupCn("select Purchase_GrnNumber GrnNumber,Purchase_SerialNumber SerialNumber,Purchase_GrnDate GrnDate,Purchase_ItemCode ItemCode, i1.item_Slno ItemID,Purchase_BrandCode BrandCode,Purchase_DesignCode DesignCode,Purchase_SizeCode SizeCode,Purchase_ColorCode ColorCode,Purchase_Qty Qty,Purchase_ExcessQty ExcessQty,Purchase_ShortageQty ShortageQty,Purchase_price price,Purchase_SalePrice SalePrice,Purchase_SalesmanCommissionPercent SMCp,Purchase_SalesmanCommission SMC,Purchase_ProfitPercent pp,Purchase_SaleDiscountPercent sdp,Purchase_ItemwiseDiscountPercent IDp,Tax_Code,Purchase_WholeSalePrice WSP,Invoice_SerialNo ISN from vasthrapsrdb.dbo.purchase_items p1 left join vasthrapsrdb.dbo.Item_Master i1 on p1.purchase_itemID = i1.item_ID where purchase_GrnNumber in (Select * from Latest_GRN)", ref Dt);
                if (Check_Table("TempPurchaseitems") == false)
                {
                    Execute("Create table TempPurchaseitems as Select * from oldpurchaseitems");
                }
                Execute("delete from TempPurchaseitems where grndate>'" + string.Format("{0:dd-MMM-yyyy}", GRNDate()) + "'");
                ReturnQueries_FromDataTable(out OutQueries, ref Dt, "Insert into TempPurchaseitems values (", String.Empty, "GrnNumber", "SerialNumber", "GrnDate", "ItemCode", "ItemID", "BrandCode", "DesignCode", "SizeCode", "ColorCode", "Qty", "ExcessQty", "ShortageQty", "price", "SalePrice", "SMCp", "SMC", "pp", "sdp", "IDp", "Tax_Code", "WSP", "ISN");
                Run(OutQueries);
                if (Check_Table("OldTempPurchaseitems"))
                {
                    Execute("Drop table OldTempPurchaseitems");
                }
                Execute("Alter table OldpurchaseItems rename to OldTempPurchaseItems");
                Execute("Alter table TempPurchaseItems rename to OldpurchaseItems");
                GRNDate_Update(MaxDate("OldpurchaseMaster", "GRNDate", ""));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Load_ItemDiscountMaster()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String[] OutQueries;
            try
            {
                Load_Data_BackupCn("Select * from vasthraPSRDB.DBO.Item_Discount_Master", ref Dt);
                if (Check_Table("TempItemDiscountMaster") == false)
                {
                    Execute("Create table TempItemDiscountMaster as Select * from item_Discount_Master");
                }
                Execute("Truncate Table TempItemDiscountMaster");
                ReturnQueries_FromDataTable (out OutQueries, ref Dt,"Insert into tempItemDiscountMaster values (",String.Empty,"ItemCode","Discount_Code","Discount_Percentage","Employee_Code","System_Code");
                Run(OutQueries);
                if (Check_Table ("OldTempItemDiscountMaster"))
                {
                    Execute("Drop table OldTempItemDiscountMaster");
                }
                Execute("Alter table Item_Discount_Master rename to OldTempItemDiscountMaster");
                Execute("Alter table TempItemDiscountMaster rename to Item_Discount_Master");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Load_DiscountMaster()
        {
            System.Data.DataTable Dt = new System.Data.DataTable();
            String[] OutQueries;
            try
            {
                Load_Data_BackupCn("Select * from vasthraPSRDB.DBO.Discount_Master", ref Dt);
                if (Check_Table("TempDiscountMaster") == false)
                {
                    Execute("Create table TempDiscountMaster as Select * from Discount_Master");
                }
                Execute("Truncate Table TempDiscountMaster");
                ReturnQueries_FromDataTableDis(out OutQueries, ref Dt, "Insert into tempDiscountMaster values (", String.Empty, "Discount_Code", "Discount_Description", "Discount_PrintName", "Discount_From", "Discount_To", "Discount_Status", "Employee_Code", "System_Code");
                Run(OutQueries);
                if (Check_Table("OldTempDiscountMaster"))
                {
                    Execute("Drop table OldTempDiscountMaster");
                }
                Execute("Alter table Discount_Master rename to OldTempDiscountMaster");
                Execute("Alter table TempDiscountMaster rename to Discount_Master");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

#region oldStock
        public void STK_Queries(DateTime FromDate, DateTime ToDate)
        {
            String Str;
            try
            {
                Purchase_Value();

                // To Get Opening Stock
                Str = "select i1.item_description Item, Sum(g1.QMT) OpQty, 0 as OPMTR, (case when Sum(Amount) is null then 0 else  Sum(Amount) end) as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from gsn_acceptance_details g1 left join gsn_acceptance_master g3 on g1.gsn_acc_slno = g3.gsn_acc_slno left join item_master i1 on g1.item_id = i1.item_id where g3.Gsn_Acc_date < '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 OpQty, Sum(g1.QMT) as OPMTR, (case when Sum(Amount) is null then 0 else  Sum(Amount) end) as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from gsn_acceptance_details g1 left join gsn_acceptance_master g3 on g1.gsn_acc_slno = g3.gsn_acc_slno left join item_master i1 on g1.item_id = i1.item_id where g3.Gsn_Acc_date < '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  Sum(g2.QMT) as SalesQty, 0 as SalesMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from cash_purchase g2 left join Cashbill_master g3 on g2.Cashbill_slno = g3.Cashbill_slno and g2.Cashbill_date = g3.Cashbill_date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g2.sale_return <> 'Cancel' and g3.Cashbill_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, Sum(g2.QMT) as SalesMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from cash_purchase g2 left join Cashbill_master g3 on g2.Cashbill_slno = g3.Cashbill_slno and g2.Cashbill_date = g3.Cashbill_date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g2.sale_return <> 'Cancel' and g3.Cashbill_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, Sum(g2.QMT) As SRQty, 0 as SRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from SR_purchase g2 left join SalesReturn_master g3 on g2.SR_slno = g3.SR_slno and g2.SR_Date = g3.SR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.SR_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, Sum(g2.QMT) as SRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from SR_purchase g2 left join SalesReturn_master g3 on g2.SR_slno = g3.SR_slno and g2.SR_Date = g3.SR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.SR_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, Sum(g2.QMT) as DCQty, 0 as DCMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DC_purchase g2 left join DC_master g3 on g2.DC_slno = g3.DC_slno and g2.DC_Date = g3.DC_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DC_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, Sum(g2.QMT) as DCMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DC_purchase g2 left join DC_master g3 on g2.DC_slno = g3.DC_slno and g2.DC_Date = g3.DC_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DC_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, Sum(g2.QMT) as DCRQty, 0 as DCRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DCR_purchase g2 left join DCReturn_master g3 on g2.DCR_slno = g3.DCR_slno and g2.DCR_Date = g3.DCR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DCR_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, Sum(g2.QMT) as DCRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DCR_purchase g2 left join DCReturn_master g3 on g2.DCR_slno = g3.DCR_slno and g2.DCR_Date = g3.DCR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DCR_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, Sum(g2.QMT) as JWQty, 0 as JWMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JO_purchase g2 left join JOborder_master g3 on g2.JO_slno = g3.JO_slno and g2.JO_Date = g3.JO_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JO_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, Sum(g2.QMT) as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JO_purchase g2 left join Joborder_master g3 on g2.JO_slno = g3.JO_slno and g2.jo_Date = g3.JO_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JO_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, Sum(g2.QMT) as JWRQty, 0 as JWRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JOR_purchase g2 left join JO_Return_master g3 on g2.JOR_slno = g3.JOR_slno and g2.JOR_Date = g3.JOR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JOR_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, Sum(g2.QMT) as JWRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JOR_purchase g2 left join JO_Return_master g3 on g2.JOR_slno = g3.JOR_slno and g2.JOR_Date = g3.JOR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JOR_date <= '" + string.Format("{0:dd/MMM/yyyy}", FromDate.AddDays(-1)) + "' and item_description Like 'DRESS%' group by i1.item_description";


                Execute_Qry(Str, "ViewStockLedgerDet");

                if (Check_Table("ViewStockLedgerDet1"))
                {
                    Execute("Drop table ViewStockledgerDet1");
                }
                Execute("Create table ViewStockledgerDet1 as select * from ViewStockledgerDet");

                Str = "Select Item, Sum(OpQty) as OpQty,Sum(OpMTR) as OpMTR, Sum(op_Value) as OP_Value, Sum(SalesQty) as SalesQty, Sum(SalesMTR) as SalesMTR, Sum(S_Value) as S_Value, Sum(SRQty) as SRQty,Sum(SRMTR) as SRMTR, Sum(SR_Value) as SR_value, Sum(DCQty) as DCQty,Sum(DCMTR) as DCMTR, SUM(DC_Value) as DC_Value, Sum(DCRQty) as DCRQty, Sum(DCRMTR) as DCRMTR, SUM(DCR_Value) as DCR_Value, Sum(JWQty) as JWQty,Sum(JWMTR) as JWMTR, SUM(JW_Value) as JW_Value, SUM(JwrQty) as JwrQty,SUM(JwrMTR) as JwrMTR, Sum(JWR_Value) as JWR_Value, Sum(GSNQty) as GSNQty, Sum(GSNMTR) as GSNMTR, SUM(GSN_Value) as GSN_Value, SUM(GSNRQty) as GSNRQty,SUM(GSNRMTR) as GSNRMTR, SUM(GSNR_Value) as GSNR_Value, (Sum(OpQty) + Sum(SRQty) +Sum(DCRQty)+ Sum(GSNRQty) +SUM(JWRQty)) - (Sum(SalesQty) + Sum(DCQty) + SUM(JWQty) + SUM(GSNQty)) as ClosingQty, (Sum(OpMTR) + Sum(SRMTR) +Sum(DCRMTR)+ Sum(GSNRMTR) +SUM(JWRMTR)) - (Sum(SalesMTR) + Sum(DCMTR) + SUM(JWMTR) + SUM(GSNMTR)) as ClosingMTR, (Sum(op_Value) + Sum(SR_Value) +Sum(DCR_Value)+ Sum(GSNR_Value) +SUM(JWR_Value)) - (Sum(S_Value) + Sum(DC_Value) + SUM(JW_Value) + SUM(GSN_Value)) as Closing_Value from ViewStockLedgerDet1 group by item Order by Item";
                Execute_Qry(Str, "ViewStockLedgerClosing");

                if (Check_Table("ViewStockLedgerClosing1"))
                {
                    Execute("Drop table ViewStockLedgerClosing1");
                }
                Execute("Create table ViewStockLedgerClosing1 as select * from ViewStockLedgerClosing");


                // Current Stock
                Str = "select Item, g1.ClosingQty as OpQty, g1.ClosingMTR as OPMTR, Closing_Value as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from ViewStockLedgerClosing1 g1 left join item_master i1 on g1.item = i1.item_Description union ";

                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, Sum(g1.QMT) as InwQty, 0 as INWMTR, (case when Sum(Amount) is null then 0 else  Sum(Amount) end) as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from gsn_acceptance_details g1 left join gsn_acceptance_master g3 on g1.gsn_acc_slno = g3.gsn_acc_slno left join item_master i1 on g1.item_id = i1.item_id where g3.Gsn_Acc_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, Sum(g1.QMT) as INWMTR, (case when Sum(Amount) is null then 0 else  Sum(Amount) end) as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from gsn_acceptance_details g1 left join gsn_acceptance_master g3 on g1.gsn_acc_slno = g3.gsn_acc_slno left join item_master i1 on g1.item_id = i1.item_id where g3.Gsn_Acc_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, Sum(g2.QMT) as SalesQty, 0 as SalesMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from CASH_PURCHASE g2 left join Cashbill_master g3 on g2.Cashbill_slno = g3.Cashbill_slno and g2.Cashbill_Date = g3.Cashbill_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.Cashbill_date Between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and g2.sale_return <> 'Cancel' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, Sum(g2.QMT) as SalesMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from CASH_PURCHASE g2 left join Cashbill_master g3 on g2.Cashbill_slno = g3.Cashbill_slno and g2.Cashbill_Date = g3.Cashbill_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.Cashbill_date Between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and g2.sale_return <> 'Cancel' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, Sum(g2.QMT) As SRQty, 0 as SRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from SR_PURCHASE g2 left join SalesReturn_master g3 on g2.SR_slno = g3.SR_slno and g2.SR_Date = g3.SR_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.SR_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, Sum(g2.QMT) as SRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from SR_PURCHASE g2 left join SalesReturn_master g3 on g2.SR_slno = g3.SR_slno and g2.SR_Date = g3.SR_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.SR_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, Sum(g2.QMT) as DCQty, 0 as DCMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DC_PURCHASE g2 left join DC_master g3 on g2.DC_slno = g3.DC_slno and g2.DC_Date = g3.DC_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DC_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, Sum(g2.QMT) as DCMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DC_PURCHASE g2 left join DC_master g3 on g2.DC_slno = g3.DC_slno and g2.DC_Date = g3.DC_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DC_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, Sum(g2.QMT) as DCRQty, 0 as DCRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DCR_PURCHASE g2 left join DCReturn_master g3 on g2.DCR_slno = g3.DCR_slno and g2.DCR_Date = g3.DCR_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DCR_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0  as OpQty, 0 as OPMTR, 0 as OP_Value, 0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, Sum(g2.QMT) as DCRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from DCR_PURCHASE g2 left join DCReturn_master g3 on g2.DCR_slno = g3.DCR_slno and g2.DCR_Date = g3.DCR_Date inner join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.DCR_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, Sum(g2.QMT) as JWQty, 0 as JWMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JO_purchase g2 left join JOborder_master g3 on g2.JO_slno = g3.JO_slno and g2.JO_Date = g3.JO_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JO_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, Sum(g2.QMT) as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JW_Value, 0 as JWRQty, 0 as JWRMTR, 0 as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JO_purchase g2 left join Joborder_master g3 on g2.JO_slno = g3.JO_slno and g2.jo_Date = g3.JO_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JO_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Like 'DRESS%' group by i1.item_description union ";

                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, Sum(g2.QMT) as JWRQty, 0 as JWRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JOR_purchase g2 left join JO_Return_master g3 on g2.JOR_slno = g3.JOR_slno and g2.JOR_Date = g3.JOR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JOR_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Not Like 'DRESS%' group by i1.item_description union ";
                Str += "select i1.item_description Item, 0 as OpQty, 0 as OPMTR, 0 as OP_Value,  0 as InwQty, 0 as INWMTR, 0 as In_Value, 0 as SalesQty, 0 as SalesMTR, 0 as S_Value, 0 As SRQty, 0 as SRMTR, 0 as SR_Value, 0 as DCQty, 0 as DCMTR, 0 as DC_Value, 0 as DCRQty, 0 as DCRMTR, 0 as DCR_Value, 0 as JWQty, 0 as JWMTR, 0 as JW_Value, 0 as JWRQty, Sum(g2.QMT) as JWRMTR, (case when Sum(g2.P_PRice) is null then 0 else  Sum(g2.P_PRice) end) as JWR_Value, 0 as GSNQty, 0 as GSNMTR, 0 as GSN_Value, 0 as GSNRQty, 0 as GSNRMTR, 0 as GSNR_Value from JOR_purchase g2 left join JO_Return_master g3 on g2.JOR_slno = g3.JOR_slno and g2.JOR_Date = g3.JOR_Date left join GSNAcceptance1 g1 on g2.item_no = g1.item_no left join item_master i1 on g1.item_id = i1.item_id where g3.JOR_date between '" + string.Format("{0:dd/MMM/yyyy}", FromDate) + "' and '" + string.Format("{0:dd/MMM/yyyy}", ToDate) + "' and item_description Like 'DRESS%' group by i1.item_description";

                Execute_Qry(Str, "ViewStockLedger");

                if (Check_Table("ViewStockLedger1"))
                {
                    Execute("Drop table ViewStockLedger1");
                }
                Execute("Create table ViewStockLedger1 as select * from ViewStockLedger");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void GSN_View()
        {
            try
            {
                Execute_Qry("select distinct g1.GRN_No, g1.GRN_Date, g1.Supplier_Code, s1.Supplier_Name, g1. Item_No, g1.item_ID, g1.Amount, g1.P_PRice from GSN_Acceptance_Details g1 left join supplier_master s1 on g1.supplier_Code = s1.supplier_Code", "GSNAcceptance");
                if (Check_Table("GSNAcceptance1"))
                {
                    Execute("Drop table GSNAcceptance1");
                }
                Execute("Create table GSNAcceptance1 as select * from GSNAcceptance");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        void Purchase_Value()
        {
            try
            {
                GSN_View();
                Execute_Qry("Select distinct c1.cashbill_Slno, c1.cashbill_Date, C1.SALE_RETURN, c1.item_no, c1.Qmt, c1.Price, g1.Item_ID, g1.P_Price PU_PRICE, (C1.QMT * g1.P_price) as P_PRICE from cashbill_Details c1 left join gsn_Acceptance_details g1 on c1.item_no = g1.item_No", "Cash_PurchaseView");
                if (Check_Table("Cash_Purchase"))
                {
                    Execute("Drop table Cash_Purchase");
                }
                Execute("Create table Cash_Purchase as select * from Cash_PurchaseView");

                Execute_Qry("Select distinct c1.SR_Slno, c1.SR_Date, c1.item_no, c1.Qmt, c1.Price, g1.Item_ID, g1.P_Price PU_PRICE, (C1.QMT * g1.P_price) as P_PRICE from Salesreturn_Details c1 left join gsn_Acceptance_details g1 on c1.item_no = g1.item_No", "SR_PurchaseView");
                if (Check_Table("SR_Purchase"))
                {
                    Execute("Drop table SR_Purchase");
                }
                Execute("Create table SR_Purchase as select * from SR_PurchaseView");

                Execute_Qry("Select distinct c1.DC_Slno, c1.DC_Date, c1.item_no, c1.Qmt, c1.S_Price PRice, g1.Item_ID, g1.P_Price PU_PRICE, (C1.QMT * g1.P_price) as P_PRICE from DC_Details c1 left join gsn_Acceptance_details g1 on c1.item_no = g1.item_No", "DC_PurchaseView");
                if (Check_Table("DC_Purchase"))
                {
                    Execute("Drop table DC_Purchase");
                }
                Execute("Create table DC_Purchase as select * from DC_PurchaseView");

                Execute_Qry("Select distinct c1.DCR_Slno, c1.DCR_Date, c1.item_no, c1.Qmt, c1.S_Price PRice, g1.Item_ID, g1.P_Price PU_PRICE, (C1.QMT * g1.P_price) as P_PRICE from DCReturn_Details c1 left join gsn_Acceptance_details g1 on c1.item_no = g1.item_No", "DCR_PurchaseView");
                if (Check_Table("DCR_Purchase"))
                {
                    Execute("Drop table DCR_Purchase");
                }
                Execute("Create table DCR_Purchase as select * from DCR_PurchaseView");

                Execute_Qry("Select distinct c1.JO_Slno, c1.JO_Date, c1.item_no, c1.Qmt, c1.S_Price PRice, g1.Item_ID, g1.P_Price PU_PRICE, (C1.QMT * g1.P_price) as P_PRICE from JobOrder_Details c1 left join gsn_Acceptance_details g1 on c1.item_no = g1.item_No", "JO_PurchaseView");
                if (Check_Table("JO_Purchase"))
                {
                    Execute("Drop table JO_Purchase");
                }
                Execute("Create table JO_Purchase as select * from JO_PurchaseView");

                Execute_Qry("Select distinct c1.JOR_Slno, c1.JOR_Date, c1.item_no, c1.Qmt, c1.S_Price PRice, g1.Item_ID, g1.P_Price PU_PRICE, (C1.QMT * g1.P_price) as P_PRICE from JO_return_Details c1 left join gsn_Acceptance_details g1 on c1.item_no = g1.item_No", "JOR_PurchaseView");
                if (Check_Table("JOR_Purchase"))
                {
                    Execute("Drop table JOR_Purchase");
                }
                Execute("Create table JOR_Purchase as select * from JOR_PurchaseView");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
#endregion
    }
}
