using System.Collections.Generic;

namespace Ara3D.BimOpenSchema;

/// <summary>
/// Contains all the BIM Data for a discipline or federated model.
/// 
/// Optimized for efficient loading into analytical tools as a set of Parquet files, or a DuckDB database.
/// Provides a simple and efficient standardized way to interact with BIM data from different tools,
/// without having to go through APIs, or ad-hoc representations.
///
/// It is optimized for space and load-times, not ease of queries.
/// A typical workflow would be to ingest this into a DuckDB database then to use SQL to
/// create denormalized (wide) tables depending on an end-user's specific use-case
/// and what data they are interested in.  
/// 
/// This expresses the schema as an object model that is
/// independent of any specific serialization format,
/// whether it is JSON, Parquet, CSV, SQLite, or something else.
///
/// When exporting to a database, each list corresponds to a table.
/// When exporting to parquet, each list corresponds to a parquet file.
///
/// This data structure can also be used directly in C# code as am efficient
/// in-memory data structure for code-based workflows.
/// </summary>
public interface IBimData
{
    Manifest Manifest { get; }
    IReadOnlyList<ParameterDescriptor> Descriptors { get; } 
    IReadOnlyList<ParameterInt> IntegerParameters { get; } 
    IReadOnlyList<ParameterSingle> SingleParameters { get; } 
    IReadOnlyList<ParameterString> StringParameters { get; } 
    IReadOnlyList<ParameterEntity> EntityParameters { get; } 
    IReadOnlyList<ParameterPoint> PointParameters { get; } 
    IReadOnlyList<Document> Documents { get; } 
    IReadOnlyList<Entity> Entities { get; } 
    IReadOnlyList<string> Strings { get; } 
    IReadOnlyList<Point> Points { get; } 
    IReadOnlyList<EntityRelation> Relations { get; }
    IReadOnlyList<Diagnostic> Diagnostics { get; }
    BimGeometry Geometry { get; }
}

// Usually stored as a .JSON file in the BOS package. 
public class Manifest
{
    public const string CurrentVersion = "0.1";
    public string BimOpenSchemaVersion { get; set; } = CurrentVersion;
    public string GeneratorApplication { get; set; }
    public string GeneratorVersion { get; set; }
    public object ExportOptions { get; set; }
}

//==
// Enumerations used for indexing tables. Provides type-safety and convenience in code
//
// The choice of long provides future proofing. 

public enum EntityIndex : long { }
public enum PointIndex : long { }
public enum DocumentIndex : long { }
public enum DescriptorIndex : long { }
public enum StringIndex : long { }
public enum RelationIndex : long { }

//==
// Main data type 

/// <summary>
/// Corresponds roughly to an element in the Revit file.
/// Some items are associated with entities that are not expressly derived from Element (e.g., Document, 
/// </summary>
public record struct Entity
(
    // ElementID in Revit, and Step Line # in IFC
    // Will be unique when combined with a DocumentIndex (e.g., "${LocalId}-{Document}" would be a unique string identifier within the database). 
    // But multiple documents can share the same entity  
    long LocalId,

    // UniqueID in Revit, and GlobalID in IFC (not stored in string table, because it is never duplicated)
    StringIndex GlobalId, 

    // The index of the document this entity is part of 
    DocumentIndex Document,

    // The name of the entity 
    StringIndex Name,

    // The category of the entity
    EntityIndex Category,
    
    // The "type" of the entity if it is an instance 
    EntityIndex Type
);

/// <summary>
/// Corresponds with a specific Revit or IFC file 
/// </summary>
public record struct Document
(
    StringIndex Title,
    StringIndex Path
);

/// <summary>
/// Represents 3D location data.
/// </summary>
public record struct Point
(
    float X,
    float Y,
    float Z
);

/// <summary>
/// Important for grouping the different kinds of parameter data ...
/// otherwise we can have two parameter with the same name, but different underlying parameter types.
/// </summary>
public enum ParameterType
{
    Int, 
    Bool = Int,
    Number,
    Entity,
    String,
    Point,
}

/// <summary>
/// Meta-information for understanding a parameter 
/// </summary>
public record struct ParameterDescriptor
(
    StringIndex Name,
    StringIndex Units,
    StringIndex Group,
    ParameterType Type
);

//==
// Parameter data 
//
// All parameter data is arranged in one of a set of EAV (Entity Attribute Value) tables.
// Each one designed for a specific type. 

/// <summary>
/// A 32-bit integer parameter value
/// </summary>
public record struct ParameterInt
(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    int Value
);

/// <summary>
/// A parameter value representing text
/// </summary>
public record struct ParameterString
(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    StringIndex Value
);

/// <summary>
/// A 32-bit single precision floating point numeric parameter value
/// </summary>
public record struct ParameterSingle
(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    float Value
);

/// <summary>
/// A parameter value which references another entity 
/// </summary>
public record struct ParameterEntity
(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    EntityIndex Value
);

/// <summary>
/// A 32-bit integer parameter value
/// </summary>
public record struct ParameterPoint
(
    EntityIndex Entity,
    DescriptorIndex Descriptor,
    PointIndex Value
);

//==
// Relations data

/// <summary>
/// Expresses different kinds of relationships between entities 
/// </summary>
public record struct EntityRelation
(
    EntityIndex EntityA,
    EntityIndex EntityB,
    RelationType RelationType
);

/// <summary>
/// The various kinds of relations, aimed at covering both the Revit API and IFC
/// </summary>
public enum RelationType
{
    // For parts of a whole. Represents composition.
    PartOf = 0,

    // For elements of a group or set or layer. Represents aggregations. 
    MemberOf = 1,

    // Represents spatial relationships. Like part of a level, or a room.  
    ContainedIn = 2,

    // For parts or openings that occur within a host (such as windows or doorways). 
    HostedBy = 3,

    // For parent-child relationships in a graph (e.g. sub-categories)
    ChildOf = 4,

    // Represents relationship of compound structures and their constituents 
    HasLayer = 5,

    // Represents different kinds of material relationships
    HasMaterial = 6,

    // Two-way connectivity relationship. Can assume that only one direction is stored in DB 
    ConnectsTo = 7,

    // MEP networks and connection manager
    HasConnector = 8,
    
    // For space <-> boundary relationships. 
    BoundedBy = 9,

    // Can traverse from one space to another (e.g., portal)
    TraverseTo = 10, 

    // Relationship between openings (e.g., doorways, window frame) and hosts  
    Voids = 11,

    // When an object like a door or window fills a void 
    Fills = 12,

    // For finishes on walls/floors/ceilings
    Covers = 13,

    // For MEP systems (e.g., HVAC) providing service to a zone
    Serves = 14,
}


public enum DiagnosticType
{
    RevitWarning,
    RevitError,
    ExporterWarning,
    ExporterError,
    ExporterInfo
}

public record struct Diagnostic
(
    DiagnosticType Type,
    DocumentIndex Document,
    EntityIndex Entity,
    StringIndex Message
);