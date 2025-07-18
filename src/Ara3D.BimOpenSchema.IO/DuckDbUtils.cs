using Ara3D.DataTable;
using Ara3D.Utils;
using DuckDB.NET.Data;

namespace Ara3D.BimOpenSchema.IO;

public static class DuckDbWriter
{
    public static void WriteToDuckDB(this IDataSet set, FilePath fileName)
    {
        using var conn = new DuckDBConnection($"DataSource={fileName}");
        conn.Open();
        var id = 0;
        foreach (var t in set.Tables)
            WriteTable(conn, t, $"{t.Name}_{id++}");
    }

    public static void WriteTable(
        this DuckDBConnection conn,
        IDataTable table,
        string tableName)
    {
        var sqlColumns = string.Join(", ",
            table.Columns.Select(c => $"\"{c.Descriptor.Name}\" {ToDuckType(c.Descriptor.Type)}"));

        // Create table if needed
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText =
                $"CREATE TABLE IF NOT EXISTS \"{tableName}\" ({sqlColumns});";
            cmd.ExecuteNonQuery();
        }

        // Bulk-insert with Appender
        using var app = conn.CreateAppender(tableName);
        
        foreach (var row in table.Rows)
        {
            var appRow = app.CreateRow();
            foreach (var c in table.Columns)
                Append(appRow, row[c.ColumnIndex], c.Descriptor.Type);
            appRow.EndRow();                       
        }
    }

    private static string ToDuckType(Type t)
        => t.IsEnum ? "INTEGER"
            : t == typeof(byte) ? "UTINYINT"
            : t == typeof(sbyte) ? "TINYINT"
            : t == typeof(short) ? "SMALLINT"
            : t == typeof(ushort) ? "USMALLINT"
            : t == typeof(int) ? "INTEGER"
            : t == typeof(uint) ? "UINTEGER"
            : t == typeof(long) ? "BIGINT"
            : t == typeof(ulong) ? "UBIGINT"
            : t == typeof(float) ? "FLOAT"
            : t == typeof(double) ? "DOUBLE"
            : t == typeof(decimal) ? "DECIMAL(38,9)"
            : t == typeof(bool) ? "BOOLEAN"
            : t == typeof(DateTime) ? "TIMESTAMP"
            : t == typeof(Guid) ? "UUID"
            : "VARCHAR"; // fall-back

    private static void Append(IDuckDBAppenderRow row, object? v, Type t)
    {
        if (v is null) { row.AppendNullValue(); return; }
        if (t.IsEnum) t = Enum.GetUnderlyingType(t); 

        switch (Type.GetTypeCode(t))
        {
            case TypeCode.String: row.AppendValue((string)v); break;
            case TypeCode.Boolean: row.AppendValue((bool)v); break;
            case TypeCode.Byte: row.AppendValue((byte)v); break;
            case TypeCode.SByte: row.AppendValue((sbyte)v); break;
            case TypeCode.Int16: row.AppendValue((short)v); break;
            case TypeCode.UInt16: row.AppendValue((ushort)v); break;
            case TypeCode.Int32: row.AppendValue((int)v); break;
            case TypeCode.UInt32: row.AppendValue((uint)v); break;
            case TypeCode.Int64: row.AppendValue((long)v); break;
            case TypeCode.UInt64: row.AppendValue((ulong)v); break;
            case TypeCode.Single: row.AppendValue((float)v); break;
            case TypeCode.Double: row.AppendValue((double)v); break;
            case TypeCode.Decimal: row.AppendValue((decimal)v); break;
            case TypeCode.DateTime: row.AppendValue((DateTime)v); break;
            default:
                if (t == typeof(Guid)) row.AppendValue((Guid)v);
                else row.AppendValue(v.ToString());
                break;
        }
    }
}