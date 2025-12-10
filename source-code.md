# Contributing to and Understanding the Open Source Code

This repository only contains a specification document, and sample file. Our core tools and libraries are maintained in the [Ara 3D SDK repository](https://github.com/ara3d/ara3d-sdk):

## Code Organization  

The Ara 3D SDK is a mono-repository for all of the open-source C# and Plato development done at Ara 3D. 

Source code is split into two main areas:

1. [`src`](https://github.com/ara3d/ara3d-sdk/tree/main/src) - core libraries, minimial dependencies, cross-platform, and .NET 8 compatible
2. [`ext`](https://github.com/ara3d/ara3d-sdk/tree/main/ext) - additional libraries with platform dependencies (e.g., Windows specific) or 3rd party library dependencies (e.g., Autodesk Revit)

## Core Library 

The BIM Open Schema core library can be found at https://github.com/ara3d/ara3d-sdk/tree/main/src/Ara3D.BimOpenSchema . This library contains the:

- data structures used for raw serialization
- names and types of common Revit parameters
- an object model representation for more convenient programmatic access

## Code As Specification

The official specification of the current version of BIM Open Schema are the following files 

- https://github.com/ara3d/ara3d-sdk/blob/main/src/Ara3D.BimOpenSchema/BimOpenSchema.cs
- https://github.com/ara3d/ara3d-sdk/blob/main/src/Ara3D.BimOpenSchema/BimGeometry.cs

Which are mirrored in this repository: 

- https://github.com/ara3d/bim-open-schema/blob/main/spec/BimOpenSchema.cs
- https://github.com/ara3d/bim-open-schema/blob/main/spec/BimGeometry.cs

In the case of a discrepancy, please let us know via issues, but the former shall take precedence over the latter.  

## Revit Exporter 

The code for the Revit exporter can be found at: 

- [Revit 2025 Exporter](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.BimOpenSchema.Revit2025)

It has a dependency on a library called: 

- [Ara3D.Bowerbird.Revit.Samples](https://github.com/ara3d/ara3d-sdk/tree/main/ext/Ara3D.Bowerbird.RevitSamples)

This way it can be used from the Bowerbird plug-in as a script or from the Browser plug-in.   

Two files in particular do the bulk of the work:

- [BimOpenSchemaRevitBuilder.cs](https://github.com/ara3d/ara3d-sdk/blob/main/ext/Ara3D.Bowerbird.RevitSamples/BimOpenSchemaRevitBuilder.cs)
- [MeshGatherer.cs](https://github.com/ara3d/ara3d-sdk/blob/main/ext/Ara3D.Bowerbird.RevitSamples/MeshGatherer.cs)

## Dependencies and Helpers 

There are several additional projects which are used together to enable the workflows:


https://github.com/ara3d/bim-open-schema/blob/main/spec/BimOpenSchema.cs
