# BIM Open Schema 

![NuGet Version](https://img.shields.io/nuget/v/Ara3D.BimOpenSchema.IO)

>  _BIM Analytics via Parquet and DuckDB_

**BIM Open Schema** is an open formal specification of BIM data that is designed for modern 
analytics tools and optimized for columnar data formats like [**Parquet**](https://parquet.apache.org/) and 
[**DuckDB**](https://duckdb.org/). 

## 📖 Repo Contents

This repo provides:

1. Official specification [in the form of valid C# code](https://github.com/ara3d/bim-open-schema/blob/main/src/Ara3D.BimOpenSchema/BIMOpenSchema.cs).
2. [Sample test file](https://github.com/ara3d/bim-open-schema/tree/main/data/input) generated from the Snowdon sample 
3. C# libraries and tests files for reading / writing BIM Open Schema data, also [distributed on Nuget](https://www.nuget.org/packages/Ara3D.BimOpenSchema.IO). 
4. WPF-Based GUI Sample  

<img width="776" height="432" alt="image" src="https://github.com/user-attachments/assets/1dc9a766-32b5-47f7-b78c-49b0167993a9" />

## 🌐 Platform and Language Agnostic 

The schema is optimized for columnar data formats, but it is *not* tied to any one particular serialization format, 
and can be easily converted to many different formats for fast inspection in your tool of choice. 

This project comes with C# code which acts as the official specification, but the schema is platform independent 
and language agnostic. 

We welcome code contributions in any language. 

## 🎯Target Workflows

1. **ETL pipe**  :  Revit/IFC -> Parquet/DuckDB -> downstream BI/ML.
2. **Quick inspection**  :  Open the Parquet/DuckDB database with SQL, PowerBI or Python/Pandas.
3. **Inter‑tool hand‑off**  :  Share a small, self‑contained bundle instead of heavyweight RVT/IFC when geometry is not required.

## 🧑‍🤝‍🧑Target Users

Data scientists, BI analysts and application developers who need properties, relationships, and additional BIM data without 
geometry. 

## 📐 Design Principles

- _Column‑oriented storage_: Each list maps cleanly to a Parquet column chunk or a DuckDB table.
- _String & point interning_: Repeated values are stored once and referenced by a typed index, keeping files small.
- _EAV‑flavoured parameters_: A minimal core (Entity, Descriptor) plus type‑specific value tables yields flexibility while preserving strong types.
- _Relation set_: A single EntityRelation edge list expresses most graph‑like BIM relationships found in Revit or IFC.

## 🤔 What is ETL? 

ETL (Extract, Transform, and Load) is a three-phase computing process where data is extracted from an input source, 
transformed (including cleaning), and loaded into an output data container.

## 📝Serialization Formats (Readers and Writers) 

We provide tools and examples to convert BIM Open Schema to/from:

- [DuckDB](https://duckdb.org/) - A simple, fast, open-source database system optimized for in-process analytical work.
- [Parquet](https://parquet.apache.org/) - an efficient, open source, column-oriented data file format with wide tooling support.
- [JSON](https://json.org) - A lightweight and ubiquitous human-readable format for exchanging data over the web.

To extract data from Revit you can use **[Bowerbird](https://github.com/ara3d/bowerbird)** and the command _"Export BIM Open Schema"_: 

<img width="1699" height="1002" alt="image" src="https://github.com/user-attachments/assets/a732808c-b18b-47fe-84fd-8886a50bcbd3" />

## 🔗 Related Projects

Some open-source projects which are related:

- [SVF2 to Parquet](https://github.com/wallabyway/vibe-duckdb-svf2-properties)
- [Fragments](https://github.com/ThatOpen/engine_fragment) 
- [VIM Format](https://github.com/vimaec/vim-format)
- [IFC5](https://github.com/buildingSMART/IFC5-development)

## 👥 Contributors and Supporters

Supporting and contributing to this project is as simple as [providing feedback](https://github.com/ara3d/bim-open-schema/issues/new?template=feedback.md).

Some of the people who have contributed are (in alphabetical order): 

* Ahmad Saleem Z - AnkerDB
* Christopher Diggins - Ara 3D
* Daryl Irvine - DG Jones and Partners 
* Karim Daw - Gensler
* Tomo Sugeta - Cundall
* Yskert Schindel - Vyssuals

We have an active [Discord server and discussion forum](https://discord.gg/u3MtaAASGu) that you can also join.

## 💼 Commercial Support and Services 

If you are interesting in professional help in leveraging the format and learning what you can do with it, reach out to 
us at [info@ara3d.com](mailto:info@ara3d.com).
