using System.Windows;
using Ara3D.BimOpenSchema.IO;
using Ara3D.DataTable;
using Ara3D.Utils;

namespace Ara3D.BimOpenSchema.Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded; 
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 1. Build the file path
            var fp = PathUtil.GetCallerSourceFolder()
                .RelativeFile("..", "..", "data", "input",
                    "snowdon.bimdata.parquet.zip");

            // 2. Do the I/O *asynchronously* without blocking the UI thread
            var dataSet = await fp.ReadParquetFromZipAsync()
                .ConfigureAwait(false);   // don’t capture UI ctx

            // 3. Marshal back to the UI thread for UI work (if you used ConfigureAwait(false))
            await Dispatcher.InvokeAsync(() =>
            {
                foreach (var t in dataSet.Tables)
                {
                    var grid = TabControl.AddDataGridTab(t.Name);
                    grid.AssignDataTable(t);
                }
            });
        }
    }
}   