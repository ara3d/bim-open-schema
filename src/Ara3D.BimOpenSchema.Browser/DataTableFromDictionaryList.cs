using Ara3D.DataTable;

namespace Ara3D.BimOpenSchema.Browser;

public class DataTableFromDictionaryList : IDataTable
{
    public class PseudoRow : IDataRow
    {
        public DataTableFromDictionaryList Parent;
        public int RowIndex { get; init; }
        public IDataTable DataTable => Parent;
        public IReadOnlyList<object> Values => Parent.Columns.Select(c => c[RowIndex]).ToList().AsReadOnly();
        public object this[int index] => Parent.Columns[index][RowIndex];

        public PseudoRow(DataTableFromDictionaryList parent, int index)
        {
            Parent = parent;
            RowIndex = index;
        }
    }

    public class PseudoColumn : IDataColumn
    {
        public DataTableFromDictionaryList Parent;
        public string Name => Descriptor.Name;

        public PseudoColumn(DataTableFromDictionaryList parent, string name, int index)
        {
            Parent = parent;
            ColumnIndex = index;
            Descriptor = new DataDescriptor(name, typeof(object), index);
        }

        public Array AsArray()
            => Enumerable.Range(0, Count).Select(i => this[i])
                .ToArray();

        public int ColumnIndex { get; }
        public IDataDescriptor Descriptor { get; }
        public int Count => Parent.Dictionaries.Count;

        public object this[int n]
            => Parent.Dictionaries[n].GetValueOrDefault(Name);
    }

    private IReadOnlyList<Dictionary<string, object>> Dictionaries { get; }
    public Dictionary<string, PseudoColumn> ColumnLookup { get; } = new();

    public DataTableFromDictionaryList(IReadOnlyList<Dictionary<string, object>> dictionaries, string name)
    {
        Dictionaries = dictionaries;
        foreach (var dict in dictionaries)
        {
            foreach (var kvp in dict)
            {
                if (!ColumnLookup.ContainsKey(kvp.Key))
                {
                    ColumnLookup[kvp.Key] = new PseudoColumn(this, kvp.Key, ColumnLookup.Count);
                }
            }
        }
        Columns = ColumnLookup.Values.ToList().AsReadOnly();
        Rows = dictionaries
            .Select((_, index) => new PseudoRow(this, index))
            .ToList()
            .AsReadOnly();
        Name = name;
    }

    public string Name { get; }
    public IReadOnlyList<IDataRow> Rows { get; }
    public IReadOnlyList<IDataColumn> Columns { get; }

    public object this[int column, int row] => Columns[column][row];
}