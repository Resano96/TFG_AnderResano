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

## Pendiente detectado en el código (5 de octubre, ver estado actualizado al final)

- `ButtonCommand` es `internal`; Revit necesita que sea `public`.
- `OnShutdown` lanza `NotImplementedException`; debe devolver `Result.Succeeded`.
- En `ButtonCommand`, la variable `largo` lee `HOST_AREA_COMPUTED` (área); la longitud es `CURVE_ELEM_LENGTH`.
- Falta comprobar parámetros nulos y capturar la cancelación con Esc.
- Quedarse con una sola clase de cinta y borrar las otras dos.
- Mover la lectura de Revit fuera del comando, a un lector que implemente una interfaz de Core.

## 7 de octubre de 2026 — Planteamiento en Miro y reorganización de la solución

**Hecho**

1. Tablero "TFG" en Miro con el flujo de la aplicación: botón en Revit → extraer elementos del modelo → elegir parámetros → mostrarlos en una ventana → filtrar → exportar.
2. Boceto de la ventana: botones Medir, Filtrar y Exportar, y una tabla de elementos.
3. Tipos de elementos a medir: muros, suelos, puertas y ventanas.
4. Proyectos nuevos: `MiPlugin.UI` (biblioteca de clases de WPF) y `MiPlugin.Infrastructure`.
5. `MiPlugin.Core` renombrado a `MiPlugin.Domain` (proyecto, carpeta, solución y referencias).
6. Primer commit y repositorio en GitHub (`Resano96/TFG_AnderResano`), con `.gitignore` para .NET y Revit.

**Problemas y solución**

| Problema | Causa | Solución |
| --- | --- | --- |
| La solución no compilaba | `Muro` y `Suelo` (Domain) implementaban una interfaz que estaba en Infrastructure | La interfaz se elimina; ver D12 |
| En GitHub la carpeta seguía llamándose Core | Solo se había renombrado el `.csproj` | `git mv` de la carpeta y rutas corregidas en `.slnx` y referencias |
| `FilteredElementCollector` traía vistas y materiales | Se recogía todo el modelo sin filtrar | Filtrar en el propio colector por categoría (`ElementMulticategoryFilter`) |

## 8 de octubre de 2026 — Modelo de mediciones y agrupación

**Hecho**

1. Código traducido a inglés (identificadores y textos).
2. Eliminadas las clases por tipo (`Muro`, `Suelo`, `Puerta`, `Ventana`) y la interfaz `Medir`.
3. `Measurement`: un elemento medido (Id, categoría, familia, tipo, área y perímetro opcionales).
4. `MeasurementGroup`: grupo de mediciones con recuento, área total, perímetro total y resumen.
5. Regla de visualización: se muestran área y perímetro si existen; si el elemento no tiene ninguno (puertas, ventanas), se muestra el recuento.
6. Diseño de la ventana: un desplegable por categoría con su total; al abrirlo, la tabla con cada elemento.

**Pendiente**

- Implementar `MeasurementGrouping` y `MeasurementsViewModel` (ahora son plantillas vacías).
- Crear `ViewModelBase`, la ventana `MeasurementsWindow` y conectar `ButtonCommand`.
- Lector de Revit en `MiPlugin/Services` (colector por categorías y conversión de unidades).
- `ButtonCommand` sigue siendo `internal` y `OnShutdown` lanza `NotImplementedException`.
- Decidir si se elimina `MiPlugin.Infrastructure` (D14).
- Tests de `MeasurementGroup` y del agrupador.

## 9 de octubre de 2026 — Funcionamiento definitivo de la ventana

Se concreta el funcionamiento a partir del diagrama de Miro:

1. El botón del plugin abre una ventana modal con tres botones y un DataGrid.
2. **Medir**: carga en el DataGrid todos los elementos del modelo con su cantidad y unidad, agrupados por categoría con su total.
3. **Filtrar**: reduce el DataGrid a muros, suelos, ventanas y puertas.
4. **Exportar**: guarda el contenido del DataGrid en Excel.

Decisiones del día (ver `02-decisiones.md`):

