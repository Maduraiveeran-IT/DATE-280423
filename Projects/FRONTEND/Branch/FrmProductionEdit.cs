using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Accounts
{
    public partial class FrmProductionEdit : Form
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        Int32 M = 0;
        DataTable Dt = new DataTable();
        DataTable Dt1 = new DataTable();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataRow Dr;
        String S;
        String[] t;
        String[] Queries;
        TextBox Txt = null;
        TextBox Txt1 = null;
        Int64 Master_ID = 0;
        Int64 Detail_ID = 0;

        public FrmProductionEdit()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}