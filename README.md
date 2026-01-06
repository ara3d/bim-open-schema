# BIM Open Schema 

**BIM Open Schema** is an open formal specification of BIM data, including 3D geometry, that is optimized and designed for large-scale 
data and modern tools and pipelines that uses [**Parquet**](https://parquet.apache.org/) to store data in a compact and widely supported 
binary format.

👉 [***Get it here!***](https://github.com/ara3d/bim-open-schema/releases)

## Repository Contents

This repository provides:

1. [Official specification](https://github.com/ara3d/bim-open-schema/blob/main/spec) - in the form of valid C# code.
2. [Sample test files](https://github.com/ara3d/bim-open-schema/tree/main/examples) - generated from the Autodesk sample files.
3. [BIM Open Schema Exporter](https://github.com/ara3d/bim-open-schema/releases) - an exporter for Revit 2025 bundled with Ara 3D Studio. 

https://github.com/user-attachments/assets/a02ae405-e3a1-484f-8f09-8601dbe5db72

## Additional BIM Open Schema Tools and Resources

BIM Open Schema comes with an ecosystem of open-source tools in other repositories for 

- [Reading and querying BIM Open Schema (.BOS) files](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BimOpenSchema.IO)
- [Data structures for querying and construction BIM Open Schema data](https://github.com/ara3d/ara3d-sdk/tree/main/src/Ara3D.BimOpenSchema)
- [Exporting BOS files from Revit](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BIMOpenSchema.Revit2025)
- [Displaying BOS data in WPF Datagrid controls](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BimOpenSchema.Browser) along with exporting to GLTF and Excel files
- [Loading, Viewing, and Querying BOS files in the browser](https://github.com/ara3d/ara3d-webgl)
- [Querying BOS data in the browser via DuckDB](https://bim-open-schema-reader.vercel.app)

## About Parquet

[**Parquet**](https://parquet.apache.org/) is a very compact, efficient, and widely supported binary format for tabular data.  
It is self-describing and validating. Unlike with CSV or JSON, a Parquet importer can determine exactly how many rows and columns of data 
there are and what data types are contained within. 

## The .bos File Extension

BIM Open Schema files have the extension `.bos` and are zip archives containing several `.parquet` files. 
You can rename the extension to a `.zip` and open the archive with Windows explorer and other common tools.

## Example Use Cases for Parquet Data

1. analytics - [**pandas**](https://pandas.pydata.org/), [**Power BI**](https://www.microsoft.com/en-us/power-platform/products/power-bi)
2. databases - [**DuckDB**](https://duckdb.org/), [**BIM Lakehouse**](https://bimlakehouse.com), [**BIM Open Schema Reader**](https://bim-open-schema-reader.vercel.app/)
3. serialization - [**Parquet**](https://parquet.apache.org/)
4. interactive visualization - [**Ara 3D Studio**](https://github.com/ara3d/ara3d-studio), [**Ara 3D WebGL**](https://github.com/ara3d/ara3d-webgl).

## What is a Schema? 

A schema describes the meaning, relationships, and structure of data. The **BIM Open Schema** project is agnostic of the specific serialization format
(e.g. you could use JSON or FlatBuffer), but it is optimized for structured self-describing columnar binary data formats, particularly **Parquet**.  

## Platform and Language Agnostic 

The schema is optimized for serialization to/from columnar and tabular data formats, such as those used by relational databases, 
but it is *not* tied to any one particular serialization format, and can be easily converted to many different 
formats for fast inspection in your tool of choice. 

This project comes with C# code which acts as the official specification, but the schema is platform independent 
and language agnostic. 

We welcome code contributions in any language. 

## Parquet Files contained in the BOS Archive

The contents of the .bos archive are the following parquet files: 

- **Non Geometry Data**
  - Entities.parquet
  - Descriptors.parquet
  - Documents.parquet
  - Points.parquet
  - Strings.parquet
  - DoubleParameters.parquet
  - IntegerParameter.parquet
  - PointParameters.parquet
  - StringParameters.parquet
- **Geometry Data** 
  - Elements.parquet
  - Transforms.parquet
  - VertexBuffer.parquet
  - IndexBuffer.parquet
  - Materials.parquet
  - Meshes.parquet

## Show me the Code

The bulk of the source code for this project, is actively maintained in a separate repository.  

See [source-code.md](source-code.md) to find where the code is, learn how it is structured, and how to contribute to it. 

## Contributors and Supporters

Supporting and contributing to this project is as simple as [providing feedback](https://github.com/ara3d/bim-open-schema/issues/new?template=feedback.md).

Some of the people who have contributed are (in alphabetical order): 

* Ahmad Saleem Z - AnkerDB
* Christopher Diggins - Ara 3D
* Daryl Irvine - DG Jones and Partners 
* Karim Daw - Gensler
* Pablo Derendinger - e-verse
* Tom van Diggelen - BIMcollab
* Tomo Sugeta - Cundall
* Valentin Noves - e-verse
* Yskert Schindel - Vyssuals

We have an active Discord server and discussion forum that you can join if you are interested. 
Just send us an [email request](mailto:info@ara3d.com)

## Commercial Support and Services 

If you are interesting in professional help in leveraging the format and learning what you can do with it, reach out to 
us at [info@ara3d.com](mailto:info@ara3d.com).
