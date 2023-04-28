using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmCRViewer : Form
    {
        Control_Modules MyBase = new Control_Modules();
        CrystalDecisions.Shared.TableLogOnInfo CnInfo = new CrystalDecisions.Shared.TableLogOnInfo();
        //CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
        CrystalDecisions.CrystalReports.Engine.SubreportObject SubRpt;
        CrystalDecisions.CrystalReports.Engine.ReportDocument SubRptDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

        public FrmCRViewer()
        {
            InitializeComponent();
        }

        private void FrmCRViewer_Load(object sender, EventArgs e)
        {
        }

        public void Print()
        {
            try
            {
                CRViewer.PrintReport();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadCR_Print(ref CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt)
        {
            try
            {

                MyBase.Initialize_Report_Details();
                CnInfo.ConnectionInfo.ServerName = MyBase.Server_Name;
                CnInfo.ConnectionInfo.DatabaseName = MyBase.DB_Name;
                CnInfo.ConnectionInfo.UserID = MyBase.UserName;
                CnInfo.ConnectionInfo.Password = MyBase.Pwd;

                for (int i = 0; i <= ObjRpt.Database.Tables.Count - 1; i++)
                {
                    ObjRpt.Database.Tables[i].ApplyLogOnInfo(CnInfo);
                }

                for (int i = 0; i <= ObjRpt.ReportDefinition.Sections.Count - 1; i++)
                {
                    for (int j = 0; j <= ObjRpt.ReportDefinition.Sections[i].ReportObjects.Count - 1; j++)
                    {
                        if (ObjRpt.ReportDefinition.Sections[i].ReportObjects[j].Kind == CrystalDecisions.Shared.ReportObjectKind.SubreportObject)
                        {
                            SubRpt = (CrystalDecisions.CrystalReports.Engine.SubreportObject)ObjRpt.ReportDefinition.Sections[i].ReportObjects[j];
                            SubRptDoc = SubRpt.OpenSubreport(SubRpt.SubreportName);
                            for (int i1 = 0; i1 <= SubRptDoc.Database.Tables.Count - 1; i1++)
                            {
                                SubRptDoc.Database.Tables[i1].ApplyLogOnInfo(CnInfo);
                            }
                        }
                    }
                }
                ObjRpt.VerifyDatabase();

                CRViewer.ReportSource = ObjRpt;
                CRViewer.Refresh();
                ObjRpt.PrintToPrinter(1, true, 1, 1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void LoadCR(ref CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt)
        {
            try
            {
                MyBase.Initialize_Report_Details();
                CnInfo.ConnectionInfo.ServerName = MyBase.Server_Name;
                CnInfo.ConnectionInfo.DatabaseName = MyBase.DB_Name;
                CnInfo.ConnectionInfo.UserID = MyBase.UserName;
                CnInfo.ConnectionInfo.Password = MyBase.Pwd;

                for (int i = 0; i <= ObjRpt.Database.Tables.Count - 1; i++)
                {
                    ObjRpt.Database.Tables[i].ApplyLogOnInfo(CnInfo);
                }

                for (int i = 0; i <= ObjRpt.ReportDefinition.Sections.Count - 1; i++)
                {
                    for (int j = 0; j <= ObjRpt.ReportDefinition.Sections[i].ReportObjects.Count - 1; j++)
                    {
                        if (ObjRpt.ReportDefinition.Sections[i].ReportObjects[j].Kind == CrystalDecisions.Shared.ReportObjectKind.SubreportObject)
                        {
                            SubRpt = (CrystalDecisions.CrystalReports.Engine.SubreportObject)ObjRpt.ReportDefinition.Sections[i].ReportObjects[j];
                            SubRptDoc = SubRpt.OpenSubreport(SubRpt.SubreportName);
                            for (int i1 = 0; i1 <= SubRptDoc.Database.Tables.Count - 1; i1++)
                            {
                                SubRptDoc.Database.Tables[i1].ApplyLogOnInfo(CnInfo);
                            }
                        }
                    }
                }
                ObjRpt.VerifyDatabase();
                CRViewer.ReportSource = ObjRpt;
                CRViewer.Refresh();
                CRViewer.Show();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void View_PDF(ref CrystalDecisions.CrystalReports.Engine.ReportDocument ObjRpt, String FileName, Boolean Message_Flag)
        {
            try
            {
                MyBase.Initialize_Report_Details();
                CnInfo.ConnectionInfo.ServerName = MyBase.Server_Name;
                CnInfo.ConnectionInfo.DatabaseName = MyBase.DB_Name;
                CnInfo.ConnectionInfo.UserID = MyBase.UserName;
                CnInfo.ConnectionInfo.Password = MyBase.Pwd;

                for (int i = 0; i <= ObjRpt.Database.Tables.Count - 1; i++)
                {
                    ObjRpt.Database.Tables[i].ApplyLogOnInfo(CnInfo);
                }

                for (int i = 0; i <= ObjRpt.ReportDefinition.Sections.Count - 1; i++)
                {
                    for (int j = 0; j <= ObjRpt.ReportDefinition.Sections[i].ReportObjects.Count - 1; j++)
                    {
                        if (ObjRpt.ReportDefinition.Sections[i].ReportObjects[j].Kind == CrystalDecisions.Shared.ReportObjectKind.SubreportObject)
                        {
                            SubRpt = (CrystalDecisions.CrystalReports.Engine.SubreportObject)ObjRpt.ReportDefinition.Sections[i].ReportObjects[j];
                            SubRptDoc = SubRpt.OpenSubreport(SubRpt.SubreportName);
                            for (int i1 = 0; i1 <= SubRptDoc.Database.Tables.Count - 1; i1++)
                            {
                                SubRptDoc.Database.Tables[i1].ApplyLogOnInfo(CnInfo);
                            }
                        }
                    }
                }

                ObjRpt.VerifyDatabase();
                CRViewer.ReportSource = ObjRpt;
                CRViewer.Refresh();
                CrystalDecisions.CrystalReports.Engine.ReportDocument Rd = (CrystalDecisions.CrystalReports.Engine.ReportDocument)ObjRpt;
                Rd.ExportToDisk(ExportFormatType.PortableDocFormat, FileName);
                if (Message_Flag)
                {
                    MessageBox.Show("Exported ...!", "Vaahini");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public void View(ref Object ObjRpt)
        {
            try
            {
                CRViewer.ReportSource = ObjRpt;
                CRViewer.Refresh();
                CRViewer.Show();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}