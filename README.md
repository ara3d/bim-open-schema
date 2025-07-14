# BIM Open Schema 

**BIM Open Schema** is a portable, efficient, and analytics‑friendly snapshot of a potentially federated BIM model that can be written 
once and then queried from any data tool - without Revit/IFC APIs in the loop

It is *not* tied to any one particular serialization format, and can be easily converted to many different formats for fast inspection 
in your tool of choice. 

## 🎯Target Workflows

1. **ETL pipe**: Revit/IFC → exporter → Parquet/DuckDB → downstream BI / ML.
2. **Quick inspection**: Open the Parquet/DuckDB database with SQL, PowerBI or Python/Pandas.
3. **Inter‑tool hand‑off**: Share a small, self‑contained bundle instead of heavyweight RVT/IFC when geometry is not required.

## 🧑‍🤝‍🧑Target Users

Data scientists, BI analysts and application developers who need properties, relationships, and additional BIM data without 
geometry. 

## 📐 Design Principles

• Column‑oriented storage: Each list maps cleanly to a Parquet column chunk or a DuckDB table.
• String & point interning: Repeated values are stored once and referenced by a typed index, keeping files small.
• EAV‑flavoured parameters: A minimal core (Entity, Descriptor) plus type‑specific value tables yields flexibility while preserving strong types.
• Relation set: A single EntityRelation edge list expresses most graph‑like BIM relationships found in Revit or IFC.

## 🤔 What is ETL? 

ETL (Extract, Transform, and Load) is a three-phase computing process where data is extracted from an input source, transformed (including cleaning), and loaded into an output data container.

## 📝Serialization Formats (Readers and Writers) 

We are in the process of developing tools and examples to convert BIM Open Schema to/from:

- [DuckDB](https://duckdb.org/) - A simple, fast, open-source database system optimized for in-process analytical work.
- [Apache Parquet](https://parquet.apache.org/) - an efficient, open source, column-oriented data file format with wide tooling support.
- [JSON](https://json.org) - A lightweight and ubiquitous human-readable format for exchanging data over the web.
- [MessagePack](https://msgpack.org/) - A compact and efficent binary format for interchange that can be used as a replacement for JSON.

## 👥 Contributors and Supporters

Supporting this project is as simple as [providing feedback](https://github.com/ara3d/bim-open-schema/issues/new?template=feedback.md).

Active contributors are (in alphabetical order): 

* Christopher Diggins - Ara 3D
* Daryl Irvine - DG Jones and Partners 
* Karim Daw - Gensler
* Tomo Sugeta - Cundall
* Yskert Schindel - Vyssuals
