# Decisiones

Registro de decisiones del proyecto. Cada una indica el contexto, lo decidido, las alternativas descartadas y sus consecuencias. Las decisiones sustituidas se mantienen y se marcan como tales.

## D01 — Plugin de Revit como TFG

- **Contexto**: había que elegir proyecto intermodular; la alternativa era una app clasificadora de gastos.
- **Decisión**: plugin para Revit, aprobado por el tutor.
- **Motivo**: aplica el stack de trabajo real (C#, WPF, MVVM) sobre una API profesional.

## D02 — Revit 2025 y .NET 8

- **Decisión**: una única versión objetivo, Revit 2025 sobre `net8.0-windows`.
- **Descartado**: soporte de versiones anteriores (.NET Framework 4.8), que obliga a compilar para varios destinos.

## D03 — Solución en varios proyectos (actualizada el 8 de octubre)

- **Contexto**: la API de Revit solo funciona dentro de Revit; el código que la usa no se puede probar de forma automática.
- **Decisión**:

| Proyecto | Contiene | Referencia a |
| --- | --- | --- |
| `AR.Revit.Prometeo.Domain` | Modelos y lógica (agrupación, totales). Sin Revit | Nada |
| `AR.Revit.Prometeo.UI` | Vistas WPF, ViewModels y exportación a Excel | Domain |
| `AR.Revit.Prometeo` | Arranque, cinta, comandos y lectura del modelo de Revit | Domain, UI, API de Revit |
| `AR.Revit.Prometeo.Tests` | Pruebas unitarias | Domain y UI |

- **Regla**: si una clase necesita `using Autodesk.Revit`, va en `AR.Revit.Prometeo`; si no, en Domain o en UI.
- **Consecuencia**: Domain y UI se prueban sin abrir Revit.

## D04 — Inversión de dependencias en la lectura del modelo

- **Decisión**: el lector de Revit produce objetos de Domain (`Measurement`); el resto de la aplicación no conoce la API.
- **Consecuencia**: el comando solo coordina: crea el lector y el ViewModel (pasándole la función de lectura) y abre la ventana. La lectura ocurre al pulsar Medir.

## D05 — Conversión de unidades en la frontera

- **Contexto**: Revit devuelve siempre unidades internas (pies, pies²).
- **Decisión**: convertir a métricas al leer el parámetro, antes de crear el `Measurement`.
- **Consecuencia**: Domain trabaja siempre en metros.

## D06 — Sin base de datos (actualizada el 19 de noviembre)

- **Decisión**: persistencia en ficheros, validada por el tutor. La única persistencia implementada es la exportación a Excel; la configuración del usuario en JSON queda como trabajo futuro.

## D07 — Sin autenticación

- **Decisión**: el plugin no tiene inicio de sesión propio; se ejecuta dentro de Revit con la sesión y la licencia ya validadas.

## D08 — Sin presupuestos

- **Decisión**: el plugin mide, pero no asigna precios.

## D09 — Manifiesto de tipo Application

- **Decisión**: un único `.addin` de tipo `Application` que crea pestaña y botón al arrancar.

## D10 — Alcance funcional: medir, filtrar y exportar (actualizada el 9 de octubre; columnas, ver D25)

- **Contexto**: el planteamiento inicial incluía mediciones, filtro y modificación de parámetros.
- **Decisión**: una ventana modal con tres botones y un DataGrid:
  - **Medir**: carga todos los elementos del modelo con las columnas Categoría, Familia, Cantidad y Unidad, agrupados por categoría con su total.
  - **Filtrar**: reduce el DataGrid a muros, suelos, ventanas y puertas. Para volver a ver todo se pulsa de nuevo Medir.
  - **Exportar**: guarda en Excel un elemento por fila, en la ruta que elija el usuario.
- La modificación de parámetros pasa a trabajos futuros.
- **Motivo**: una función principal sólida y probada en lugar de tres a medias; encaja con el flujo diseñado en Miro.

## D11 — Medir lee todo el modelo; Filtrar trabaja sobre los datos ya leídos (actualizada el 9 de octubre)

> **Sustituida en parte por D20**: Filtrar vuelve a leer el modelo y clasifica con `ElementKind`, no con un diccionario de nombres de categoría. El resto de D11 sigue vigente.

- **Medir**: lee todos los elementos físicos del modelo con un `FilteredElementCollector`, excluyendo tipos y elementos no geométricos (vistas, materiales), que un colector sin filtrar también devuelve.
- **Filtrar**: no vuelve a consultar Revit; filtra las mediciones ya cargadas con un diccionario de categorías (muros, suelos, ventanas y puertas). Es inmediato y se puede probar sin Revit.
- **Lectura desde la ventana**: el comando entrega al ViewModel la función de lectura (inyección de dependencias), para que UI no dependa de la API de Revit.
- **Sin interfaces**: el ViewModel recibe un `Func<List<Measurement>>`, no una interfaz `IMeasurementReader`. Con una sola implementación y un solo método, la función basta; la interfaz sería sobrediseño.
- **Descartado**: leer el modelo en `ButtonCommand` antes de abrir la ventana. Es más simple, pero lee aunque el usuario no pulse Medir y retrasa la apertura en modelos grandes.
- **Exportador**: `ExcelExporter` va en `AR.Revit.Prometeo.UI` porque no depende de Revit; el ViewModel lo usa directamente, sin interfaz.

## D12 — Un único modelo `Measurement` en lugar de una clase por tipo

- **Contexto**: había clases `Muro`, `Suelo`, `Puerta` y `Ventana` que recalculaban área y perímetro a partir de sus dimensiones, y una interfaz que obligaba a implementar magnitudes que no aplicaban (`Volumen` lanzaba `NotImplementedException`).
- **Decisión**: una sola clase `Measurement` con área opcional (`double?`), leída directamente de Revit.
- **Motivo**: en el flujo todos los elementos se tratan igual (filas de una tabla); Revit ya calcula las magnitudes, descontando huecos, mejor que el cálculo propio.
- **Consecuencia**: lo que cambia por categoría es qué parámetro de Revit se lee, y eso se resuelve en el lector.

## D13 — Cantidad y unidad: m² o unidades (actualizada el 9 de octubre)

- **Decisión**: cada elemento se mide en m² si tiene área; si no (puertas, ventanas), cuenta como 1 ud. El total de cada grupo es la suma de m² o el número de unidades.
- **Descartado**: mostrar también el perímetro.
- **Implementación**: `MeasurementGroup`, con el área total a `null` cuando ningún elemento del grupo tiene área (para distinguir "no aplica" de "mide cero").

## D14 — Agrupación por categoría, con totales por grupo (confirmada el 9 de octubre)

- **Decisión**: el DataGrid muestra un grupo por categoría con su total (por ejemplo, "Suelos — 30,00 m²" o "Puertas — 12 ud.") y, al desplegarlo, cada elemento con su medición.
- **Descartado**: agrupar por familia de Revit.
- **Totales**: los calcula `MeasurementGroup` en Domain, porque así se pueden probar sin Revit. Se descarta la agrupación nativa del DataGrid, que agrupa y cuenta pero no suma.
- **Ubicación**: el agrupador va en Domain porque no necesita Revit, se puede probar y lo usa el ViewModel.

## D15 — Proyecto Infrastructure eliminado

- **Contexto**: se creó `MiPlugin.Infrastructure` para el acceso externo.
- **Decisión**: eliminarlo; la lectura de Revit va en `AR.Revit.Prometeo/Services`.
- **Motivo**: la única dependencia externa es Revit y `AR.Revit.Prometeo` ya depende de ella; un proyecto más no aísla nada y añade una DLL y referencias.
- **Revisión**: recuperarlo como proyecto si la exportación y la configuración crecen.

## D16 — Código en inglés

- **Decisión**: identificadores, comentarios y textos del código en inglés; la documentación y la memoria, en español.
- **Motivo**: convención habitual en el sector y coherencia con la API de Revit y .NET.

## D17 — MVVM sin librerías

- **Decisión**: `ViewModelBase` y `RelayCommand` escritos a mano.
- **Descartado**: `CommunityToolkit.Mvvm`.
- **Motivo**: son pocas líneas, se pueden explicar en la defensa y se evita cargar otra DLL dentro de Revit, con riesgo de conflictos de versión con otros plugins.

## D18 — Pruebas después de implementar

- **Decisión**: las pruebas unitarias del dominio y de los ViewModels se escriben tras implementar cada funcionalidad, no antes (sin TDD estricto).
- **Motivo**: el diseño de la API de Revit y de la ventana se está aprendiendo sobre la marcha; probar después de tener la funcionalidad estable evita reescribir tests.

## D19 — Nombre de la solución: AR.Revit.Prometeo (11 de noviembre)

- **Decisión**: renombrar `MiPlugin` a `AR.Revit.Prometeo` (carpetas, proyectos, espacios de nombres, XAML, `.addin` y documentación).
- **Cómo**: con `git mv`, para que git registre los cambios como renombrados y conserve el historial de cada archivo.
- **Consecuencia**: las entradas antiguas de la bitácora mantienen los nombres de su momento.

## D20 — Superficie solo en elementos anfitriones (11 de noviembre)

- **Contexto**: al probar en Revit, sanitarios, luminarias y ventanas aparecían medidos en m², porque las familias cargables también exponen un área calculada (`HOST_AREA_COMPUTED`).
- **Decisión**: leer el área solo si el elemento es un `HostObject` (muros, suelos, cubiertas, techos); el resto cuenta como 1 ud.
- **Descartado**: una lista de categorías (hay que mantenerla) y comparar nombres de categoría (depende del idioma de Revit).
- **Lectura**: el colector se queda con las instancias de categorías con cantidades de material (`HasMaterialQuantities`), que descarta vistas, niveles, cotas y tipos.

## D21 — Filtrar: clasificación propia e independiente del idioma (11 de noviembre)

- **Decisión**: un enum `ElementKind` (Other, Wall, Floor, Window, Door) en Domain. El lector lo asigna desde la categoría interna de Revit (`BuiltInCategory`), y `MeasurementFilter` conserva los tipos de una lista explícita (`HashSet`).
- **Motivo**: los nombres de categoría cambian con el idioma de Revit; `BuiltInCategory` no. Domain no depende de la API de Revit, así que el filtro se prueba sin abrirlo.
- **Lista explícita y no "todo menos Other"**: añadir un valor al enum no cambia el filtro sin decidirlo.
- **Filtrar vuelve a leer el modelo** (sustituye a parte de D11): Medir y Filtrar comparten el método que rellena la tabla (`ShowGroups`) y solo cambian los datos que le pasan. Así Filtrar funciona sin pulsar antes Medir y refleja los cambios hechos en Revit.

## D22 — Exportar lo que se ve (13 de noviembre)

- **Decisión**: el ViewModel guarda las mediciones mostradas cada vez que rellena la tabla, y Exportar usa esa lista: todo o filtrado, según la vista. El botón está desactivado mientras no hay datos (`CanExecute`).
- **Formato**: un elemento por fila con cabeceras (Categoría, Familia, Tipo, Id, Cantidad, Unidad); la cantidad se guarda como número para poder sumarla en Excel.
- **Errores**: solo se captura `IOException` (fichero abierto o bloqueado) y se muestra un aviso en la ventana, que sigue abierta. Los errores inesperados no se ocultan: los recoge el comando y Revit los muestra.

## D23 — Diálogo de guardado inyectado en el ViewModel (13 de noviembre)

- **Decisión**: el ViewModel recibe una función `Func<string?>` que pide la ruta y devuelve `null` si el usuario cancela. En Revit es `FileDialogs.AskExcelPath` (diálogo «Guardar como»); en las pruebas, una ruta temporal.
- **Motivo**: el ViewModel no abre ventanas; si lo hiciera, las pruebas se quedarían esperando a que alguien eligiera un fichero. Es el mismo patrón que la lectura del modelo (D11).

## D24 — Dependencias del plugin dentro de Revit (12 de noviembre)

- **Contexto**: Revit carga sus propias bibliotecas; una dependencia con otra versión puede fallar en tiempo de ejecución aunque compile.
- **Comprobación previa**: las dependencias de ClosedXML (`DocumentFormat.OpenXml`, `System.IO.Packaging`, `SixLabors.Fonts`) no están en la carpeta de Revit 2025, así que no hay conflicto.
- **Decisión**: `EnableDynamicLoading` en el proyecto del plugin, porque una biblioteca de clases no copia por defecto las DLL de sus paquetes; y `ExcludeAssets=runtime` en el paquete de la API de Revit, para no copiar sus DLL (la salida pasa de 49 MB a 9,7 MB).
- **Alternativa preparada**: exportar a CSV si ClosedXML hubiera dado conflicto.

## D25 — Interfaz: columnas y formato (11 de noviembre)

- **Decisión**: añadir las columnas Tipo e Id a la tabla, porque los elementos de una misma familia (por ejemplo, «Muro básico») no se distinguían. El Id permite localizarlos en Revit con «Seleccionar por ID».
- **Categoría**: no se muestra dentro de cada grupo, porque ya aparece en su cabecera; el Excel sí la incluye.
- **Formato numérico**: la ventana usa el idioma del sistema (`XmlLanguage`), para que los totales y la tabla usen el mismo separador decimal.

## D26 — Tema oscuro con estilos propios (13 de noviembre)

- **Decisión**: rediseño con tema oscuro y acento verde a partir de bocetos: menú lateral con iconos, grupos como tarjetas con el total a la derecha, tabla sin líneas, estado vacío y barra de estado con color según el resultado.
- **Cómo**: estilos y plantillas propios en `Window.Resources` (colores como recursos con nombre, `ControlTemplate` para botones y grupos, estilos implícitos para la tabla) y `DataTrigger` enlazados al ViewModel (`IsFiltered`, `IsError`, `ShownCount`), sin código en la vista.
- **Descartado**: librerías de temas (MaterialDesign, WPF-UI), por el riesgo de conflicto de DLL dentro de Revit y porque un plugin no tiene `App.xaml` donde cargarlas.
- **Iconos**: fuente Segoe Fluent Icons (Windows 11) con Segoe MDL2 Assets como respaldo (Windows 10), sin imágenes ni dependencias.
- **Contraste**: el verde de los botones (`#088923`) lleva texto blanco; para texto verde sobre fondo oscuro se usa un tono más claro (`#4CC66A`).

## D27 — Cinta definida en un diccionario (20 de noviembre)

- **Decisión**: `CreateMenu` (clase estática) sustituye a `CreateButton`. La cinta se describe en un diccionario: cada clave es un panel y su valor, la lista de botones del panel, cada uno como una tupla (id, texto, comando). Crea la pestaña una vez y recorre el diccionario para crear paneles y botones.
- **Motivo**: añadir un botón o un panel es añadir una línea al diccionario, sin tocar la lógica de creación. Cada botón lleva un id distinto, porque Revit no admite dos botones con el mismo nombre interno en un panel.
- **Sin estado**: `UIControlledApplication` se pasa como parámetro a cada método en lugar de guardarse en un campo estático.
