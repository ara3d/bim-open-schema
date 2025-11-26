# BIM Open Schema 

**BIM Open Schema** is an open formal specification of BIM data, including 3D geometry, that is optimized and designed for real-world large-scale data and modern tools and pipelines:

When used with Parquet, BIM Open Schema allows you to efficiently represent extremely large BIM projects, with a very small footprint
and to support a wide range of tools off the shelf. 

## Example Use Cases

1. analytics - [**pandas**](https://pandas.pydata.org/), [**Apache Spark**](https://spark.apache.org/), [**BIM Open Schema Reader**](https://bim-open-schema-reader.vercel.app/), [**Power BI**](https://www.microsoft.com/en-us/power-platform/products/power-bi)
2. databases - [**DuckDB**](https://duckdb.org/), [**BIM Lakehouse**](https://bimlakehouse.com) 
3. serialization - [**Parquet**](https://parquet.apache.org/)
4. interactive visualization - [**Ara 3D Studio**](https://github.com/ara3d/ara3d-studio)

## What is a Schema? 

A schema describes the meaning, relationships, and structure of data. The **BIM Open Schema** project is agnostic of the specific serialization format
(e.g. you could use JSON or FlatBuffer), but it is optimized for structured self-describing columnar binary data formats, particularly **Parquet**.  

## Why do we Recommend Parquet?

[**Parquet**](https://parquet.apache.org/) is very efficient to read/write, easy to use, compact, with wide support. It is also self-describing and validating. 
Unlike with CSV or JSON, a Parquet importer can determine exactly how many rows and columns of data there are and what data types are contained.  

## Repo Contents

This repo provides:

1. Official specification [in the form of valid C# code](https://github.com/ara3d/bim-open-schema/blob/main/spec).
2. [Sample test file](https://github.com/ara3d/bim-open-schema/tree/main/data/examples) generated from the Autodesk Revit 2025 Snowdon sample  

## Open-Source Samples and Tools

We provide a number of open-source tools and libraries in the [Ara 3D SDK repository](https://github.com/ara3d/ara3d-sdk):

- [Core Library](https://github.com/ara3d/ara3d-sdk/tree/main/src/Ara3D.BimOpenSchema)
- [Browser Tool for Windows (includes Excel and glTF exporter)](https://github.com/ara3d/ara3d-sdk/tree/main/wip/Ara3D.BimOpenSchema.Browser)  
- [IO Libraries](https://github.com/ara3d/ara3d-sdk/tree/main/wip/Ara3D.BimOpenSchema.IO)
- Revit 2025 Exporter- Coming soon

## Platform and Language Agnostic 

The schema is optimized for serialization to/from columnar and tabular data formats, such as those used by relational databases, 
but it is *not* tied to any one particular serialization format, and can be easily converted to many different 
formats for fast inspection in your tool of choice. 

This project comes with C# code which acts as the official specification, but the schema is platform independent 
and language agnostic. 

We welcome code contributions in any language. 

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
