using System;
using System.Collections.Generic;
using System.Linq;
using Ara3D.Models;

namespace Ara3D.BimOpenSchema
{
    /// <summary>
    /// The object model is a convenient and denormalized representation of the data as objects.
    /// It is not canonical and is designed for common programming tasks. 
    /// </summary>
    public class BimObjectModel
    {
        public BimData Data;

        public List<DocumentModel> Documents { get; } = new();
        public List<EntityModel> Entities { get; } = new();
        public List<DescriptorModel> Descriptors { get; } = new();

        public BimObjectModel(BimData data)
        {
            Data = data;

            foreach (var d in data.Documents)
                Documents.Add(new DocumentModel
                {
                    Path = Get(d.Path),
                    Title = Get(d.Title)
                });

            Entities = data.
                EntityIndices().Select(Create).ToList();
            
            Descriptors = data.
                DescriptorIndices().Select(di => Create(di, data.Get(di))).ToList();

            foreach (var p in data.DoubleParameters)
                AddParameter(p.Entity, Create(p));

            foreach (var p in data.IntegerParameters)
                AddParameter(p.Entity, Create(p));

            foreach (var p in data.StringParameters)
                AddParameter(p.Entity, Create(p));

            foreach (var p in data.PointParameters)
                AddParameter(p.Entity, Create(p));

            foreach (var p in data.EntityParameters)
                AddParameter(p.Entity, Create(p));

            foreach (var r in data.Relations)
            {
                var source = Get(r.EntityA);
                var target = Get(r.EntityB);
                source.OutgoingRelations.Add(new RelationModel(r.RelationType, target));
                target.IncomingRelations.Add(new RelationModel(r.RelationType, source));
            }

            // For each element, add the corresponding instance struct to the entity.
            if (Entities.Count > 0)
            {
                var numElements = data.Geometry.GetNumElements();
                for (var i = 0; i < numElements; i++)
                {
                    var inst = data.Geometry.GetInstanceStruct(i);
                    var entityIndex = inst.EntityIndex;
                    if (entityIndex >= 0)
                    {
                        var em = Entities[entityIndex];
                        em.Instances.Add(inst);
                    }
                }
            }
        }

        public EntityModel Create(EntityIndex ei)
            => new(this, ei);

        public DescriptorModel Create(DescriptorIndex index, ParameterDescriptor desc) => new DescriptorModel
        {
            Index = index,
            Name = Data.Get(desc.Name),
            Units = Data.Get(desc.Units),
            Group = Data.Get(desc.Group),
            ParameterType = desc.Type
        };

        public EntityModel Get(EntityIndex ei) => Entities[(int)ei];
        public DescriptorModel Get(DescriptorIndex di) => Descriptors[(int)di];
        public Point Get(PointIndex pi) => Data.Get(pi);
        public string Get(StringIndex si) => Data.Get(si);

        public void AddParameter(EntityIndex ei, ParameterModel pm)
        {
            var e = Get(ei);
            e.ParameterValues[pm.Descriptor.Name] = pm.Value;
            e.Parameters.Add(pm);
        }

        public ParameterModel Create(ParameterDouble p) => new(p.Value, Get(p.Descriptor));
        public ParameterModel Create(ParameterInt p) => new(p.Value, Get(p.Descriptor));
        public ParameterModel Create(ParameterString p) => new(Get(p.Value), Get(p.Descriptor));
        public ParameterModel Create(ParameterEntity p) => new(Get(p.Value), Get(p.Descriptor));
        public ParameterModel Create(ParameterPoint p) => new(Get(p.Value), Get(p.Descriptor));
    }

    public class EntityModel
    {
        // Stored data
        public BimObjectModel Model { get;  }
        public EntityIndex Index { get; }
        public List<InstanceStruct> Instances { get; } = new();

        // Always accessible data 
        public BimData Data => Model.Data;
        public Entity Entity => Model.Data.Get(Index);
        public DocumentModel Document => Model.Documents[(int)Entity.Document];
        public string DocumentTitle => Document.Title;
        public long LocalId => Entity.LocalId;
        public string GlobalId => Entity.GlobalId;
        public string Category => Data.Get(Entity.Category);
        public string Name => Data.Get(Entity.Name);
        public bool HasGeometry => Instances.Count > 0;
        
        // Commonly present data stored in parameters
        public string CategoryType => GetParameterAsEntity(CommonRevitParameters.ObjectCategory)?.GetParameterAsString(CommonRevitParameters.CategoryBuiltInType);
        public string ClassName => GetParameterAsString(CommonRevitParameters.ObjectTypeName);
        public string LevelName => GetParameterAsEntity(CommonRevitParameters.ElementLevel)?.Name;
        public string GroupName => GetParameterAsEntity(CommonRevitParameters.ElementGroup)?.Name;
        public string AssemblyName => GetParameterAsEntity(CommonRevitParameters.ElementAssemblyInstance)?.Index.ToString();
        public int WorksetId => GetParameterAsInt(CommonRevitParameters.ElementWorksetId);
        public double Elevation => GetParameterAsEntity(CommonRevitParameters.ElementLevel)?.GetParameterAsNumber(CommonRevitParameters.LevelElevation) ?? 0;
 
        // Family instance parameters
        public string FamilyType => GetParameterAsEntity(CommonRevitParameters.FIFamilyType)?.Name;
        public string RoomName => GetParameterAsEntity(CommonRevitParameters.FISpace)?.Name;

        public EntityModel(BimObjectModel model, EntityIndex ei)
        {
            Model = model;
            Index = ei;
        }

        public Dictionary<string, object> ParameterValues { get; } = new();
        public List<RelationModel> OutgoingRelations { get; } = new();
        public List<RelationModel> IncomingRelations { get; } = new();
        public List<ParameterModel> Parameters { get; } = new();

        public override string ToString()
            => $"{Name}(#{LocalId})";

        public int GetParameterAsInt(string name)
            => (int)ParameterValues.GetValueOrDefault(name, -1);

        public string GetParameterAsString(string name)
            => ParameterValues.GetValueOrDefault(name) as string;

        public double GetParameterAsNumber(string name)
            => (double)ParameterValues.GetValueOrDefault(name);

        public EntityModel GetParameterAsEntity(string name)
            => ParameterValues.GetValueOrDefault(name) as EntityModel;
    }

    public class RelationModel
    {
        public RelationType RelationType { get; init; }
        public EntityModel Target { get; init; }

        public RelationModel(RelationType type, EntityModel target)
        {
            RelationType = type;
            Target = target;
        }
    }

    public class ParameterModel
    {
        public object Value { get; }
        public DescriptorModel Descriptor { get; }

        public ParameterModel(object value, DescriptorModel descriptor)
        {
            Value = value;
            Descriptor = descriptor;
        }

        public override string ToString()
            => $"{Descriptor.Name} ({Descriptor.ParameterType}) = {Value}";
    }

    public class DocumentModel
    {
        public string Path { get; init; }
        public string Title { get; init; }
    }

    public class DescriptorModel
    {
        public DescriptorIndex Index { get; init; }
        public string Name { get; init; }
        public string Units { get; init; }
        public string Group { get; init; }
        public ParameterType ParameterType { get; init; }

        public Type DotNetType
        {
            get
            {
                switch (ParameterType)
                {
                    case ParameterType.Int:
                        return typeof(int);
                    case ParameterType.Double:
                        return typeof(double);
                    case ParameterType.Entity:
                        return typeof(int);
                    case ParameterType.String:
                        return typeof(string);
                    case ParameterType.Point:
                        return typeof(Point);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }
}
