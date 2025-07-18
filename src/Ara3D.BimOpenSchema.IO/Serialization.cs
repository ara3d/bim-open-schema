using System.Diagnostics;
using System.IO.Compression;
using System.Text.Json;
using Ara3D.DataTable;
using Ara3D.Utils;
using BIMOpenSchema;

namespace Ara3D.BimOpenSchema.IO;
    
public class SerializationStats
{
    public TimeSpan Elapsed;
    public string Path;
    public long Size;
}

public static class Serialization
{
  
    public static BIMData LoadBimDataFromJsonZip(FilePath fp)
        => LoadBimDataFromJson(new GZipStream(fp.OpenRead(), CompressionMode.Decompress));

    public static BIMData LoadBimDataFromJson(FilePath fp)
        => LoadBimDataFromJson(fp.OpenRead());

    public static BIMData LoadBimDataFromJson(Stream stream)
        => JsonSerializer.Deserialize<BIMData>(stream);
    
    public static void WriteBIMDataToJson(BIMData data, FilePath fp, bool withIndenting, bool withZip)
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

    public static void WriteDuckDB(BIMData data, FilePath fp)
        => data.ToDataSet().WriteToDuckDB(fp);

    public static void WriteToExcel(BIMData data, FilePath fp)
        => data.ToDataSet().WriteToExcel(fp);

    public static void AddTable<T>(IDataSet set, List<T> list, string name)
    {
        var table = set.GetTable(name);
        if (table == null)
            throw new Exception($"Could not find table {name}");
        var vals = table.ToArray<T>();
        list.AddRange(vals);
    }

    public static BIMData ToBimData(this IDataSet set)
    {
        var r = new BIMData();
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