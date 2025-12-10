// parquetLoader.js
// Reads Parquet tables from a JSZip archive into a BimGeometry-shaped object.

export async function loadBimGeometryFromZip(zip) {
  // 1. Load hyparquet + compressors as ESM from jsDelivr
  const [{ parquetReadObjects }, { compressors }] = await Promise.all([
    import("https://cdn.jsdelivr.net/npm/hyparquet@1.22.1/+esm"),
    import("https://cdn.jsdelivr.net/npm/hyparquet-compressors@1.1.1/+esm"),
  ]);

  // 2. Locate a file in the ZIP by suffix (case-insensitive)
  function findFileEndingWith(suffix) {
    const lowerSuffix = suffix.toLowerCase();
    const name = Object.keys(zip.files).find((n) =>
      n.toLowerCase().endsWith(lowerSuffix)
    );
    if (!name) {
      throw new Error(`Could not find "${suffix}" in zip archive.`);
    }
    return name;
  }

  // 3. Read a Parquet file from the ZIP into an array of row objects
  async function readParquetTable(nameSuffix) {
    const entryName = findFileEndingWith(nameSuffix);
    const arrayBuffer = await zip.files[entryName].async("arraybuffer");
    const rows = await parquetReadObjects({
      file: arrayBuffer,
      compressors, // enable Brotli, ZSTD, etc.
    });
    return rows; // Array<Record<string, any>>
  }

  // 4. Helpers for extracting columns
  function pickValue(row, candidates) {
    for (const c of candidates) {
      if (c in row && row[c] !== null && row[c] !== undefined) {
        return row[c];
      }
    }
    return undefined;
  }

  function extractNumberColumn(rows, candidates) {
    if (!rows.length) return [];
    const result = new Array(rows.length);
    for (let i = 0; i < rows.length; i++) {
      const v = pickValue(rows[i], candidates);
      result[i] = v == null ? 0 : Number(v);
    }
    return result;
  }

  function extractByteColumn(rows, candidates) {
    if (!rows.length) return [];
    const result = new Array(rows.length);
    for (let i = 0; i < rows.length; i++) {
      const v = pickValue(rows[i], candidates);
      const n = v == null ? 0 : Number(v);
      result[i] = Math.max(0, Math.min(255, n | 0));
    }
    return result;
  }

  // 5. Read each table — adjust filenames if your BOS export names differ

  // Element table
  const elementRows = await readParquetTable("Element.parquet");
  const ElementEntityIndex = extractNumberColumn(elementRows, [
    "ElementEntityIndex",
    "EntityIndex",
    "entity_index",
  ]);
  const ElementMaterialIndex = extractNumberColumn(elementRows, [
    "ElementMaterialIndex",
    "MaterialIndex",
    "material_index",
  ]);
  const ElementMeshIndex = extractNumberColumn(elementRows, [
    "ElementMeshIndex",
    "MeshIndex",
    "mesh_index",
  ]);
  const ElementTransformIndex = extractNumberColumn(elementRows, [
    "ElementTransformIndex",
    "TransformIndex",
    "transform_index",
  ]);

  // Vertex table
  const vertexRows = await readParquetTable("Vertex.parquet");
  const VertexX = extractNumberColumn(vertexRows, ["VertexX", "X", "x"]);
  const VertexY = extractNumberColumn(vertexRows, ["VertexY", "Y", "y"]);
  const VertexZ = extractNumberColumn(vertexRows, ["VertexZ", "Z", "z"]);

  // Index table (flat index buffer)
  const indexRows = await readParquetTable("Index.parquet");
  const IndexBuffer = extractNumberColumn(indexRows, [
    "IndexBuffer",
    "Index",
    "index",
    "i",
  ]);

  // Mesh table
  const meshRows = await readParquetTable("Mesh.parquet");
  const MeshVertexOffset = extractNumberColumn(meshRows, [
    "MeshVertexOffset",
    "VertexOffset",
    "vertex_offset",
  ]);
  const MeshIndexOffset = extractNumberColumn(meshRows, [
    "MeshIndexOffset",
    "IndexOffset",
    "index_offset",
  ]);

  // Material table
  const materialRows = await readParquetTable("Material.parquet");
  const MaterialRed = extractByteColumn(materialRows, [
    "MaterialRed",
    "Red",
    "R",
    "r",
  ]);
  const MaterialGreen = extractByteColumn(materialRows, [
    "MaterialGreen",
    "Green",
    "G",
    "g",
  ]);
  const MaterialBlue = extractByteColumn(materialRows, [
    "MaterialBlue",
    "Blue",
    "B",
    "b",
  ]);
  const MaterialAlpha = extractByteColumn(materialRows, [
    "MaterialAlpha",
    "Alpha",
    "A",
    "a",
  ]);
  const MaterialRoughness = extractByteColumn(materialRows, [
    "MaterialRoughness",
    "Roughness",
    "roughness",
  ]);
  const MaterialMetallic = extractByteColumn(materialRows, [
    "MaterialMetallic",
    "Metallic",
    "metallic",
  ]);

  // Transform table
  const transformRows = await readParquetTable("Transform.parquet");
  const TransformTX = extractNumberColumn(transformRows, [
    "TransformTX",
    "TX",
    "tx",
  ]);
  const TransformTY = extractNumberColumn(transformRows, [
    "TransformTY",
    "TY",
    "ty",
  ]);
  const TransformTZ = extractNumberColumn(transformRows, [
    "TransformTZ",
    "TZ",
    "tz",
  ]);

  const TransformQX = extractNumberColumn(transformRows, [
    "TransformQX",
    "QX",
    "qx",
  ]);
  const TransformQY = extractNumberColumn(transformRows, [
    "TransformQY",
    "QY",
    "qy",
  ]);
  const TransformQZ = extractNumberColumn(transformRows, [
    "TransformQZ",
    "QZ",
    "qz",
  ]);
  const TransformQW = extractNumberColumn(transformRows, [
    "TransformQW",
    "QW",
    "qw",
  ]);

  const TransformSX = extractNumberColumn(transformRows, [
    "TransformSX",
    "SX",
    "sx",
    "ScaleX",
    "scale_x",
  ]);
  const TransformSY = extractNumberColumn(transformRows, [
    "TransformSY",
    "SY",
    "sy",
    "ScaleY",
    "scale_y",
  ]);
  const TransformSZ = extractNumberColumn(transformRows, [
    "TransformSZ",
    "SZ",
    "sz",
    "ScaleZ",
    "scale_z",
  ]);

  // 6. Return BimGeometry-shaped object
  return {
    ElementEntityIndex,
    ElementMaterialIndex,
    ElementMeshIndex,
    ElementTransformIndex,

    VertexX,
    VertexY,
    VertexZ,

    IndexBuffer,

    MeshVertexOffset,
    MeshIndexOffset,

    MaterialRed,
    MaterialGreen,
    MaterialBlue,
    MaterialAlpha,
    MaterialRoughness,
    MaterialMetallic,

    TransformTX,
    TransformTY,
    TransformTZ,
    TransformQX,
    TransformQY,
    TransformQZ,
    TransformQW,
    TransformSX,
    TransformSY,
    TransformSZ,
  };
}
