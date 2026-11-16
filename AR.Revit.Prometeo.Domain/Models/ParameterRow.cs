
namespace AR.Revit.Prometeo.Domain.Models
{
    /// <summary>
    /// Clase creada para exportar parametros 
    /// </summary>
    public class ParameterRow
    {
        public string Id { get; init; } = string.Empty;
        public string Category { get; init; } = string.Empty;
        public string Family { get; init; } = string.Empty;
        public string TypeName { get; init; } = string.Empty;

        ///<summary> Nombre del parametro --> valor como texto</summary>
        public Dictionary<string, string> Values { get; init; } = new();
    }
}
