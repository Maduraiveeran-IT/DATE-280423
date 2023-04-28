using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using SelectionTool_NmSp;
using System.Reflection;
using System.Text;
using Accounts_ControlModules;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using Microsoft.Win32;
using System.Security;
using System.Security.Permissions;
using System.Security.AccessControl;

namespace Accounts
{
    public partial class FrmIeBrowser : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        int BrowserVer, RegVal, ieVersion;

        public FrmIeBrowser()
        {
            InitializeComponent();
        }

        private void FrmIeBrowser_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);

                   // WebBrowser Ie = new WebBrowser();
                    String appName = "/" + System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe";
                    String ModuleName = appName.Substring(appName.IndexOf("/") + 1, appName.IndexOf(".") - 1);
                    RegistryKey Regkey = null;

                    BrowserVer = webBrowser1.Version.Major;
                    if (BrowserVer >= 11)
                    {
                        RegVal = 11001;
                    }
                    else if (BrowserVer == 10)
                    {
                        RegVal = 10001;
                    }
                    else if (BrowserVer == 9)
                    {
                        RegVal = 9999;
                    }
                    else if (BrowserVer == 8)
                    {
                        RegVal = 8888;
                    }
                    else
                    {
                        RegVal = 7000;
                    }
                    Regkey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                    if (Regkey == null)
                    {
                        Regkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true);
                    }
                    Regkey.SetValue(System.Diagnostics.Process.GetCurrentProcess().ProcessName + ".exe", RegVal, RegistryValueKind.DWord);
                    Regkey.Close();
                    webBrowser1.Width = System.Windows.Forms.SystemInformation.VirtualScreen.Width;
                    webBrowser1.Height = System.Windows.Forms.SystemInformation.VirtualScreen.Height;
                    webBrowser1.Navigate("http://192.168.1.169/MIS_FILESERVER_HOME.aspx?var=1&UserCode=" + MyParent.Emplno + "&Module=" + ModuleName + "&IP=");
                    webBrowser1.ScriptErrorsSuppressed = true;


                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
