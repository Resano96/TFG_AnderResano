# AR.Revit.Prometeo — Mediciones para Revit

Plugin para Autodesk Revit 2025. Su botón abre una ventana que mide todos los elementos del modelo agrupados por categoría, los filtra a muros, suelos, ventanas y puertas y exporta lo que se muestra a Excel. Proyecto intermodular (TFG) del ciclo de Desarrollo de Aplicaciones Multiplataforma.

## Estado

Las tres funciones (Medir, Filtrar y Exportar) están implementadas y probadas en Revit 2025, con interfaz de tema oscuro. Pendiente: cierre de la memoria. Ver `AR.Revit.Prometeo/docs/03-roadmap.md`.

## Funciones

| Función | Descripción | Estado |
| --- | --- | --- |
| Medir | Muestra todos los elementos físicos del modelo agrupados por categoría, con el total de cada grupo. Muros, suelos, cubiertas y techos se miden en m²; el resto, en unidades | Hecho |
| Filtrar | Reduce la tabla a muros, suelos, ventanas y puertas, con los totales recalculados. Funciona igual sea cual sea el idioma de Revit | Hecho |
| Exportar | Guarda en Excel lo que muestra la tabla (todo o filtrado), un elemento por fila, en la ruta que elija el usuario | Hecho |

## Requisitos

- Autodesk Revit 2025
- .NET 8 SDK
- Visual Studio 2022 o posterior

## Estructura

```
AR.Revit.Prometeo.Domain/   Modelos (Measurement, MeasurementGroup, ElementKind), agrupación y filtro. Sin Revit
AR.Revit.Prometeo.UI/       Ventana WPF, ViewModels (MVVM), exportación a Excel y diálogo de guardado. Sin Revit
AR.Revit.Prometeo/          Arranque, cinta, comando y lectura del modelo de Revit
AR.Revit.Prometeo.Tests/    Pruebas unitarias de Domain, del ViewModel y del exportador
```

Dependencias: `AR.Revit.Prometeo` → `UI` → `Domain`, y `AR.Revit.Prometeo` → `Domain`. Domain no depende de nadie. El ViewModel recibe del comando dos funciones (leer el modelo y pedir la ruta del Excel), así que UI no conoce la API de Revit ni abre ventanas por su cuenta.

Bibliotecas externas: ClosedXML (exportación a Excel) y xUnit (pruebas). El proyecto del plugin copia sus dependencias junto a la DLL (`EnableDynamicLoading`) y excluye las de la API de Revit, que ya están en el programa.

## Instalación

1. Clonar el repositorio y abrir `PluginAnder.slnx` en Visual Studio.
2. Compilar en `Debug | x64`, con Revit cerrado.
3. En `AR.Revit.Prometeo/TFG_AnderResano.addin`, poner en `<Assembly>` la ruta completa a `AR.Revit.Prometeo.dll` (`AR.Revit.Prometeo\bin\Debug\net8.0-windows\`).
4. Copiar el `.addin` a `%AppData%\Autodesk\Revit\Addins\2025\`.
5. Abrir Revit y aceptar la carga del complemento.

## Uso

1. Abrir un modelo.
2. En la pestaña **AR_Prometeo**, panel **AR_Solution**, pulsar el botón **AR_medir**.
3. En la ventana se abre un menú lateral con tres acciones. El botón de la vista activa se marca en verde.
   - **Medir** carga todos los elementos, agrupados por categoría. Cada grupo muestra su total y se despliega para ver sus elementos (familia, tipo, Id, cantidad y unidad). El Id permite localizarlos en Revit con *Gestionar → Seleccionar por ID*.
   - **Filtrar** deja solo muros, suelos, ventanas y puertas. Para volver a ver todo, pulsar de nuevo **Medir**.
   - **Exportar** (activo cuando hay datos) pide la ruta y guarda en Excel lo que se está mostrando. El resultado aparece al pie de la ventana; si el fichero está abierto en Excel, se muestra un aviso y la ventana sigue abierta.

## Desarrollo

- Depurar: en las propiedades de `AR.Revit.Prometeo`, iniciar `Revit.exe` como programa externo y pulsar F5.
- Revit bloquea la DLL: cerrarlo antes de recompilar.
- Pruebas: `dotnet test AR.Revit.Prometeo.Tests` (no necesitan Revit). Se escriben después de implementar cada funcionalidad.
- Convenciones: código en inglés; documentación en español.

## Documentación

- `AR.Revit.Prometeo/docs/01-pasos-dados.md`: bitácora del desarrollo
- `AR.Revit.Prometeo/docs/02-decisiones.md`: decisiones y sus motivos
- `AR.Revit.Prometeo/docs/03-roadmap.md`: plan por hitos

## Autor

Ander Resano. Tutor: Jordi Cidoncha Navarrete.
