using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
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

        foreach (var table in set.Tables)
        {
            var entryName = $"{table.Name}.parquet";
            var entry = zip.CreateEntry(entryName, level);
            await using var parquetBuffer = new MemoryStream();
            await table.WriteParquetAsync(parquetBuffer, level, method);
            parquetBuffer.Position = 0;
            await using var entryStream = entry.Open();
            await parquetBuffer.CopyToAsync(entryStream);
        }
    }

    public static async Task<IDataTable> ReadParquetAsync(this FilePath filePath, string? name = null)
    {
        name ??= filePath.GetFileNameWithoutExtension();
        var reader = await ParquetReader.CreateAsync(filePath);
        var parquetColumns = await reader.ReadEntireRowGroupAsync();
        var araColumns = parquetColumns.Select((c, i) => new ParquetColumnAdapter(c, i)).ToList();
        return new ReadOnlyDataTable(name, araColumns);
    }

    public static async Task<IDataTable> ReadParquetAsync(this Stream stream, string name)
    {
        var reader = await ParquetReader.CreateAsync(stream);
        var parquetColumns = await reader.ReadEntireRowGroupAsync();
        var araColumns = parquetColumns.Select((c, i) => new ParquetColumnAdapter(c, i)).ToList();
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
            var table = await ReadParquetAsync(ms, Path.GetFileNameWithoutExtension(entry.Name));
            tables.Add(table);
        }

        return tables.ToDataSet();
    }

    public class ParquetColumnAdapter : IDataColumn
    {
        public DataColumn Column;

        public ParquetColumnAdapter(DataColumn dc, int index)
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


    public static void WriteParquetZip(this BimGeometry bg, FilePath file,
        CompressionMethod method = CompressionMethod.Brotli, CompressionLevel level = CompressionLevel.Optimal)
        => Task.Run(() => WriteParquetZipAsync(bg, file, method, level)).GetAwaiter().GetResult();

    public static async Task WriteParquetZipAsync(this BimGeometry bg, FilePath file, CompressionMethod method = CompressionMethod.Brotli, CompressionLevel level = CompressionLevel.Optimal)
    {
        var builders = bg.ToParquet();
        await using var fs = new FileStream(file, FileMode.Create, FileAccess.Write, FileShare.None);
        using var zip = new ZipArchive(fs, ZipArchiveMode.Create, leaveOpen: false);
        foreach (var builder in builders)
        {
            var entryName = $"{builder.Name}.parquet";
            // Quickly compress data
            var entry = zip.CreateEntry(entryName, CompressionLevel.Fastest);
            await using var parquetBuffer = new MemoryStream();
            await builder.SaveToStream(parquetBuffer, method, level);
            parquetBuffer.Position = 0;
            await using var entryStream = entry.Open();
            await parquetBuffer.CopyToAsync(entryStream);
        }
    }

    public static List<ParquetBuilder> ToParquet(this BimGeometry bg)
    {
        var r = new List<ParquetBuilder>();
        {
            var pb = new ParquetBuilder("Material");
            pb.Add(bg.MaterialRed, nameof(bg.MaterialRed));
            pb.Add(bg.MaterialGreen, nameof(bg.MaterialGreen));
            pb.Add(bg.MaterialBlue, nameof(bg.MaterialBlue));
            pb.Add(bg.MaterialAlpha, nameof(bg.MaterialAlpha));
            pb.Add(bg.MaterialMetallic, nameof(bg.MaterialRoughness));
            pb.Add(bg.MaterialRoughness, nameof(bg.MaterialMetallic));
            r.Add(pb);
        }
        {
            var pb = new ParquetBuilder("Vertex");
            pb.Add(bg.VertexXData, nameof(bg.VertexXData));
            pb.Add(bg.VertexYData, nameof(bg.VertexYData));
            pb.Add(bg.VertexZData, nameof(bg.VertexZData));
            r.Add(pb);
        }
        {
            var pb = new ParquetBuilder("Index");
            pb.Add(bg.IndexData, nameof(bg.IndexData));
            r.Add(pb);
        }
        {
            var pb = new ParquetBuilder("Element");
            pb.Add(bg.ElementEntityIndices, nameof(bg.ElementEntityIndices));
            pb.Add(bg.ElementMaterialIndices, nameof(bg.ElementMaterialIndices));
            pb.Add(bg.ElementMeshIndices, nameof(bg.ElementMeshIndices));
            pb.Add(bg.ElementTransformIndices, nameof(bg.ElementTransformIndices));
            r.Add(pb);
        }
        {
            var pb = new ParquetBuilder("Mesh");
            pb.Add(bg.MeshIndexOffset, nameof(bg.MeshIndexOffset));
            pb.Add(bg.MeshVertexOffset, nameof(bg.MeshVertexOffset));
            r.Add(pb);
        }
        {
            var pb = new ParquetBuilder("Transform");
            pb.Add(bg.TransformData, nameof(bg.TransformData));
            r.Add(pb);
        }

        return r;
    }
}