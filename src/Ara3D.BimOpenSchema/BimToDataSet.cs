using Ara3D.DataTable;
using BIMOpenSchema;

namespace Ara3D.BimOpenSchema
{
    public static class BimToDataSet
    {
        public static IDataSet ToDataSet(this BIMData self)
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
}
