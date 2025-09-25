using Autodesk.Revit.UI;
using System;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Ara3D.BimOpenSchema;
using Ara3D.BimOpenSchema.IO;
using Ara3D.Utils;
using FilePath = Autodesk.Revit.DB.FilePath;

namespace Ara3D.BIMOpenSchema.Revit2025;

public class CommandExportBIMOpenSchema 
{
    public string Name => "Export BIM Open Schema";

    public void Execute(object arg)
    {
        ExportData(arg as UIApplication);
    }

    public void ExportData(UIApplication app)
    {
        var uiDoc = app.ActiveUIDocument;
        var doc = uiDoc.Document;

        var timer = Stopwatch.StartNew();
        var builder = new RevitToOpenBimSchema(doc, true, true);

        var processingTime = timer.Elapsed;
        timer.Restart();

        var bimData = builder.bdb.Data;
        var dataSet = bimData.ToDataSet();
        var buildTime = timer.Elapsed;

        var fp = Path.ChangeExtension(doc.PathName, "bimdata.parquet.zip");
        Task.Run(() => dataSet.WriteParquetToZipAsync(fp)).GetAwaiter().GetResult();
        OutputData(bimData, processingTime, buildTime, fp);
    }

    public static void OutputData(BimData bd, TimeSpan processingTime, TimeSpan buildTime, Ara3D.Utils.FilePath fp)
    {
        var text = $"Processed {bd.Documents.Count:N1} documents\r\n" +
                   $"{bd.Entities.Count:N1} entities\r\n" +
                   $"{bd.Descriptors.Count:N1} descriptors\r\n" +
                   $"{bd.IntegerParameters.Count:N1} integer parameters\r\n" +
                   $"{bd.DoubleParameters.Count:N1} double parameters\r\n" +
                   $"{bd.EntityParameters.Count:N1} entity parameters\r\n" +
                   $"{bd.StringParameters.Count:N1} string parameters\r\n" +
                   $"{bd.PointParameters.Count:N1} point parameters\r\n" +
                   $"{bd.Points.Count:N1} points\r\n" +
                   $"{bd.Strings.Count:N1} strings\r\n" +
                   $"{bd.Relations.Count:N1} relations\r\n" +
                   $"Processing took {processingTime.TotalSeconds:F} seconds\r\n" + 
                   $"Building took {buildTime.TotalSeconds:F} seconds\r\n" +
                   $"Output size was {fp.GetFileSizeAsString()}\r\n" +
                   $"File is {fp}";

        MessageBox.Show(text);
    }
}