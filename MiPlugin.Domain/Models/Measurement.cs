using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiPlugin.Domain.Models
{
    public class Measurement
    {
        public string Id { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Family { get; init; } = string.Empty;
        public string TypeName { get; init; } = string.Empty;
        public double? Area { get; init; } 
    }
}