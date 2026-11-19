using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Revit.Prometeo.Domain.Models
{
    /// <summary>Un parámetro de un elemento cuyo valor ha cambiado en el Excel.</summary>
    public class ParameterChange
    {
        public string ElementId { get; init; } = string.Empty;
        public string Parameter { get; init; } = string.Empty;
        public string OldValue { get; init; } = string.Empty;
        public string NewValue { get; init; } = string.Empty;

    }
}
