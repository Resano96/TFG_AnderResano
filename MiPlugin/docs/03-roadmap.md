# Roadmap

Plan de trabajo por hitos, del 28 de septiembre al 1 de diciembre de 2026. Los códigos R0x remiten al RFTP de la memoria.

## Hito 0 — Base del plugin (hecho)

- [x] Proyecto, manifiesto y carga en Revit
- [x] Pestaña, panel y botón propios
- [x] Repositorio Git en GitHub con `.gitignore`

## Hito 1 — Estructura y modelo (en curso)

- [x] Solución en proyectos: Domain, UI, MiPlugin y Tests
- [x] Código traducido a inglés
- [x] `Measurement` y `MeasurementGroup`
- [x] `MiPlugin.Infrastructure` eliminado (D15)
- [ ] Correcciones: `ButtonCommand` público, `OnShutdown` sin excepción
- [ ] Quitar `Perimeter` y `TotalPerimeter` de `Measurement` y `MeasurementGroup`
- [ ] Borrar la carpeta sobrante `MiPlugin.Core` (solo contiene `obj`)
- [ ] Memoria de la primera entrega parcial

## Hito 2 — Ventana y Medir (R02)

- [ ] `ViewModelBase`, `RelayCommand` y `MeasurementsViewModel`
- [ ] `MeasurementsWindow`: botones Medir, Filtrar y Exportar y DataGrid
- [ ] `ButtonCommand` abre la ventana como modal sobre Revit
- [ ] Lector de Revit en `MiPlugin/Services`: todos los elementos físicos del modelo, con el área convertida a m²
- [ ] El ViewModel recibe la función de lectura (`Func<List<Measurement>>`) desde `ButtonCommand`
- [ ] `MeasurementGrouping` (agrupación por categoría) y tests
- [ ] Botón Medir: carga el DataGrid con los grupos y sus totales

## Hito 3 — Filtrar (R03)

- [ ] Diccionario de categorías del filtro: muros, suelos, ventanas y puertas
- [ ] Botón Filtrar: reduce el DataGrid y recalcula los totales
- [ ] Tests del filtro en el ViewModel (`MiPlugin.Tests` referencia `MiPlugin.UI` y pasa a `net8.0-windows`)

## Hito 4 — Exportar (R04)

- [ ] `ExcelExporter` en `MiPlugin.UI` (ClosedXML); comprobar que su DLL no choca con otros plugins de Revit
- [ ] Botón Exportar: un elemento por fila en la ruta que elija el usuario
- [ ] Gestión de rutas no válidas y ficheros en uso

## Hito 5 — Cierre

- [ ] Planificación real, desviaciones y diagrama de Gantt
- [ ] Conclusiones, referencias y revisión final de la memoria
- [ ] Presentación

## Fuera de alcance

Modificación de parámetros, integración continua, presupuestos, base de datos, autenticación, ventanas no modales, soporte de otras versiones de Revit e instalador.
