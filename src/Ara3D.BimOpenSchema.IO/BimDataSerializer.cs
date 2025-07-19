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


}