using Ara3D.DataSetBrowser.WPF;
using Ara3D.Utils;
using BIMOpenSchema;
using System.Diagnostics;
using System.IO.Compression;
using Ara3D.BimOpenSchema;
using Ara3D.BimOpenSchema.IO;
using Ara3D.DataTable;

namespace Ara3D.BIMOpenSchema.Tests
{
    public static class Tests
    {
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

        public static DirectoryPath InputFolder = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "input");
        public static DirectoryPath OutputFolder = PathUtil.GetCallerSourceFolder().RelativeFolder("..", "output");

        [Test]
        public static void TestDuckDB()
        {
            var outputFile = OutputFolder.RelativeFile("bimdata.duckdb");
            var data = GetData();
            var stats = Write(data,
                (fp, bd) => Serialization.WriteDuckDB(bd, fp), outputFile);
            OutputStats(data, stats);
        }

        [Test]
        public static void TestParquet()
        {
            var outputFolder = OutputFolder.RelativeFolder("parquet");
            outputFolder.CreateAndClearDirectory();
            var sw = Stopwatch.StartNew();
            var data = GetData();
            var loadTime = sw.Elapsed;
            sw.Restart();
            var dataSet = data.ToDataSet();
            var toDataSetTime = sw.Elapsed;
            var tasks = dataSet.Tables.Select((t, i) => t.WriteParquetAsync(outputFolder.RelativeFile($"{t.Name}.parquet")));
            Task.WaitAll(tasks);
            var writeTime = sw.Elapsed;
            Console.WriteLine($"Loading in {loadTime.Seconds:F} seconds");
            Console.WriteLine($"Conversion in {toDataSetTime.Seconds:F} seconds");
            Console.WriteLine($"Writing in {writeTime.Seconds:F} seconds");
            Console.WriteLine($"Total size of files = {PathUtil.BytesToString(outputFolder.GetDirectorySizeInBytes())}");
        }

        [Test]
        public static void TestExcel()
        {
            var outputFile = OutputFolder.RelativeFile("bimdata.xlsx");
            var data = GetData();
            var stats = Write(data,
                (fp, bd) => Serialization.WriteToExcel(bd, fp), outputFile);
            OutputStats(data, stats);
        }

        [Test]
        public static void TestJsonWithZip()
        {
            var outputFile = OutputFolder.RelativeFile("bimdata.json.zip");
            var data = GetData();
            var stats = Write(data, 
                (fp, bd) => Serialization.WriteBIMDataToJson(bd, fp, true, true), outputFile);
            OutputStats(data, stats);
        }

        [Test]
        public static void TestJson()
        {
            var outputFile = OutputFolder.RelativeFile("bimdata.json");
            var data = GetData();
            var stats = Write(data, 
                (fp, bd) => Serialization.WriteBIMDataToJson(bd, fp, true, false), outputFile);
            OutputStats(data, stats);
        }

        public static void OutputBimData(BIMData bd)
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

        public static void OutputStats(BIMData bd, SerializationStats stats)
        {
            OutputBimData(bd);
            Console.WriteLine($"Wrote {PathUtil.BytesToString(stats.Size)}");
            Console.WriteLine($"Took {stats.Elapsed.Seconds:F} seconds");
            Console.WriteLine($"File name is {stats.Path}");
        }

        public static BIMData GetData()
        {
            throw new NotImplementedException();
        }

        [Test]
        public static async Task TestReadParquet()
        {
            var inputFolder = OutputFolder.RelativeFolder("parquet");
            var tableTasks = inputFolder.GetFiles().Select(f => f.ReadParquet());
            var tables = (await Task.WhenAll(tableTasks)).ToList();
            var dataSet = new ReadOnlyDataSet(tables);
            OutputDataSet(dataSet);
        }

        [Test]
        public static async Task TestReadWriteParquetZip()
        {
            var dataSet = await ReadParquetDataSet();
            OutputDataSet(dataSet);
            var outputFile = OutputFolder.RelativeFile("bimdata.parquet.zip");
            await dataSet.WriteParquetToZipAsync(outputFile);
        }

        public static async Task<IDataSet> ReadParquetDataSet()
        {
            var inputFolder = OutputFolder.RelativeFolder("parquet");
            var tableTasks = inputFolder.GetFiles().Select(f => f.ReadParquet());
            var tables = (await Task.WhenAll(tableTasks)).ToList();
            return new ReadOnlyDataSet(tables);
        }
        
        [Test]
        public static async Task TestParameterStatistics()
        {
            var dataSet = await ReadParquetDataSet();
            var bimData = dataSet.ToBimData();
            OutputBimData(bimData);
            var d = bimData.GetStatistics();
            var stats = d.Values.OrderBy(ps => ps.Index).ToList();
            var dt = stats.ToDataTable("parameters");

            var outputFile = OutputFolder.RelativeFile("parameters.xlsx");
            dt.WriteToExcel(outputFile);
        }

        [Test]
        public static async Task BimDataExpanded()
        {
            var dataSet = await ReadParquetDataSet();
            var bimData = dataSet.ToBimData();
            var expBimData = new ExpandedBIMData(bimData);

            var entityTable = expBimData.Entities.ToDataTable("entities");
            entityTable.WriteToExcel(OutputFolder.RelativeFile("expanded-entities.xlsx"));
            
            var paramTable = expBimData.Parameters.ToDataTable("parameters");
            paramTable.WriteToExcel(OutputFolder.RelativeFile("expanded-parameters.xlsx"));
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