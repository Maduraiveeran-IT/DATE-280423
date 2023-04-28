using System;
using System.Data;
using System.Windows.Forms;
using System.Configuration;

namespace DotnetVFGrid
{
    public class MyDataGridView : DataGridView
    {
        protected override bool ProcessDialogKey(Keys keyData)
        {
            try
            {
                if (keyData == Keys.Enter)
                {
                    this.EndEdit();
                    this.CurrentCell = this[this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex];
                    base.OnKeyDown(new KeyEventArgs(Keys.Enter));
                    //return false;s
                    return true;
                }
                else if (keyData == Keys.Escape)
                {
                    if (this.CurrentCell != null)
                    {
                        if (this.CurrentCell.IsInEditMode)
                        {
                            if (Convert.ToString(this.CurrentCell.Value).Trim() == String.Empty)
                            {
                                this.EndEdit();
                                return false;
                            }
                            else
                            {
                                this.EndEdit();
                                this.BeginEdit(true);
                                return true;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                return base.ProcessDialogKey(keyData);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return true;
            }
        }

        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (this.IsCurrentCellInEditMode)
                {
                    return false;
                }
            }
            else if (e.KeyCode == Keys.Enter)
            {
                if (Convert.ToString(this.CurrentCell.Value).Trim() != String.Empty)
                {
                    if (this.CurrentCell.ColumnIndex != this.Columns.Count - 1)
                    {
                        e.Handled = true;
                        if (this.Columns[1].Name.ToUpper() == "TYPE")
                        {
                            NextCell_Focus(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex, false);
                        }
                        else
                        {
                            NextCol_Focus(this.CurrentCell.RowIndex, this.CurrentCell.ColumnIndex, false);
                        }
                        return false;
                    }
                    else
                    {
                        if (this.Name.ToUpper() == "GRID_BUDGET_PR")
                        {
                            if (this.Rows.Count - 1 == this.CurrentCell.RowIndex)
                            {
                                this.CurrentCell = this.CurrentCell;
                            }
                            else
                            {
                                this.CurrentCell = this[this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex + 1];
                                this.Focus();
                                this.BeginEdit(true);
                            }
                            return false;
                        }
                        else
                        {
                            if (this.Columns[1].Name.ToUpper() == "TYPE")
                            {
                                this.CurrentCell = this[1, this.CurrentCell.RowIndex];
                                this.Focus();
                                this.BeginEdit(true);
                            }
                            else
                            {
                                this.CurrentCell = this[0, this.CurrentCell.RowIndex];
                            }
                        }
                    }
                }
                else
                {
                    this.CurrentCell = this[this.CurrentCell.ColumnIndex, this.CurrentCell.RowIndex];
                    return false;
                }
            }
            return base.ProcessDataGridViewKey(e);
        }

        Boolean NextCell_Focus(int RowIndex, int ColIndex, Boolean Flag)
        {
            if (this.SelectionMode != DataGridViewSelectionMode.FullRowSelect)
            {
                if (ColIndex != this.Columns.Count - 1)
                {
                    for (ColIndex = ColIndex + 1; ColIndex <= this.Columns.Count - 1; ColIndex++)
                    {
                        if (this[ColIndex,this.CurrentCell.RowIndex].ReadOnly == false)
                        {
                            if (this.Columns[ColIndex].Visible == true)
                            {
                                this.CurrentCell = this[ColIndex, RowIndex];
                                this.Focus();
                                this.BeginEdit(true);
                                if (Flag == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    //return false;
                                    return true;
                                }
                            }
                        }
                    }
                    for (ColIndex = 0; ColIndex <= this.Columns.Count - 1; ColIndex++)
                    {
                        if (this[ColIndex, this.CurrentCell.RowIndex].ReadOnly == false)
                        {
                            if (this.Columns[ColIndex].Visible == true)
                            {
                                if (this.Rows.Count - 1 != RowIndex)
                                {
                                    this.CurrentCell = this[ColIndex, RowIndex + 1];
                                    this.Focus();
                                    this.BeginEdit(true);
                                }
                                else
                                {
                                    return false;
                                }
                                if (Flag == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    //return false;
                                    return true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (ColIndex = 0; ColIndex <= this.Columns.Count - 1; ColIndex++)
                    {
                        if (this[ColIndex, this.CurrentCell.RowIndex].ReadOnly == false)
                        {
                            if (this.Columns[ColIndex].Visible == true)
                            {
                                this.CurrentCell = this[ColIndex, RowIndex + 1];
                                this.Focus();
                                this.BeginEdit(true);
                                if (Flag == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    //return false;
                                    return true;
                                }
                            }
                        }
                    }
                }
                if (Flag == true)
                {
                    return true;
                }
                else
                {
                    //return false;
                    return true;
                }
            }
            else
            {
                return this.ProcessDownKey(Keys.Down);
            }
        }

        Boolean NextCol_Focus(int RowIndex, int ColIndex, Boolean Flag)
        {
            if (this.SelectionMode != DataGridViewSelectionMode.FullRowSelect)
            {
                if (ColIndex != this.Columns.Count - 1)
                {
                    for (ColIndex = ColIndex + 1; ColIndex <= this.Columns.Count - 1; ColIndex++)
                    {
                        if (this.Columns[ColIndex].ReadOnly == false)
                        {
                            if (this.Columns[ColIndex].Visible == true)
                            {
                                this.CurrentCell = this[ColIndex, RowIndex];
                                this.Focus();
                                this.BeginEdit(true);
                                if (Flag == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                    //return true;
                                }
                            }
                        }
                    }
                    for (ColIndex = 0; ColIndex <= this.Columns.Count - 1; ColIndex++)
                    {
                        if (this.Columns[ColIndex].ReadOnly == false)
                        {
                            if (this.Columns[ColIndex].Visible == true)
                            {
                                if (this.Rows.Count - 1 != RowIndex)
                                {
                                    this.CurrentCell = this[ColIndex, RowIndex + 1];
                                    this.Focus();
                                    this.BeginEdit(true);
                                }
                                else
                                {
                                    return false;
                                }
                                if (Flag == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                    //return true;
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (ColIndex = 0; ColIndex <= this.Columns.Count - 1; ColIndex++)
                    {
                        if (this.Columns[ColIndex].ReadOnly == false)
                        {
                            if (this.Columns[ColIndex].Visible == true)
                            {
                                this.CurrentCell = this[ColIndex, RowIndex + 1];
                                this.Focus();
                                this.BeginEdit(true);
                                if (Flag == true)
                                {
                                    return true;
                                }
                                else
                                {
                                    return false;
                                    //return true;
                                }
                            }
                        }
                    }
                }
                if (Flag == true)
                {
                    return true;
                }
                else
                {
                    //return false;
                    return true;
                }
            }
            else
            {
                return this.ProcessDownKey(Keys.Down);
            }
        }
    }
}


