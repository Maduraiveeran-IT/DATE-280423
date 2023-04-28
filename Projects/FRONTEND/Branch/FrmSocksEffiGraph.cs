using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp; 
using Accounts; 
using System.Windows.Forms;
using Graph = System.Windows.Forms.DataVisualization.Charting;

namespace Accounts
{
    public partial class FrmSocksEffiGraph : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        Int32 Code;
        Graph.Chart chart;
        public FrmSocksEffiGraph()
        {
            InitializeComponent();
        }




        private void FrmSocksEffiGraph_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;               
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select EntryDAte, OEE, Limit, Date_NO from oee_graph_Tbl ", ref Tdt);
                if (Tdt.Rows.Count == 0)
                {
                    return;
                }
                //
                DataTable Tdt1 = new DataTable();
                MyBase.Load_Data("Select    (Case When Min(Limit) < Min(OEE) Then Min(Limit) Else Min(OEE) End) - 15 Mi, Max(OEE) + 15 MA  from oee_graph_Tbl  ", ref Tdt1);
                if (Tdt1.Rows.Count == 0)
                {
                    return;
                }
                chart = new Graph.Chart();
                //chart.BackColor = System.Drawing.Color.Blue; 
                chart.Location = new System.Drawing.Point(10, 10);
                chart.Size = new System.Drawing.Size(1000, 600);
                // Add a chartarea called "draw", add axes to it and color the area black
                chart.ChartAreas.Add("draw");
                chart.ChartAreas["draw"].BackColor = Color.White;
                chart.ChartAreas["draw"].BorderColor = Color.Red;
                chart.ChartAreas["draw"].AxisX.Minimum = 0;
                chart.ChartAreas["draw"].AxisX.Maximum = 31;
                chart.ChartAreas["draw"].AxisX.Interval = 1;
                chart.ChartAreas["draw"].AxisX.Title = "DATE";
                chart.ChartAreas["draw"].AxisY.Title = "EFFI";
                chart.ChartAreas["draw"].AxisX.TitleForeColor = Color.DarkRed;
                chart.ChartAreas["draw"].AxisY.TitleForeColor = Color.DarkRed;
                
                chart.ChartAreas["draw"].AxisX.MajorGrid.LineColor = Color.White;
                chart.ChartAreas["draw"].AxisX.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;
                chart.ChartAreas["draw"].AxisY.Minimum = Convert.ToDouble(Tdt1.Rows[0]["MI"].ToString());
                chart.ChartAreas["draw"].AxisY.Maximum = Convert.ToDouble(Tdt1.Rows[0]["MA"].ToString());
                chart.ChartAreas["draw"].AxisY.Interval = 2;
                chart.ChartAreas["draw"].AxisY.MajorGrid.LineColor = Color.White;
                //chart.ChartAreas["draw"].AxisY.MajorGrid.LineDashStyle = Graph.ChartDashStyle.Dash;
                //chart.ChartAreas["draw"].AxisY.IsLabelAutoFit = true;
               // chart.ChartAreas["draw"].BackColor = Color.Black;

                // Create a new function series
                chart.Series.Add("MyFunc");
                chart.Series.Add("MyFunc1");
                // Set the type to line      
                chart.Series["MyFunc"].ChartType = Graph.SeriesChartType.Line;
                chart.Series["MyFunc1"].ChartType = Graph.SeriesChartType.Line;
                // Color the line of the graph light green and give it a thickness of 3
                chart.Series["MyFunc"].Color = Color.LightGreen;
                chart.Series["MyFunc1"].Color = Color.LightBlue;
                chart.Series["MyFunc"].BorderWidth = 3;
                chart.Series["MyFunc1"].BorderWidth = 3;
                chart.Series["MyFunc"].IsValueShownAsLabel = true;
                chart.Series["MyFunc"].LabelForeColor = Color.Red;
                chart.Series["MyFunc1"].IsValueShownAsLabel = true;
                chart.Series["MyFunc1"].LabelForeColor = Color.Blue;
                for (int i = 0; i < Tdt.Rows.Count - 1; i++)
                {                                        
                    chart.Series["MyFunc"].Points.AddXY(Tdt.Rows[i]["Date_NO"].ToString(), Tdt.Rows[i]["OEE"].ToString());
                    chart.Series["MyFunc1"].Points.AddXY(Tdt.Rows[i]["Date_NO"].ToString(), Tdt.Rows[i]["LIMIT"].ToString());                    
                        
                }                
                chart.Series["MyFunc"].LegendText = "OEE";
                chart.Series["MyFunc1"].LegendText = "STANDARD";
                // Create a new legend called "MyLegend".
                chart.Legends.Add("MyLegend");
                chart.Legends["MyLegend"].BorderColor = Color.Tomato; // I like tomato juice!
                Controls.Add(this.chart);           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }




        void FrmSocksEffiGraph_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        if (MyParent._New == true || MyParent.Edit == true)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
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

        
    }
}