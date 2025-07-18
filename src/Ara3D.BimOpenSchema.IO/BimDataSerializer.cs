using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using Ara3D.DataTable;
using Ara3D.Utils;

namespace Ara3D.BimOpenSchema.IO;

public static class BimDataSerializer
{
    public static BimData LoadBimDataFromJsonZip(this FilePath fp)
        => LoadBimDataFromJson(new GZipStream(fp.OpenRead(), CompressionMode.Decompress));

    public static BimData LoadBimDataFromJson(this FilePath fp)
        => LoadBimDataFromJson(fp.OpenRead());

    public static BimData LoadBimDataFromJson(this Stream stream)
        => JsonSerializer.Deserialize<BimData>(stream);
    
    public static void WriteToJson(this BimData data, FilePath fp, bool withIndenting, bool withZip)
    {
        using var stream = fp.OpenWrite();
        if (!withZip)
        {
            JsonSerializer.Serialize(stream, data, new JsonSerializerOptions() { WriteIndented = withIndenting });
        }
        else
        {
            var zipStream = new GZipStream(stream, CompressionMode.Compress);
            JsonSerializer.Serialize(zipStream, data, new JsonSerializerOptions() { WriteIndented = withIndenting });
        }
    }

    public static void WriteDuckDB(this BimData data, FilePath fp)
        => data.ToDataSet().WriteToDuckDB(fp);

    public static void WriteToExcel(this BimData data, FilePath fp)
        => data.ToDataSet().WriteToExcel(fp);

    public static void AddTable<T>(this IDataSet set, List<T> list, string name)
    {
        var table = set.GetTable(name);
        if (table == null)
            throw new Exception($"Could not find table {name}");
        var vals = table.ToArray<T>();
        list.AddRange(vals);
    }

    public static BimData ToBimData(this IDataSet set)
    {
        var r = new BimData();
        AddTable(set, r.Points, nameof(r.Points));
        AddTable(set, r.DoubleParameters, nameof(r.DoubleParameters));
        AddTable(set, r.EntityParameters, nameof(r.EntityParameters));
        AddTable(set, r.IntegerParameters, nameof(r.IntegerParameters));
        AddTable(set, r.PointParameters, nameof(r.PointParameters));
        AddTable(set, r.Relations, nameof(r.Relations));
        AddTable(set, r.StringParameters, nameof(r.StringParameters));
        AddTable(set, r.Strings, nameof(r.Strings));
        AddTable(set, r.Descriptors, nameof(r.Descriptors));
        AddTable(set, r.Documents, nameof(r.Documents));
        AddTable(set, r.Entities, nameof(r.Entities));
        return r;
    }
}