using AR.Revit.Prometeo.Domain.Models;
using Autodesk.Revit.DB;
using System.Globalization;

namespace AR.Revit.Prometeo.Services
{
    /// <summary>
    /// Lee del modelo de Revit los parámetros editables de los elementos principales
    /// (muros, suelos, ventanas y puertas) y los convierte en filas para el Excel.
    /// </summary>
    public class RevitParameterReader
    {
        private readonly Document _doc;
        public RevitParameterReader(Document doc) => _doc = doc;

        /// <summary>Lee los parámetros editables de muros, suelos, ventanas y puertas.</summary>
        /// <returns>Una fila por elemento, con sus datos y sus parámetros editables.</returns>        
        public List<ParameterRow> ReadMainElements() =>
            new FilteredElementCollector(_doc)
                .WhereElementIsNotElementType()
                .Where(e => e.Category != null
                            && RevitMeasurementReader.ToKind(e.Category) != ElementKind.Other)
                .Select(ToRow)
                .ToList();

        /// <summary>Convierte un elemento de Revit en una fila con sus parámetros editables.</summary>
        /// <param name="element">Elemento del modelo que se quiere convertir.</param>
        /// <returns>Fila con el Id, categoría, familia, tipo y parámetros del elemento.</returns>
        private ParameterRow ToRow(Element element)
        {
            ElementType? type = _doc.GetElement(element.GetTypeId()) as ElementType;

            var row = new ParameterRow
            {
                Id = element.Id.Value.ToString(),
                Category = element.Category.Name,
                Family = type?.FamilyName ?? string.Empty,
                TypeName = type?.Name ?? element.Name
            };

            foreach (Parameter p in element.Parameters)
            {
                // TryAdd: si dos parámetros se llaman igual (sistema y compartido), se queda el primero
                if (IsEditable(p))
                    row.Values.TryAdd(p.Definition.Name, ReadValue(p));
            }
            return row;
        }
        /// <summary>
        /// Indica si el usuario puede cambiar el parámetro desde el Excel:
        /// no es de solo lectura y guarda texto, un entero o un número.
        /// </summary>
        /// <param name="p">Parámetro que se quiere comprobar.</param>
        /// <returns>true si es editable; false si es de solo lectura o de otro tipo (por ejemplo, ElementId).</returns>
        private static bool IsEditable(Parameter p) =>
            !p.IsReadOnly
            && (p.StorageType is StorageType.String or StorageType.Integer or StorageType.Double);

        /// <summary>Lee el valor del parámetro como texto, según cómo lo guarda Revit.</summary>
        /// <param name="p">Parámetro que se quiere leer.</param>
        /// <returns>El valor como texto; vacío si no tiene valor.</returns>
        private static string ReadValue(Parameter p) => p.StorageType switch
        {
            StorageType.String => p.AsString() ?? string.Empty,
            StorageType.Integer => p.AsInteger().ToString(CultureInfo.InvariantCulture),
            StorageType.Double => ReadDouble(p),
            _ => string.Empty
        };

        /// <summary>
        /// Lee un parámetro numérico y lo convierte de las unidades internas de Revit (pies)
        /// a las unidades del proyecto.
        /// </summary>
        /// <param name="p">Parámetro numérico (StorageType.Double).</param>
        /// <returns>El número como texto, con punto decimal y como máximo 6 decimales.</returns>
        private static string ReadDouble(Parameter p)
        {
            double value = p.AsDouble();

            // Solo se convierten los números con unidad (longitud, área…); los "puros" no tienen unidad
            if (UnitUtils.IsMeasurableSpec(p.Definition.GetDataType()))
                value = UnitUtils.ConvertFromInternalUnits(value, p.GetUnitTypeId());

            // Redondeado para evitar que salgan decimales extra y los cuente como cambio
            return Math.Round(value, 6).ToString(CultureInfo.InvariantCulture);
        }

    }
}
