# Johan Manuel Vicente Berroa

## Issue #1 — Reorganización del repositorio y estructura de carpetas

### ¿Qué se hizo?
Se limpió y reorganizó el repositorio para que todos los miembros del equipo puedan trabajar con una estructura consistente desde el inicio. Se eliminó la carpeta `proyecto_final_GrupoB/` que existía por defecto y se estableció la estructura mínima requerida por el profesor.

### Estructura final del repositorio

RouteX GPS RD/
├── src/          → código fuente del proyecto (Program.cs, algoritmos, modelos)
├── tests/        → proyecto xUnit con todas las pruebas unitarias
├── docs/         → documentación individual de cada miembro
├── metrics/      → resultados de comparativas de rendimiento
├── .gitignore    → excluye bin/, obj/, .vs/ y archivos de build
└── .sln          → solución que apunta a /src y /tests

### Decisiones tomadas
- Se eliminó `proyecto_final_GrupoB/` porque el código fuente debe vivir en `/src` según los requisitos del profesor.
- Se creó el proyecto xUnit en `/tests` desde este issue para que todos los miembros puedan agregar sus pruebas sin bloqueos.
- El `.gitignore` cubre `bin/`, `obj/`, `.vs/`, `*.dll`, `*.exe` y output de publish para evitar subir archivos compilados al repositorio.
- El archivo `.sln` fue verificado para que apunte correctamente a ambos proyectos (`/src` y `/tests`).

## Issue #2 — MapMatrix.cs y RouteResult.cs

### ¿Qué se hizo?
Se crearon los dos archivos base que todos los algoritmos necesitan para funcionar. MapMatrix.cs genera la matriz de costos compartida y RouteResult.cs define el modelo de datos estándar que retorna cada algoritmo.

### MapMatrix.cs
Clase estática con método `Generate()` que retorna `int[1000, 1000]`.

**Lógica de generación:**
- Se recorre solo el triángulo superior de la matriz (i < j) para evitar procesar celdas duplicadas.
- Cada celda tiene un 30% de probabilidad de tener una conexión con valor aleatorio entre 1 y 100.
- El 70% restante queda en 0, simulando que no todas las ciudades están conectadas directamente.
- Se copia simétricamente: `matrix[j, i] = matrix[i, j]`, garantizando que si Ciudad A conecta con Ciudad B, Ciudad B conecta con Ciudad A con el mismo costo.
- La diagonal siempre es 0 porque no tiene sentido viajar de un nodo a sí mismo.

### RouteResult.cs
Modelo de datos sin lógica, solo propiedades. Todas las clases de algoritmos retornan este mismo tipo para que Program.cs pueda procesarlos de forma uniforme.

| Propiedad | Tipo | Descripción |
|---|---|---|
| AlgorithmId | int | Identificador del algoritmo (1-4) |
| AlgorithmName | string | Nombre descriptivo del algoritmo |
| Path | List<int> | Lista de nodos que forman la ruta |
| TotalCost | int | Suma de costos de todas las conexiones de la ruta |
| Stops | int | Número de paradas intermedias (Path.Count - 2) |
| ElapsedMs | long | Tiempo que tardó el algoritmo en ms |
| Found | bool | true si encontró ruta, false si no |

### Decisiones de diseño
- `MapMatrix` es estática porque la matriz es un recurso compartido de solo lectura, no necesita instanciarse.
- `RouteResult` no tiene lógica interna para respetar el principio de responsabilidad única — cada algoritmo decide cómo llenar el modelo.
- Se usó `List<int>` para el Path porque el tamaño de la ruta es variable y desconocido al momento de crear el objeto.

## Issue #12 — Test unitario final integrador

### ¿Qué se hizo?
Se creó `IntegrationTests.cs` en `/tests` con 5 pruebas que verifican el funcionamiento
completo del sistema de punta a punta, integrando todos los componentes desarrollados
por el equipo.

### Pruebas implementadas

| Prueba | Descripción | Resultado |
|---|---|---|
| Prueba1_AlMenosUnAlgoritmoEncuentraRuta | Genera matriz real, corre los 4 algoritmos en paralelo con CancellationTokenSource compartido y verifica que al menos uno retorna Found=true | ✅ |
| Prueba2_ConcurrentBagRecibeResultados | Verifica que el ConcurrentBag recibe al menos un resultado después de la ejecución paralela | ✅ |
| Prueba3_MetricsReportPrintNoLanzaExcepcion | Verifica que MetricsReport.Print() no lanza excepción con resultados reales de los algoritmos | ✅ |
| Prueba4_MetricsReportExportCreaArchivo | Verifica que MetricsReport.Export() crea efectivamente el archivo CSV en la ruta indicada | ✅ |
| Prueba5_OrigenIgualDestinoNoLanzaExcepcion | Verifica que el sistema maneja el caso origen = destino sin lanzar excepción | ✅ |

### Decisiones tomadas
- Se usó `Path.GetTempPath()` en la Prueba 4 para no depender de rutas absolutas del
repositorio durante los tests, haciendo las pruebas portables en cualquier máquina.
- Se reutilizó el mismo patrón de `CancellationTokenSource` compartido que usa `Program.cs`
para que el test integrador refleje el comportamiento real del sistema.
- Los 5 tests pasaron en verde en `dotnet test` con un tiempo total de 335ms, confirmando
que el sistema completo funciona correctamente de punta a punta.