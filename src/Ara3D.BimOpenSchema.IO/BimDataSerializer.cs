using System.IO;
using System.IO.Compression;
using System.Text.Json;
using System.Threading.Tasks;
using Ara3D.Utils;

namespace Ara3D.BimOpenSchema.IO;

public static class BimDataSerializer
{
    public static BimData ReadBimDataFromJsonZip(this FilePath fp)
        => ReadBimDataFromJson(new GZipStream(fp.OpenRead(), CompressionMode.Decompress));

    public static async Task<BimData> ReadBimDataFromJsonZipAsync(this FilePath fp)
        => await ReadBimDataFromJsonAsync(new GZipStream(fp.OpenRead(), CompressionMode.Decompress));

    public static BimData ReadBimDataFromJson(this FilePath fp)
        => ReadBimDataFromJson(fp.OpenRead());

    public static async Task<BimData> ReadBimDataFromJsonAsync(this FilePath fp)
        => await ReadBimDataFromJsonAsync(fp.OpenRead());

    public static BimData ReadBimDataFromJson(this Stream stream)
        => JsonSerializer.Deserialize<BimData>(stream);

    public static async Task<BimData> ReadBimDataFromJsonAsync(this Stream stream)
        => await JsonSerializer.DeserializeAsync<BimData>(stream);

    public static async Task<BimData> ReadBimDataFromParquetZipAsync(this FilePath fp)
        => (await fp.ReadParquetFromZipAsync()).ToBimData();

    public static BimData ReadBimDataFromParquetZip(this FilePath fp)
        => Task.Run(fp.ReadBimDataFromParquetZipAsync).GetAwaiter().GetResult();

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

    public static Task WriteToParquetZipAsync(this BimData data, FilePath fp)
        => data.ToDataSet().WriteParquetToZipAsync(fp);

    public static void WriteToParquetZip(this BimData data, FilePath fp)
        => Task.Run(() => data.WriteToParquetZipAsync(fp)).GetAwaiter().GetResult();
}