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
            var fp = PathUtil.GetCallerSourceFolder().RelativeFile("..", "input", "bimdata.mpz");

            // TODO: read from the file. 
            var dataSet = new ReadOnlyDataSet([]);
            
            foreach (var t in dataSet.Tables)
            {
                var dataGrid = TabControl.AddDataGridTab(t.Name);
                dataGrid.AssignDataTable(t);
            }


        }
    }
}   