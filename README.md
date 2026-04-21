# RouteX GPS RD

Sistema de búsqueda de rutas GPS con programación paralela desarrollado en C# (.NET 8).
Implementa cuatro algoritmos de búsqueda que compiten especulativamente en paralelo
sobre una matriz de costos de 1,000 x 1,000 nodos.

---

## Integrantes

| Nombre | Rol |
|---|---|
| Demi Alexander Moore De los Santos | Líder |
| Diego Marte | Desarrollador |
| Elmer Joel Montilla Castro | Desarrollador |
| Johan Manuel Vicente Berroa | Tech Lead |

Instituto Tecnológico de las Américas (ITLA)
Programación Paralela — 2025

---

## Descripción

RouteX GPS RD simula el comportamiento de un sistema GPS moderno: en lugar de calcular
una sola ruta de forma secuencial, lanza cuatro algoritmos en paralelo con criterios
distintos y responde tan pronto como el más rápido encuentra una ruta válida.

### Algoritmos implementados

| # | Algoritmo | Criterio |
|---|---|---|
| 1 | Dijkstra | Menor costo total |
| 2 | BFS | Menos paradas |
| 3 | Dijkstra Filtrado | Evita conexiones con costo mayor a 70 |
| 4 | Greedy | Ruta más directa por costo inmediato |

---

## Estructura del repositorio

    Proyecto_Final_Paralela_Grupo_B/
    ├── src/        codigo fuente (algoritmos, modelos, Program.cs)
    ├── tests/      proyecto xUnit con pruebas unitarias e integradoras
    ├── docs/       documentacion individual de cada miembro
    ├── Metrics/    resultados de comparativas y graficas de rendimiento
    ├── .gitignore
    └── .sln

## Tecnologias

- Lenguaje: C# (.NET 8)
- Paralelismo: Task Parallel Library (TPL)
- Sincronizacion: ConcurrentBag, CancellationTokenSource
- Pruebas: xUnit
- Control de versiones: Git / GitHub

---

## Ejecucion

### Requisitos
- .NET 8 SDK
- Visual Studio 2022 o VS Code con extension C#

### Pasos

```bash
git clone https://github.com/demialexander30/Proyecto_Final_Paralela_Grupo_B
cd Proyecto_Final_Paralela_Grupo_B/src/RouteX_Project_RD/RouteX_Project
dotnet run
```

### Pruebas

```bash
cd Proyecto_Final_Paralela_Grupo_B/tests/TestProject
dotnet test
```

---

## Metricas de rendimiento

El sistema ejecuta los cuatro algoritmos en modo secuencial y luego en paralelo,
calculando automaticamente el speedup y la eficiencia por nucleo al finalizar cada busqueda.
Los resultados se exportan como CSV en la carpeta `/Metrics`.

| Procesadores | tSecuencial (ms) | tParalelo (ms) | Speedup |
|---|---|---|---|
| 1 | 28 | 10.5 | 2.67x |
| 2 | 43.5 | 6 | 7.25x |
| 3 | 35.5 | 5 | 7.1x |
| 4 | 35.5 | 4 | 8.88x |

---

## Repositorio

https://github.com/demialexander30/Proyecto_Final_Paralela_Grupo_B
