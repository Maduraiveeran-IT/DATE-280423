using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using Accounts;
using SelectionTool; 
using System.Text;

namespace SelectionTool_NmSp
{
    class SelectionTool_Class
    {
        public enum ViewType
        {
            NormalView = 0,
            AddressView = 1,
        }

        public DataRow Selection_Tool_WOMDI(Form OwnerForm, int _Left, int _Top, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool Main;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            //Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.ShowDialog(OwnerForm);
            if (Main.Approval == false)
            {
                Main.Selected_Row = null;
            }
            return Main.Selected_Row;
        }


        public DataRow Selection_Tool(Form OwnerForm,int _Left, int _Top, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width) 
        {
            FrmSelectionTool Main;
            MDIMain Myparent = (MDIMain)OwnerForm.MdiParent;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            //Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            Main.CompName = Myparent.CompName;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.ShowDialog(OwnerForm);   
            if (Main.Approval == false)
            {
                Main.Selected_Row = null;    
            }
            return Main.Selected_Row;
        }

        public DataRow Selection_Tool_Except_New_WOMDI(String ColName, Form OwnerForm, int _Left, int _Top, ref DataTable Dt, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool Main;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            //Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.Dv = Distinct_Datatable_New(ColName, ref Dt, ref Main.Dv);
            if (Main.Dv.Table.Rows.Count == 0)
            {
                MessageBox.Show(ColName + " Not Available ...!");
                return Main.Selected_Row;
            }
            else
            {
                Main.Grid_Refresh();
                Main.ShowDialog(OwnerForm);
                if (Main.Approval == false)
                {
                    Main.Selected_Row = null;
                }
                return Main.Selected_Row;
            }
        }


        public DataRow Selection_Tool_Except_New(String ColName, Form OwnerForm, int _Left, int _Top, ref DataTable Dt, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool Main;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            //Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.Dv = Distinct_Datatable_New(ColName, ref Dt, ref Main.Dv);
            if (Main.Dv.Table.Rows.Count == 0)
            {
                MessageBox.Show(ColName + " Not Available ...!");
                return Main.Selected_Row;
            }
            else
            {
                Main.Grid_Refresh();
                Main.ShowDialog(OwnerForm);
                if (Main.Approval == false)
                {
                    Main.Selected_Row = null;
                }
                return Main.Selected_Row;
            }
        }

        DataView Distinct_Datatable_New(String ColName, ref DataTable Source, ref DataView Destination)
        {
            Boolean Update = false;
            try
            {
                for (int i = 0; i <= Source.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Destination.Table.Rows.Count - 1; j++)
                    {
                        if (Source.Rows[i][ColName].ToString() == Destination.Table.Rows[j][ColName].ToString())
                        {
                            Destination.Table.Rows.RemoveAt(j);
                            Update = true;
                        }
                    }
                }
                if (Update)
                {
                    for (int i = 0; i <= Destination.Table.Rows.Count - 1; i++)
                    {
                        Destination.Table.Rows[i]["Id"] = i;
                    }
                }
                return Destination;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataRow Selection_Tool_Resize(Form OwnerForm, int _Left, int _Top, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool Main;
            MDIMain Myparent = (MDIMain)OwnerForm.MdiParent;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            //Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            // Main.CompName = Myparent.CompName;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.Resize();
            Main.ShowDialog(OwnerForm);
            if (Main.Approval == false)
            {
                Main.Selected_Row = null;
            }
            return Main.Selected_Row;
        }


        public DataRow Selection_Tool_Sizing(Form OwnerForm, int _Left, int _Top, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool Main;
            MDIMain Myparent = (MDIMain)OwnerForm.MdiParent;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            Main.Approval = false;
            Main.CompName = Myparent.CompName;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail_Other_DB(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail_Other_DB(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.ShowDialog(OwnerForm);
            if (Main.Approval == false)
            {
                Main.Selected_Row = null;
            }
            return Main.Selected_Row;
        }



        public DataRow Selection_Tool_Ledger(Form OwnerForm, int _Left, int _Top, String Title, String TblName, String Field_Name, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool_ledger Main;
            Main = new FrmSelectionTool_ledger();
            Main.Selected_Row = null;
            Main.Approval = false;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            //Main.Grid_Detail(false, Sql, Column_Width);
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.ShowDialog(OwnerForm);
            if (Main.Approval == false)
            {
                Main.Selected_Row = null;
            }
            return Main.Selected_Row;
        }

        public DataRow Selection_Tool_Except(Form OwnerForm, int _Left, int _Top, ref DataTable Dt, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionTool Main;
            Main = new FrmSelectionTool();
            Main.Selected_Row = null;
            //Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.Dv = Distinct_Datatable(ref Dt, ref Main.Dv);
            if (Main.Dv.Table.Rows.Count == 0)
            {
                MessageBox.Show("RefDocs Not Available ...!");
                return Main.Selected_Row;
            }
            else
            {
                Main.Grid_Refresh();
                Main.ShowDialog(OwnerForm);
                if (Main.Approval == false)
                {
                    Main.Selected_Row = null;
                }
                return Main.Selected_Row;
            }
        }

        DataView Distinct_Datatable(ref DataTable Source, ref DataView Destination)
        {
            Boolean Update = false;
            try
            {
                for (int i = 0; i <= Source.Rows.Count - 1; i++)
                {
                    for (int j = 0; j <= Destination.Table.Rows.Count - 1; j++)
                    {
                        if (Source.Rows[i]["Mode"].ToString() == "A")
                        {
                            if (Source.Rows[i]["RefDoc"].ToString() == Destination.Table.Rows[j]["RefDoc"].ToString() && Source.Rows[i]["RefDate"].ToString() == Destination.Table.Rows[j]["RefDate"].ToString())
                            {
                                Destination.Table.Rows.RemoveAt(j);
                                Update = true;
                            }
                        }
                    }
                }
                if (Update)
                {
                    for (int i = 0; i <= Destination.Table.Rows.Count - 1; i++)
                    {
                        Destination.Table.Rows[i]["Id"] = i;
                    }
                }
                return Destination;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public DataRow Selection_Tool_Item(Form OwnerForm, int _Left, int _Top, ViewType V, String Title, String Sql, String RelatedWord, params int[] Column_Width)
        {
            FrmSelectionToolItem Main;
            Main = new FrmSelectionToolItem();
            Main.Selected_Row = null;
            Main.BackColor = System.Drawing.Color.LightSkyBlue;
            Main.Approval = false;
            if (RelatedWord.Trim() != String.Empty)
            {
                Main.Related_Word = RelatedWord;
            }
            if (V == 0)
            {
                Main.Grid_Detail(false, Sql, Column_Width);
            }
            else
            {
                Main.Grid_Detail(true, Sql, Column_Width);
            }
            Main.Caption(Title);
            Main.StartPosition = FormStartPosition.Manual;
            Main.Left = _Left;
            Main.Top = _Top;
            Main.ShowDialog(OwnerForm);
            if (Main.Approval == false)
            {
                Main.Selected_Row = null;
            }
            return Main.Selected_Row;
        }

    }
}
