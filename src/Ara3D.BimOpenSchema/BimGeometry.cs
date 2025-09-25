using System.Collections.Generic;

namespace Ara3D.BimOpenSchema;

public class BimGeometry
{
    //==
    // Element data
    //
    // An element consists of a mesh, material, transform associated with a specific object
    // An object may have different elements.
    // Meshes, materials, and transforms may all be shared to reduce repetition 

    // The index of the associated entity 
    public List<int> ElementEntityIndices { get; set; } = new();

    // Index of the material associated with this element
    public List<int> ElementMaterialIndices { get; set; } = new();

    // Index of the mesh associated with this element
    public List<int> ElementMeshIndices { get; set; } = new();

    // Index of the transform associated with this element 
    public List<int> ElementTransformIndices { get; set; } = new();

    //==
    // Vertex point data
    //
    // Each buffer is the same size. 

    // X position of each vertex
    public List<float> VertexXData { get; set; } = new();
    
    // Y position of each vertex
    public List<float> VertexYData { get; set; } = new();
    
    // Z position of each vertex
    public List<float> VertexZData { get; set; } = new();

    //==
    // Index data

    // A multiple of the face size, used for indirectly referencing the vertices 
    // Needs to be added to the appropriate mesh vertex offset. 
    public List<int> IndexData { get; set; } = new();
    
    //==
    // Mesh data 
    
    // The offset into the vertex buffer where each mesh starts 
    public List<int> MeshVertexOffset { get; set; } = new();
    
    // The offset into the index buffer where each index starts. Indices reference  
    public List<int> MeshIndexOffset { get; set; } = new();
    
    //==
    // Material data 

    public List<byte> MaterialRed { get; set; } = new();
    public List<byte> MaterialGreen { get; set; } = new();
    public List<byte> MaterialBlue { get; set; } = new();
    public List<byte> MaterialAlpha { get; set; } = new();
    public List<byte> MaterialRoughness { get; set; } = new();
    public List<byte> MaterialMetallic { get; set; } = new();
    
    //==
    // Transform data.

    // Transforms are stored as 4x4 matrices following the System.Numerics layout
    public List<float> TransformData { get; set; } = new();
}