using System.Collections.Generic;

namespace BIMOpenSchema;

/// <summary>
/// Contains all the BIM Data for a federated model.
/// Optimized for efficient representation as a set of Parquet files, or a database optimized for analytics like DuckDB
/// Provides a simple and efficient standardized way to interact with BIM data from different tools, without having to go through APIs, or ad-hoc representations
/// </summary>
public class BIMData
{
    public List<ParameterDescriptor> Descriptors { get; set; } = [];
    public List<ParameterInt> IntegerParameters { get; set; } = [];
    public List<ParameterDouble> DoubleParameters { get; set; } = [];
    public List<ParameterString> StringParameters { get; set; } = [];
    public List<ParameterEntity> EntityParameters { get; set; } = [];
    public List<ParameterPoint> PointParameters { get; set; } = [];
    public List<Document> Documents { get; set; } = [];
    public List<Entity> Entities { get; set; } = [];
    public List<string> Strings { get; set; } = [];
    public List<Point> Points { get; set; } = [];
    public List<EntityRelation> Relations { get; set; } = [];
}

//==
// Enumerations used for indexing tables. Provides type-safety and convenience in code

public enum EntityIndex : long { }
public enum PointIndex : long { }
public enum DocumentIndex : long { }
public enum DescriptorIndex : long { }
public enum StringIndex : long { }

//==
// Main data types 

public record Entity(
    StringIndex Id,
    DocumentIndex Document,
    StringIndex Name,
    StringIndex Category);

public record Document(
    string Title,
    string PathName);

public record Point(
    double X,
    double Y,
    double Z);

public record ParameterDescriptor(
    string Name,
    string Units,
    string Group);

//==
// Parameter data 

public record ParameterInt(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    int Value);

public record ParameterString(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    StringIndex Value);

public record ParameterDouble(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    double Value);

public record ParameterEntity(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    EntityIndex Value);

public record ParameterPoint(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    PointIndex Value);

//==
// Relations data

public record EntityRelation(
    EntityIndex EntityA,
    EntityIndex EntityB,
    RelationType RelationTypeIndex);

public enum RelationType : byte
{
    // Corresponds to the following relationships in IFC 
    MemberOf,

    // Containment spatial relationships. Like part of a level, or a room.  
    ContainedIn,

    // Used to express family instance to family type relationship of in IFC: 
    InstanceOf,

    // Parts or openings that occur within a host. 
    HostedBy,

    // Two-way connectivity relationship. Can assume that only one direction is stored in DB 
    ConnectsTo,

    // MEP networks and connection manager
    HasConnector,

    // Useful for accessing properties and materials of compound structures 
    HasLayer,

    // IfcRelAssociatesMaterial
    HasMaterial,
}

//==
// Additional helpers for informational purposes

public static class Helpers
{
    // Names of special parameter containing data extracted from Revit API method or property invocations 
    public static string[] ApiParameterNames = [
        "rvt:ApiTypeName",
        "rvt:UniqueId",
        "rvt:LastChangedBy",
        "rvt:LastSavedTime",
        "rvt:FromRoom",
        "rvt:ToRoom",
        "rvt:LocationPoint",
        "rvt:LocationEndPointA",
        "rvt:LocationEndPointB",
        "rvt:BoundingBoxMin",
        "rvt:BoundingBoxMax",
        "rvt:SolidVolume",
        "rvt:SolidArea",
        "rvt:LayerIndex",
        "rvt:MEPSectionNumber",
    ];

    // Within Revit a number of classes represent entities but that do not derive from element 
    public static string[] NonElementEntities =
    [
        // https://www.revitapidocs.com/2016/aa8f7f05-16c7-2fbf-5004-d819a1fd0b6d.htm
        "rvt:type:Workset",
        
        // https://www.revitapidocs.com/2016/dc1a081e-8dab-565f-145d-a429098d353c.htm
        "rvt:type:CompoundStructure",
        
        // Doesn't exist as an actual class, just as an index of the compound structure. Use "rvt:LayerIndex" to identify it. 
        "rvt:pseudotype:Layer",
        
        // https://www.revitapidocs.com/2016/11e07082-b3f2-26a1-de79-16535f44716c.htm
        "rvt:type:Connector",

        // Part of a MEP system, which itself is an element
        "rvt:type:MEPSection",
    ];

    // Informal and incomplete mapping of IFC Relationships to relation types for documentation purposes 
    public static Dictionary<string, RelationType> IfcRelationToRelationType
    = new() {
        { "IfcRelAggregates", RelationType.MemberOf },
        { "IfcRelAssignsToGroup", RelationType.MemberOf }, 
        { "IfcRelDecomposes", RelationType.MemberOf },
        { "IfcRelNests", RelationType.MemberOf }, 
        { "IfcRelContainedInSpatialStructure", RelationType.ContainedIn },
        { "IfcRelDefinesByType", RelationType.InstanceOf },
        { "IfcRelVoidsElement", RelationType.HostedBy },
        { "IfcRelConnectsPortToPort", RelationType.ConnectsTo },
        { "IfcRelConnectsElements", RelationType.ConnectsTo },
        { "IfcRelFillsElement", RelationType.HostedBy },
        { "IfcPort", RelationType.HasConnector },
        { "IfcMaterialLayerSetUsage", RelationType.HasLayer },
        { "IfcRelAssociatesMaterial", RelationType.HasMaterial },
    };
}

