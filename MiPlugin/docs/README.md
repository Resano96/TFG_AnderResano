# MiPlugin — Mediciones para Revit

Plugin para Autodesk Revit 2025 que añade una pestaña propia con herramientas de medición. Proyecto intermodular (TFG) del ciclo de Desarrollo de Aplicaciones Multiplataforma.

> Mover este archivo a la raíz del repositorio.

## Estado

En desarrollo. Funciona: carga en Revit, pestaña con botón y lectura de un muro seleccionado. Ver `docs/03-roadmap.md`.

## Funciones

| Función | Estado |
| --- | --- |
| Medición de elementos seleccionados | En curso |
| Filtro de elementos | Previsto |
| Modificación de parámetros | Previsto |
| Exportación a Excel | Previsto |

## Requisitos

- Autodesk Revit 2025
- .NET 8 SDK
- Visual Studio 2022

## Estructura

```
MiPlugin.Core/     Modelos y lógica, sin dependencias de Revit
MiPlugin/          Punto de entrada, cinta, comandos y acceso a la API de Revit
MiPlugin.Tests/    Pruebas unitarias de Core (previsto)
docs/              Documentación
```

## Instalación

1. Clonar el repositorio y abrir la solución en Visual Studio.
2. Compilar en `Debug | x64`, con Revit cerrado.
3. En `TFG_AnderResano.addin`, poner en `<Assembly>` la ruta completa a `MiPlugin.dll` (`MiPlugin\bin\Debug\net8.0-windows\`).
4. Copiar el `.addin` a `%AppData%\Autodesk\Revit\Addins\2025\`.
5. Abrir Revit y aceptar la carga del complemento.

## Uso

1. Abrir un modelo.
2. En la pestaña del plugin, pulsar el botón de medición.
3. Seleccionar un elemento; se muestran sus dimensiones.

## Desarrollo

- Depurar: en las propiedades del proyecto, iniciar `Revit.exe` como programa externo y pulsar F5.
- Revit bloquea la DLL: cerrarlo antes de recompilar.
- Pruebas: `dotnet test` sobre `MiPlugin.Tests`.

## Documentación

- `docs/01-pasos-dados.md`: bitácora del desarrollo
- `docs/02-decisiones.md`: decisiones y sus motivos
- `docs/03-roadmap.md`: plan por hitos
- `docs/diagramas/`: diagramas en PlantUML

## Autor

Ander Resano. Tutor: Jordi Cidoncha Navarrete.
