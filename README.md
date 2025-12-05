# BIM Open Schema 

**BIM Open Schema** is an open formal specification of BIM data, including 3D geometry, that is optimized and designed for real-world large-scale 
data and modern tools and pipelines.

When used with Parquet, BIM Open Schema allows you to efficiently represent extremely large BIM projects, with a very small footprint
and supported by a wide range of tools off the shelf. 

## Repo Contents

This repo provides:

1. [Official specification](https://github.com/ara3d/bim-open-schema/blob/main/spec) - in the form of valid C# code.
2. [Sample test file](https://github.com/ara3d/bim-open-schema/tree/main/examples) - generated from the Autodesk Revit 2025 Snowdon sample
3. [BIM Open Schema Exporter for Revit 2025](https://github.com/ara3d/bim-open-schema/releases)
   
## About Parquet

[**Parquet**](https://parquet.apache.org/) is a very compact, efficient, and widely supported binary format for tabular data.  
It is self-describing and validating. Unlike with CSV or JSON, a Parquet importer can determine exactly how many rows and columns of data 
there are and what data types are contained within. 

## The .bos File Extension

BIM Open Schema files have the extension `.bos` and are zip archives containing several `.parquet` files. 
You can rename the extension to a `.zip` and open the archive with Windows explorer and other common tools.

## Example Use Cases

1. analytics - [**pandas**](https://pandas.pydata.org/), [**Power BI**](https://www.microsoft.com/en-us/power-platform/products/power-bi)
2. databases - [**DuckDB**](https://duckdb.org/), [**BIM Lakehouse**](https://bimlakehouse.com) 
3. serialization - [**Parquet**](https://parquet.apache.org/)
4. interactive visualization - [**Ara 3D Studio**](https://github.com/ara3d/ara3d-studio)

## Demo Video

https://github.com/user-attachments/assets/fe591704-08a7-451a-a257-adae73ad4c9d

## What is a Schema? 

A schema describes the meaning, relationships, and structure of data. The **BIM Open Schema** project is agnostic of the specific serialization format
(e.g. you could use JSON or FlatBuffer), but it is optimized for structured self-describing columnar binary data formats, particularly **Parquet**.  

## Open-Source Sample Code and Tools

We provide a number of open-source tools and libraries in the [Ara 3D SDK repository](https://github.com/ara3d/ara3d-sdk):

- [Core Library](https://github.com/ara3d/ara3d-sdk/tree/main/src/Ara3D.BimOpenSchema)
- [Browser Tool for Windows (includes Excel and glTF exporter)](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BimOpenSchema.Browser)  
- [IO Libraries](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BimOpenSchema.IO)
- [Revit 2025 Exporter](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BimOpenSchema.Revit2025)

Additionally Tomo Sugeta maintains:

- [BIM Open Schema Reader](https://bim-open-schema-reader.vercel.app/)

## Platform and Language Agnostic 

The schema is optimized for serialization to/from columnar and tabular data formats, such as those used by relational databases, 
but it is *not* tied to any one particular serialization format, and can be easily converted to many different 
formats for fast inspection in your tool of choice. 

This project comes with C# code which acts as the official specification, but the schema is platform independent 
and language agnostic. 

We welcome code contributions in any language. 

## Appendix: Parquet Files

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
