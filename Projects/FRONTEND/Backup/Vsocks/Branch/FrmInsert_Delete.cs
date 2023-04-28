using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmInsert_Delete : Form
    {
        public Int32 Value = 0;
        
        public FrmInsert_Delete()
        {
            InitializeComponent();
        }

        private void ButInsert_Click(object sender, EventArgs e)
        {
            Value = 1;
            this.Close();
        }

        private void ButDelete_Click(object sender, EventArgs e)
        {
            Value = 2;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Value = 3;
            this.Close();
        }
    }
}