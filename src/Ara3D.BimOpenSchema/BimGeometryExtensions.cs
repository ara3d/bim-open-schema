using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Ara3D.Utils;

namespace Ara3D.BimOpenSchema;

public static class BimGeometryExtensions
{
    public static int NumMaterials(this BimGeometry self) => self.MaterialRed.Count;
    public static int NumVertices(this BimGeometry self) => self.VertexXData.Count;
    public static int NumFaces(this BimGeometry self) => self.IndexData.Count / 3;
    public static int NumTransforms(this BimGeometry self) => self.TransformData.Count / 16;
    public static int NumMeshes(this BimGeometry self) => self.MeshIndexOffset.Count;
    public static int NumElements(this BimGeometry self) => self.ElementMeshIndices.Count;

}