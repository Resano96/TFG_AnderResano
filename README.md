# AR.Revit.Prometeo — Mediciones para Revit

Plugin para Autodesk Revit 2025. Su botón abre una ventana que mide todos los elementos del modelo, los filtra a muros, suelos, ventanas y puertas y exporta el resultado a Excel. Proyecto intermodular (TFG) del ciclo de Desarrollo de Aplicaciones Multiplataforma.

## Estado

En desarrollo. Funciona: carga en Revit y pestaña con botón. En curso: lectura del modelo y ventana de resultados. Ver `AR.Revit.Prometeo/docs/03-roadmap.md`.

## Funciones

| Función | Descripción | Estado |
| --- | --- | --- |
| Medir | Carga en la tabla todos los elementos del modelo (Categoría, Familia, Cantidad y Unidad: m² o ud), agrupados por categoría con su total | En curso |
| Filtrar | Reduce la tabla a muros, suelos, ventanas y puertas | Previsto |
| Exportar | Guarda en Excel un elemento por fila, en la ruta que elija el usuario | Previsto |

## Requisitos

- Autodesk Revit 2025
- .NET 8 SDK
- Visual Studio 2022

## Estructura

```
AR.Revit.Prometeo.Domain/   Modelos (Measurement, MeasurementGroup) y lógica de agrupación. Sin Revit
AR.Revit.Prometeo.UI/       Vistas WPF, ViewModels (MVVM) y exportación a Excel. Sin Revit
AR.Revit.Prometeo/          Arranque, cinta, comando y lectura del modelo de Revit
AR.Revit.Prometeo.Tests/    Pruebas unitarias de Domain y de los ViewModels
```

Dependencias: `AR.Revit.Prometeo` → `UI` → `Domain`, y `AR.Revit.Prometeo` → `Domain`. Domain no depende de nadie. El ViewModel lee Revit a través de una función que le entrega el comando, así que UI no conoce la API de Revit.

## Instalación

1. Clonar el repositorio y abrir `PluginAnder.slnx` en Visual Studio.
2. Compilar en `Debug | x64`, con Revit cerrado.
3. En `AR.Revit.Prometeo/TFG_AnderResano.addin`, poner en `<Assembly>` la ruta completa a `AR.Revit.Prometeo.dll` (`AR.Revit.Prometeo\bin\Debug\net8.0-windows\`).
4. Copiar el `.addin` a `%AppData%\Autodesk\Revit\Addins\2025\`.
5. Abrir Revit y aceptar la carga del complemento.

## Uso

1. Abrir un modelo.
2. En la pestaña **TFG-AnderResano**, pulsar el botón.
3. En la ventana: **Medir** carga la tabla, **Filtrar** la reduce a muros, suelos, ventanas y puertas (para volver a ver todo, pulsar de nuevo **Medir**) y **Exportar** guarda en Excel un elemento por fila, en la ruta que se elija.

## Desarrollo

- Depurar: en las propiedades de `AR.Revit.Prometeo`, iniciar `Revit.exe` como programa externo y pulsar F5.
- Revit bloquea la DLL: cerrarlo antes de recompilar.
- Pruebas: `dotnet test AR.Revit.Prometeo.Tests`. Se escriben después de implementar cada funcionalidad.
- Convenciones: código en inglés; documentación en español.

## Documentación

- `AR.Revit.Prometeo/docs/01-pasos-dados.md`: bitácora del desarrollo
- `AR.Revit.Prometeo/docs/02-decisiones.md`: decisiones y sus motivos
- `AR.Revit.Prometeo/docs/03-roadmap.md`: plan por hitos

## Autor

Ander Resano. Tutor: Jordi Cidoncha Navarrete.
