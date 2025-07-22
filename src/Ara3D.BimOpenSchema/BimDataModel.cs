
using System.Collections.Generic;
using System.Linq;

namespace Ara3D.BimOpenSchema
{
    public class EntityModel
    {
        public EntityIndex Index { get; init; }
        public long LocalId { get; init; }
        public string Name { get; init; }
        public string Category { get; init; }
        public Dictionary<string, object> ParameterValues { get; } = new();
        public List<RelationModel> OutgoingRelations { get; } = new();
        public List<RelationModel> IncomingRelations { get; } = new();
        public List<ParameterModel> Parameters { get; } = new();

        public override string ToString()
            => $"{Name}(#{LocalId})";
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
            => $"{Descriptor.Name} ({Descriptor.Type}) = {Value}";
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
        public ParameterType Type { get; init; }
    }

    public class BimDataModel
    {
        public BimData Data;

        public List<DocumentModel> Documents { get; } = new();
        public List<EntityModel> Entities { get; } = new();
        public List<DescriptorModel> Descriptors { get; } = new();

        public BimDataModel(BimData data)
        {
            Data = data;
            
            foreach (var d in data.Documents)
                Documents.Add(new DocumentModel
                {
                    Path = Get(d.Path),
                    Title = Get(d.Title)
                });

            Entities = data.EntityIndices().Select(ei => Create(ei, data.Get(ei))).ToList();
            Descriptors = data.DescriptorIndices().Select(di => Create(di, data.Get(di))).ToList();

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
        }

        public EntityModel Create(EntityIndex ei, Entity e) => new EntityModel
        {
            LocalId = e.LocalId,
            Index = ei,
            Name = Data.Get(e.Name),
            Category = Data.Get(e.Category),
        };

        public DescriptorModel Create(DescriptorIndex index, ParameterDescriptor desc) => new DescriptorModel
        {
            Index = index,
            Name = Data.Get(desc.Name),
            Units = Data.Get(desc.Units),
            Group = Data.Get(desc.Group),
            Type = desc.Type
        };

        public EntityModel Get(EntityIndex ei) => Entities[(int)ei];
        public DescriptorModel Get(DescriptorIndex di) => Descriptors[(int)di];
        public Point Get(PointIndex pi) => Data.Get(pi);
        public string Get(StringIndex si) => Data.Get(si);

        public void AddParameter(EntityIndex ei, ParameterModel pm)
        {
            Get(ei).ParameterValues[pm.Descriptor.Name] = pm.Value;
            Get(ei).Parameters.Add(pm);
        }

        public ParameterModel Create(ParameterDouble p) => new(p.Value, Get(p.Descriptor));
        public ParameterModel Create(ParameterInt p) => new(p.Value, Get(p.Descriptor));
        public ParameterModel Create(ParameterString p) => new(Get(p.Value), Get(p.Descriptor));
        public ParameterModel Create(ParameterEntity p) => new(Get(p.Value), Get(p.Descriptor));
        public ParameterModel Create(ParameterPoint p) => new(Get(p.Value), Get(p.Descriptor));
    }
}
