using System.Windows;
using Ara3D.DataTable;
using System.Windows.Controls;
using System.Windows.Data;

namespace Ara3D.BimOpenSchema.Browser;

public static class DataGridUtils
{
    public static void AssignDataTable(this DataGrid grid, IDataTable t)
    {
        grid.Columns.Clear();
        grid.Columns.Add(new DataGridTextColumn
        {
            Header = "Index",
            Binding = new Binding(nameof(IDataRow.RowIndex)) { Mode = BindingMode.OneTime }
        });
        for (int c = 0; c < t.Columns.Count; c++)
        {
            grid.Columns.Add(new DataGridTextColumn
            {
                Header = t.Columns[c].Descriptor.Name,
                Binding = new Binding($"[{c}]") { Mode = BindingMode.OneTime }
            });
        }
        grid.ItemsSource = t.Rows;
    }

    /// <summary>
    /// Adds a new tab containing an empty DataGrid and returns that DataGrid.
    /// Call from the UI thread, or any thread – the method marshals to Dispatcher.
    /// </summary>
    public static DataGrid AddDataGridTab(this TabControl host, string headerText)
    {
        // marshal if needed
        if (!host.Dispatcher.CheckAccess())
            return host.Dispatcher.Invoke(() => AddDataGridTab(host, headerText));

        // 1. create the grid (tweak defaults here as you like)
        var dg = new DataGrid
        {
            AutoGenerateColumns = false,   
            CanUserAddRows = false,
            CanUserDeleteRows = false,
            IsReadOnly = false,
            Margin = new Thickness(6)
        };

        // 2. wrap it in a tab
        var tab = new TabItem
        {
            Header = headerText,
            Content = dg
        };

        // 3. add + select
        host.Items.Add(tab);
        host.SelectedItem = tab;

        return dg;
    }
}
