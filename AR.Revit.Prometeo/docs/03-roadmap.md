# Roadmap

Plan de trabajo por hitos, del 28 de septiembre al 1 de diciembre de 2026. Los códigos R0x remiten al RFTP de la memoria.

## Hito 0 — Base del plugin (hecho)

- [x] Proyecto, manifiesto y carga en Revit
- [x] Pestaña, panel y botón propios
- [x] Repositorio Git en GitHub con `.gitignore`

## Hito 1 — Estructura y modelo (en curso)

- [x] Solución en proyectos: Domain, UI, AR.Revit.Prometeo y Tests
- [x] Código traducido a inglés
- [x] `Measurement` y `MeasurementGroup`
- [x] `MiPlugin.Infrastructure` eliminado (D15)
- [ ] Correcciones: `ButtonCommand` público, `OnShutdown` sin excepción
- [x] Quitar `Perimeter` y `TotalPerimeter` de `Measurement` y `MeasurementGroup`
- [x] Borrar la carpeta sobrante `MiPlugin.Core`
- [ ] Memoria de la primera entrega parcial

## Hito 2 — Ventana y Medir (R02)

- [x] `ViewModelBase`, `RelayCommand` y `MeasurementsViewModel`
- [x] `MeasurementsWindow`: botones Medir, Filtrar y Exportar y DataGrid
- [x] `ButtonCommand` abre la ventana como modal sobre Revit
- [x] Lector de Revit en `AR.Revit.Prometeo/Services`: todos los elementos físicos del modelo; área en m² solo para anfitriones (`HostObject`)
- [x] El ViewModel recibe la función de lectura (`Func<List<Measurement>>`) desde `ButtonCommand`
- [x] `MeasurementGrouping` (agrupación por categoría) y tests
- [x] Botón Medir: carga el DataGrid con los grupos y sus totales

## Hito 3 — Filtrar (R03)

- [x] Tipos del filtro: enum `ElementKind` en Domain (muro, suelo, ventana, puerta); el lector lo asigna desde `BuiltInCategory`
- [x] Botón Filtrar: reduce el DataGrid y recalcula los totales (`MeasurementFilter`)
- [x] Tests del filtro en Domain y en el ViewModel

## Hito 4 — Exportar (R04)

- [x] `ExcelExporter` en `AR.Revit.Prometeo.UI` (ClosedXML); sin conflicto con las DLL de Revit 2025
- [x] Botón Exportar: un elemento por fila en la ruta que elija el usuario
- [x] Gestión de ficheros en uso (`IOException`): aviso en la ventana sin cerrarla

## Hito 5 — Cierre

- [ ] Planificación real, desviaciones y diagrama de Gantt
- [ ] Conclusiones, referencias y revisión final de la memoria
- [ ] Presentación

## Fuera de alcance

Modificación de parámetros, integración continua, presupuestos, base de datos, autenticación, ventanas no modales, soporte de otras versiones de Revit e instalador.
