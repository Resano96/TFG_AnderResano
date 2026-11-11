namespace AR.Revit.Prometeo.Domain.Models
{
    /// <summary>
    /// Enum de categorias que se quieren filtrar. Actualmente Wall, Floor, Window, Door, Other
    /// </summary>
    //Se usa enum porque es un tipo cerrado de valores, si lo escribes mal en algun lado, no compila. Si lo haces con un string si compila y es mas dificil de encontrar el fallo
    //Se pone primero other porque asi si no cumple con ninguno de los otros asigna other
    public enum ElementKind
    {
        Other,
        Wall,
        Floor,
        Window,
        Door
    }
}
