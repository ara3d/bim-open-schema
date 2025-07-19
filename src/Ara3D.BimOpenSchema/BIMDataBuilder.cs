using System.Diagnostics;
using System.Xml.Linq;

namespace Ara3D.BimOpenSchema;

// This is a helper class for incrementally constructing a BIMData object without repeating objects. 
public class BimDataBuilder
{
    private readonly Dictionary<Entity, int> _entities = new();
    private readonly Dictionary<Document, int> _documents = new();
    private readonly Dictionary<Point, int> _points = new();
    private readonly Dictionary<ParameterDescriptor, int> _descriptors = new();
    private readonly Dictionary<string, int> _strings = new();

    public readonly BimData Data = new BimData();

    private int Add<T>(Dictionary<T, int> d, List<T> list, T val)
    {
        Debug.Assert(val != null);
        if (val == null)
            Debugger.Break();
        if (d.TryGetValue(val, out var index))
            return index;
        var r = d.Count;
        d.Add(val, r);
        list.Add(val);
        Debug.Assert(d.Count == list.Count);
        return r;
    }
    
    public void AddRelation(EntityIndex a, EntityIndex b, RelationType rt)
        => Data.Relations.Add(new(a, b, rt));

    public EntityIndex AddEntity(long localId, string globalId, DocumentIndex d, string name, string category)
        => (EntityIndex)Add(_entities, Data.Entities, new(localId, globalId, d, AddString(name), AddString(category)));

    public DocumentIndex AddDocument(string title, string pathName)
        => (DocumentIndex)Add(_documents, Data.Documents, new(AddString(title), AddString(pathName)));

    public PointIndex AddPoint(Point p)
        => (PointIndex)Add(_points, Data.Points, p);

    public DescriptorIndex AddDescriptor(string name, string units, string group, ParameterType pt)
        => (DescriptorIndex)Add(_descriptors, Data.Descriptors, new(AddString(name), AddString(units), AddString(group), pt));

    public StringIndex AddString(string name)
        => (StringIndex)Add(_strings, Data.Strings, name ?? "");

    public void AddParameter(EntityIndex e, double val, DescriptorIndex d)
        => Data.DoubleParameters.Add(new(e, d, val));

    public void AddParameter(EntityIndex e, int val, DescriptorIndex d)
        => Data.IntegerParameters.Add(new(e, d, val));

    public void AddParameter(EntityIndex e, EntityIndex val, DescriptorIndex d)
        => Data.EntityParameters.Add(new(e, d, val));

    public void AddParameter(EntityIndex e, string val, DescriptorIndex d)
        => Data.StringParameters.Add(new(e, d, AddString(val)));

    public void AddParameter(EntityIndex e, PointIndex pi, DescriptorIndex d)
        => Data.PointParameters.Add(new(e, d, pi));

    public void AddParameter(EntityIndex e, Point p, DescriptorIndex d)
        => Data.PointParameters.Add(new(e, d, AddPoint(p)));

    public void AddParameter(EntityIndex e, double val, string name, string units, string group)
        => AddParameter(e, val, AddDescriptor(name, units, group, ParameterType.Double));

    public void AddParameter(EntityIndex e, int val, string name, string units, string group)
        => AddParameter(e, val, AddDescriptor(name, units, group, ParameterType.Int));

    public void AddParameter(EntityIndex e, EntityIndex val, string name, string units, string group)
        => AddParameter(e, val, AddDescriptor(name, units, group, ParameterType.Entity));

    public void AddParameter(EntityIndex e, string val, string name, string units, string group)
        => AddParameter(e, val, AddDescriptor(name, units, group, ParameterType.String));

    public void AddParameter(EntityIndex e, Point p, string name, string units, string group)
        => AddParameter(e, p, AddDescriptor(name, units, group, ParameterType.Point));

    public void AddParameter(EntityIndex e, PointIndex pi, string name, string units, string group)
        => AddParameter(e, pi, AddDescriptor(name, units, group, ParameterType.Int));
}

