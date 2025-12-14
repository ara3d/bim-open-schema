namespace Ara3D.BimOpenSchema;

/// <summary>
/// A columnar representation of the 3D representation (geometry and appearance) for large Building Information Models (BIM).
/// This can be serialized via various serialization formats, such as JSON, MessagePack,
/// but we recommend Parquet. 
/// 
/// This 3D format is optimized for representing complex scenes used in AEC (Architecture, Engineering, and Construction) industry
/// to represent the built environment. 
///
/// It is a collection of elements, meshes, materials, transforms, and instances.
/// A mesh is a triangular mesh consisting  
///
/// Coordinates: Z-Up, right-handed.
/// Units: meters
/// Geometry point units: 0.1 mm
/// </summary>
public class BimGeometry
{
    public const string InstanceTableName = nameof(BimGeometryTableName.Instances);
    public const string IndexTableName = nameof(BimGeometryTableName.IndexBuffer);
    public const string MaterialTableName = nameof(BimGeometryTableName.Materials);
    public const string MeshTableName = nameof(BimGeometryTableName.Meshes);
    public const string TransformTableName = nameof(BimGeometryTableName.Transforms);
    public const string VertexTableName = nameof(BimGeometryTableName.VertexBuffer);

    public static string[] TableNames
        = [InstanceTableName, IndexTableName, MaterialTableName, MeshTableName, TransformTableName, VertexTableName];

    //==
    // Instance Table
    //
    // Represent a distinct geometric part. 
    // An instance consists of a mesh, material, transform associated with a specific entity 
    // An entity may have multiple instances.
    // Meshes, materials, and transforms may all be shared to reduce repetition 
    // An element corresponds roughly to an "InstanceStructs" in the Ara3D.Models

    // Index of the entity associated with this instance
    public int[] InstanceEntityIndex { get; set; } = [];

    // Index of the material associated with this instance
    public int[] InstanceMaterialIndex { get; set; } = [];

    // Index of the mesh associated with this instance
    public int[] InstanceMeshIndex { get; set; } = [];

    // Index of the transform associated with this instance
    public int[] InstanceTransformIndex { get; set; } = [];

    //==
    // Vertex Table

    // Multiply vertices by this factor to get the real floating-point value. 
    public const float VertexMultiplier = 10_000f;

    // X Position of the vertices in local space 
    public int[] VertexX { get; set; } = [];

    // X Position of the vertices in local space
    public int[] VertexY { get; set; } = [];

    // X Position of the vertices in local space
    public int[] VertexZ { get; set; } = [];

    //==
    // Index Table

    // Local mesh face-corner indices
    // If you use the single shared vertex buffer you need to add the mesh vertex offsets.
    // Otherwise, you need to slice the vertex buffer for each mesh. 
    public int[] IndexBuffer { get; set; } = [];
    
    //==
    // Mesh Table 
    
    // The offset into the global vertex buffer where each mesh starts 
    public int[] MeshVertexOffset { get; set; } = [];
    
    // The offset into the global index buffer where each index starts.
    public int[] MeshIndexOffset { get; set; } = [];

    //==
    // Material Table 
    // Bytes represent value from 0 to 1 

    public byte[] MaterialRed { get; set; } = [];
    public byte[] MaterialGreen { get; set; } = [];
    public byte[] MaterialBlue { get; set; } = [];
    public byte[] MaterialAlpha { get; set; } = [];
    public byte[] MaterialRoughness { get; set; } = [];
    public byte[] MaterialMetallic { get; set; } = [];
    
    //==
    // Transform Table.
    // Transform Matrix = Scaling Matrix * Rotation Matrix * Translation Matrix
    // Note: mirrored objects occur when the product of the scale component is negative 

    // Translation
    public float[] TransformTX { get; set; } = [];
    public float[] TransformTY { get; set; } = [];
    public float[] TransformTZ { get; set; } = [];
    
    // Quaternion rotation
    public float[] TransformQX { get; set; } = [];
    public float[] TransformQY { get; set; } = [];
    public float[] TransformQZ { get; set; } = [];
    public float[] TransformQW { get; set; } = [];

    // Scaling 
    public float[] TransformSX { get; set; } = [];
    public float[] TransformSY { get; set; } = [];
    public float[] TransformSZ { get; set; } = [];
}