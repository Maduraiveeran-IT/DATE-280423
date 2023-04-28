using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using System.IO;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmGreyStoreBarcodePrint : Form
    {
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        Int64 Code = 0;
        DataRow Dr;
        MDIMain MyParent;
        TextBox Txt = null;
        String Buffer_Table = String.Empty;
        String Str;

        Int16 M = 0;
        DataTable Tdt1 = new DataTable();

        Int64 EtyNo = 0;

        public FrmGreyStoreBarcodePrint()
        {
            InitializeComponent();
        }

        private void GBMain_Enter(object sender, EventArgs e)
        {

        }

        private void FrmGreyStoreBarcodePrint_Load(object sender, EventArgs e)
        {
            try
            {
                MyParent = (MDIMain)this.MdiParent;
                MyBase.Clear(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGreyStoreBarcodePrint_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl is TextBox)
                {
                    if (this.ActiveControl.Name == String.Empty)
                    {
                    }
                    else if (this.ActiveControl.Name == "TxtCount")
                    {
                        MyBase.Valid_Number(Txt, e);
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmGreyStoreBarcodePrint_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "BtnPrint")
                    {
                        if (TxtCount.Text.ToString() == String.Empty || TxtCount.Text.ToString() == "0")
                        {
                            TxtCount.Focus();
                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Delete)
                {
                    e.Handled = true;
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (TxtCount.Text.ToString() == String.Empty || TxtCount.Text.ToString() == "0")
            {
                MessageBox.Show("Enter Number Of Barcode's...Gainup!");
                TxtCount.Focus();
                return;
            }
            else
            {
                Barcode_Process();
                return;
            }
        }

        void Barcode_Process()
        {
            try
            {
                Tdt1 = new DataTable();
                MyBase.Load_Data(" Select Isnull(MAX(Replace(BarcodeNo, 'S', '')), 0)Max_Bar from Socks_Grey_Store_Barcode_Master S1 Left Join Socks_Grey_Store_Barcode_Details S2 on S1.Rowid = S2.Master_ID", ref Tdt1);
                if (Tdt1.Rows.Count > 0)
                {
                    DataTable Tdt2 = new DataTable();
                    MyBase.Load_Data("Select Isnull(Max(EntryNo),0)EntryNo from Socks_Grey_Store_Barcode_Master ", ref Tdt2);
                    if (Tdt2.Rows.Count > 0)
                    {
                        EtyNo = Convert.ToInt64(Tdt2.Rows[0]["EntryNo"].ToString()) + 1;
                        String[] Queries;
                        Int32 Array_Index = 0;
                        Queries = new String[5];

                        Queries[Array_Index++] = " Insert Into Socks_Grey_Store_Barcode_Master (EntryNo, EntryDate)Values(" + EtyNo + ", GETDATE()); Select Scope_Identity()";
                        Queries[Array_Index++] = " Insert Into Socks_Grey_Store_Barcode_Details (Master_ID, BarcodeNo) Select @@IDENTITY Master_ID, ('S' ++ Right('0000000000' + Cast(No As Varchar(10)),10))BarcodeNo from Number_Series(" + Convert.ToInt64(Tdt1.Rows[0]["Max_Bar"].ToString()) + " + 1, " + Convert.ToInt64(Tdt1.Rows[0]["Max_Bar"].ToString()) + " + " + TxtCount.Text + ") ";

                        MyBase.Run_Identity(false, Queries);

                        Print_BarCode4();
                        MessageBox.Show("Ok ...!", "Gainup");
                        MyBase.Clear(this); 
                    }
                
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Print_BarCode4()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                Str = "Select S2.BarcodeNo, S2.BarcodeNo++G1.Processid Barcode, Process, SUBSTRING(process,1,2)Process_Short From Socks_Grey_Store_Barcode_Master S1 Left Join Socks_Grey_Store_Barcode_Details S2 on S1.Rowid = S2.Master_ID Left Join Grey_Bar_Process() G1 On 1 = 1 Where S1.EntryNo = " + EtyNo + " Order By BarcodeNo Asc, Processid Desc";
                
                MyBase.Load_Data(Str, ref Tdt);
                
                Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar4.txt");
                while (i <= Tdt.Rows.Count - 1)
                {
                    Sr.WriteLine("N");
                    Sr.WriteLine("ZT");
                    Sr.WriteLine("q814");
                    Sr.WriteLine("Q196, 24");
                    Sr.WriteLine("JF");
                    Sr.WriteLine("D9");
                    Sr.WriteLine("S4");
                    Sr.WriteLine("O");
                    Sr.WriteLine("A190,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BarcodeNo"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A200,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process_Short"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("B170,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A70,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("");

                    i += 1;
                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A380,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BarcodeNo"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A390,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process_Short"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B360,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A280,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        i += 1;
                    }

                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A570,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BarcodeNo"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A580,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process_Short"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B550,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A450,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        i += 1;
                    }

                    if (i <= Tdt.Rows.Count - 1)
                    {
                        Sr.WriteLine("A760,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["BarcodeNo"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A770,280,1,4,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process_Short"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("B740,50,1,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["Barcode"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("A640,50,1,3,1,1,N," + Convert.ToChar(34) + Tdt.Rows[i]["Process"].ToString() + Convert.ToChar(34));
                        Sr.WriteLine("");
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");

                        i += 1;
                    }
                    else
                    {
                        Sr.WriteLine("P1");
                        Sr.WriteLine("FE");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                        Sr.WriteLine("");
                    }
                }
                //MyBase.Run("update Socks_Barcode_Details set Print_Status = 'Y' Where Socks_Bundle_Details_Slno Between " + TxtFrmSlno.Text + " And " + TxtToSlno.Text + " "); 
                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Socks_Bar4.txt");
                Sr = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (Sr != null)
                {
                    Sr.Close();
                }
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                MyBase.Clear(this);
                TxtCount.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}