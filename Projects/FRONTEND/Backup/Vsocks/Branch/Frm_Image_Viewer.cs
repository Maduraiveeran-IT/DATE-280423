using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Accounts_ControlModules;
using System.Text;
using System.Windows.Forms;

namespace Accounts
{
    public partial class Frm_Image_Viewer : Form
    {
        public Bitmap Bmp = null;

        public Frm_Image_Viewer()
        {
            InitializeComponent();
        }

        private void Frm_Image_Viewer_Load(object sender, EventArgs e)
        {
            try
            {
                if (Bmp != null)
                {
                    PIC_Image.Image = Bmp;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Btn_Ok_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }



    }
}