
using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.DataTable;

namespace Ara3D.BimOpenSchema;

public static class BimDataExtension
{
    public static string Get(this BimData self, StringIndex index) => self.Strings[(int)index];
    public static Entity Get(this BimData self, EntityIndex index) => self.Entities[(int)index];
    public static Document Get(this BimData self, DocumentIndex index) => self.Documents[(int)index];
    public static Point Get(this BimData self, PointIndex index) => self.Points[(int)index];
    public static EntityRelation Get(this BimData self, RelationIndex index) => self.Relations[(int)index];
    public static ParameterDescriptor Get(this BimData self, DescriptorIndex index) => self.Descriptors[(int)index];

    public static IEnumerable<EntityIndex> EntityIndices(this BimData self) 
        => Enumerable.Range(0, self.Entities.Count).Select(i => (EntityIndex)i);

    public static IEnumerable<DocumentIndex> DocumentIndices(this BimData self)
        => Enumerable.Range(0, self.Documents.Count).Select(i => (DocumentIndex)i);

    public static IEnumerable<DescriptorIndex> DescriptorIndices(this BimData self)
        => Enumerable.Range(0, self.Descriptors.Count).Select(i => (DescriptorIndex)i);

    public static IEnumerable<StringIndex> StringIndices(this BimData self)
        => Enumerable.Range(0, self.Strings.Count).Select(i => (StringIndex)i);

    public static IEnumerable<PointIndex> PointIndices(this BimData self)
        => Enumerable.Range(0, self.Points.Count).Select(i => (PointIndex)i);

    public static IDataSet ToDataSet(this BimData self)
        => new ReadOnlyDataSet([
            self.Points.ToDataTable(nameof(self.Points)),
            self.Strings.ToDataTable(nameof(self.Strings)),
            self.Descriptors.ToDataTable(nameof(self.Descriptors)),
            self.Documents.ToDataTable(nameof(self.Documents)),
            self.Entities.ToDataTable(nameof(self.Entities)),
            self.Relations.ToDataTable(nameof(self.Relations)),
            self.DoubleParameters.ToDataTable(nameof(self.DoubleParameters)),
            self.IntegerParameters.ToDataTable(nameof(self.IntegerParameters)),
            self.StringParameters.ToDataTable(nameof(self.StringParameters)),
            self.EntityParameters.ToDataTable(nameof(self.EntityParameters)),
            self.PointParameters.ToDataTable(nameof(self.PointParameters)),
        ]);


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

    public static int ToInt(this StringIndex self) => (int)self;
    public static int ToInt(this EntityIndex self) => (int)self;
    public static int ToInt(this DocumentIndex self) => (int)self;
    public static int ToInt(this RelationIndex self) => (int)self;
    public static int ToInt(this PointIndex self) => (int)self;
    public static int ToInt(this DescriptorIndex self) => (int)self;
}