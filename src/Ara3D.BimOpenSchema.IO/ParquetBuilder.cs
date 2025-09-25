using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Ara3D.Utils;
using Parquet;
using Parquet.Data;
using Parquet.Schema;

namespace Ara3D.BimOpenSchema.IO;

public class ParquetBuilder
{
    public ParquetBuilder(string name)
        => Name = name;

    public readonly string Name;
    public readonly List<Array> Arrays = new();
    public readonly List<DataField> Fields = new();

    public void Add<T>(IEnumerable<T> data, string name)
    {
        var df = new DataField(name, typeof(T));
        Arrays.Add(data.ToArray());
        Fields.Add(df);
    }

    public ParquetSchema BuildSchema()
        => new ParquetSchema(Fields);
    
    public IEnumerable<DataColumn> GetColumns()
        => Fields.Select((t, i) => new DataColumn(t, Arrays[i]));

    public async Task SaveToFile(FilePath filePath, CompressionMethod method, CompressionLevel level)
    {
        await using var stream = filePath.OpenWrite();
        await SaveToStream(stream, method, level);
    }

    public async Task SaveToStream(Stream stream, CompressionMethod method, CompressionLevel level)
    {
        var schema = BuildSchema();
        await using var writer = await ParquetWriter.CreateAsync(schema, stream);
        writer.CompressionLevel = level;
        writer.CompressionMethod = method;
        var rg = writer.CreateRowGroup();
        var columns = GetColumns().ToList();
        foreach (var c in columns)
            await rg.WriteColumnAsync(c);
    }
}