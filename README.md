<img src="brand/lockup.svg" alt="BIM Open Schema" width="368" height="64">

# BIM Open Schema

**BIM Open Schema** is an open formal specification of BIM data, including 3D geometry, that is optimized and designed for large-scale 
data and modern tools and pipelines that uses [**Parquet**](https://parquet.apache.org/) to store data in a compact and widely supported 
binary format.

👉 [***Get the Revit 2025 exporter here!***](https://github.com/ara3d/bim-open-schema/releases)

The official release comes with an exporter for Revit 2025 and Ara 3D Studio for viewing and exploring the contents of BIM Open Schema files.  

## Repository Contents

This repository provides:

1. [Official specification](src/Ara3D.BimOpenSchema) - the C# types that define the tables, columns, enums, and manifest. The project has no dependencies. See [source-code.md](source-code.md).
2. [Sample test files](examples) - generated from the Autodesk sample files.
3. [BIM Open Schema Exporter](https://github.com/ara3d/bim-open-schema/releases) - an exporter for Revit 2025 bundled with Ara 3D Studio.

The C# reference implementation (reading, writing, querying, converting from IFC and Revit) lives in
[BIM Open Toolkit](https://github.com/ara3d/bim-open-toolkit).

https://github.com/user-attachments/assets/a02ae405-e3a1-484f-8f09-8601dbe5db72

## Additional BIM Open Schema Tools and Resources

BIM Open Schema comes with an ecosystem of open-source tools in other repositories for 

- [Reading and querying BIM Open Schema (.BOS) files](https://github.com/ara3d/bim-open-toolkit/tree/main/src/Ara3D.BimOpenSchema.IO) and [loading them into DuckDB](https://github.com/ara3d/bim-open-toolkit/tree/main/src/Ara3D.BimOpenSchema.DuckDb) with BIM Open Toolkit
- [Converting IFC files to BOS and analysing them](https://github.com/ara3d/bim-open-toolkit) with BIM Open Toolkit
- [Exporting BOS files from Revit](https://github.com/ara3d/bim-open-toolkit/tree/main/plugins/Ara3D.BIMOpenSchema.Revit2025)
- [Displaying BOS data in WPF Datagrid controls](https://github.com/ara3d/bim-open-toolkit/tree/main/apps/Ara3D.BimOpenSchema.Browser) along with exporting to GLTF and Excel files
- [Loading, Viewing, and Querying BOS files in the browser](https://github.com/ara3d/ara3d-webgl)
- [Querying BOS data in the browser via DuckDB](https://bim-open-schema-reader.vercel.app)

## Absent Values

An index of -1 in any column that points into another table (a string, number, entity, document, descriptor, or point index) means the value is absent: the source had none.
A reader shows it as missing (NULL in SQL, null in code), never as an empty string or 0.
An entity's `LocalId` is -1 when it has no local id, and a parameter with no value has no row.
An empty string from the source is a real value and is stored in the string table.
Files written before this rule store an absent `GlobalId` or `Name` as the index of `""`; readers treat that as absent too.

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
  - Numbers.parquet
  - Parameters.parquet
- **Geometry Data** 
  - Elements.parquet
  - Transforms.parquet
  - VertexBuffer.parquet
  - IndexBuffer.parquet
  - Materials.parquet
  - Meshes.parquet

## Show me the Code

The specification itself is the C# project under [`src`](src). See [source-code.md](source-code.md)
for what each file defines, how to build it, and how to change a table.

The C# reference implementation, with its tests, lives in
[BIM Open Toolkit](https://github.com/ara3d/bim-open-toolkit).

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

## Project Ideas 

If you have ideas for projects that we, or others could do please share them! 
We have some additional code on the side that we can provide to help accelerate your work (e.g., if you want to start a project in C++). 

- Blender to BIM Open Schema
- Rhino to BIM Open Schema

## Commercial Support and Services 

If you are interesting in professional help in leveraging the format and learning what you can do with it, reach out to 
us at [info@ara3d.com](mailto:info@ara3d.com).
