using AR.Revit.Prometeo.Domain.Models;

namespace AR.Revit.Prometeo.Domain.Services
{
    public static class MeasurementGrouping
    {
        public static List<MeasurementGroup> ByCategory(IEnumerable<Measurement> measurements)
        {
            return measurements
                .GroupBy(m => m.Category)
                .Select(g => new MeasurementGroup(g.Key, g.ToList()))
                .OrderBy(g => g.Name)
                .ToList();

        }

    }
}
