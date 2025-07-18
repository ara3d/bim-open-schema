namespace BIMOpenSchema;

public class ParameterStatistics
{
    public long Index { get; set; }
    public string Name { get; set; }
    public string Group { get; set; }
    public string Units { get; set; }
    public string Type { get; set; }
    public object Min { get; set; }
    public object Max { get; set; }
    public long NumValues { get; set; }
    public long NumDistinctValues { get; set; }
}

public class ParameterLongStats : ParameterStatistics
{
    public List<long> Values { get; } = new();
}

public class ParameterDoubleStats : ParameterStatistics
{
    public List<double> Values { get; } = new();
}

public class ParameterStringStats : ParameterStatistics
{
    public List<string> Values { get; } = new();
}

public static class ParameterStatisticsExtensions
{
    public static T CreateStats<T>(this BIMData data, DescriptorIndex descIndex) where T : ParameterStatistics, new()
    {
        var stats = new T();
        var desc = data.Get(descIndex);
        stats.Index = (long)descIndex;
        stats.Name = data.Get(desc.Name);
        stats.Group = data.Get(desc.Group);
        stats.Units = data.Get(desc.Units);
        return stats;
    }

    public static T GetOrCreate<T>(this BIMData self, Dictionary<DescriptorIndex, ParameterStatistics> d, DescriptorIndex i)
        where T : ParameterStatistics, new()
    {
        if (d.TryGetValue(i, out var value))
            return (T)value;
        var r = CreateStats<T>(self, i);
        d[i] = r;
        return r;
    }

    public static Dictionary<DescriptorIndex, ParameterStatistics> GetStatistics(this BIMData self)
    {
        var r = new Dictionary<DescriptorIndex, ParameterStatistics>();

        foreach (var p in self.DoubleParameters)
        {
            var stats = self.GetOrCreate<ParameterDoubleStats>(r, p.Descriptor);
            stats.Values.Add(p.Value);
        }

        foreach (var p in self.StringParameters)
        {
            var stats = self.GetOrCreate<ParameterStringStats>(r, p.Descriptor);
            stats.Values.Add(self.Get(p.Value));
        }

        foreach (var p in self.IntegerParameters)
        {
            var stats = self.GetOrCreate<ParameterLongStats>(r, p.Descriptor);
            stats.Values.Add(p.Value);
        }

        foreach (var p in self.PointParameters)
        {
            var stats = self.GetOrCreate<ParameterLongStats>(r, p.Descriptor);
            stats.Values.Add((long)p.Value);
        }

        foreach (var p in self.EntityParameters)
        {
            var stats = self.GetOrCreate<ParameterLongStats>(r, p.Descriptor);
            stats.Values.Add((long)p.Value);
        }

        foreach (var stats in r.Values)
        {
            if (stats is ParameterLongStats pls)
            {
                stats.NumDistinctValues = pls.Values.Distinct().Count();
                stats.NumValues = pls.Values.Count;
                stats.Min = pls.Values.Min();
                stats.Max = pls.Values.Max();
            } 
            else if (stats is ParameterStringStats pss)
            {
                stats.NumDistinctValues = pss.Values.Distinct().Count();
                stats.NumValues = pss.Values.Count;
            }
            else if (stats is ParameterDoubleStats pds)
            {
                stats.NumDistinctValues = pds.Values.Distinct().Count();
                stats.NumValues = pds.Values.Count;
                stats.Min = pds.Values.Min();
                stats.Max = pds.Values.Max();
            }
        }

        return r;
    }
}

