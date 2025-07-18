using System.IO.Compression;
using Ara3D.DataTable;
using Ara3D.Utils;
using Parquet;
using Parquet.Data;
using Parquet.Schema;

namespace Ara3D.BimOpenSchema.IO;

public static class ParquetUtils
{
    public static async Task WriteParquetAsync(
        this IDataTable table,
        FilePath filePath,
        CompressionLevel level = CompressionLevel.Optimal,
        CompressionMethod method = CompressionMethod.Brotli)
    {
        await using var fs = File.Create(filePath);
        await table.WriteParquetAsync(fs, level, method);
    }

    public static async Task WriteParquetAsync(
        this IDataTable table,
        Stream stream,
        CompressionLevel level = CompressionLevel.Optimal,
        CompressionMethod method = CompressionMethod.Brotli)
    {
        var dataFields = table.Columns.Select(c => new DataField(c.Descriptor.Name, c.Descriptor.Type)).ToList();
        var schema = new ParquetSchema(dataFields);

        await using var writer = await ParquetWriter.CreateAsync(schema, stream);
        writer.CompressionLevel = level;
        writer.CompressionMethod = method;
        using var rg = writer.CreateRowGroup();
        foreach (var c in table.Columns)
        {
            var df = dataFields[c.ColumnIndex];
            var array = Array.CreateInstance(c.Descriptor.Type, c.Count);
            for (var i = 0; i < c.Count; i++)
                array.SetValue(c[i], i);
            var dc = new DataColumn(df, array);
            await rg.WriteColumnAsync(dc);
        }
    }

   public static async Task WriteParquetToZipAsync(
        this IDataSet set,
        string zipPath,
        CompressionMethod method = CompressionMethod.Brotli,
        CompressionLevel level = CompressionLevel.Optimal)
    {
        await using var fs = new FileStream(zipPath, FileMode.Create, FileAccess.Write, FileShare.None);
        using var zip = new ZipArchive(fs, ZipArchiveMode.Create, leaveOpen: false);

        for (var i = 0; i < set.Tables.Count; ++i)
        {
            var table = set.Tables[i];
            var entryName = $"{table.Name}.parquet";
            var entry = zip.CreateEntry(entryName, level);
            await using var parquetBuffer = new MemoryStream();
            await table.WriteParquetAsync(parquetBuffer, level, method);
            parquetBuffer.Position = 0;
            await using var entryStream = entry.Open();
            await parquetBuffer.CopyToAsync(entryStream);
        }
    }

    public static async Task<IDataTable> ReadParquet(this FilePath filePath, string? name = null)
    {
        name ??= filePath.GetFileNameWithoutExtension();
        var reader = await ParquetReader.CreateAsync(filePath);
        var parquetColumns = await reader.ReadEntireRowGroupAsync();
        var araColumns = Enumerable.ToList<ParquetColumnAdpater>(parquetColumns.Select((c, i) => new ParquetColumnAdpater(c, i)));
        return new ReadOnlyDataTable(name, araColumns);
    }

    public static async Task<IDataTable> ReadParquet(this Stream stream, string name)
    {
        var reader = await ParquetReader.CreateAsync(stream);
        var parquetColumns = await reader.ReadEntireRowGroupAsync();
        var araColumns = Enumerable.ToList<ParquetColumnAdpater>(parquetColumns.Select((c, i) => new ParquetColumnAdpater(c, i)));
        return new ReadOnlyDataTable(name, araColumns);
    }

    /// <summary>
    /// Reads every "*.parquet" entry from <paramref name="zipPath"/>
    /// and returns them as a list of tables.
    /// </summary>
    public static async Task<IDataSet> ReadParquetFromZipAsync(this FilePath zipPath)
    {
        var tables = new List<IDataTable>();

        await using var fs = new FileStream(zipPath, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var zip = new ZipArchive(fs, ZipArchiveMode.Read, leaveOpen: false);

        foreach (var entry in zip.Entries
                     .Where(e => e.Name.EndsWith(".parquet", StringComparison.OrdinalIgnoreCase))
                     .OrderBy(e => e.FullName))
        {
            await using var entryStream = entry.Open();
            await using var ms = new MemoryStream();
            await entryStream.CopyToAsync(ms);

            ms.Position = 0;
            var table = await ReadParquet(ms, Path.GetFileNameWithoutExtension(entry.Name));
            tables.Add(table);
        }

        return tables.ToDataSet();
    }

    public class ParquetColumnAdpater : IDataColumn
    {
        public DataColumn Column;

        public ParquetColumnAdpater(DataColumn dc, int index)
        {
            Column = dc;
            ColumnIndex = index;
            Descriptor = new DataDescriptor(dc.Field.Name, dc.Field.ClrType, index);
            Count = Column.NumValues;
        }

        public int ColumnIndex { get; }
        public IDataDescriptor Descriptor { get; }
        public int Count { get; }
        public object this[int n] => Column.Data.GetValue(n);
        public Array AsArray() => Column.Data;
    }
}