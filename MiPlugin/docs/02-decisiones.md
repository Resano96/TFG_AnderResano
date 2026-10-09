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
| `MiPlugin.Domain` | Modelos y lógica (agrupación, totales). Sin Revit | Nada |
| `MiPlugin.UI` | Vistas WPF, ViewModels y exportación a Excel | Domain |
| `MiPlugin` | Arranque, cinta, comandos y lectura del modelo de Revit | Domain, UI, API de Revit |
| `MiPlugin.Tests` | Pruebas unitarias | Domain y UI |

- **Regla**: si una clase necesita `using Autodesk.Revit`, va en `MiPlugin`; si no, en Domain o en UI.
- **Consecuencia**: Domain y UI se prueban sin abrir Revit.

## D04 — Inversión de dependencias en la lectura del modelo

- **Decisión**: el lector de Revit produce objetos de Domain (`Measurement`); el resto de la aplicación no conoce la API.
- **Consecuencia**: el comando solo coordina: crea el lector y el ViewModel (pasándole la función de lectura) y abre la ventana. La lectura ocurre al pulsar Medir.

## D05 — Conversión de unidades en la frontera

- **Contexto**: Revit devuelve siempre unidades internas (pies, pies²).
- **Decisión**: convertir a métricas al leer el parámetro, antes de crear el `Measurement`.
- **Consecuencia**: Domain trabaja siempre en metros.

## D06 — Sin base de datos

- **Decisión**: persistencia en ficheros (configuración en JSON, resultados en Excel). Validado por el tutor.

## D07 — Sin autenticación

- **Decisión**: el plugin no tiene inicio de sesión propio; se ejecuta dentro de Revit con la sesión y la licencia ya validadas.

## D08 — Sin presupuestos

- **Decisión**: el plugin mide, pero no asigna precios.

## D09 — Manifiesto de tipo Application

- **Decisión**: un único `.addin` de tipo `Application` que crea pestaña y botón al arrancar.

## D10 — Alcance funcional: medir, filtrar y exportar (actualizada el 9 de octubre)

- **Contexto**: el planteamiento inicial incluía mediciones, filtro y modificación de parámetros.
- **Decisión**: una ventana modal con tres botones y un DataGrid:
  - **Medir**: carga todos los elementos del modelo con las columnas Categoría, Familia, Cantidad y Unidad, agrupados por categoría con su total.
  - **Filtrar**: reduce el DataGrid a muros, suelos, ventanas y puertas. Para volver a ver todo se pulsa de nuevo Medir.
  - **Exportar**: guarda en Excel un elemento por fila, en la ruta que elija el usuario.
- La modificación de parámetros pasa a trabajos futuros.
- **Motivo**: una función principal sólida y probada en lugar de tres a medias; encaja con el flujo diseñado en Miro.

## D11 — Medir lee todo el modelo; Filtrar trabaja sobre los datos ya leídos (actualizada el 9 de octubre)

- **Medir**: lee todos los elementos físicos del modelo con un `FilteredElementCollector`, excluyendo tipos y elementos no geométricos (vistas, materiales), que un colector sin filtrar también devuelve.
- **Filtrar**: no vuelve a consultar Revit; filtra las mediciones ya cargadas con un diccionario de categorías (muros, suelos, ventanas y puertas). Es inmediato y se puede probar sin Revit.
- **Lectura desde la ventana**: el comando entrega al ViewModel la función de lectura (inyección de dependencias), para que UI no dependa de la API de Revit.
- **Sin interfaces**: el ViewModel recibe un `Func<List<Measurement>>`, no una interfaz `IMeasurementReader`. Con una sola implementación y un solo método, la función basta; la interfaz sería sobrediseño.
- **Descartado**: leer el modelo en `ButtonCommand` antes de abrir la ventana. Es más simple, pero lee aunque el usuario no pulse Medir y retrasa la apertura en modelos grandes.
- **Exportador**: `ExcelExporter` va en `MiPlugin.UI` porque no depende de Revit; el ViewModel lo usa directamente, sin interfaz.

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
- **Decisión**: eliminarlo; la lectura de Revit va en `MiPlugin/Services`.
- **Motivo**: la única dependencia externa es Revit y `MiPlugin` ya depende de ella; un proyecto más no aísla nada y añade una DLL y referencias.
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
