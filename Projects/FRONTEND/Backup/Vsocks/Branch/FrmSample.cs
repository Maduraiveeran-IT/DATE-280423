using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.IO;
using System.Data.Odbc;
using System.Windows.Forms;
using Accounts_ControlModules;
using SelectionTool_NmSp;

namespace Accounts
{
    public partial class FrmSample : Form, Entry
    {
        Control_Modules MyBase = new Control_Modules();
        MDIMain MyParent;
        SelectionTool_Class Tool = new SelectionTool_Class();
        DataTable Dt = new DataTable();
        DataRow Dr;
        Int64 Code = 0;
        TextBox Txt = null;
        TextBox Txt_Instruction = null;
        TextBox Txt_Process = null;
        DataTable Dt_Instruction = new DataTable();
        DataTable Dt_Process = new DataTable();
        int Size_Change = 0;
        
        public FrmSample()
        {
            InitializeComponent();
        }

        private void FrmSample_Load(object sender, EventArgs e)
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

        void Grid_Data()
        {
            String Str = String.Empty;
            try
            {
                if (MyParent._New == true)
                {
                    if (ChkFrom.Checked == true)
                    {
                        //As Per Saravanan Sir Instruction Item, Color, Size Choose From Product Master
                        //Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On V1.ItemId = I1.itemid left join size S1 On V1.SizeID = S1.sizeid left join color C1 On V1.ColorID = C1.colorid where Master_ID = " + Code + " Order by Slno";
                        Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, Isnull(P2.Ply,'')Ply, V1.Ply_ID, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On P1.ItemId = I1.itemid left join size S1 On P1.SizeID = S1.sizeid left join color C1 On P1.ColorID = C1.colorid Left Join Socks_Yarn_Ply_Master P2 On V1.Ply_ID = P2.RowID Where Master_ID = " + Code + " Order by Slno";
                    }
                    else if(ChkSize.Checked == true)
                    {
                        if (Size_Change == 0)
                        {
                            Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, Isnull(P2.Ply,'')Ply, V1.Ply_ID, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On P1.ItemId = I1.itemid left join size S1 On P1.SizeID = S1.sizeid left join color C1 On P1.ColorID = C1.colorid Left Join Socks_Yarn_Ply_Master P2 On V1.Ply_ID = P2.RowID Where Master_ID = " + Code + " Order by Slno";
                        }
                        else if (Size_Change == 1)
                        {
                            Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.Item, V1.SizeID, S1.size, V1.ColorID, C1.color, Isnull(P2.Ply,'')Ply, V1.Ply_ID, V1.Before, (V1.Before - Cast((V1.Final / (V3.Sample_Qty * V3.Weight)) * (V3.Sample_Qty * " + TxtWeight.Text + ") as Numeric(20, 3))) After, Cast((V1.Final / (V3.Sample_Qty * V3.Weight)) * (V3.Sample_Qty * " + TxtWeight.Text + ") as Numeric(20, 3))Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On P1.ItemId = I1.itemid Left join size S1 On P1.SizeID = S1.sizeid Left join color C1 On P1.ColorID = C1.colorid Left Join VFit_Sample_Master V3 On V1.Master_Id = V3.RowID Left Join Socks_Yarn_Ply_Master P2 On V1.Ply_ID = P2.RowID Where V1.Master_ID = " + Code + " Order by Slno ";
                        }
                    }
                    else
                    {
                        Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, Isnull(P2.Ply,'')Ply, V1.Ply_ID, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On V1.ItemId = I1.itemid left join size S1 On V1.SizeID = S1.sizeid left join color C1 On V1.ColorID = C1.colorid Left Join Socks_Yarn_Ply_Master P2 On V1.Ply_ID = P2.RowID Where Master_ID = 1 and 1 = 2 Order by Slno";
                    }
                }
                //else if (MyParent._New)
                //{
                //    Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On V1.ItemId = I1.itemid left join size S1 On V1.SizeID = S1.sizeid left join color C1 On V1.ColorID = C1.colorid where Master_ID = 1 and 1 = 2 Order by Slno";
                //}
                else
                {
                    //As Per Saravanan Sir Instruction Item, Color, Size Choose From Product Master
                    //Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On V1.ItemId = I1.itemid left join size S1 On V1.SizeID = S1.sizeid left join color C1 On V1.ColorID = C1.colorid where Master_ID = " + Code + " Order by Slno";
                    Str = "Select V1.Slno, V1.FeederID, V2.Name Feeder, V1.Product_ID, P1.Product_No, V1.ItemId, I1.item, V1.SizeID, S1.size, V1.ColorID, C1.color, Isnull(P2.Ply,'')Ply, V1.Ply_ID, V1.Before, V1.After, V1.Final, V1.RM, '' T From VFit_Sample_Details V1 Left join VFit_Sample_Feeder_Master V2 On V1.FeederID = V2.RowID Left join VFit_Sample_Product_Master P1 On V1.Product_ID = P1.RowID Left Join Item I1 On P1.ItemId = I1.itemid left join size S1 On P1.SizeID = S1.sizeid left join color C1 On P1.ColorID = C1.colorid Left Join Socks_Yarn_Ply_Master P2 On V1.Ply_ID = P2.RowID Where Master_ID = " + Code + " Order by Slno";
                }
                Grid.DataSource = MyBase.Load_Data(Str, ref Dt);
                MyBase.Grid_Designing(ref Grid, ref Dt, "Product_ID", "FeederID", "ItemID", "SizeID", "ColorID", "Ply_ID", "T");
                if (ChkSize.Checked == false)
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Feeder", "Product_No", "Ply", "Before", "After");
                }
                else if (ChkSize.Checked == true)
                {
                    MyBase.ReadOnly_Grid_Without(ref Grid, "Product_No", "Ply");
                }
                MyBase.Grid_Colouring(ref Grid, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid, 40, 70, 90, 120, 120, 150, 100, 90, 90, 90, 90);


                Grid.Columns["Size"].HeaderText = "Count";

                Grid.Columns["Before"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Before"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["After"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["After"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["Final"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["Final"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.Columns["RM"].DefaultCellStyle.Format = "0.0000";
                Grid.Columns["RM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

                Grid.RowHeadersWidth = 10;


                if (MyParent._New)
                {
                    if (ChkFrom.Checked == true)
                    {
                        Str = "Select 0 Slno, V2.RowID Instruction_ID, V2.Name Instruction, V1.Value Details, '' T From VFit_Sample_Instruction_Details V1 Right Join VFit_Sample_Instruction_Master V2 On V1.Instruction_ID = V2.RowID Where V1.Master_ID = " + Code + "  Order by V2.RowID";
                    }
                    else
                    {
                        //Str = "Select 0 Slno, V2.RowID Instruction_ID, V2.Name Instruction, V1.Value Details, '' T From VFit_Sample_Instruction_Details V1 Right Join VFit_Sample_Instruction_Master V2 On V1.Instruction_ID = V2.RowID Order by V2.RowID";
                        Str = "Select 0 Slno, V1.RowID Instruction_ID, V1.Name Instruction,'' Details, '' T From VFit_Sample_Instruction_Master V1 Order by V1.RowID";
                    }
                }
                else
                {
                    Str = "Select V1.Order_Slno Slno, V1.Instruction_ID, V2.Name Instruction, V1.Value Details, '' T From VFit_Sample_Instruction_Details V1 Left Join VFit_Sample_Instruction_Master V2 On V1.Instruction_ID = V2.RowID Where V1.Master_ID = " + Code + " Order by V1.Order_Slno";
                }
                Grid_Instruction.DataSource = MyBase.Load_Data(Str, ref Dt_Instruction);

                MyBase.Grid_Designing(ref Grid_Instruction, ref Dt_Instruction, "Instruction_ID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid_Instruction, "Details");

                MyBase.Grid_Colouring(ref Grid_Instruction, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_Instruction, 60, 350, 150);

                if (MyParent._New)
                {
                    MyBase.Row_Number (ref Grid_Instruction);
                }

                Grid_Instruction.RowHeadersWidth = 10;

                if (MyParent._New)
                {
                    if (ChkFrom.Checked == true)
                    {
                        Str = "Select V1.Order_Slno Slno, V1.Process_ID, V2.Name Process, '' T From VFit_Sample_Process_Details V1 Left Join VFit_Sample_Process_Master V2 On V1.Process_ID = V2.RowID Where V1.Master_ID = " + Code + " Order by V1.Order_Slno";
                    }
                    else
                    {
                        Str = "Select 0 as Slno, RowID Process_ID, Name Process, '' T From VFit_Sample_Process_Master Order by RowID";
                    }
                }
                else
                {
                    Str = "Select V1.Order_Slno Slno, V1.Process_ID, V2.Name Process, '' T From VFit_Sample_Process_Details V1 Left Join VFit_Sample_Process_Master V2 On V1.Process_ID = V2.RowID Where V1.Master_ID = " + Code + " Order by V1.Order_Slno";
                }
                Grid_Process.DataSource = MyBase.Load_Data(Str, ref Dt_Process);

                MyBase.Grid_Designing(ref Grid_Process, ref Dt_Process, "Process_ID", "T");
                MyBase.ReadOnly_Grid_Without(ref Grid_Process, "Process");

                MyBase.Grid_Colouring(ref Grid_Process, Control_Modules.Grid_Design_Mode.Column_Wise);
                MyBase.Grid_Width(ref Grid_Process, 60, 350);

                if (MyParent._New)
                {
                    MyBase.Row_Number(ref Grid_Process);
                }

                Grid_Process.RowHeadersWidth = 10;

                if (TxtSampleNo.Text != String.Empty)
                {
                    DataTable TDt = new DataTable();
                    Str = "Select B.color from Fabric_Plan A left join color B on A.ColorID=B.colorid Where B.color='" + TxtSampleNo.Text + "' ";
                    MyBase.Load_Data(Str, ref TDt);

                    DataTable TDt1 = new DataTable();
                    Str = "Select RowID From Socks_Order_Details Where Sample_ID = " + Code;
                    MyBase.Load_Data(Str, ref TDt1);

                    if (MyParent.Edit == true || MyParent.Delete == true)
                    {
                        if (TDt.Rows.Count > 0 || TDt1.Rows.Count > 0)
                        {
                            MyBase.Enable_Controls(this, false);
                            MessageBox.Show("Already Order Done.You Can't Alter...!", "Gainup");
                            Entry_New();
                            return;
                        }
                    }
                    else
                    {
                        MyBase.Enable_Controls(this, true);
                        TxtSampleQty.Enabled = false;
                        TxtSampQtyUom.Enabled = false;
                        TxtWeightUOM.Enabled = false;
                    }
                }
                else
                {
                    MyBase.Enable_Controls(this, true);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Load_Image()
        {
            try
            {
                String Str = String.Empty;
                DataTable TmpDt = new DataTable();
                Str = "Select * From VFit_Sample_Photo where MAster_ID = " + Code;
                MyBase.Load_Data(Str, ref TmpDt);
                if (TmpDt.Rows.Count > 0)
                {
                    if (TmpDt.Rows[0]["Photo"] != DBNull.Value)
                    {
                        Byte[] Data = (Byte[])TmpDt.Rows[0]["Photo"];
                        Image Ephoto;
                        using (MemoryStream MS = new MemoryStream(Data, 0, Data.Length))
                        {
                            MS.Write(Data, 0, Data.Length);
                            Ephoto = Image.FromStream(MS, true);
                        }
                        PhImage.SizeMode = PictureBoxSizeMode.StretchImage;
                        PhImage.Image = Ephoto;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_New()
        {
            try
            {
                MyBase.Clear(this);
                Code = 0;
                Size_Change = 0;
                Grid_Data();
                GBImage.Visible = false;
                Fill_UOM();
                tabControl1.SelectTab(0);
                ChkFrom.Checked = false;
                ChkSize.Checked = false;
                TxtBuyer.Focus();
                Size_Change = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Save()
        {
            Boolean PhotoFlag = false;
            try
            {

                 Total();

                if (Convert.ToDouble(TxtWeight.Text) <= 0 || Convert.ToDouble(TxtAvgWeight.Text) <= 0 || Convert.ToDouble(TxtTotalWeight.Text) <= 0)
                {
                    MessageBox.Show("Invalid Details ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtWeightUOM.Focus();
                    return;
                }

                if (TxtBuyer.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Buyer ....!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtBuyer.Focus();
                    return;
                }

                //if (TxtBuyer.Text.Trim() == "DECATHLON" || Convert.ToInt64(TxtBuyer.Tag.ToString()) == 100)
                //{
                //    MessageBox.Show("Invalid Buyer ....!", "Gainup");
                //    MyParent.Save_Error = true;
                //    TxtBuyer.Focus();
                //    return;
                //}

                //if (Convert.ToInt64(TxtBuyer.Tag.ToString()) == 100)
                //{
                //    MessageBox.Show("Invalid Buyer ....!", "Gainup");
                //    MyParent.Save_Error = true;
                //    TxtBuyer.Focus();
                //    return;
                //}

                if (TxtModel.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Model ....!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtModel.Focus();
                    return;
                }

                if (TxtImanNo.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid IAMN NUMBER ....!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtImanNo.Focus();
                    return;
                }

                if (TxtItem.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Item Type....!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtItem.Focus();
                    return;
                }

                if (TxtNeedle.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Needle ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtNeedle.Focus();
                    return;
                }

                if (TxtLinking.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Linking ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtLinking.Focus();
                    return;
                }

                if (TxtMeasurement.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Measurement ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtMeasurement.Focus();
                    return;
                }
                if (TxtStyle.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid Style ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtStyle.Focus();
                    return;
                }
                if (TxtSize.Text.Trim() == String.Empty || TxtSize.Text.ToString().Contains("ZZZ"))
                {
                    MessageBox.Show("Invalid Size ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtSize.Focus();
                    return;
                }

                if (TxtRemarks.Text.Trim() == String.Empty || TxtRemarks.Text.ToString().Contains("ZZZ"))
                {
                    MessageBox.Show("Invalid Color ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtRemarks.Focus();
                    return;
                }

                if (TxtCycleUOM.Text.Trim() == String.Empty || TxtWeightUOM.Text.Trim() == String.Empty || TxtTimeUOM.Text.Trim() == String.Empty || TxtSampQtyUom.Text.Trim() == String.Empty)
                {
                    MessageBox.Show("Invalid UOM ...!", "Gainup");
                    MyParent.Save_Error = true;
                    TxtCycleUOM.Focus();
                    return;
                }

                if (Dt_Instruction.Rows.Count == 0)
                {
                    MessageBox.Show ("Invalid Instruction ...!", "Gainup");
                    MyParent.Save_Error = true;
                    tabControl1.SelectTab(1);
                    Grid_Instruction.CurrentCell = Grid_Instruction["Instruction", Grid_Instruction.CurrentCell.RowIndex];
                    Grid_Instruction.Focus();
                    Grid_Instruction.BeginEdit(true);
                    return;
                }

                if (PhImage.Image == null)
                {
                    if (MessageBox.Show("Invalid Image ..! Sure to Continue ?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        PhotoFlag = true;
                    }
                    else
                    {
                        PhotoFlag = false;
                        MyParent.Save_Error = true;
                        return;
                    }
                }

                if (Dt_Process.Rows.Count == 0)
                {
                    MessageBox.Show("Invalid Process ...!", "Gainup");
                    MyParent.Save_Error = true;
                    tabControl1.SelectTab(2);
                    Grid_Process.CurrentCell = Grid_Process["Process", Grid_Process.CurrentCell.RowIndex];
                    Grid_Process.Focus();
                    Grid_Process.BeginEdit(true);
                    return;
                }

                //if (Math.Round(Convert.ToDouble(TxtWeight.Text)) != Math.Round(Convert.ToDouble(TxtAvgWeight.Text)))
                //{
                //    MessageBox.Show("Invalid Weight ...!", "Gainup");
                //    MyParent.Save_Error = true;
                //    tabControl1.SelectTab(1);
                //    Grid_Process.CurrentCell = Grid_Process["Instruction", Grid_Process.CurrentCell.RowIndex];
                //    Grid_Process.Focus();
                //    Grid_Process.BeginEdit(true);
                //    return;
                //}

                if (Math.Round(Convert.ToDouble(TxtAvgWeight.Text), 3) != Math.Round((Convert.ToDouble(TxtWeight.Text) + ((Convert.ToDouble(TxtWeight.Text) * Convert.ToDouble(TxtWaste.Text)) / 100)), 3))
                {
                    MessageBox.Show("Invalid Weight...!", "Gainup");
                    MyParent.Save_Error = true;
                    tabControl1.SelectTab(1);
                    Grid_Process.CurrentCell = Grid_Process["Instruction", Grid_Process.CurrentCell.RowIndex];
                    Grid_Process.Focus();
                    Grid_Process.BeginEdit(true);
                    return;
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if (Grid[j, i].Value == DBNull.Value)
                        {
                            MessageBox.Show("' " + Grid.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt.Columns.Count - 1; j++)
                    {
                        if(Grid[j, i].Value.ToString().Contains("ZZZ"))
                        {
                            MessageBox.Show("' ZZZ In " + Grid.Columns[j].Name + " ' is Not Valid For Store " + (i + 1) + "  ", "Gainup");
                            Grid.CurrentCell = Grid[j, i];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                for (int i = 0; i <= Dt_Instruction.Rows.Count - 1; i++)
                {
                    for (int j = 1; j < Dt_Instruction.Columns.Count - 1; j++)
                    {
                        if (Grid_Instruction[j, i].Value == DBNull.Value || Grid_Instruction[j, i].Value.ToString().Trim() == String.Empty || Grid_Instruction[j, i].Value.ToString().Trim() == null)
                        {
                            MessageBox.Show("' " + Grid_Instruction.Columns[j].Name + " ' is Invalid  in Row " + (i + 1) + "  ", "Gainup");
                            Grid_Instruction.CurrentCell = Grid[j, i];
                            Grid_Instruction.Focus();
                            Grid_Instruction.BeginEdit(true);
                            MyParent.Save_Error = true;
                            return;
                        }
                    }
                }

                DataTable Tdt = new DataTable();
                if (MyParent._New == true||ChkFrom.Checked == true)
                {
                    MyBase.Load_Data("Select (Isnull(Max(Cast(Replace(Sample_no, 'HO/', '') as Bigint)), 0) + 1) From VFit_Sample_Master ", ref Tdt);
                    TxtSampleNo.Text = "HO/" + String.Format("{0:00000}", Convert.ToDouble(Tdt.Rows[0][0]));
                }
                
                String[] Queries = new String[Dt.Rows.Count + Dt_Instruction.Rows.Count + Dt_Process.Rows.Count + 100000];
                Int32 Array_Index = 0;


                MyBase.Cn_Open();
                MyBase.ODBCCmd = new OdbcCommand();
                MyBase.ODBCTrans = MyBase.Cn.BeginTransaction();


                MyBase.ODBCCmd.Connection = MyBase.Cn;
                MyBase.ODBCCmd.Transaction = MyBase.ODBCTrans;

                if (MyParent._New)
                {
                    MyBase.ODBCCmd.CommandText = "Insert Into VFit_Sample_Master (BuyerID, Sample_No, NeedleID, EDate, Linkingid, Cycle, UOm1, Time1, UOm2, Washing, Template, MeasurementID, Sample_Qty, uom3, Weight, UOM4, Remarks, UserCode, SysCode, Entryat, Total_weight, Average_Weight, Waste_Per, Styleid, Sizeid, ModelID, IMANNO, SampleItemID, Approval_Flag) Values (" + TxtBuyer.Tag.ToString() + ", '" + TxtSampleNo.Text.Trim() + "', " + TxtNeedle.Tag.ToString() + ", '" + String.Format("{0:dd-MMM-yyyy}", DtpDate.Value) + "', " + TxtLinking.Tag.ToString() + ", '" + String.Format("{0:hh:mm:ss}", DtpCycle.Value) + "', " + TxtCycleUOM.Tag.ToString() + ", '" + String.Format("{0:hh:mm:ss}", DtpTime.Value) + "', " + TxtTimeUOM.Tag.ToString() + ", '" + TxtWashing.Text.ToString() + "', '" + TxtTemplate.Text + "', " + TxtMeasurement.Tag.ToString() + ", " + TxtSampleQty.Text + ", " + TxtSampQtyUom.Tag.ToString() + ", " + TxtWeight.Text + ", " + TxtWeightUOM.Tag.ToString() + ", '" + TxtRemarks.Text.Trim() + "', " + MyParent.UserCode + ", " + MyParent.SysCode + ", '" + String.Format("{0:dd-MMM-yyyy} {0:T}", MyBase.GetServerDateTime()) + "', " + TxtTotalWeight.Text + ", " + TxtAvgWeight.Text + ", " + TxtWaste.Text + ", " + TxtStyle.Tag.ToString() + "," + TxtSize.Tag.ToString() + ", " + TxtModel.Tag.ToString() + ", '" + TxtImanNo.Text.ToString() + "', " + TxtItem.Tag.ToString() + ", (Case When " + TxtBuyer.Tag.ToString() + " in (100, 141) Then 'F' Else 'T' End)); Select Scope_Identity ()";
                    Code = Convert.ToInt64 (MyBase.ODBCCmd.ExecuteScalar());
                }
                else
                {
                    MyBase.ODBCCmd.CommandText = "Delete from VFit_Sample_Photo Where MAster_ID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCCmd.CommandText = "Delete from VFit_Sample_Details Where MAster_ID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCCmd.CommandText = "Delete from VFit_Sample_Instruction_Details Where Master_ID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCCmd.CommandText = "Delete from VFit_Sample_Process_Details Where Master_ID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();

                    MyBase.ODBCCmd.CommandText = "Update VFit_Sample_Master Set NeedleID = " + TxtNeedle.Tag.ToString() + ", styleid = " + TxtStyle.Tag.ToString() + ", sizeid = " + TxtSize.Tag.ToString() + ", LinkingID = " + TxtLinking.Tag.ToString() + ", Cycle = '" + String.Format("{0:hh:mm:ss}", DtpCycle.Value) + "', UOm1 = " + TxtCycleUOM.Tag.ToString() + ", Time1 = '" + String.Format("{0:hh:mm:ss}", DtpTime.Value) + "', UOM2 = " + TxtTimeUOM.Tag.ToString() + ", Washing = '" + TxtWashing.Text.ToString() + "' , Template = " + TxtTemplate.Text + ", MeasurementID = " + TxtMeasurement.Tag.ToString() + ", Sample_Qty = " + TxtSampleQty.Text + ", UOM3 = " + TxtSampQtyUom.Tag.ToString() + ", Weight = " + TxtWeight.Text + ", UOM4 = " + TxtWeightUOM.Tag.ToString() + ", Remarks = '" + TxtRemarks.Text.Trim() + "', UserCode = " + MyParent.UserCode + ", SysCode = " + MyParent.SysCode + ", Entryat = '" + String.Format("{0:dd-MMM-yyyy} {0:T}", MyBase.GetServerDateTime()) + "', Total_Weight = " + TxtTotalWeight.Text + ", Average_Weight = " + TxtAvgWeight.Text + ", Waste_Per = " + TxtWaste.Text + ", ModelID = " + TxtModel.Tag.ToString() + ", ImanNo = '" + TxtImanNo.Text.ToString() + "', SampleItemID = " + TxtItem.Tag.ToString() + ", Approval_Flag = (Case When " + TxtBuyer.Tag.ToString() + " in (100, 141) Then 'F' Else 'T' End) Where RowID = " + Code;
                    MyBase.ODBCCmd.ExecuteNonQuery();
                }

                for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                {
                    MyBase.ODBCCmd.CommandText = "Insert Into VFit_Sample_Details (Master_ID, Slno, FeederID, ItemID, SizeID, ColorID, Before, After, Final, RM, Product_ID, Ply_ID) Values (" + Code + ", " + Convert.ToInt32(i + 1) + ", " + Dt.Rows[i]["FeederID"].ToString() + ", " + Dt.Rows[i]["ItemID"].ToString() + ", " + Dt.Rows[i]["SizeID"].ToString() + ", " + Dt.Rows[i]["ColorID"].ToString() + ", " + Dt.Rows[i]["Before"].ToString() + ", " + Dt.Rows[i]["After"].ToString() + ", " + Dt.Rows[i]["Final"].ToString() + ", " + Dt.Rows[i]["RM"].ToString() + ", " + Dt.Rows[i]["Product_ID"].ToString() + ", " + Dt.Rows[i]["Ply_ID"].ToString() + ")";
                    MyBase.ODBCCmd.ExecuteNonQuery();
                }

                for (int i = 0; i <= Dt_Instruction.Rows.Count - 1; i++)
                {
                    MyBase.ODBCCmd.CommandText = "Insert into VFit_Sample_Instruction_Details (Master_ID, Order_Slno, Instruction_ID, Value) Values (" + Code + ", " + Convert.ToInt32(i + 1) + ", " + Dt_Instruction.Rows[i]["Instruction_ID"].ToString() + ", '" + Dt_Instruction.Rows[i]["Details"].ToString() + "')";
                    MyBase.ODBCCmd.ExecuteNonQuery();
                }

                for (int i = 0; i <= Dt_Process.Rows.Count - 1; i++)
                {
                    MyBase.ODBCCmd.CommandText = "Insert into VFit_Sample_Process_Details (Master_ID, Order_Slno, Process_ID) Values (" + Code + ", " + Convert.ToInt32(i + 1) + ", " + Dt_Process.Rows[i]["Process_ID"].ToString() + ")";
                    MyBase.ODBCCmd.ExecuteNonQuery();
                }


                // Image
                String Str = String.Empty;

                if (PhImage.Image != null)
                {
                    MemoryStream DefaultStream = new MemoryStream();
                    Image NewImage = PhImage.Image;
                    Bitmap NewImage1 = new Bitmap(NewImage, new Size(240, 320));
                    Image B = (Image)NewImage1;
                    B.Save(DefaultStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Byte[] Image1 = DefaultStream.ToArray();

                    Str = "Insert into VFit_Sample_Photo (Master_ID, Photo) Values (" + Code + ",?) ";

                    MyBase.ODBCCmd.CommandText = Str;
                    MyBase.ODBCCmd.Parameters.Add("@Photo", OdbcType.Image);
                    MyBase.ODBCCmd.Parameters["@Photo"].Value = Image1;
                    int Result = MyBase.ODBCCmd.ExecuteNonQuery();

                }
                
                MyBase.ODBCTrans.Commit();
                MyBase.Cn_Close();

                if (MyParent.Edit == true)
                {
                    MyBase.Run("Exec Delete_Item_Master_From_VSocks '" + TxtSampleNo.Text + "' ", "Exec Delete_Sample_Details_From_VSocks '" + TxtSampleNo.Text + "' ","Exec Delete_Sample_Master_From_VSocks '" + TxtSampleNo.Text + "' ");
                }

                MyBase.Run("exec Check_T2", "exec Get_Itemcode_From_VSocks", "exec Get_Sample_Master_From_VSocks", "exec Get_Sample_Details_From_VSocks","exec Insert_item_Master_VSocks", "exec Insert_Style_From_Sample_Master", "EXEC Insert_Stage_Color", "EXEC Insert_Stage_Item", "EXEC Insert_Stage_Count");

                MessageBox.Show("Saved ...!", "Gainup");
                MyParent.Save_Error = false;
                MyBase.Clear (this);

            }
            catch (Exception ex)
            {
                if (MyBase.ODBCTrans != null)
                {
                    MyBase.ODBCTrans.Rollback();
                }

                MyParent.Save_Error = true;
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_Datas(Int64 RowID)
        {
            try
            {
                DataTable Tdt = new DataTable();
                MyBase.Load_Data("Select V1.Sample_No, V1.BuyerID, B1.buyer, V1.NeedleID, V2.Name Needle, V1.EDate, V1.LinkingID, V3.Name Linking, V1.Cycle, V1.UOM1, U1.GUOM Cycle_Uom, V1.Time1, V1.UOM2, U2.GUOM Time_UOM, V1.Washing, V1.Template, V1.MeasurementID, V4.Name Measurement, V1.Sample_Qty, V1.UOM3, U3.GUOM Sample_UOM, V1.Weight, v1.UOM4, U4.GUOM Weight_UOM, V1.Total_Weight, V1.Average_Weight, V1.Waste_Per ,V1.Styleid, U5.Style, V1.Sizeid, U6.Size, Isnull(M1.Model_Name,'') Model, V1.ModelID, Isnull(V1.IMANNO,'')IMANNO, Isnull(I1.Item,'')Item, V1.SampleItemID  From VFit_Sample_Master V1 left Join buyer B1 On V1.BuyerID = B1.buyerid Left Join VFit_Sample_Needle_Master V2 On V1.NeedleID = V2.RowID Left Join VFit_Sample_linking_Master V3 On V1.Linkingid = V3.RowID Left Join Garment_UOM U1 On V1.UOM1 = U1.GUOMid Left Join Garment_UOM U2 On V1.UOM2 = U2.GUOMid Left Join VFit_Sample_Measurement_Master V4 On V1.MeasurementID = V4.RowID Left Join Garment_UOM U3 On V1.UOM3 = U3.GUOMid Left Join Garment_UOM U4 On V1.UOM4 = U4.GUOMid Left Join Socks_Style U5 On V1.Styleid = u5.Styleid Left Join Size U6 On V1.Sizeid = U6.Sizeid Left Join Socks_Model M1 On V1.ModelID = M1.Rowid Left Join Item I1 On V1.SampleItemID = I1.ItemID Where V1.RowID = " + RowID, ref Tdt);
                if (Tdt.Rows.Count > 0)
                {
                    Code = RowID;
                    TxtSampleNo.Text = Tdt.Rows[0]["Sample_No"].ToString();
                    TxtBuyer.Text = Tdt.Rows[0]["Buyer"].ToString();
                    TxtBuyer.Tag = Tdt.Rows[0]["BuyerID"].ToString();
                    TxtNeedle.Tag = Tdt.Rows[0]["NeedleID"].ToString();
                    TxtNeedle.Text = Tdt.Rows[0]["Needle"].ToString();
                    if (ChkFrom.Checked == true)
                    {
                        DataTable Tmpdt = new DataTable();
                        String Str = "Select Getdate()";
                        MyBase.Load_Data(Str, ref Tmpdt);
                        DtpDate.Value = Convert.ToDateTime(Tmpdt.Rows[0][0]);
                    }
                    else
                    {
                        DtpDate.Value = Convert.ToDateTime(Tdt.Rows[0]["EDate"]);
                    }
                    TxtLinking.Text = Tdt.Rows[0]["Linking"].ToString();
                    TxtLinking.Tag = Tdt.Rows[0]["LinkingID"].ToString();
                    DtpCycle.Value = Convert.ToDateTime(Tdt.Rows[0]["Cycle"]);
                    TxtCycleUOM.Tag = Tdt.Rows[0]["UOM1"].ToString();
                    TxtCycleUOM.Text = Tdt.Rows[0]["Cycle_UOM"].ToString();
                    TxtStyle.Tag = Tdt.Rows[0]["StyleID"].ToString();
                    TxtStyle.Text = Tdt.Rows[0]["Style"].ToString();
                    TxtSize.Tag = Tdt.Rows[0]["Sizeid"].ToString();
                    TxtSize.Text = Tdt.Rows[0]["SIze"].ToString();
                    TxtMeasurement.Tag = Tdt.Rows[0]["MeasurementID"].ToString();
                    TxtMeasurement.Text = Tdt.Rows[0]["Measurement"].ToString();
                    TxtRemarks.Text = Dr["Remarks"].ToString();
                   
                    Load_Image();

                    if (GBImage.Visible)
                    {
                        GBImage.Visible = false;
                    }

                    DtpTime.Value = Convert.ToDateTime(Tdt.Rows[0]["Time1"]);
                    TxtTimeUOM.Tag = Tdt.Rows[0]["UOM2"].ToString();
                    TxtTimeUOM.Text = Tdt.Rows[0]["Time_UOM"].ToString();

                    TxtWashing.Text = Tdt.Rows[0]["Washing"].ToString();
                    TxtTemplate.Text = Tdt.Rows[0]["Template"].ToString();

                    TxtSampleQty.Text = Tdt.Rows[0]["Sample_Qty"].ToString();
                    TxtSampQtyUom.Tag = Tdt.Rows[0]["UOM3"].ToString();
                    TxtSampQtyUom.Text = Tdt.Rows[0]["Sample_UOM"].ToString();

                    TxtWeight.Text = Tdt.Rows[0]["Weight"].ToString();
                    TxtWeightUOM.Tag = Tdt.Rows[0]["UOM4"].ToString();
                    TxtWeightUOM.Text = Tdt.Rows[0]["Weight_UOM"].ToString();

                    TxtTotalWeight.Text = Tdt.Rows[0]["Total_Weight"].ToString();
                    TxtAvgWeight.Text = Tdt.Rows[0]["Average_Weight"].ToString();
                    TxtWaste.Text = Tdt.Rows[0]["Waste_Per"].ToString();

                    TxtModel.Text = Tdt.Rows[0]["Model"].ToString();
                    TxtModel.Tag = Tdt.Rows[0]["ModelID"].ToString();
                    TxtImanNo.Text = Tdt.Rows[0]["IMANNO"].ToString();
                    TxtItem.Text = Tdt.Rows[0]["Item"].ToString();
                    TxtItem.Tag = Tdt.Rows[0]["SampleItemID"].ToString();

                    Grid_Data();

                }
                else
                {
                    MessageBox.Show("Invalid Details ..!", "Gainup");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Entry_Edit()
        {
            try
            {
                MyBase.Clear(this);

                //Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Sample - Edit", "Select V1.EDate, V1.Sample_No, V2.Name Needle, Cast(DatePart(MI, Cycle) As varchar)+':'+Cast(DatePart(SS, Cycle)As Varchar) Cycle,Cast(DatePart(MI,  Time1) As varchar)+':'+Cast(DatePart(SS,  Time1)As Varchar) Time, V1.Remarks, V1.NeedleID, V1.RowID From VFit_Sample_Master V1 Left Join VFit_Sample_Needle_Master V2 On V1.NeedleID = V2.RowID Where (V1.Approval_Flag = 'F' and V1.BuyerID in(100, 141)) or (V1.Approval_Flag = 'T' and V1.BuyerID Not in(100, 141))", String.Empty, 100, 120, 100, 100, 150);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Sample - Edit", "Select V1.EDate, V1.Sample_No, V2.Name Needle, Cast(DatePart(MI, Cycle) As varchar)+':'+Cast(DatePart(SS, Cycle)As Varchar) Cycle,Cast(DatePart(MI,  Time1) As varchar)+':'+Cast(DatePart(SS,  Time1)As Varchar) Time, V1.Remarks, V1.NeedleID, V1.RowID From VFit_Sample_Master V1 Left Join VFit_Sample_Needle_Master V2 On V1.NeedleID = V2.RowID ", String.Empty, 100, 120, 100, 100, 150);

                if (Dr != null)
                {
                    Fill_Datas(Convert.ToInt64(Dr["RowID"]));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Sample - Delete", "Select V1.EDate, V1.Sample_No, V2.Name Needle, Cast(DatePart(MI, Cycle) As varchar)+':'+Cast(DatePart(SS, Cycle)As Varchar) Cycle, Cast(DatePart(MI,  Time1) As varchar)+':'+Cast(DatePart(SS,  Time1)As Varchar) Time, V1.Remarks, V1.NeedleID, V1.RowID From VFit_Sample_Master V1 Left Join VFit_Sample_Needle_Master V2 On V1.NeedleID = V2.RowID Where (V1.Approval_Flag = 'F' and V1.BuyerID in(100, 141)) or (V1.Approval_Flag = 'T' and V1.BuyerID Not in(100, 141)) ", String.Empty, 100, 120, 100, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Convert.ToInt64(Dr["RowID"]));
                    MyParent.Load_DeleteConfirmEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_Delete_Confirm()
        {
            try
            {
                if (Code > 0)
                {
                    MyBase.Run("Delete from VFit_Sample_Photo Where MAster_ID = " + Code, "Delete from VFit_Sample_Process_Details Where Master_ID = " + Code, "Delete from VFit_Sample_Instruction_Details Where Master_Id = " + Code, "Delete From VFit_Sample_Details Where Master_ID = " + Code, "Delete from VFit_Sample_Master Where RowID = " + Code);
                    MessageBox.Show("Deleted ...!", "Gainup");
                    MyParent.Load_DeleteEntry();
                }
                else
                {
                    MessageBox.Show("Invalid Details to Delete ...!", "Gainup");
                    MyParent.Load_DeleteEntry();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Entry_View()
        {
            try
            {
                MyBase.Clear(this);
                Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Sample - View", "Select V1.EDate, V1.Sample_No, V2.Name Needle, Cast(DatePart(MI, Cycle) As varchar)+':'+Cast(DatePart(SS, Cycle)As Varchar) Cycle, Cast(DatePart(MI,  Time1) As varchar)+':'+Cast(DatePart(SS,  Time1)As Varchar) Time, V1.Remarks, V1.NeedleID, V1.RowID From VFit_Sample_Master V1 Left Join VFit_Sample_Needle_Master V2 On V1.NeedleID = V2.RowID ", String.Empty, 100, 120, 100, 100, 150);
                if (Dr != null)
                {
                    Fill_Datas(Convert.ToInt64(Dr["RowID"]));
                }
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
                MyParent.View_Browser("MIS_SOCKS_SAMPLE_REQ_SHEET", Code);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSample_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "TxtTotalWeight" || this.ActiveControl.Name == "TxtAvgWeight" || this.ActiveControl.Name == "TxtWaste" || this.ActiveControl.Name == "TxtSampleNo" || this.ActiveControl.Name == "TxtBuyer" || this.ActiveControl.Name == "TxtSampQtyUom" || this.ActiveControl.Name == "TxtNeedle" || this.ActiveControl.Name == "TxtMeasurement" || this.ActiveControl.Name == "TxtLinking" || this.ActiveControl.Name == "TxtCycleUOM" || this.ActiveControl.Name == "TxtWeightUOM" || this.ActiveControl.Name == "TxtTimeUOM" || this.ActiveControl.Name == "TxtSize" || this.ActiveControl.Name == "TxtStyle" || this.ActiveControl.Name == "TxtRemarks" || this.ActiveControl.Name == "TxtModel")
                {
                    MyBase.Valid_Null((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name == "TxtSampleQty")
                {
                    MyBase.Valid_Number ((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name == "TxtTemplate" || this.ActiveControl.Name == "TxtWeight")
                {
                    MyBase.Valid_Decimal((TextBox)this.ActiveControl, e);
                }
                else if (this.ActiveControl.Name == "TxtImanNo")
                {
                    MyBase.Valid_Alpha_Numeric((TextBox)this.ActiveControl, e);
                    MyBase.Return_Ucase(e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Fill_UOM()
        {
            try
            {
                TxtCycleUOM.Tag = "27";
                TxtTimeUOM.Tag = "26";
                TxtSampQtyUom.Tag = "27";
                TxtWeightUOM.Tag = "27";


                TxtCycleUOM.Text = MyBase.GetData_InString("Garment_UOM", "GUomID", TxtCycleUOM.Tag.ToString(), "GUOM");
                TxtTimeUOM.Text = MyBase.GetData_InString("Garment_UOM", "GUomID", TxtTimeUOM.Tag.ToString(), "GUOM");
                TxtSampQtyUom.Text = MyBase.GetData_InString("Garment_UOM", "GUomID", TxtSampQtyUom.Tag.ToString(), "GUOM");
                TxtWeightUOM.Text = MyBase.GetData_InString("Garment_UOM", "GUomID", TxtWeightUOM.Tag.ToString(), "GUOM");

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void FrmSample_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    if (this.ActiveControl.Name == "TxtSize")
                    {
                        TxtWeight.Focus();
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtImanNO")
                    {
                        tabControl1.SelectTab(0);
                        Grid.CurrentCell = Grid["Feeder", 0];
                        Grid.Focus();
                        Grid.BeginEdit(true);
                        return;
                    }
                    else if (this.ActiveControl.Name == "TxtWeight")
                    {
                        if (TxtWeight.Text.Trim() == String.Empty)
                        {
                            TxtWeight.Text = "0.00";
                        }
                        else if (Convert.ToDouble(TxtWeight.Text.ToString()) > 0)
                        {
                            if (ChkSize.Checked == true)
                            {
                                Size_Change = 1;
                                Grid_Data();
                                Total();
                            }
                        }
                        TxtRemarks.Focus();

                    }
                    else if (this.ActiveControl.Name == "TxtWaste")
                    {
                        if (MyParent._New || MyParent.Edit)
                        {
                            MyParent.Load_SaveEntry();
                        }
                    }
                    else if (ChkFrom.Checked == true && TxtBuyer.Text.ToString() == String.Empty)
                    {
                        MessageBox.Show("Select Buyer ...!", "Gainup");
                        TxtBuyer.Focus();
                    }
                    else
                    {
                        SendKeys.Send("{Tab}");
                    }
                }
                else if (e.KeyCode == Keys.Down)
                {
                    if (this.ActiveControl.Name == "TxtBuyer")
                    {
                        if (ChkFrom.Checked == true || ChkSize.Checked == true)
                        {
                            MyBase.Clear(this);
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Sample - View", "Select V1.EDate, V1.Sample_No, V2.Name Needle, V1.NeedleID,V1.Remarks, V1.RowID From VFit_Sample_Master V1 Left Join VFit_Sample_Needle_Master V2 On V1.NeedleID = V2.RowID ", String.Empty, 100, 120, 150);
                            if (Dr != null)
                            {
                                Fill_Datas(Convert.ToInt64(Dr["RowID"]));
                            }
                            TxtSampleQty.Enabled = false;
                            TxtSampQtyUom.Enabled = false;
                            TxtWeightUOM.Enabled = false;
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Buyer", "Select buyer, buyerid From buyer", String.Empty, 250, 80);
                            if (Dr != null)
                            {
                                TxtBuyer.Tag = Dr["BuyerID"].ToString();
                                TxtBuyer.Text = Dr["Buyer"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name.ToUpper().Contains("UOM"))
                    {
                        if (this.ActiveControl.Name == "TxtCycleUOM" || this.ActiveControl.Name == "TxtSampQtyUom" || this.ActiveControl.Name == "TxtWeightUOM")
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select UOM", "Select GUOM UOM, GUOMid UOMID From Garment_UOM where GUOMid=37", String.Empty, 200, 80);
                            if (Dr != null)
                            {
                                TxtCycleUOM.Tag = Dr["UOMID"].ToString();
                                TxtCycleUOM.Text = Dr["UOM"].ToString();

                                //TxtTimeUOM.Tag = Dr["UOMID"].ToString();
                                //TxtTimeUOM.Text = Dr["UOM"].ToString();

                                TxtSampQtyUom.Tag = Dr["UOMID"].ToString();
                                TxtSampQtyUom.Text = Dr["UOM"].ToString();

                                TxtWeightUOM.Tag = Dr["UOMID"].ToString();
                                TxtWeightUOM.Text = Dr["UOM"].ToString();
                            }
                        }
                        else
                        {
                            Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select UOM", "Select GUOM UOM, GUOMid UOMID From Garment_UOM where GUOMid=31 ", String.Empty, 200, 80);
                            if (Dr != null)
                            {
                                this.ActiveControl.Tag = Dr["UOMID"].ToString();
                                this.ActiveControl.Text = Dr["UOM"].ToString();
                            }
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtMeasurement")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Measurement", "Select Name, RowID From VFit_Sample_Measurement_Master", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtMeasurement.Tag = Dr["RowID"].ToString();
                            TxtMeasurement.Text = Dr["Name"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtStyle")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Style", "Select Style, StyleID From Socks_Style Order By Style", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtStyle.Tag = Dr["Styleid"].ToString();
                            TxtStyle.Text = Dr["Style"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtSize")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size", "Select Size, SizeID from Size Where Item_Type = 'Garment' And LEN(LTRIM(RTRIM(Size))) > 1 Order By Size", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtSize.Tag = Dr["Sizeid"].ToString();
                            TxtSize.Text = Dr["Size"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtLinking")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Linking", "Select Name, RowID from VFit_Sample_Linking_Master", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtLinking.Text = Dr["Name"].ToString();
                            TxtLinking.Tag = Dr["RowID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtNeedle")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Needle", "Select Name, RowID from VFit_Sample_Needle_Master", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtNeedle.Text = Dr["Name"].ToString();
                            TxtNeedle.Tag = Dr["RowID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtRemarks")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color", "Select Color, ColorID from Color Where Color Not Like '%ZZZ' And LEN(LTRIM(RTRIM(Color))) > 1 Order By Color", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtRemarks.Text = Dr["Color"].ToString();
                            TxtRemarks.Tag = Dr["ColorID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtModel")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Model", "Select Model_Name, Rowid ModelID From Socks_Model Where Model_Name Not Like 'ZZZ%' Order By Model_Name", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtModel.Text = Dr["Model_Name"].ToString();
                            TxtModel.Tag = Dr["ModelID"].ToString();
                        }
                    }
                    else if (this.ActiveControl.Name == "TxtItem")
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item_Type", "Select Item, ItemID From Item Where Item_Type = 'Garment'", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            TxtItem.Text = Dr["Item"].ToString();
                            TxtItem.Tag = Dr["ItemID"].ToString();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    if (this.ActiveControl.Name == "TxtSampleNo" || this.ActiveControl.Name == "TxtStyle" || this.ActiveControl.Name == "TxtStyle" || this.ActiveControl.Name == "TxtBuyer" || this.ActiveControl.Name == "TxtSampQtyUom" || this.ActiveControl.Name == "TxtNeedle" || this.ActiveControl.Name == "TxtMeasurement" || this.ActiveControl.Name == "TxtLinking" || this.ActiveControl.Name == "TxtCycleUOM" || this.ActiveControl.Name == "TxtWeightUOM" || this.ActiveControl.Name == "TxtTimeUOM")
                    {
                        e.Handled = true;
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    MyBase.ActiveForm_Close (this, MyParent);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt == null)
                {
                    Txt = (TextBox)e.Control;
                    Txt.KeyDown += new KeyEventHandler(Txt_KeyDown);
                    Txt.KeyPress += new KeyPressEventHandler(Txt_KeyPress);
                    Txt.Leave += new EventHandler(Txt_Leave);
                    Txt.GotFocus += new EventHandler(Txt_GotFocus);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Total()
        {
            try
            {
                TxtTotalWeight.Text = String.Format ("{0:0.0000}", Convert.ToDouble(MyBase.Sum(ref Grid, "Final", "Product_No", "ItemID", "SizeID", "ColoriD")));
                //TxtTotalWeight.Text = Convert.ToString((Convert.ToDouble(TxtTotalWeight.Text) / Convert.ToDouble(TxtSampleQty.Text)));
                if (TxtSampleQty.Text.Trim() == String.Empty || Convert.ToDouble(TxtSampleQty.Text) == 0 || Convert.ToDouble(TxtTotalWeight.Text) == 0)
                {
                    return;
                }
                else
                {
                    TxtAvgWeight.Text = String.Format ("{0:0.0000}", Convert.ToDouble(TxtTotalWeight.Text) / Convert.ToDouble(TxtSampleQty.Text));
                    //TxtWaste.Text = String.Format("{0:0.0000}", ((Convert.ToDouble(TxtAvgWeight.Text) - Convert.ToDouble(TxtWeight.Text)) / Convert.ToDouble(TxtWeight.Text)) * 100);
                    TxtWaste.Text = String.Format("{0:0.0000}", ((Convert.ToDouble(TxtAvgWeight.Text) - Convert.ToDouble(TxtWeight.Text)) / Convert.ToDouble(TxtWeight.Text)) * 100);
                    for (int i = 0; i <= Dt.Rows.Count - 1; i++)
                    {
                        if (Grid["Final", i].Value != null && Grid["Final", i].Value != DBNull.Value && Grid["Final", i].Value.ToString() != String.Empty)
                        {
                            Grid["RM", i].Value = String.Format("{0:0.0000}", ((Convert.ToDouble(Grid["Final", i].Value) / Convert.ToDouble(TxtTotalWeight.Text)) * 100));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void Txt_GotFocus(object sender, EventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Feeder"].Index)
                {
                    MyBase.Row_Number(ref Grid);
                    Total();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Leave(object sender, EventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Before"].Index || Grid.CurrentCell.ColumnIndex == Grid.Columns["After"].Index)
                {
                    MyBase.Valid_Decimal(Txt, e);
                }
                else
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Feeder"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New ("FeederID", this, 30, 70, ref Dt, SelectionTool_Class.ViewType.NormalView, "Select Feeder", "Select Name, RowID FeederID from VFit_Sample_Feeder_Master", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Name"].ToString();
                            Grid["Feeder", Grid.CurrentCell.RowIndex].Value = Dr["Name"].ToString();
                            Grid["FeederID", Grid.CurrentCell.RowIndex].Value = Dr["FeederID"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Product_No"].Index)
                    {
                        if (ChkSize.Checked == true)
                        {
                            //if (Grid["Feeder", Grid.CurrentCell.RowIndex].Value == String.Empty && Grid["Before", Grid.CurrentCell.RowIndex].Value == String.Empty)
                            if (Grid["Feeder", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == null || Grid["Feeder", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                MessageBox.Show("Can't Insert New Items..!");
                                return;
                            }
                        }
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Product", "Select V1.Product_No, I1.Item, C1.color, S1.size, V1.ItemID, V1.ColorID, V1.SizeID, V1.RowID Product_ID From VFit_Sample_Product_Master V1 Left Join Item I1 On V1.ItemID = I1.itemid Left join Color C1 On V1.ColorID = C1.colorid Left join Size S1 On V1.SizeID = S1.Sizeid Where I1.Item Not Like 'ZZZ%' And C1.Color Not Like 'ZZZ%' And S1.Size Not Like 'ZZZ%' ", String.Empty, 100, 150, 150, 150);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Product_No"].ToString();
                            Grid["Product_id", Grid.CurrentCell.RowIndex].Value = Dr["Product_id"].ToString();
                            Grid["Product_No", Grid.CurrentCell.RowIndex].Value = Dr["Product_NO"].ToString();
                            Grid["item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                            Grid["itemid", Grid.CurrentCell.RowIndex].Value = Dr["Itemid"].ToString();
                            Grid["Sizeid", Grid.CurrentCell.RowIndex].Value = Dr["Sizeid"].ToString();
                            Grid["Colorid", Grid.CurrentCell.RowIndex].Value = Dr["Colorid"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Item"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Item", "Select Item, ItemID from Item", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Item"].ToString();
                            Grid["Item", Grid.CurrentCell.RowIndex].Value = Dr["Item"].ToString();
                            Grid["ItemID", Grid.CurrentCell.RowIndex].Value = Dr["ItemID"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Size"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Size", "Select Size, SizeID from Size", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Size"].ToString();
                            Grid["Size", Grid.CurrentCell.RowIndex].Value = Dr["Size"].ToString();
                            Grid["SizeID", Grid.CurrentCell.RowIndex].Value = Dr["SizeID"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Color"].Index)
                    {
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Color", "Select Color, ColorID from Color", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Color"].ToString();
                            Grid["Color", Grid.CurrentCell.RowIndex].Value = Dr["Color"].ToString();
                            Grid["ColorID", Grid.CurrentCell.RowIndex].Value = Dr["ColorID"].ToString();
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Ply"].Index)
                    {
                        if (ChkSize.Checked == true)
                        {
                            if (Grid["Feeder", Grid.CurrentCell.RowIndex].Value.ToString().Trim() == null || Grid["Feeder", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty)
                            {
                                MessageBox.Show("Can't Insert New Items..!");
                                return;
                            }
                        }
                        Dr = Tool.Selection_Tool(this, 30, 70, SelectionTool_Class.ViewType.NormalView, "Select Ply", "Select Ply, RowID From Socks_Yarn_Ply_Master", String.Empty, 200, 80);
                        if (Dr != null)
                        {
                            Txt.Text = Dr["Ply"].ToString();
                            Grid["Ply", Grid.CurrentCell.RowIndex].Value = Dr["Ply"].ToString();
                            Grid["Ply_ID", Grid.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (GBImage.Visible)
                {
                    GBImage.Visible = false;
                }
                else
                {
                    GBImage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Show_Image1()
        {
            try
            {
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "Pictures(*.Jpg,*.Gif,*.Bmp)|*.Jpg;,*.Gif;,*.Bmp;";
                openFileDialog1.FileName = String.Empty;
                openFileDialog1.ShowDialog();

                if (openFileDialog1.FileName.Trim() != String.Empty)
                {
                    Update_Image1(openFileDialog1.FileName);
                }
                else
                {
                    //Image1.Image = Image1.InitialImage;
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update_Image1(String Term)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<String>(Update_Image1), new Object[] { Term });
                    return;
                }
                PhImage.Image = Image.FromFile(Term);
                PhImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Thread Th = new Thread(Show_Image1);
                Th.SetApartmentState(ApartmentState.STA);
                Th.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (GBImage.Visible)
                {
                    GBImage.Visible = false;
                }
                else
                {
                    GBImage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show (ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                PhImage.Image = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (Grid.CurrentCell.ColumnIndex == Grid.Columns["Before"].Index)
                    {
                        if (Txt.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid Before Value ...!", "Gainup");
                            Grid.CurrentCell = Grid["Before", Grid.CurrentCell.RowIndex];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                    }
                    else if (Grid.CurrentCell.ColumnIndex == Grid.Columns["After"].Index)
                    {
                        if (Txt.Text.Trim() == String.Empty)
                        {
                            MessageBox.Show("Invalid After Value ...!", "Gainup");
                            Grid.CurrentCell = Grid["Before", 0];
                            Grid.Focus();
                            Grid.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            if (Grid["Before", Grid.CurrentCell.RowIndex].Value.ToString() == String.Empty || Convert.ToDouble(Grid["Before", Grid.CurrentCell.RowIndex].Value) == 0)
                            {
                                MessageBox.Show("Invalid Before Value ...!", "Gainup");
                                Grid.CurrentCell = Grid["Before", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }

                            if (Convert.ToDouble(Txt.Text) > Convert.ToDouble(Grid["Before", Grid.CurrentCell.RowIndex].Value))
                            {
                                MessageBox.Show("Invalid After Value. Greater than Before Value ...!", "Gainup");
                                Grid.CurrentCell = Grid["Before", 0];
                                Grid.Focus();
                                Grid.BeginEdit(true);
                                return;
                            }

                            Grid["Final", Grid.CurrentCell.RowIndex].Value = String.Format("{0:0.00}", (Convert.ToDouble(Grid["Before", Grid.CurrentCell.RowIndex].Value) - Convert.ToDouble(Grid["After", Grid.CurrentCell.RowIndex].Value)));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    tabControl1.SelectTab(1);
                    Grid_Instruction.CurrentCell = Grid_Instruction["Details", 0];
                    Grid_Instruction.Focus();
                    Grid_Instruction.BeginEdit(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Instruction_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Instruction == null)
                {
                    Txt_Instruction = (TextBox)e.Control;
                    Txt_Instruction.KeyDown += new KeyEventHandler(Txt_Instruction_KeyDown);
                    Txt_Instruction.KeyPress += new KeyPressEventHandler(Txt_Instruction_KeyPress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Instruction_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Grid_Instruction.CurrentCell.ColumnIndex == Grid_Instruction.Columns["Instruction"].Index)
                {
                    MyBase.Valid_Null(Txt, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Instruction_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    /* if (Grid_Instruction.CurrentCell.ColumnIndex == Grid_Instruction.Columns["Instruction"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Instruction", this, 30, 70, ref Dt_Instruction, SelectionTool_Class.ViewType.NormalView, "Select Instrucion", "Select Name Instruction, RowID Instruction_ID from VFit_Sample_Instruction_Master", String.Empty, 250);
                        if (Dr != null)
                        {
                            MyBase.Row_Number(ref Grid_Instruction);
                            Grid_Instruction["Instruction", Grid_Instruction.CurrentCell.RowIndex].Value = Dr["Instruction"].ToString();
                            Grid_Instruction["Instruction_ID", Grid_Instruction.CurrentCell.RowIndex].Value = Dr["Instruction_ID"].ToString();
                            Txt_Instruction.Text = Dr["Instruction"].ToString();
                        } 
                    } */
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    if (Grid_Instruction.CurrentCell.ColumnIndex == Grid_Instruction.Columns["Instruction"].Index)
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

        private void Grid_Process_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                if (Txt_Process == null)
                {
                    Txt_Process = (TextBox)e.Control;
                    Txt_Process.KeyDown += new KeyEventHandler(Txt_Process_KeyDown);
                    Txt_Process.KeyPress += new KeyPressEventHandler(Txt_Process_KeyPress);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Process_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                MyBase.Valid_Null(Txt_Process, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void Txt_Process_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down)
                {
                    if (Grid_Process.CurrentCell.ColumnIndex == Grid_Process.Columns["Process"].Index)
                    {
                        Dr = Tool.Selection_Tool_Except_New("Process", this, 30, 70, ref Dt_Process, SelectionTool_Class.ViewType.NormalView, "Select Process", "Select Name Process, RowID From VFit_Sample_Process_Master", String.Empty, 300);
                        if (Dr != null)
                        {
                            MyBase.Row_Number(ref Grid_Process);
                            Grid_Process["Process_ID", Grid_Process.CurrentCell.RowIndex].Value = Dr["RowID"].ToString();
                            Grid_Process["Process", Grid_Process.CurrentCell.RowIndex].Value = Dr["Process"].ToString();
                            Txt_Process.Text = Dr["Process"].ToString();
                        }
                    }
                }
                else if (e.KeyCode == Keys.Delete || e.KeyCode == Keys.Back)
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Instruction_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    tabControl1.SelectTab(2);
                    Grid_Process.CurrentCell = Grid_Process["Process", 0];
                    Grid_Process.Focus();
                    Grid_Process.BeginEdit(true);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Process_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == Convert.ToChar(Keys.Escape))
                {
                    TxtWaste.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //MyBase.Grid_Delete(ref Grid, ref Dt, Grid.CurrentCell.RowIndex);
                if (ChkSize.Checked == false)
                {
                    if (MessageBox.Show("Are you sure to Delete..?", "Gainup", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        Dt.Rows.RemoveAt(Grid.CurrentCell.RowIndex);
                    }
                }
                else if (ChkSize.Checked == true)
                {
                    MessageBox.Show("Can't Delete...!Gainup");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Grid_Instruction_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_Instruction, ref Dt_Instruction, Grid_Instruction.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Grid_Process_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                MyBase.Grid_Delete(ref Grid_Process, ref Dt_Process, Grid_Process.CurrentCell.RowIndex);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void DtpCycle_Leave_1(object sender, EventArgs e)
        {
            Double Value = 0;
            int Hour = 0;
            int Min = 0;
            int Sec = 0;
            try
            {
                if (DtpCycle.Value.Hour == 12)
                {
                    Value = (DtpCycle.Value.Minute * 60);
                    Value += DtpCycle.Value.Second;
                }
                else
                {
                    Value += ((DtpCycle.Value.Hour * 60) * 60);
                    Value += (DtpCycle.Value.Minute * 60);
                    Value += DtpCycle.Value.Second;
                }
                
                //Hour = Value / 3600
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmSample_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (this.ActiveControl.Name == "DtpTime")
                {
                  //  DtpCycle.Value = Convert.ToDateTime(DtpTime.Value);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DtpTime_ValueChanged(object sender, EventArgs e)
        {
            //DtpCycle.Value = Convert.ToDateTime(DtpTime.Value);
            //DtpTime.Value = DtpCycle.Value.AddHours(DtpTime.Value.Hour);
            //DtpTime.Value = DtpCycle.Value.AddMinutes(DtpTime.Value.Minute);
            DtpCycle.Value = Convert.ToDateTime(DtpTime.Value);
            DtpCycle.Value = DtpCycle.Value.AddMinutes(DtpTime.Value.Minute);
            DtpCycle.Value = DtpCycle.Value.AddSeconds(DtpTime.Value.Second);
        }

        private void DtpTime_Leave(object sender, EventArgs e)
        {
            DtpCycle.Value = Convert.ToDateTime(DtpTime.Value);
            DtpCycle.Value = DtpCycle.Value.AddMinutes(DtpTime.Value.Minute);
            DtpCycle.Value = DtpCycle.Value.AddSeconds(DtpTime.Value.Second);
        }

        private void TxtRemarks_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void ChkFrom_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkFrom.Checked == true)
                {
                    if (ChkSize.Checked == true)
                    {
                        ChkSize.Checked = false;
                    }
                    ChkSize.Enabled = false;
                }
                else if (ChkFrom.Checked == false)
                {
                    if (ChkSize.Checked == true)
                    {
                        ChkSize.Checked = false;
                    }
                    ChkSize.Enabled = true;
                }
                MyBase.Clear(this);
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChkSize_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkSize.Checked == true)
                {
                    if (ChkFrom.Checked == true)
                    {
                        ChkFrom.Checked = false;
                    }
                    ChkFrom.Enabled = false;
                    Size_Change = 1;
                    TxtSampleQty.Enabled = false;
                    TxtSampQtyUom.Enabled = false;
                    TxtWeightUOM.Enabled = false;
                }
                else if (ChkSize.Checked == false)
                {
                    if (ChkFrom.Checked == true)
                    {
                        ChkFrom.Checked = false;
                    }
                    ChkFrom.Enabled = true;
                    Size_Change = 0;
                    TxtSampleQty.Enabled = true;
                    TxtSampQtyUom.Enabled = true;
                    TxtWeightUOM.Enabled = true;
                }
                MyBase.Clear(this);
                TxtBuyer.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}