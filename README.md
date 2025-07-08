# Ara3D Revit Schema 

Ara 3D Revit Schema is a joint project to define a standard for structured schema for BIM data represented within Revit.

## Why Schemas Matter

The choice of schema affects the complexity and performance of tools that work with data. 

A well designed schema:

- speeds up ETL workflows
- leads to compact data representations with minimal duplication  
- contains the data required for common analysis 
- minimizes the needs of complex joins

Having a predefined schema that is jointly defined by experts significantly reduces the cost and effort of developing tools for unlocking the potential of BIM data.  

## Design Goals of Ara 3D Revit Schema

The Ara 3D Revit Schema is optimized for the following design goals in order:

1. Efficiently extract data out of Revit
2. Efficiently import into columnar data formats and relational databases.
3. Simplicity and ease of use for analytics and machine learning  

## Serialization Formats (Readers and Writers) 

A Schema should be agnostic of any specific serialization format. We provide tools and examples to convert Ara 3D Revit Schema to/from:

- Revit (via Plug-ins)
- JSON
- MessagePack
- Parquet
- BFAST  
