using Ara3D.Utils;
using System.Diagnostics;
using System.IO.Compression;
using Ara3D.BimOpenSchema;
using Ara3D.BimOpenSchema.IO;
using Ara3D.BimOpenSchema.Tests;
using Ara3D.DataTable;

namespace Ara3D.BIMOpenSchema.Tests
{
    public static class Tests
    {
        public class SerializationStats
        {
            public TimeSpan Elapsed;
            public string Path;
            public long Size;
        }

        public static T Read<T>(Func<FilePath, T> f, FilePath fp, string description, out SerializationStats stats)
        {
            var sw = Stopwatch.StartNew();
            var r = f(fp);
            stats = new SerializationStats()
            {
                Path = fp,
                Elapsed = sw.Elapsed,
                Size = fp.GetFileSize(),
            };
            return r;
        }

        public static SerializationStats Write<T>(T value, Action<FilePath, T> f, FilePath fp)
        {
            var sw = Stopwatch.StartNew();
            f(fp, value);
            return new SerializationStats()
            {
                Path = fp,
                Elapsed = sw.Elapsed,
                Size = fp.GetFileSize(),
            };
        }

        public static DirectoryPath InputFolder = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "..", "data", "input");
        public static DirectoryPath OutputFolder = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "..", "data", "output");

        public static FilePath InputFile => InputFolder.RelativeFile("snowdon.bimdata.parquet.zip");

        [Test]
        public static void TestInputFileExists()
        {
            Assert.IsTrue(InputFile.Exists());
            OutputFileDetails(InputFile);
        }

        public static BimData GetTestInputData()
            => InputFile.ReadBimDataFromParquetZip();

        public static void OutputFileDetails(FilePath fp)
        {
            Console.WriteLine($"File: {fp}");
            Console.WriteLine($"Has size: {fp.GetFileSizeAsString()}");
        }

        [Test]
        public static void TestReadInputFile()
        {
            var bd = GetTestInputData();
            OutputBimData(bd);
        }

        public static void TestWriteData(BimData data, string ext, Action<BimData, FilePath> writer)
        {
            var outputFile = InputFile.ChangeDirectoryAndExt(OutputFolder, ext);
            var sw = Stopwatch.StartNew();
            writer.Invoke(data, outputFile);
            var sz = outputFile.GetFileSizeAsString();
            Console.WriteLine($"Wrote {sz} to {outputFile.GetFileName()} in {sw.Elapsed.Seconds:F} seconds");
        }

        [Test]
        public static void TestWriter()
        {
            var sw = Stopwatch.StartNew();
            var bimData = GetTestInputData();
            Console.WriteLine($"Loaded {InputFile.GetFileSizeAsString()} of BIM data in {sw.Elapsed.Seconds:F} seconds");

            TestWriteData(bimData, "duckdb", (bd, f) => bd.WriteDuckDB(f));
            TestWriteData(bimData, "xlsx", (bd, f) => bd.WriteToExcel(f));
            TestWriteData(bimData, "json", (bd, f) => bd.WriteToJson(f, true, false));
            TestWriteData(bimData, "json.zip", (bd, f) => bd.WriteToJson(f, true, true));
            TestWriteData(bimData, "parquet.zip", (bd, f) => bd.WriteToParquetZip(f));
        }

        public static void OutputBimData(BimData bd)
        {
            Console.WriteLine($"# documents = {bd.Documents.Count}");
            Console.WriteLine($"# entities = {bd.Entities.Count}");
            Console.WriteLine($"# descriptors = {bd.Descriptors.Count}");
            Console.WriteLine($"# points = {bd.Points.Count}");
            Console.WriteLine($"# string = {bd.Strings.Count}");
            Console.WriteLine($"# string parameters = {bd.StringParameters.Count}");
            Console.WriteLine($"# point parameters  = {bd.PointParameters.Count}");
            Console.WriteLine($"# integer parameters = {bd.IntegerParameters.Count}");
            Console.WriteLine($"# double parameters = {bd.DoubleParameters.Count}");
            Console.WriteLine($"# entity parameters = {bd.EntityParameters.Count}");
            Console.WriteLine($"# relations = {bd.Relations.Count}");
        }

        public static void TestParameterStatistics()
        {
            var bimData = GetTestInputData();
            var d = bimData.GetStatistics();
            var stats = d.Values.OrderBy(ps => ps.Index).ToList();
            var dt = stats.ToDataTable("parameters");
            var outputFile = OutputFolder.RelativeFile("parameters.xlsx");
            dt.WriteToExcel(outputFile);
        }

        [Test]
        public static void BimDataObjectModel()
        {
            var bimData = GetTestInputData();
            var model = new BimDataModel(bimData);
            Console.WriteLine($"# documents = {model.Documents.Count}");
            Console.WriteLine($"# entities = {model.Entities.Count}");
            Console.WriteLine($"# descriptors = {model.Descriptors.Count}");
            var cats = model.Entities
                .Select(e => e.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
            foreach (var cat in cats)
                Console.WriteLine($"Category: {cat}");
        }

        public static void OutputDataSet(IDataSet set)
        {
            Console.WriteLine($"# tables = {set.Tables.Count}");
            foreach (var t in set.Tables)
            {
                Console.WriteLine($"Table {t.Name} # columns = {t.Columns.Count}, # rows = {t.Rows.Count}");
                for (var i = 0; i < t.Columns.Count; i++)
                {
                    var cd = t.Columns[i].Descriptor;
                    Console.WriteLine($" Column {i} = {cd.Name} {cd.Type}");
                }
            }
        }
    }
}