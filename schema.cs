using System.Collections.Generic;

namespace BIMOpenSchema;

public enum ElementIndex : long { }
public enum PointIndex : long { }
public enum RoomIndex : long { }
public enum LevelIndex : long { }
public enum CategoryIndex : long { }
public enum DocumentIndex : long { }
public enum DescriptorIndex : long { }

public class ParameterDescriptorData
{
    public string Name { get; set; }

    // Created from the BuiltInParameterGroup enumeration 
    // https://www.revitapidocs.com/2022/9942b791-2892-0658-303e-abf99675c5a6.htm
    public string Group { get; set; }
    
    // The ID used to identify built-in parameters 
    public int BuiltInParameterId { get; set; }
    
    // Shared Parameters have GUIDs
    public string SharedParameterGuid { get; set; }
    
    // Forge type ID
    public string TypeId { get; set; } 
}

public class ElementData 
{
    // This is the so-called Unique ID in Revit, which is more stable than ElementId
    public string Id { get; set; }
    public string Name { get; set; }

    // This is available only if the element is an ElementType. 
    public string FamilyName { get; set; }
    
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
    public double X { get; set; }
    public double Y { get; set; }
    public double Z { get; set; }
}

public class ElementRef
{
    public ElementIndex Element { get; set; }
}

public class DocumentData : ElementRef
{
    public string PathName { get; set; }
    public string Title { get; set; }
    public bool IsLinked { get; set; }
    public bool IsDetached { get; set; }
}

public class RoomData : ElementRef
{
    public string Number { get; set; }
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

public class ParameterData
{
    public ElementIndex Element { get; set; } 
    public DescriptorIndex Descriptor { get; set; }
    public string Value { get; set; }
}

public class RevitData
{
    public List<ParameterData> Parameters { get; set; } = new();
    public List<ParameterDescriptorData> Descriptors { get; set; } = new();
    public List<PointData> Points { get; set; } = new();
    public List<CategoryData> Category { get; set; } = new();
    public List<RoomData> Rooms { get; set; } = new();
    public List<LevelData> Levels { get; set; } = new();
    public List<DocumentData> Documents { get; set; } = new();
    public List<ElementData> Elements { get; set; } = new();
}

public class RevitDataBuilder
{
    public RevitData RevitData { get; set; } = new();

    public Dictionary<int, CategoryData> Categories = new();
    public Dictionary<string, RoomData> Rooms = new();
    public Dictionary<string, LevelData> Levels = new();
    public Dictionary<string, DocumentData> Documents = new();
    public Dictionary<string, ElementData> Elements = new();

}
