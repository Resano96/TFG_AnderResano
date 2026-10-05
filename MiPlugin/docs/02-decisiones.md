# Decisiones

Registro de decisiones del proyecto. Cada una indica el contexto, lo decidido, las alternativas descartadas y sus consecuencias.

## D01 — Plugin de Revit como TFG

- **Contexto**: había que elegir proyecto intermodular; la alternativa era una app clasificadora de gastos.
- **Decisión**: plugin para Revit, aprobado por el tutor.
- **Motivo**: aplica el stack de trabajo real (C#, WPF, MVVM) sobre una API profesional.
- **Consecuencia**: las pruebas de la parte de Revit son manuales; hay que aislar la lógica para poder probarla.

## D02 — Revit 2025 y .NET 8

- **Decisión**: una única versión objetivo, Revit 2025 sobre `net8.0-windows`.
- **Descartado**: dar soporte a versiones anteriores (.NET Framework 4.8), que obligaría a compilar para varios destinos.
- **Consecuencia**: la compatibilidad con otras versiones queda como trabajo futuro.

## D03 — Solución en varios proyectos

- **Contexto**: la API de Revit solo funciona dentro de Revit, así que el código que la usa no se puede probar de forma automática.
- **Decisión**: `MiPlugin.Core` (modelos, servicios e interfaces, sin Revit), `MiPlugin` (Revit, comandos y UI) y `MiPlugin.Tests`.
- **Descartado**: un solo proyecto con carpetas, más rápido pero sin nada que impida mezclar capas.
- **Consecuencia**: Core se prueba sin abrir Revit; a cambio hay más estructura inicial.
- **Estado**: pendiente de confirmar con el tutor, que pidió ser consultado ante cambios de arquitectura.

## D04 — Inversión de dependencias en la lectura del modelo

- **Decisión**: Core define la interfaz de lectura y el proyecto de Revit la implementa.
- **Motivo**: en los tests se sustituye el lector real por uno simulado.
- **Consecuencia**: los comandos solo coordinan; no contienen cálculos.

## D05 — Conversión de unidades en la frontera

- **Contexto**: Revit devuelve siempre unidades internas (pies).
- **Decisión**: convertir a métricas al leer el parámetro, antes de crear los modelos de Core.
- **Consecuencia**: Core trabaja siempre en metros y no conoce las unidades de Revit.

## D06 — Sin base de datos

- **Decisión**: persistencia en ficheros (configuración en JSON, resultados en Excel). Validado por el tutor.
- **Motivo**: los datos de origen ya están en el modelo de Revit y el uso es monopuesto.
- **Consecuencia**: los apartados de E/R y base de datos de la memoria describen el modelo conceptual y la estructura de los ficheros.

## D07 — Sin autenticación

- **Decisión**: el plugin no tiene inicio de sesión propio.
- **Motivo**: se ejecuta dentro de Revit, con la sesión y la licencia ya validadas.

## D08 — Sin presupuestos

- **Decisión**: el plugin mide, pero no asigna precios.
- **Motivo**: acotar el alcance a las 198 horas disponibles.

## D09 — Manifiesto de tipo Application

- **Decisión**: un único `.addin` de tipo `Application` que crea pestaña y botones al arrancar.
- **Descartado**: un manifiesto de tipo `Command` por cada comando, que los deja en *Herramientas externas* sin cinta propia.

## D10 — Alcance funcional (abierta)

- **Contexto**: el planteamiento aprobado incluye mediciones, filtro y modificación de parámetros, con exportación a Excel como función secundaria.
- **En estudio**: dejar mediciones como única función principal y ampliarla (más tipos de elemento, agrupación con subtotales, unidades configurables, exportación).
- **A favor**: una función sólida y bien probada; menos riesgo de plazos.
- **En contra**: menos variedad para justificar los módulos; cambio visible respecto al anteproyecto.
- **Siguiente paso**: consultarlo con el tutor antes de modificar la memoria.