- Columnas de la tabla: Categoría, Familia, Cantidad y Unidad. Cantidad en m² si el elemento tiene área; si no, 1 ud. Se descarta el perímetro.
- Agrupación por categoría; los totales los calcula `MeasurementGroup`.
- Filtrar solo filtra; para volver a ver todo se pulsa Medir.
- Exportar: un elemento por fila, en la ruta que elija el usuario.
- El ViewModel recibe la función de lectura (`Func`), sin interfaz. El exportador va en `MiPlugin.UI`.
- `MiPlugin.Infrastructure` eliminado.
- Las pruebas se escriben después de implementar cada funcionalidad.

Memoria y documentación corregidas: la versión anterior describía una medición limitada a cuatro categorías, que no correspondía con este diseño. Se confirma la agrupación por categoría (no por familia).

## 11/11/2026 — Renombrado del proyecto

- La solución pasa de `MiPlugin` a `AR.Revit.Prometeo`: carpetas, `.csproj`, `.slnx`, referencias entre proyectos, namespaces, XAML, `.addin` y documentación (`git mv`, conserva el historial).
- Las entradas anteriores de este registro mantienen los nombres antiguos porque describen lo que se hizo en su momento.
- `RevitMeasurementReader` en `AR.Revit.Prometeo/Services`: recoge las instancias de categorías con cantidades de material (`HasMaterialQuantities`), lee familia y tipo, y el área (`HOST_AREA_COMPUTED`, convertida a m²).
- Solo se mide en m² lo que es anfitrión (`HostObject`: muros, suelos, cubiertas, techos). Las familias cargables (ventanas, sanitarios, luminarias…) también exponen un área, pero se cuentan en unidades.
- `ButtonCommand` usa el lector real en lugar de los datos de prueba, comprueba que haya un modelo abierto y captura errores (`Result.Failed` con mensaje).
- Tabla: columnas Tipo e Id para distinguir elementos de la misma familia; números con el formato del idioma del sistema.
- Corregida una errata en la ruta del `.addin` tras el renombrado.
- Filtrar: enum `ElementKind` en Domain (Other, Wall, Floor, Window, Door). El lector lo asigna desde `BuiltInCategory`, que no depende del idioma de Revit; Domain no conoce la API de Revit.
- `MeasurementFilter.MainElements` deja solo los tipos principales, con la lista explícita en un `HashSet` para que añadir valores al enum no cambie el filtro.
- El ViewModel comparte `ShowGroups` entre Medir y Filtrar; Filtrar vuelve a leer el modelo, así funciona sin pulsar Medir antes y muestra datos actualizados.
- Tests del filtro (Domain y ViewModel); se sustituye el test que comprobaba que Filtrar estaba desactivado.

## 12-13/11/2026 — Exportar a Excel

- ClosedXML en `AR.Revit.Prometeo.UI`. Antes de programar se comprobó que sus dependencias (`DocumentFormat.OpenXml`, `System.IO.Packaging`, `SixLabors.Fonts`) no están en la carpeta de Revit 2025, así que no hay conflicto de versiones.
- `EnableDynamicLoading` en el proyecto del plugin: una biblioteca de clases no copia por defecto las DLL de sus paquetes NuGet, y Revit no encontraría ClosedXML. El paquete de la API de Revit se marca con `ExcludeAssets=runtime` para que sus DLL no se copien (la salida pasa de 49 MB a 9,7 MB).
- `ExcelExporter.Export`: cabeceras en negrita, un elemento por fila y la cantidad como número.
- El ViewModel guarda en `_shown` lo que se está mostrando, de modo que se exporta lo que se ve (todo o filtrado). Exportar se desactiva mientras no hay datos.
- El diálogo «Guardar como» se inyecta en el ViewModel como `Func<string?>` (`FileDialogs.AskExcelPath`), igual que la lectura del modelo: el ViewModel no abre ventanas y en los tests se sustituye por una ruta temporal.
- Si el fichero está abierto en Excel, se captura solo `IOException` y se muestra un aviso en la propiedad `Status`, enlazada a la ventana con `OnPropertyChanged`; la ventana no se cierra.
- Tests de `ExcelExporter` (escriben y releen un .xlsx temporal) y de Exportar en el ViewModel. En los proyectos con `UseWPF` hay que añadir `using System.IO` a mano, porque .NET lo excluye de los usings implícitos para evitar el conflicto con `System.Windows.Shapes.Path`.
