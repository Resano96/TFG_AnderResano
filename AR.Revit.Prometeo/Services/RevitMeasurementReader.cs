using Autodesk.Revit.DB;
using AR.Revit.Prometeo.Domain.Models;

namespace AR.Revit.Prometeo.Services
{
    public class RevitMeasurementReader
    {
        //Document--> es el modelo de revit abierto, se usa con readonly para evitar que cambie

        private readonly Document _doc;

        //se recibe doc en el constructor para que solo sepa leer, le da igual quien le pase el doc
        public RevitMeasurementReader(Document doc) => _doc = doc;
        /// <summary>
        /// Lee el modelo
        /// </summary>
        /// <returns>Lista de Measurement</returns>
        public List<Measurement> ReadModel()
        {
            return new FilteredElementCollector(_doc)
                //new FilteredElementCollector(_doc) es el “buscador” de Revit.
                //Empieza con todos los elementos del modelo, que pueden ser miles: vistas, niveles, tipos, cotas…
                .WhereElementIsNotElementType()
                //.WhereElementIsNotElementType() quita los tipos y deja solo los elementos colocados(instancias).
                //Si no, cada tipo de puerta contaría como una puerta más.
                .Where(e => e.Category != null && e.Category.HasMaterialQuantities)
                //HasMaterialQuantities deja solo los elementos físicos (muros, suelos, puertas, ventanas…).
                .Select(ToMeasurement)
                .ToList();
        }

        ///<summary> Lee el area calculada del elemento y la convierte a m2</summary>
        ///<remarks>Aqui se filtra si el elemento es de la clase de anfitriones, como muros, suelos o techos.
        ///Tambien se filtra si no tiene un valor en el area
        ///de esta forma nos aseguramos medir en m2 lo que buscamos</remarks>
        /// <param name="element">Elemento del que quieres leer el area</param>
        ///<returns> El area en m2, o null si el elemento no tiene area</returns>
        private static double? ReadArea(Element element)
        {
            if (element is not HostObject)
                return null;

            Parameter area = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED);

            if (area == null || !area.HasValue)
                return null;

            return UnitUtils.ConvertFromInternalUnits(area.AsDouble(), UnitTypeId.SquareMeters);
        }

        /// <summary>
        /// Transforma un elemento en un objeto de la clase Measurement
        /// </summary>
        /// <param name="element">Elemento a transformar</param>
        /// <returns>Objeto de la clase measurement</returns>
        private Measurement ToMeasurement(Element element)
        {
            ElementType? type = _doc.GetElement(element.GetTypeId()) as ElementType;

            return new Measurement
            {
                Id = element.Id.Value.ToString(),
                Category = element.Category.Name,
                Family = type?.FamilyName ?? string.Empty,
                TypeName = type?.Name ?? element.Name,
                Area = ReadArea(element)
            };
        }
    }
}
