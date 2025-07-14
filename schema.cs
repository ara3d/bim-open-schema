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
public enum UnitIndex : long { }

//==
// Main data type 

public record Entity(
    // ElementID in Revit, and Step Line # in IFC
    long FileId,

    // UniqueID in Revit, and GlobalID in IFC (not stored in string table, because it is NEVER duplicated)
    string StableId, 

    // The index of the document this entity was found int
    DocumentIndex Document,

    // The name of the entity 
    StringIndex Name,

    // The category of the entity
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
    UnitIndex Units,
    string Group);

public record Unit(
    // UCUM symbol
    string Symbol,

    // ISO 80000 quantity name 
    string IsoQuantity,
    
    // Official symbol of the base SI unit
    string SiBaseUnitSymbol,

    // Conversion factor to the SI base unit
    double SiFactor,

    // Offset required to be added before conversion the SI base unit
    double SiOffset = 0);

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
