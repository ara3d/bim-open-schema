# BIM Open Schema 

**BIM Open Schema** is a joint project to define a standard for structured schema for BIM data represented within AEC Design tools for ETL workflows.

> ETL (Extract, Transform, and Load) is a three-phase computing process where data is extracted from an input source, transformed (including cleaning), and loaded into an output data container.

The Schema is expressed formally as a [C# source file](https://github.com/ara3d/bim-open-schema/blob/main/schema.cs).  

## ✨ Key Benefits

Having a predefined schema that is jointly defined by experts significantly reduces the cost and effort of developing tools for unlocking the potential of BIM data. 
We can start building from a shared ecosystem of tools and libraries. 

The choice of schema affects the complexity and performance of tools that work with data. 

A well designed schema:

- speeds up ETL workflows
- leads to compact data representations with minimal duplication  
- contains the data required for common analysis 
- minimizes the needs of complex joins

## 📐 Guiding Principles

The BIM Open Schema is optimized for the following design goals in order:

1. Efficiently extract data out of popular AEC design tools
2. Efficiently import into columnar data formats and relational databases.
3. Simplicity and ease of use for analytics and machine learning  

## 📝Serialization Formats (Readers and Writers) 

A schema should be agnostic of any specific serialization format. 

We are in the process of developing  tools and examples to convert BIM Open Schema to/from:

- Revit (via Plug-ins)
- JSON
- MessagePack
- Parquet
- BFAST  

## 👥 Contributors and Supporters

Supporting this project is as simple as [providing feedback](https://github.com/ara3d/bim-open-schema/issues/new?template=feedback.md).

