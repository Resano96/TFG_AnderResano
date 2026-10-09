using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Domain.Models
{
    public class MeasurementGroup
    {
        public string Name { get; }
        public IReadOnlyList<Measurement> Items { get; }

        public MeasurementGroup(string name, IReadOnlyList<Measurement> items)
        {
            Name = name;
            Items = items;
        }

        public int Count => Items.Count;

        public double? TotalArea =>
            Items.Any(i => i.Area.HasValue) ? Items.Sum(i => i.Area ?? 0) : null;

        
        public string Summary
        {
            get
            {
                var parts = new List<string>();
                if (TotalArea.HasValue) parts.Add($"{TotalArea:N2} m²");
                return parts.Count > 0 ? string.Join(" · ", parts) : $"{Count} ud.";
            }
        }
    }
}