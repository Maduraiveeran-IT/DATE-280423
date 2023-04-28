using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Accounts_ControlModules;
using SelectionTool_NmSp;
using Accounts;
using System.IO;
using System.Windows.Forms;

namespace Accounts
{
    public partial class FrmBarcodeDetails : Form, Entry
    {
        MDIMain MyParent;
        Control_Modules MyBase = new Control_Modules();
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        //TextBox Txt = null;
        TextBox Txt_Qty = null;
        TextBox Txt_Cont = null;
        Int64 Code = 0;
        DataTable[] DtQty;
        DataTable[] DtCont;
        String Str;
        Int16 Vis = 0;
        int Pos = 0;

        public FrmBarcodeDetails()
        {
            InitializeComponent();
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                DataTable Dth = new DataTable();
                DtQty = new DataTable[30];
                TxtGrnNO.Focus();
                TxtQC.Text = "PASSED";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            String[] Queries;
            Int32 Array_Index = 0;
            double Line_Flag = 0;
            try
            {
                
                if (TxtGrnNO.Text.Trim() == String.Empty || TxtSupplier.Text.Trim() == String.Empty ||  TxtCount.Text.Trim() == String.Empty || TxtBuyer.Text.Trim() == String.Empty || TxtColor.Text.Trim() == String.Empty || TxtLot.Text.ToString() == String.Empty ||  TxtQuantity.Text.Trim() == String.Empty || TxtRack.Text.Trim() == String.Empty || TxtPO.Text.Trim() == String.Empty || TxtQC.Text.Trim() == String.Empty )
                {
                    MessageBox.Show("Invalid Total ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtGrnNO.Focus();
                    return;
                }

                if (MyParent._New)
                {
                    TxtEntryNo.Text = MyBase.MaxOnlyWithoutComp("Socks_Yarn_Store_Barcode", "EntryNo", String.Empty, String.Empty, 0).ToString();
                }

                if (MyParent._New)
                {
                    MyBase.Execute("Insert into Socks_Yarn_Store_Barcode (EntryNo, EntryDate, Grn_No, Supplier, Count, Buyer, Color, LotNO, Quantity, RackNO, PONo, QCStatus, Material) values ('" + TxtEntryNo.Text + "', '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', '" + TxtGrnNO.Text + "', '" + TxtSupplier.Text + "', '" + TxtCount.Text + "', '" + TxtBuyer.Text + "', '" + TxtColor.Text + "', '" + TxtLot.Text + "', '" + TxtQuantity.Text + "', '" + TxtRack.Text +"', '" + TxtPO.Text +"', '" + TxtQC.Text +"', '" + TxtMaterial.Text + "')");
                }
                else
                {
                    MyBase.Run("update Socks_Yarn_Store_Barcode Set Grn_No = '" + TxtGrnNO.Text + "', Supplier = '" + TxtSupplier.Text + "', Count = '" + TxtCount.Text + "', Buyer = '" + TxtBuyer.Text + "', Color = '" + TxtColor.Text + "', LotNO = '" + TxtLot.Text + "', Quantity = '" + TxtQuantity.Text + "', RackNO = '" + TxtRack.Text + "', PONo = '" + TxtPO.Text + "', QCStatus = '" + TxtQC.Text + "', Material = '" + TxtMaterial.Text + "' Where EntryNo = " + TxtEntryNo.Text + " And EntryDate = '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "'");
                }
                MessageBox.Show("Saved ...!", "Gainup");
                MyBase.Clear(this);
                MyParent.Save_Error = false;

            }
            catch (Exception ex)
            {
                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void TxtRoll_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmBarcodeDetails_Load(object sender, EventArgs e)
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

        public void Entry_Print()
        {
            try
            {
                Print_BarCode();
                MessageBox.Show("Ok ...!", "Gainup");
                Entry_View();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Print_BarCode()
        {
            StreamWriter Sr = null;
            DataTable Tdt = new DataTable();
            Int32 i = 0;
            String Str = String.Empty;
            try
            {
                MyBase.Load_Data("Select Grn_No, Supplier, Count, Buyer, Color, LotNO, Quantity, RackNO, PONo, QCStatus, Material From Socks_Yarn_Store_Barcode Where RowID = " + Code + "", ref Tdt);
                Sr = new StreamWriter("C:\\vaahrep\\Socks_Bar_Temp.txt");

                for (i = 0; i <= Tdt.Rows.Count - 1; i++)
                {
                    Sr.WriteLine("N");
                    Sr.WriteLine("ZT");
                    Sr.WriteLine("q814");
                    Sr.WriteLine("Q196, 24");
                    Sr.WriteLine("JF");
                    Sr.WriteLine("D9");
                    Sr.WriteLine("S4");
                    Sr.WriteLine("O");
                    Sr.WriteLine("A110,14,0,4,1,1,N," + Convert.ToChar(34) + "Gainup Industries India Pvt Ltd - Socks" + Convert.ToChar(34));
                    Sr.WriteLine("A60,60,0,4,1,1,N," + Convert.ToChar(34) + "Supplier :" + Tdt.Rows[i]["supplier"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,90,0,4,1,1,N," + Convert.ToChar(34) + "Lot No   :" + Tdt.Rows[i]["LotNo"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,90,0,4,1,1,N," + Convert.ToChar(34) + "Color    :" + Tdt.Rows[i]["color"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,120,0,4,1,1,N," + Convert.ToChar(34) + "Count    :" + Tdt.Rows[i]["Count"].ToString().Replace("C", " ") + Convert.ToChar(34));
                    Sr.WriteLine("A450,120,0,4,1,1,N," + Convert.ToChar(34) + "Quantity :" + Tdt.Rows[i]["Quantity"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,150,0,4,1,1,N," + Convert.ToChar(34) + "Qc Status:" + Tdt.Rows[i]["QcStatus"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,150,0,4,1,1,N," + Convert.ToChar(34) + "Buyer    :" + Tdt.Rows[i]["buyer"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,180,0,4,1,1,N," + Convert.ToChar(34) + "PO No    :" + Tdt.Rows[i]["PONo"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,180,0,4,1,1,N," + Convert.ToChar(34) + "GRN No   :" + Tdt.Rows[i]["Grn_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A60,210,0,4,1,1,N," + Convert.ToChar(34) + "Material :" + Tdt.Rows[i]["Material"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("A450,210,0,4,1,1,N," + Convert.ToChar(34) + "Rack No  :" + Tdt.Rows[i]["RackNO"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("B200,270,0,1,2,4,61,B," + Convert.ToChar(34) + Tdt.Rows[i]["PONo"].ToString() + '-' + Tdt.Rows[i]["Grn_No"].ToString() + Convert.ToChar(34));
                    Sr.WriteLine("");
                    Sr.WriteLine("P1");
                    Sr.WriteLine("FE");
                    Sr.WriteLine("");
                    Sr.WriteLine("");
                    Sr.WriteLine("");
                }

                Sr.Close();
                MyBase.DosPrint("C:\\vaahrep\\Socks_Bar_Temp.txt");
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

        private void FrmBarcodeDetails_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (this.ActiveControl.Name == "TxtMaterial")
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
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtGrnNO")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Grn No..!", "Select Distinct Grn_No,Supplier,Supplierid From fitsocks.dbo.Grn_Details_For_LOT()", String.Empty, 200, 350);

                        if (Dr != null)
                        {
                            TxtGrnNO.Text = Dr["Grn_No"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSupplier")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Supplier from supplier Order by Supplier", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtSupplier.Text = Dr["Supplier"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtCount")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Count..!", "Select Size From Size Where Size Like 'C%'", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtCount.Text = Dr["Size"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer..!", "Select Buyer From Buyer Order By Buyer", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtBuyer.Text = Dr["Buyer"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtColor")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", "Select Color From Color Where Color is Not Null And LEN(Ltrim(Rtrim(Color))) > 0", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtColor.Text = Dr["Color"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtMaterial")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Material..!", "Select 'COMBED COTTON' Material Union Select 'NYLON' Union Select 'POLYESTER' Union Select 'RUBBER' Union Select 'MELANGE' Union Select 'SPANDEX' Union Select 'LYCRA'", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtMaterial.Text = Dr["Material"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtRack")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Rack..!", "Select Location from Socks_Yarn_Stores_Location_Master Order By Location", String.Empty, 300);

                        if (Dr != null)
                        {
                            TxtRack.Text = Dr["Location"].ToString();
                        }
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

        public void Entry_Edit()
        {
            try
            {
                MyBase.Enable_Controls(this, true);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "STORE BARCODE - Edit", "Select A.EntryNo, A.EntryDate, A.RowID, A.* from Socks_Yarn_Store_Barcode A order by A.RowID Desc", "", 250, 200);
                Fill_Datas(Dr);
                TxtGrnNO.Focus();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "STORE BARCODE - Delete", "Select A.EntryNo, A.EntryDate, A.RowID, A.* from Socks_Yarn_Store_Barcode A order by A.RowID Desc", "", 250, 200);
                Fill_Datas(Dr);
                if (Dr != null)
                {
                    MyParent.Load_DeleteConfirmEntry();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                String Sql;
                if (Code > 0)
                {
                    Sql = "Delete from Socks_Yarn_Store_Barcode Where RowID = " + Code;
                    MyBase.Execute(Sql);
                    MessageBox.Show("Deleted Successfully ...!");
                    MyBase.Clear(this);
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Please Select any Description details ...!");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Enable_Controls(this, false);
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 200, 100, SelectionTool_Class.ViewType.NormalView, "STORE BARCODE - View", "Select A.EntryNo, A.EntryDate, A.RowID, A.* from Socks_Yarn_Store_Barcode A order by A.RowID Desc", "", 250, 200);
                Fill_Datas(Dr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Fill_Datas(DataRow Dr)
        {
            try
            {
                if (Dr != null)
                {
                    Code = Convert.ToInt32(Dr["RowID"]);
                    TxtEntryNo.Text = Dr["EntryNo"].ToString();
                    DtpDate.Value = Convert.ToDateTime(Dr["EntryDate"]);
                    TxtGrnNO.Text = Dr["Grn_No"].ToString();
                    TxtSupplier.Text = Dr["Supplier"].ToString();
                    TxtCount.Text = Dr["Count"].ToString();
                    TxtBuyer.Text = Dr["Buyer"].ToString();
                    TxtColor.Text = Dr["Color"].ToString();
                    TxtLot.Text = Dr["LotNO"].ToString();
                    TxtQuantity.Text = Dr["Quantity"].ToString();
                    TxtRack.Text = Dr["RackNO"].ToString();
                    TxtPO.Text = Dr["PONo"].ToString();
                    TxtQC.Text = Dr["QCStatus"].ToString();
                    TxtMaterial.Text = Dr["Material"].ToString();  
                }
                else
                {
                    Code = 0;
                    MyBase.Clear(this);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void TxtLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) == Convert.ToInt16(Keys.Enter))
                {
                    e.Handled = true;
                }
                else
                {
                    MyBase.Return_Ucase(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtQuantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) == Convert.ToInt16(Keys.Enter))
                {
                    e.Handled = true;
                }
                else
                {
                    MyBase.Valid_Decimal(TxtQuantity, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtPO_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Convert.ToInt16(e.KeyChar) == Convert.ToInt16(Keys.Enter))
                {
                    e.Handled = true;
                }
                else
                {
                    MyBase.Return_Ucase(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtGrnNO_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtGrnNO.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Grn No..!", "Select Distinct Grn_No,Supplier,Supplierid From fitsocks.dbo.Grn_Details_For_LOT()", String.Empty, 200, 350);

                    if (Dr != null)
                    {
                        TxtGrnNO.Text = Dr["Grn_No"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtSupplier_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtSupplier.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Supplier..!", "Select Supplier from supplier Order by Supplier", String.Empty, 300);

                    if (Dr != null)
                    {
                        TxtSupplier.Text = Dr["Supplier"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtCount_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtCount.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Count..!", "Select Size From Size Where Size Like 'C%'", String.Empty, 300);

                    if (Dr != null)
                    {
                        TxtCount.Text = Dr["Size"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtBuyer_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtBuyer.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer..!", "Select Buyer From Buyer Order By Buyer", String.Empty, 300);

                    if (Dr != null)
                    {
                        TxtBuyer.Text = Dr["Buyer"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtColor_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtColor.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color..!", "Select Color From Color Where Color is Not Null And LEN(Ltrim(Rtrim(Color))) > 0", String.Empty, 300);

                    if (Dr != null)
                    {
                        TxtColor.Text = Dr["Color"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void TxtRack_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtRack.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Rack..!", "Select Location from Socks_Yarn_Stores_Location_Master Order By Location", String.Empty, 300);

                    if (Dr != null)
                    {
                        TxtRack.Text = Dr["Location"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtMaterial_Enter(object sender, EventArgs e)
        {
            try
            {
                if (TxtMaterial.Text.ToString() == String.Empty)
                {
                    Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Material..!", "Select 'COMBED COTTON' Material Union Select 'NYLON' Union Select 'POLYESTER' Union Select 'RUBBER' Union Select 'MELANGE' Union Select 'SPANDEX' Union Select 'LYCRA'", String.Empty, 300);

                    if (Dr != null)
                    {
                        TxtMaterial.Text = Dr["Material"].ToString();
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
