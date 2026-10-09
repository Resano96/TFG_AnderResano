using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiPlugin.Domain.Models;

namespace MiPlugin.Domain.Services
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
