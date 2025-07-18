
using Ara3D.DataTable;

namespace Ara3D.BimOpenSchema;

public static class BIMDataExtension
{
    public static String Get(this BimData self, StringIndex index) => self.Strings[(int)index];
    public static Entity Get(this BimData self, EntityIndex index) => self.Entities[(int)index];
    public static Document Get(this BimData self, DocumentIndex index) => self.Documents[(int)index];
    public static Point Get(this BimData self, PointIndex index) => self.Points[(int)index];

    public static ParameterDescriptor Get(this BimData self, DescriptorIndex index) => self.Descriptors[(int)index];

    public static IEnumerable<EntityIndex> EntityIndices(this BimData self) 
        => Enumerable.Range(0, self.Entities.Count).Select(i => (EntityIndex)i);

    public static IEnumerable<DocumentIndex> DocumentIndices(this BimData self)
        => Enumerable.Range(0, self.Documents.Count).Select(i => (DocumentIndex)i);

    public static IEnumerable<DescriptorIndex> DescriptorIndices(this BimData self)
        => Enumerable.Range(0, self.Descriptors.Count).Select(i => (DescriptorIndex)i);

    public static IDataSet ToDataSet(this BimData self)
        => new ReadOnlyDataSet([
            self.Points.ToDataTable(nameof(self.Points)),
            self.Strings.ToDataTable(nameof(self.Strings)),
            self.Descriptors.ToDataTable(nameof(self.Descriptors)),
            self.Documents.ToDataTable(nameof(self.Documents)),
            self.Entities.ToDataTable(nameof(self.Entities)),
            self.Relations.ToDataTable(nameof(self.Relations)),
            self.DoubleParameters.ToDataTable(nameof(self.DoubleParameters)),
            self.IntegerParameters.ToDataTable(nameof(self.IntegerParameters)),
            self.StringParameters.ToDataTable(nameof(self.StringParameters)),
            self.EntityParameters.ToDataTable(nameof(self.EntityParameters)),
            self.PointParameters.ToDataTable(nameof(self.PointParameters)),
        ]);
}