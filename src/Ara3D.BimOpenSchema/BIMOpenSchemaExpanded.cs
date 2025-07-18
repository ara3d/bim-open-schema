using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection.Emit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BIMOpenSchema;

public class ExpandedBIMData
{
    public ExpandedBIMData(BIMData data)
    {
        Data = data;

        for (var i = 0; i < data.Entities.Count; i++)
            Add(data.Entities[i]);

        Parameters = new List<ExpandedParameter>();
        foreach (var p in Data.IntegerParameters) Add(p);
        foreach (var p in Data.DoubleParameters) Add(p);
        foreach (var p in Data.PointParameters) Add(p);
        foreach (var p in Data.EntityParameters) Add(p);
        foreach (var p in Data.StringParameters) Add(p);

        // TODO: add relations

        // TODO: add level, material, room, type
        // rvt:Object:TypeName
        // rvt:Element:Level
        // rvt:Element:Group
        // rvt:Element:Workset
        // rvt:Element:DesignOption
        // rvt:Element:AssemblyInstance
        // rvt:Element:Category
        // rvt:FamilyInstance:Host
        // rvt:FamilyInstance:Room
        // rvt:FamilyInstance:Space
        // rvt:FamilyInstance:StructuralMaterialId
        // rvt:FamilyInstance:StructuralMaterialType
        // rvt:FamilyInstance:StructuralUsage
    }

    public void Add(Entity e)
    {
        var ee = new ExpandedEntity()
        {
            Category = Data.Get(e.Category),
            DocumentIndex = (long)e.Document,
            GlobalId = e.GlobalId,
            Name = Data.Get(e.Name),
        };
        var n = Entities.Count;
        Entities.Add(ee);
        EntityNames.Add((EntityIndex)n, ee.Name);
    }

    public string GetEntityName(EntityIndex e)
        => EntityNames.ContainsKey(e) ? EntityNames[e] : "";

    public ExpandedParameter Add(string value, EntityIndex ei, DescriptorIndex di)
    {
        var d = Data.Get(di);
        var p = new ExpandedParameter
        {
            Value = value,
            EntityIndex = ei,
            DescriptorIndex = di,
            Group = Data.Get(d.Group),
            Name = Data.Get(d.Name),
            Units = Data.Get(d.Units),
            Entity = GetEntityName(ei)
        };
        Parameters.Add(p);
        Add(ParametersByEntity, ei, p);
        Add(ParametersByName, p.Name, p);
        return p;
    }

    public static void Add<TKey, TValue>(Dictionary<TKey, List<TValue>> d, TKey key, TValue value)
    {
        if (!d.ContainsKey(key))
            d.Add(key, new List<TValue>());
        d[key].Add(value);
    }

    public ExpandedParameter Add(ParameterInt p)
        => Add(p.Value.ToString(), p.Entity, p.Descriptor);

    public ExpandedParameter Add(ParameterDouble p)
        => Add(p.Value.ToString(), p.Entity, p.Descriptor);

    public ExpandedParameter Add(ParameterPoint p)
        => Add(Data.Get(p.Value).ToString(), p.Entity, p.Descriptor);

    public ExpandedParameter Add(ParameterEntity p)
        => Add(EntityNames[p.Value], p.Entity, p.Descriptor);

    public ExpandedParameter Add(ParameterString p)
        => Add(Data.Get(p.Value), p.Entity, p.Descriptor);

    public BIMData Data;
    public Dictionary<EntityIndex, string> EntityNames = new();
    public List<ExpandedParameter> Parameters { get; set; } = [];
    public List<ExpandedEntity> Entities { get; set; } = [];
    public List<ExpandedEntityRelation> Relations { get; set; } = [];
    public Dictionary<EntityIndex, List<EntityRelation>> RelationsFrom { get; } = new();
    public Dictionary<EntityIndex, List<EntityRelation>> RelationsTo { get; } = new();
    public Dictionary<EntityIndex, List<ExpandedParameter>> ParametersByEntity { get; } = new();
    public Dictionary<string, List<ExpandedParameter>> ParametersByName { get; set; } = new();
}

public class ExpandedParameter
{
    public ParameterDescriptor Descriptor { get; set; }
    public EntityIndex EntityIndex { get; set; }
    public DescriptorIndex DescriptorIndex { get; set; }
    public string Entity { get; set; } 
    public string Name { get; set; }
    public string Units { get; set; }
    public string Value { get; set; }
    public string Group { get; set; }
}

public class ExpandedEntity
{
    public EntityIndex Index { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Level { get; set; }
    public string Type { get; set; }
    public string Material { get; set; }
    public string Room { get; set; }
    public string GlobalId { get; set; }
    public long DocumentIndex { get; set; }
}

public class ExpandedEntityRelation
{
    public EntityIndex EntityIndexA { get; set; }
    public EntityIndex EntityIndexB { get; set; }
    public string EntityA { get; set; }
    public string EntityB { get; set; }
    public string RelationType { get; set; }
}