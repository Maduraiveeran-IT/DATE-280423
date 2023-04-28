using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace V_Components
{
    class MyTextBox : TextBox
    {
        protected override void OnGotFocus(EventArgs e)
        {
            this.BackColor = System.Drawing.Color.Yellow;
            base.OnGotFocus(e);
        }

        protected override void  OnLostFocus(EventArgs e)
        {
            this.BackColor = System.Drawing.Color.White;
 	        base.OnLostFocus(e);
        } 
    }
}
