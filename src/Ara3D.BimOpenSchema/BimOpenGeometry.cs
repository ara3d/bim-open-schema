using System.Collections.Generic;

namespace Ara3D.BimOpenSchema;

public enum ModelIndex : int { }
public enum VertexIndex : int { }
public enum ElementIndex : int { }
public enum FaceIndex : int { }
public enum MeshIndex : int { }
public enum MaterialIndex : int { }
public enum TransformIndex : int { }

public class BimOpenGeometry
{
    public List<int> ModelElementOffsets { get; set; } = new();

    public List<float> VertexX { get; set; } = new();
    public List<float> VertexY { get; set; } = new();
    public List<float> VertexZ { get; set; } = new();

    public List<int> IndexBuffer { get; set; } = new();

    public List<float> Transforms { get; set; } = new();

    public List<long> ElementObjectId { get; set; } = new();
    public List<int> ElementMeshIndex { get; set; } = new();
    public List<int> ElementMaterialIndex { get; set; } = new();
    public List<int> ElementTransformIndex { get; set; } = new();

    public List<int> MeshVertexOffsets { get; set; } = new();
    public List<int> MeshIndexOffsets { get; set; } = new();

    public List<byte> MaterialR { get; set; } = new();
    public List<byte> MaterialG { get; set; } = new();
    public List<byte> MaterialB { get; set; } = new();
    public List<byte> MaterialA { get; set; } = new();
    public List<byte> MaterialMetallic { get; set; } = new();
    public List<byte> MaterialRoughness { get; set; } = new();
}

/*
public static class BimOpenGeometryExtensions 
{
    public int ModelCount => ModelElementOffsets.Count;
    public int ElementCount => ElementMeshIndex.Count;
    public int MaterialCount => MaterialR.Count;
    public int MeshCount => MeshIndexOffsets.Count;
    public int VertexCount => VertexX.Count;
    public int IndexCount => IndexBuffer.Count;
    public int TransformCount => Transforms.Count;

    public MaterialIndex AddMaterial(byte r, byte g, byte b, byte a, byte metallic, byte roughness)
    {

    }
    public record Vertex(float X, float Y, float Z);
    public record Material(byte R, byte G, byte B, byte A, byte Metallic, byte Roughness);
    public record MeshOffset(uint VertexOffset, uint IndexOffset);
    public record MeshSize(uint VertexCount, uint IndexCount);
    public record Element(int MeshIndex, long ObjectId, int MaterialIndex, int TransformIndex);
}
*/
