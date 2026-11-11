namespace AR.Revit.Prometeo.Domain.Models
{
    public class Measurement
    {
        public string Id { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Family { get; init; } = string.Empty;
        public string TypeName { get; init; } = string.Empty;
        public double? Area { get; init; }
        public ElementKind Kind { get; init; } = ElementKind.Other;
        public double Quantity => Area ?? 1;
        public string Unit => Area.HasValue ? "m²" : "ud";
    }
}