using System;

namespace BIMOpenSchema;

public enum ElementIndex : long { }
public enum PointIndex : long { }
public enum RoomIndex : long { }
public enum LevelIndex : long { }
public enum CategoryIndex : long { }
public enum DocumentIndex : long { }
public enum DescriptorIndex : long { }
public enum ParameterIndex : long { }

public class ParameterDescriptorData
{
    public string Name { get; init; } = string.Empty;

    // Created from the BuiltInParameterGroup enumeration 
    // https://www.revitapidocs.com/2022/9942b791-2892-0658-303e-abf99675c5a6.htm
    public string Group { get; init; } = string.Empty;
    
    // The ID used to identify built-in parameters 
    public int BuiltInParameterId { get; init; }
    
    // Shared Parameters have GUIDs
    public string SharedParameterGuid { get; init; } = string.Empty;
    
    // Forge type ID
    public string TypeId { get; init; } = string.Empty;

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Group, BuiltInParameterId, SharedParameterGuid, TypeId);
    }

    public override bool Equals(object obj)
    {
        return obj is ParameterDescriptorData pdd &&
               Name.Equals(pdd.Name)
               && Group.Equals(pdd.Group)
               && BuiltInParameterId.Equals(pdd.BuiltInParameterId)
               && SharedParameterGuid.Equals(pdd.SharedParameterGuid)
               && TypeId.Equals(pdd.TypeId);
    }
}

public class ElementData 
{
    // The ID is called a "Unique" id in the revit doce, but this does not always hold. 
    // For example: the Existing Phase element's UniqueId has been observed to be shared between linked models 
    // but we believe it to be intentional (so that we can link phases across models)
    // More accurately the ID is a stable ID for use across models 
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    // This is available only if the element is an ElementType. 
    public string FamilyName { get; set; } = string.Empty;

    public CategoryIndex Category { get; set; }
    public LevelIndex Level { get; set; }
    public ElementIndex Type { get; set; }
    public ElementIndex PhaseCreated { get; set; }
    public ElementIndex PhaseDemolished { get; set; }
    public PointIndex Location { get; set; }
    public PointIndex BoundsMin { get; set; }
    public PointIndex BoundsMax { get; set; }
    public ElementIndex Group { get; set; }
    public DocumentIndex Document { get; set; }
}

public class PointData
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y, Z);
    }

    public override bool Equals(object obj)
    {
        return obj is PointData pd && X.Equals(pd.X) && Y.Equals(pd.Y) && Z.Equals(pd.Z);
    }
}

public class ElementRef
{
    public ElementIndex Element { get; set; }
}

public class DocumentData : ElementRef
{
    public string PathName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public bool IsLinked { get; set; }
    public bool IsDetached { get; set; }
}

public class RoomData : ElementRef
{
    public string Number { get; set; } = string.Empty;
    public double BaseOffset { get; set; }
    public double LimitOffset { get; set; }
    public double UpperLimit { get; set; }
}

public class LevelData : ElementRef
{
    public double Elevation { get; set; }
}

public class CategoryData
{
    public string Name { get; set; }
    public long Id { get; set; }
}

// TODO: we eventually want to separate this out into the four different classes,
// One for each of the storage types: int, double, string (or unknown), ElementId
public class ParameterData
{
    public ElementIndex Element { get; set; } 
    public DescriptorIndex Descriptor { get; set; }
    public string Value { get; set; } = string.Empty;
}

/// <summary>
/// In C# the Array class can only support up to 2 billion items however,
/// we can go past if we roll our own List class. 
/// </summary>
public class BIMData
{
    public ParameterData[] Parameters { get; set; } = [];
    public ParameterDescriptorData[] Descriptors { get; set; } = [];
    public PointData[] Points { get; set; } = [];
    public CategoryData[] Categories { get; set; } = [];
    public RoomData[] Rooms { get; set; } = [];
    public LevelData[] Levels { get; set; } = [];
    public DocumentData[] Documents { get; set; } = [];
    public ElementData[] Elements { get; set; } = [];
}
