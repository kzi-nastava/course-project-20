using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Data;

namespace HealthCareCenter.Core
{
    public static class TableService
    {
        public static int GetSelectedIndex(DataGrid dataGrid)
        {
            int rowIndex;
            try
            {
                rowIndex = dataGrid.SelectedIndex;
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return -1;
            }
            return rowIndex;
        }

        public static int GetRowItemID(DataGrid grid, string key)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)grid.SelectedItems[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return -1;
            }
            return (int)row[key];
        }
        public static string GetRowItem(DataGrid grid, string key)
        {
            DataRowView row;
            try
            {
                row = (DataRowView)grid.SelectedItems[0];
            }
            catch
            {
                MessageBox.Show("Select a row from the table");
                return "";
            }
            return (string)row[key];
        }

        public static string GetComboBoxItem(ComboBox comboBox)
        {
            string selectedValue = "";
            try
            {
                selectedValue = ((ComboBoxItem)comboBox.SelectedItem).Content.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return selectedValue;
        }
    }
}
