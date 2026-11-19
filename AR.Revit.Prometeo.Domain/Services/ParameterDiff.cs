using AR.Revit.Prometeo.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AR.Revit.Prometeo.Domain.Services
{
    public static class ParameterDiff
    {
        ///<summary>Devuelve los parametros cuyo valor es distinto en excel y en el modelo</summary>
        public static List<ParameterChange> Compare(IEnumerable<ParameterRow> model, IEnumerable<ParameterRow> excel)
        {
            Dictionary<string, ParameterRow> modelById = model.ToDictionary(r => r.Id);
            var changes = new List<ParameterChange>();

            foreach(ParameterRow row in excel)
            {
                //si el id no existe en el modelo, se ignora
                if (!modelById.TryGetValue(row.Id, out ParameterRow? original))
                    continue; 
                foreach (var (name, newValue) in row.Values)
                {
                    //Si el elemento no tiene ese parametro, se ignora
                    if (!original.Values.TryGetValue(name, out string? oldValue))
                        continue;

                    if (oldValue.Trim() != newValue.Trim())
                        changes.Add(new ParameterChange
                        {
                            ElementId = row.Id,
                            Parameter = name,
                            OldValue = oldValue,
                            NewValue = newValue
                        });
                }
            }
            return changes;
        }
    }
}
