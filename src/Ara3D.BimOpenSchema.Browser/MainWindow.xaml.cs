using System.Reflection;
using System.Windows;
using Ara3D.BimOpenSchema.IO;
using Ara3D.DataTable;
using Ara3D.Utils;
using DocumentFormat.OpenXml.Drawing;
using Microsoft.Win32;

namespace Ara3D.BimOpenSchema.Browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public BimData Data;
        public BimDataModel Model;

        public OpenFileDialog OpenFileDialog = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        public static IDataTable CreateParameterDataTable(BimDataModel model, string category)
        {
            var entities = model.Entities.Where(e => e.Category == category).ToList();
            var parameters = entities.Select(e => e.ParameterValues).ToList();
            return new DataTableFromDictionaryList(parameters, category);
        }

        private async void Raw_Click(object sender, RoutedEventArgs e)
        {
            await UpdateTables();
        }

        private async void Open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ??= new OpenFileDialog()
            {
                DefaultExt = ".zip",
                Filter = "Zipped parquet files (*.zip)|*.zip|All files (*.*)|*.*"
            };

            if (OpenFileDialog.ShowDialog() == true)
            {
                var fp = new FilePath(OpenFileDialog.FileName);
                Model = null;
                Data = await fp.ReadBimDataFromParquetZipAsync().ConfigureAwait(false);
                Model = new BimDataModel(Data);
                await UpdateTables();
            }
        }

        private async Task UpdateTables()
        {
            var cats = Model.Entities
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            await Dispatcher.InvokeAsync(() =>
            {
                var tables = RawMenuItem.IsChecked
                    ? Data.ToDataSet().Tables
                    : cats.Select(c => CreateParameterDataTable(Model, c)).ToList();

                TabControl.Items.Clear();
                foreach (var t in tables)
                {
                    var grid = TabControl.AddDataGridTab(t.Name);
                    grid.AssignDataTable(t);
                }
            });

        }
    }
}   