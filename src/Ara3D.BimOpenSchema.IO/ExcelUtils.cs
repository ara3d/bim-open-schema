using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Ara3D.DataTable;
using Ara3D.Utils;
using ClosedXML.Excel;

namespace Ara3D.BimOpenSchema.IO
{
    public static class ExcelUtils
    {
        /// <summary>   
        /// Saves every DataTable in a DataSet to an .xlsx file – one worksheet per table.
        /// </summary>
        /// <param name="set">The DataSet to export.</param>
        /// <param name="filePath">Full path (including .xlsx extension) for the new workbook.</param>
        public static void WriteToExcel(this DataSet set, string filePath)
        {
            if (set == null) throw new ArgumentNullException(nameof(set));
            if (set.Tables.Count == 0) throw new ArgumentException("DataSet contains no tables.", nameof(set));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath))!);

            using var wb = new XLWorkbook();
            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < set.Tables.Count; i++)
            {
                var table = set.Tables[i];
                var sheetName = MakeValidSheetName(table.TableName, i + 1, usedNames);
                var ws = wb.Worksheets.Add(sheetName);

                // ClosedXML’s InsertTable copies headers, formats nicely, and is very fast.
                ws.Cell(1, 1).InsertTable(table, true);
            }

            wb.SaveAs(filePath);   
        }

        public static void WriteToExcel(this IDataTable table, FilePath filePath)
            => new ReadOnlyDataSet([table]).WriteToExcel(filePath);

        /// <summary>
        /// Saves every DataTable in a DataSet to an .xlsx file – one worksheet per table.
        /// </summary>
        /// <param name="set">The DataSet to export.</param>
        /// <param name="filePath">Full path (including .xlsx extension) for the new workbook.</param>
        public static void WriteToExcel(this IDataSet set, FilePath filePath)
        {
            if (set == null) throw new ArgumentNullException(nameof(set));
            if (set.Tables.Count == 0) throw new ArgumentException("DataSet contains no tables.", nameof(set));
            if (string.IsNullOrWhiteSpace(filePath)) throw new ArgumentNullException(nameof(filePath));

            Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(filePath))!);

            using var wb = new XLWorkbook();
            var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (var i = 0; i < set.Tables.Count; i++)
            {
                var table = set.Tables[i];
                var sheetName = MakeValidSheetName(table.Name, i + 1, usedNames);
                var ws = wb.Worksheets.Add(sheetName);

                ws.FirstCell().InsertTable(table.GetDataRecords(), true);
            }

            wb.SaveAs(filePath);   // Atomic save; will overwrite if the file exists
        }

        // Excel sheet names: ≤31 chars, no : \ / ? * [ ]
        private static string MakeValidSheetName(string? original, int ordinal, HashSet<string> used)
        {
            var name = string.IsNullOrWhiteSpace(original) ? $"Table{ordinal}" : original;
            name = Regex.Replace(name, @"[:\\/?*\[\]]", "_");
            name = name.Length > 31 ? name.Substring(0, 31) : name;

            // ensure uniqueness
            var finalName = name;
            int suffix = 1;
            while (!used.Add(finalName))
                finalName = (name.Length > 28 ? name[..28] : name) + "_" + suffix++;

            return finalName;
        }
    }
}
