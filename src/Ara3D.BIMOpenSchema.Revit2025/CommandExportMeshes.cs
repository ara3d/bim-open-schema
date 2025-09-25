using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows;
using Ara3D.BimOpenSchema;
using Ara3D.BimOpenSchema.IO;
using Ara3D.Utils;
using Autodesk.Revit.UI;
using Parquet;

namespace Ara3D.BIMOpenSchema.Revit2025;

public class CommandExportMeshes
{
    public string Name => "Export BIM Open Geometry";

    public UIApplication app { get; private set; }

    public void Execute(object arg)
    {
        app = (arg as UIApplication);

        if (app == null)
            throw new Exception($"Passed argument {arg} is either null or not a UI application");

        var uiDoc = app.ActiveUIDocument;
        var doc = uiDoc.Document;

        var timer = Stopwatch.StartNew();

        var bldr = new RevitModelBuilder();
        bldr.ProcessDocument(doc);

        var processingTime = timer.Elapsed;
        timer.Restart();

        var filePath = new FilePath(doc.PathName);
        var outputFilePath = filePath.ChangeDirectoryAndExt(SpecialFolders.MyDocuments, "geometry.parquet.zip");
        var model = bldr.Build();
        var buildTime = timer.Elapsed;

        timer.Restart();
        model.WriteParquetZip(outputFilePath, CompressionMethod.Brotli, CompressionLevel.Optimal);

        var serializationTime = timer.Elapsed;

        var totalSize = outputFilePath.GetFileSize();

        var text = "Just created a geometry representation using Parquet\r\n" +
                   $"# meshes = {model.NumMeshes():N0}\r\n" +
                   $"# elements = {model.NumElements():N0}\r\n" +
                   $"# materials = {model.NumMaterials():N0}\r\n" +
                   $"# vertices = {model.NumVertices():N0}\r\n" +
                   $"# faces = {model.NumFaces():N0}\r\n" + 
                   $"# transforms = {model.NumTransforms():N0}\r\n" +
                   $"Processing took {processingTime.TotalSeconds:F} seconds\r\n" +
                   $"Building took {buildTime.TotalSeconds:F} seconds\r\n" +
                   $"Serialization took {serializationTime.TotalSeconds:F} seconds\r\n" +
                   $"Generated file size is {totalSize:N0}\r\n" +
                   $"File saved as {outputFilePath}";

        outputFilePath.GetDirectory().OpenFolderInExplorer();
        MessageBox.Show(text);
    }

    public int CountMeshes(MeshGroup g)
        => g.Meshes.Count + g.Children.Sum(CountMeshes);
}