using AR.Revit.Prometeo.Domain.Models;

namespace AR.Revit.Prometeo.Domain.Services
{
    public static class MeasurementFilter
    {
        //se usa HashSet porque es una coleccion sin repetidos.
        //se usa readonly porque se crea una vez y no busca que se modifique
        /// <summary>
        /// Tipos principales de datos que vamos a filtrar.
        /// </summary>
        private static readonly HashSet<ElementKind> MainKinds =
            [ElementKind.Wall, ElementKind.Floor, ElementKind.Window, ElementKind.Door];

        /// <summary>
        /// Recibe una lista de mediciones y devuelve solo las que son muro, suelo, ventana o puerta.
        /// </summary>
        /// <param name="measurements">Recibe cualquier coleccion que se pueda recorrer</param>
        /// <returns>Lista con solo los elementos principales</returns>
        public static List<Measurement> MainElements(IEnumerable<Measurement> measurements) =>
            measurements.Where(m => MainKinds.Contains(m.Kind)).ToList();
    }
}
