# Pasos dados

Bitácora del desarrollo. Una entrada por sesión de trabajo: qué se hizo, qué problema apareció y cómo se resolvió.

## 4 de octubre de 2026 — Puesta en marcha del plugin

**Hecho**

1. Proyecto de biblioteca de clases `MiPlugin` (`net8.0-windows`, x64, WPF activado) con el paquete NuGet `Revit_All_Main_Versions_API_x64` 2025.
2. Clase `App`, que implementa `IExternalApplication` y crea la cinta de opciones en `OnStartup`.
3. Tres versiones de la creación de la cinta, para practicar miembros estáticos y de instancia:
   - `CreateButton`: todo estático.
   - `CreateButtonNoStatic`: instancia con campos `readonly`.
   - `CreateButton1`: instancia con nombres recibidos por parámetro.
4. Comando `ButtonCommand` (`IExternalCommand`) asociado al botón.
5. Manifiesto `TFG_AnderResano.addin` de tipo `Application`, copiado a `%AppData%\Autodesk\Revit\Addins\2025\`.

**Problemas y solución**

| Problema | Causa | Solución |
| --- | --- | --- |
| "El montaje MiPlugin\MiPlugin.dll no existe" | `<Assembly>` con ruta relativa a la carpeta de complementos | Ruta absoluta a `bin\Debug\net8.0-windows\MiPlugin.dll` |
| "Falta el valor de nodo de ID de complemento" | `<AddInId>` vacío en un manifiesto antiguo (`RevitPlugin.addin`) | GUID real y un único manifiesto en la carpeta |
| El botón no aparecía tras recompilar | Revit bloquea la DLL mientras está abierto | Cerrar Revit antes de compilar |
| Pestaña creada pero vacía | `CreateRibbonPanel(nombrePanel)` sin pestaña: el panel iba a *Complementos* | Usar `CreateRibbonPanel(tab, panel)`, con la pestaña primero |

## 5 de octubre de 2026 — Primera lectura del modelo y separación en capas

**Hecho**

1. Solución dividida en proyectos: `MiPlugin.Core` (lógica, sin API de Revit) referenciado desde `MiPlugin`.
2. Clase `Muro` en `MiPlugin.Core.Models`.
3. `ButtonCommand` pide un elemento con `PickObject`, lee sus parámetros, crea un `Muro` y muestra el resultado en un `TaskDialog`.
4. Memoria reorganizada según el esqueleto del centro, con los requisitos en formato RFTP.

**Problemas y solución**

| Problema | Causa | Solución |
| --- | --- | --- |
| El diálogo salía vacío | `AsString()` sobre un parámetro numérico devuelve `null` | `AsDouble()` para calcular o `AsValueString()` para mostrar |
| No se podía usar `Muro` desde el comando | Faltaba `using MiPlugin.Core.Models` en ese archivo | Añadir el `using`; la clase debe ser `public` |
| Los valores no coincidían con Revit | `AsDouble()` devuelve unidades internas (pies, pies², pies³) | `UnitUtils.ConvertFromInternalUnits` al leer |

## Pendiente detectado en el código

- `ButtonCommand` es `internal`; Revit necesita que sea `public`.
- `OnShutdown` lanza `NotImplementedException`; debe devolver `Result.Succeeded`.
- En `ButtonCommand`, la variable `largo` lee `HOST_AREA_COMPUTED` (área); la longitud es `CURVE_ELEM_LENGTH`.
- Falta comprobar parámetros nulos y capturar la cancelación con Esc.
- Quedarse con una sola clase de cinta y borrar las otras dos.
- Mover la lectura de Revit fuera del comando, a un lector que implemente una interfaz de Core.
