using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmWeb_Browser : Form
    {
        public int Compcode;
        public int Emplno;
        public String Yearcode;
        public Int64 UserCode;
        public DateTime FDate;
        public DateTime TDate;
        int Level = 100;
        public String FormName = String.Empty;
        public String IP = "172.16.10.169/misexport/";

        public FrmWeb_Browser()
        {
            InitializeComponent();
            //webBrowser1.IsWebBrowserContextMenuEnabled = false;
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

      private String  Ascii_Reverse(String AsciiTerm)
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

        private void FrmWeb_Browser_Load(object sender, EventArgs e)
        {
            try
            {
                if (FormName == "SOCKS_BOM_BUYER")
                {
                    webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?IP=" + IP + "&UserCode=" + UserCode + "&Module=" + "VSocks" + "");
                }
                else if (FormName.ToUpper() == "MIS_SOCKS_SAMPLE_REQ_SHEET" || FormName.ToUpper() == "MIS_SOCKS_YARNDYEING_INVOICE" || FormName.ToUpper() == "MIS_SOCKS_YARNDYEING_GRN" || FormName.ToUpper() == "MIS_SOCKS_YARNDYEING_DC")
                {
                    webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?IP=" + IP + "&row_id=" + UserCode);
                }
                else if (FormName == "MIS_Attendance_Home")
                {
                    if (UserCode == 2)
                    {
                        UserCode = 4;
                    }
                    webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?UserCode=" + UserCode + "&Module=" + "VSocks" + "&SysCode=" + Ascii(System.Environment.MachineName) + "&IP=" + IP + "");
                }
                else if (FormName == "MIS_SOCKS_TIME")
                {
                    if (UserCode == 1 || UserCode == 2)
                    {
                        webBrowser1.Navigate("http://" + IP + "/MIS_GARMENTS_TIME_DIVISION.aspx?EMPLNO1=" + Ascii(Convert.ToString(Emplno)) + "&Module=" + "VSocks" + "&UserCode1=" + Ascii(Convert.ToString(UserCode)) + "");
                    }
                    else
                    {
                        webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?EMPLNO1=" + Ascii(Convert.ToString(Emplno)) + "&Module=" + "VSocks" + "&UserCode1=" + Ascii(Convert.ToString(UserCode)) + "");
                    }
                }
                else if (FormName == "MIS_SOCKS_ORDER_EXPORT_DETAILS")
                {
                    webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?IP=" + IP + "&UserCode=" + UserCode + "&Module=" + "VSocks" + "&Name=All&Mode=A");
                }
                else
                {
                    webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?IP=" + IP + "&UserCode=" + UserCode + "&Module=" + "VSocks" + "");
                    //webBrowser1.Navigate("http://" + IP + "/" + FormName + ".aspx?EMPLNO=" + Emplno + "&UserCode=" + UserCode + "&SysCode=" + Ascii(System.Environment.MachineName) + "&FDate=" + String.Format("{0:dd_MMM_yyyy}", FDate) + "&TDate=" + String.Format("{0:dd_MMM_yyyy}", TDate) + "&IP=" + IP + "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            this.webBrowser1.Document.Body.Style = "zoom: " + Convert.ToString(Level) + "%";
        }

        private void webBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.Add)
            {
                Level += 10;
            }
            else if (e.Control == true && e.KeyCode == Keys.Subtract)
            {
                Level -= 10;
            }
            else if (e.Control == true && e.KeyCode == Keys.Multiply)
            {
                Level = 100;
            }
            this.webBrowser1.Document.Body.Style = "zoom: " + Convert.ToString(Level) + "%";
        }

//        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
//        {
//            try
//            {
//                HtmlElement headElement = webBrowser1.Document.GetElementsByTagName("head")[0];
//                HtmlElement scriptElement = webBrowser1.Document.CreateElement("script");
//                mshtml.IHTMLScriptElement element = (mshtml.IHTMLScriptElement)scriptElement.DomElement;
//                element.type = @"text/javascript";

//                element.text = @" 
//                             document.onkeydown = function(e) { if ((e.keyCode === 65  && e.keycode === 17) || (e.keyCode === 67  && e.keycode === 17)) { return false; }};
//                                 
//                            function disableSelection()
//                            { 
//                                document.body.onselectstart=function(){ return false; }; 
//                                document.body.ondragstart=function() { return false; };
//                            }
//
//                               if (document.layers) {
//            
//            document.captureEvents(Event.MOUSEDOWN);
//         
//            
//            document.onmousedown = function () {
//                return false;
//            };
//        }
//        else {
//            
//            document.onmouseup = function (e) {
//                if (e != null && e.type == 'mouseup') {
//                     
//                    if (e.which == 2 || e.which == 3) {
//                         
//                        return false;
//                    }
//                }
//            };
//        }
//         
//         
//        document.oncontextmenu = function () {
//            return false;
//        };
//                                
//        ";

//                headElement.AppendChild(scriptElement);

//              // webBrowser1.Document.InvokeScript(@"disableSelection");

//            }
//            catch (Exception ex)
//            {
//                MessageBox.Show(ex.Message);
//            }
//        }
    }
}