# Roadmap

Plan de trabajo por hitos. Los códigos R0x remiten al RFTP de la memoria. Depende de la decisión D10 (alcance); si se reduce a mediciones, los hitos 4 y 5 pasan a trabajos futuros y sus horas se reparten en el hito 3.

## Hito 0 — Base del plugin (hecho)

- [x] Proyecto, manifiesto y carga en Revit
- [x] Pestaña, panel y botón propios
- [x] Comando que lee un elemento seleccionado
- [x] Proyecto `MiPlugin.Core` y primera clase de modelo

## Hito 1 — Primera entrega parcial (9 de octubre de 2026)

Objetivo: reflejar el 50 % del trabajo, con estructura, memoria, módulos y objetivos definidos.

- [ ] Confirmar con el tutor la arquitectura (D03) y el alcance (D10)
- [ ] Correcciones pendientes del código (ver `01-pasos-dados.md`)
- [ ] Medición de un muro correcta y en metros, de punta a punta
- [ ] Proyecto `MiPlugin.Tests` con los primeros tests de Core
- [ ] Repositorio Git con el historial desde ahora
- [ ] Memoria en la plantilla del centro, con diagramas y capturas

## Hito 2 — Arquitectura completa (R06)

- [ ] Interfaz de lectura en Core e implementación en Revit
- [ ] Servicio de cálculo de totales, hecho con TDD
- [ ] Comando reducido a coordinar
- [ ] Una sola clase para la cinta

## Hito 3 — Mediciones (R02)

- [ ] Selección múltiple, previa o interactiva
- [ ] Longitud, área y volumen de varios tipos de elemento
- [ ] Ventana WPF de resultados con MVVM
- [ ] Gestión de errores: sin selección, parámetros ausentes, cancelación

## Hito 4 — Filtro de elementos (R03)

- [ ] Filtro por categoría
- [ ] Filtro por valor de parámetro
- [ ] Ventana de filtro

## Hito 5 — Modificación de parámetros (R04)

- [ ] Cambio en una única transacción
- [ ] Informe de elementos omitidos
- [ ] Ventana de edición

## Hito 6 — Exportación a Excel (R05)

- [ ] Exportador con una fila por elemento y totales
- [ ] Gestión de rutas no válidas y ficheros en uso

## Hito 7 — Cierre

- [ ] Integración continua que ejecute los tests
- [ ] Planificación real, desviaciones y diagrama de Gantt
- [ ] Conclusiones, referencias y revisión final de la memoria
- [ ] Presentación

## Fuera de alcance

Presupuestos, base de datos, autenticación, ventanas no modales, soporte de otras versiones de Revit e instalador.
