using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Microsoft.Reporting.WinForms;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmReportViewer : Form
    {
        public FrmReportViewer()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            try
            {
                //reportViewer1.Left = this.Left + 15;
                //reportViewer1.Width = this.Width - 25;
                //reportViewer1.Height = this.Height - 50;
                //reportViewer1.Top = this.Top + 40;

                reportViewer1.Left = this.Left + 10;
                reportViewer1.Width = this.Width - 20;
                reportViewer1.Height = this.Height - 50;
                reportViewer1.Top = this.Top + 10;
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Connection(String Server, String DBName, String UName, String PWD)
        {
            try
            {
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void Load_Report(String ReportPath)
        {
            try
            {
                reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Remote;
                reportViewer1.ServerReport.ReportServerUrl = new Uri("Http://server/reportserver");
                reportViewer1.ServerReport.ReportPath = ReportPath;
                this.reportViewer1.RefreshReport();
                this.reportViewer1.ZoomPercent = 100;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}