using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using System.Management;

//using System.Linq;
using System.Management.Instrumentation;
//using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace Accounts
{
    public partial class FrmCompLogin : Form
    {
        Int32 Company_UserCode = 0;
        Int32 Company_Emplno = 0;
        Control_Modules MyBase = new Control_Modules();
        DataTable Dt = new DataTable();
        public Int32 USerCode = 0;
        Boolean Sec_Flag = false;
        Boolean Th_Flag = false;

        public FrmCompLogin()
        {
            InitializeComponent();
        }

        public FrmCompLogin(Int32 User_Code, Int32 Emplno)
        {
            InitializeComponent();
            Company_UserCode = User_Code;
            Company_Emplno = Emplno;
        }


        private void FrmCompLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                Load_Company();

                ManagementObjectSearcher searcher;

                String SystemName = "";
                String DomainName = "";
                String Manufacturer = "";
                String Model = "";
                String Name = "";
                String PrimaryOwnerName = "";
                String SystemType = "";
                UInt64 TotalPhysicalMemory = 0;
                String UserName_Sys = "";
                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_ComputerSystem");
                foreach (ManagementObject mo in searcher.Get())
                {
                    SystemName = mo["DNSHostName"].ToString();
                    DomainName = mo["Domain"].ToString();
                    Manufacturer = mo["Manufacturer"].ToString();
                    Model = mo["Model"].ToString();
                    Name = mo["Name"].ToString();
                    //PrimaryOwnerName = mo["PrimaryOwnerName"].ToString();
                    SystemType = mo["SystemType"].ToString();
                    TotalPhysicalMemory = Convert.ToUInt64(mo["TotalPhysicalMemory"].ToString());
                    UserName_Sys = mo["UserName"].ToString();
                }

                String Disk_Model = "";
                UInt64 Disk_Size = 0;
                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive");
                foreach (ManagementObject mo in searcher.Get())
                {
                    Disk_Model = mo["Model"].ToString();
                    Disk_Size = Convert.ToUInt64(mo["Size"].ToString());
                }

                String InstallDate = "";
                String LastBootUpTime = "";
                String Os_Manufacturer = "";
                String Os_Name = "";
                String Os_Caption = "";
                String Os_Architecture = "";
                UInt64 TotalVirtualMemorySize = 0;
                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject mo in searcher.Get())
                {
                    InstallDate = mo["InstallDate"].ToString();
                    LastBootUpTime = mo["LastBootUpTime"].ToString();
                    Os_Manufacturer = mo["Manufacturer"].ToString();
                    Os_Name = mo["Name"].ToString();
                    Os_Caption = mo["Caption"].ToString();
                    Os_Architecture = mo["OSArchitecture"].ToString();
                    TotalVirtualMemorySize = Convert.ToUInt64(mo["TotalVirtualMemorySize"].ToString());
                }

                String Processor_Caption = "";
                String Processor_Description = "";
                String Processor_Manufacturer = "";
                String Processor_Name = "";
                searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Processor");
                foreach (ManagementObject mo in searcher.Get())
                {
                    Processor_Caption = mo["Caption"].ToString();
                    Processor_Description = mo["Description"].ToString();
                    Processor_Manufacturer = mo["Manufacturer"].ToString();
                    Processor_Name = mo["Name"].ToString();
                }

                String SystemIP = "";
                foreach (System.Net.IPAddress IP in System.Net.Dns.GetHostByName(System.Net.Dns.GetHostName()).AddressList)
                {
                    SystemIP = IP.ToString();
                }

                string[] ipAddresses = new string[10];
                string[] subnets = new string[10];
                string[] gateways = new string[10];
                string domains = "";
                string description = "";
                bool dhcp = false;
                string[] dnses = new string[10];

                subnets[0] = "";
                domains = "";
                description = "";
                dhcp = false;
                dnses[0] = "";
                dnses[1] = "";

                using (var mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                using (var instances = mc.GetInstances())
                {
                    foreach (ManagementObject instance in instances)
                    {
                        if (!(bool)instance["ipEnabled"])
                        {
                            continue;
                        }

                        Console.WriteLine("{0}, {1}, {2}", instance["Caption"], instance["ServiceName"], instance["MACAddress"]);

                        ipAddresses = (string[])instance["IPAddress"];
                        if (ipAddresses[0].ToString().Contains("172.16"))
                        {
                            subnets = (string[])instance["IPSubnet"];
                            gateways = (string[])instance["DefaultIPGateway"];
                            domains = (string)instance["DNSDomain"];
                            description = (string)instance["Description"];
                            dhcp = (bool)instance["DHCPEnabled"];
                            dnses = (string[])instance["DNSServerSearchOrder"];
                        }
                        //else
                        //{
                        //    subnets[0] = "";
                        //    domains = "";
                        //    description = "";
                        //    dhcp = false;
                        //    dnses[0] = "";
                        //    dnses[1] = "";
                        //}
                    }
                }


                string GetMacAddress1 = GetMacAddress();

                if (domains == null)
                {
                    domains = "";
                }

                DataTable Dt1 = new DataTable();
                String Str1 = "Select Top 1 SystemName From Fitsocks.Dbo.Gainup_System_Details Where SystemName = '" + SystemName + "' And Cast(EntryTime As Date) = Cast(Getdate() As Date) ";
                MyBase.Load_Data(Str1, ref Dt1);
                if (Dt1.Rows.Count == 0)
                {
                    //MyBase.Run("Insert Fitsocks.Dbo.Gainup_System_Details (SystemName, DomainName, Manufacturer, Model, Name, PrimaryOwnerName, SystemType, TotalPhysicalMemory, UserName_Sys, Disk_Model, Disk_Size, InstallDate, LastBootUpTime, Os_Manufacturer, Os_Name, Os_Caption, Os_Architecture, TotalVirtualMemorySize, Processor_Caption, Processor_Description, Processor_Manufacturer, Processor_Name, EntryTime, EntrySystem, SystemIP, MacAddress) Values ('" + SystemName + "', '" + DomainName + "', '" + Manufacturer + "', '" + Model + "', '" + Name + "', '" + PrimaryOwnerName + "', '" + SystemType + "', " + TotalPhysicalMemory + ", '" + UserName_Sys + "', '" + Disk_Model + "', " + Disk_Size + ", '" + InstallDate + "', '" + LastBootUpTime + "', '" + Os_Manufacturer + "', '" + Os_Name + "', '" + Os_Caption + "', '" + Os_Architecture + "', " + TotalVirtualMemorySize + ", '" + Processor_Caption + "', '" + Processor_Description + "', '" + Processor_Manufacturer + "', '" + Processor_Name + "', Getdate(), Host_Name(), '" + SystemIP + "', '" + GetMacAddress1 + "')");

                    MyBase.Run("Insert Fitsocks.Dbo.Gainup_System_Details (SystemName, DomainName, Manufacturer, Model, Name, PrimaryOwnerName, SystemType, TotalPhysicalMemory, UserName_Sys, Disk_Model, Disk_Size, InstallDate, LastBootUpTime, Os_Manufacturer, Os_Name, Os_Caption, Os_Architecture, TotalVirtualMemorySize, Processor_Caption, Processor_Description, Processor_Manufacturer, Processor_Name, EntryTime, EntrySystem, SystemIP, MacAddress, [Ip_Address], Subnets, Gateways, [Domains], [Description], [Dhcp], Preferred_Dns, Alternate_Dns, Module) Values ('" + SystemName + "', '" + DomainName + "', '" + Manufacturer + "', '" + Model + "', '" + Name + "', '" + PrimaryOwnerName + "', '" + SystemType + "', " + TotalPhysicalMemory + ", '" + UserName_Sys + "', '" + Disk_Model + "', " + Disk_Size + ", '" + InstallDate + "', '" + LastBootUpTime + "', '" + Os_Manufacturer + "', '" + Os_Name + "', '" + Os_Caption + "', '" + Os_Architecture + "', " + TotalVirtualMemorySize + ", '" + Processor_Caption + "', '" + Processor_Description + "', '" + Processor_Manufacturer + "', '" + Processor_Name + "', Getdate(), Host_Name(), '" + SystemIP + "', '" + GetMacAddress1 + "', '" + ipAddresses[0].ToString() + "', '" + subnets[0].ToString() + "', '" + gateways[0].ToString() + "', '" + domains.ToString() + "', '" + description.ToString() + "', '" + dhcp + "', '" + dnses[0].ToString() + "', '" + dnses[1].ToString() + "', 'Projects')");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }

        void Select_Company()
        {
            try
            {
                if (Dt.Rows.Count > 0)
                {
                    this.Hide();
                    MDIMain MainBase = new MDIMain();
                    MainBase.UserCode = Company_UserCode;
                    MainBase.Emplno = Company_Emplno;
                    //MainBase.UserName = MyBase.GetData_InString("Socks_User_Master", "USer_Code", Company_UserCode.ToString(), "User_Name");
                    MainBase.UserName = MyBase.GetData_InString("Projects.dbo.Projects_Login()", "USer_Code", Company_UserCode.ToString(), "User_Name");
                    MainBase.CompCode = Convert.ToInt32(Grid["Code", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompName = Grid["Company", Grid.CurrentCell.RowIndex].Value.ToString();
                    MainBase.Proj_Login_Code = Company_Emplno;
                    MainBase.Proj_Login_Name = MyBase.GetData_InString("Projects.dbo.Project_Login_Name", "Rowid", Company_Emplno.ToString(), "Name");
                    if (MainBase.CompName == ".")
                    {
                        MainBase.CompName = "DHANALAKSHMI SPINNING MILLS";
                    }
                    MainBase.CompPrintName = Grid["InPrinting", Grid.CurrentCell.RowIndex].Value.ToString();
                    if (Grid["InPrinting", Grid.CurrentCell.RowIndex].Value.ToString().ToUpper().Contains("DHANA") || Grid["InPrinting", Grid.CurrentCell.RowIndex].Value.ToString().ToUpper() == ".")
                    {
                        MainBase.YearCode = "2010-2011";
                    }
                    else
                    {
                        MainBase.YearCode = Grid["Year", Grid.CurrentCell.RowIndex].Value.ToString().Trim().Replace("*", String.Empty);
                    }
                    MainBase.SDate = Convert.ToDateTime(Grid["SDt", Grid.CurrentCell.RowIndex].Value);
                    MainBase.EDate = Convert.ToDateTime(Grid["EDt", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompPhone = Convert.ToString(Grid["Phone", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompFax = Convert.ToString(Grid["Fax", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompEmail = Convert.ToString(Grid["Mail", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompTin = Convert.ToString(Grid["Tin", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompCst = Convert.ToString(Grid["CST", Grid.CurrentCell.RowIndex].Value);
                    MainBase.CompAddress = MyBase.Company_Address(MainBase.CompCode);
                    MainBase.OnlyFor_Company = false;
                    MainBase.ShowDialog();
                    Application.Exit();
                    //Application.Run(MainBase);
                }
                else
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Grid_Color()
        {
            try
            {
                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    if (DateTime.Now >= Convert.ToDateTime(Dt.Rows[i]["Sdt"]) && DateTime.Now <= Convert.ToDateTime(Dt.Rows[i]["Edt"]))
                    {
                        //Grid.Rows[i].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                        Grid["Year", i].Value = Grid["Year", i].Value + " * ";
                    }
                }

                //if (Dt.Rows.Count > 0)
                //{
                //    Grid.Rows[0].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                //    Grid["Year", 0].Value = Grid["Year", 0].Value + " * ";
                //}
                //if (Dt.Rows.Count > 1)
                //{
                //    Grid.Rows[1].DefaultCellStyle.BackColor = System.Drawing.Color.Lime;
                //    Grid["Year", 1].Value = Grid["Year", 1].Value + " * ";
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Company()
        {
            try
            {
                if (MyBase.Get_RecordCount("projects.dbo.Projects_Companymas", "compname like '%AEGAN%'") > 0)
                {
                    if (USerCode == 1)
                    {
                        Grid.DataSource = MyBase.Load_Data("Select CompCode Code, CompName Company, cast (Datepart(Year,SDt) as varchar(4)) + '-' + Cast(Datepart(year,Edt) as varchar(4)) Year, Sdt, EDt, InPrinting, CompPhone Phone, CompFax Fax, COmpEmail Mail, CompTNGSTNo TIN, CompCstNo CST From projects.dbo.Projects_Companymas order by SDT Desc ", ref Dt);
                    }
                    else
                    {
                        Grid.DataSource = MyBase.Load_Data("Select CompCode Code, CompName Company, cast (Datepart(Year,SDt) as varchar(4)) + '-' + Cast(Datepart(year,Edt) as varchar(4)) Year, Sdt, EDt, InPrinting, CompPhone Phone, CompFax Fax, COmpEmail Mail, CompTNGSTNo TIN, CompCstNo CST From projects.dbo.Projects_Companymas where compCode = 2 order by SDT Desc ", ref Dt);
                    }
                }
                else if (MyBase.Get_RecordCount("projects.dbo.Projects_Companymas", "CompName like '%AVANEETHA%'") > 0)
                {
                    Grid.DataSource = MyBase.Load_Data("Select CompCode Code, CompName Company, cast (Datepart(Year,SDt) as varchar(4)) + '-' + Cast(Datepart(year,Edt) as varchar(4)) Year, Sdt, EDt, InPrinting, CompPhone Phone, CompFax Fax, COmpEmail Mail, CompTNGSTNo TIN, CompCstNo CST From projects.dbo.Projects_Companymas order by SDT Desc ", ref Dt);
                }
                else if (MyBase.Get_RecordCount("projects.dbo.Projects_Companymas", "CompName like '%GAINUP%'") > 0)
                {
                    Grid.DataSource = MyBase.Load_Data("Select CompCode Code, CompName Company, cast (Datepart(Year, SDt) as varchar(4)) + '-' + Cast(Datepart(year,Edt) as varchar(4)) Year, Sdt, EDt, InPrinting, CompPhone Phone, CompFax Fax, COmpEmail Mail, CompTNGSTNo TIN, CompCstNo CST From projects.dbo.Projects_Companymas order by SDT Desc ", ref Dt);
                }
                else
                {
                    Grid.DataSource = MyBase.Load_Data("Select CompCode Code, CompName Company, cast (Datepart(Year,SDt) as varchar(4)) + '-' + Cast(Datepart(year,Edt) as varchar(4)) Year, Sdt, EDt, InPrinting, CompPhone Phone, CompFax Fax, COmpEmail Mail, CompTNGSTNo TIN, CompCstNo CST From projects.dbo.Projects_Companymas order by CompCode, cast (Datepart(Year,SDt) as varchar(4)) + '-' + Cast(Datepart(year,Edt) as varchar(4)) ", ref Dt);
                }
                //MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Row_Wise);
                Grid_Color();
                Grid.RowHeadersWidth = 10;
                MyBase.Grid_Designing(ref Grid, ref Dt, "Sdt", "EDt", "InPrinting", "Phone", "Fax", "Mail", "TIN", "CST");
                MyBase.ReadOnly_Grid_Without(ref Grid);
                MyBase.Grid_Width(ref Grid, 80, 390, 130);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ButCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Application.Exit();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Load_Company_Master()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        private void ButOK_Click(object sender, EventArgs e)
        {
            try
            {
                Select_Company();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmCompLogin_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Select_Company();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ButCompany_Click(object sender, EventArgs e)
        {
            try
            {
                MDIMain MainBase = new MDIMain();
                MainBase.UserCode = USerCode;
                MainBase.UserName = MyBase.GetData_InString("PRojects_User_Master", "USer_Code", MyBase.UCode.ToString(), "User_Name");
                MainBase.CompCode = 99;
                MainBase.CompName = "Vaahini";
                MainBase.YearCode = "2009-2010";
                MainBase.OnlyFor_Company = true;
                this.Hide();
                //MainBase.SDate = Convert.ToDateTime(Grid["SDt", Grid.CurrentCell.RowIndex].Value);
                //MainBase.EDate = Convert.ToDateTime(Grid["EDt", Grid.CurrentCell.RowIndex].Value);
                MainBase.ShowDialog();
                Load_Company();
                Grid.Focus();
                this.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmCompLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Control.ModifierKeys == Keys.Control && e.KeyChar == Convert.ToChar(Keys.Space))
                {
                    Sec_Flag = true;
                }
                else if (Control.ModifierKeys == Keys.Control && e.KeyChar == Convert.ToChar(20) && Sec_Flag == true)
                {
                    Sec_Flag = false;
                    Th_Flag = true;
                }
                else if (Control.ModifierKeys == Keys.Control && e.KeyChar == Convert.ToChar(16) && Th_Flag == true)
                {
                    Th_Flag = false;
                    DataTable TDt = new DataTable();
                    MyBase.Load_Data("Select * from acc_Settings where inventory = 'True'", ref TDt);
                    if (TDt.Rows.Count > 0)
                    {
                        if (System.Environment.GetEnvironmentVariable("Company_Code") == "1")
                        {
                            Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Yellow;
                            Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
                            System.Environment.SetEnvironmentVariable("Company_Code", "50");
                            ButCompany.Visible = false;
                        }
                        else
                        {
                            Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                            Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                            System.Environment.SetEnvironmentVariable("Company_Code", "1");
                            ButCompany.Visible = true;
                        }
                    }
                    Load_Company();
                }
                else
                {
                    Sec_Flag = false;
                    Th_Flag = false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToUpper().Contains("SERVER DOES NOT EXIST"))
                {
                    Grid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Blue;
                    Grid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;
                    MessageBox.Show("Illeagal Server Settings ...!", "Vaahini", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    System.Environment.SetEnvironmentVariable("Company_Code", "1");
                    Load_Company();
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

    }
}
