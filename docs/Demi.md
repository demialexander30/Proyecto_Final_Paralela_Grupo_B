# Demi Alexander Moore

# MetricsReport

## Descripción
La clase MetricsReport se encarga de mostrar y exportar métricas de rendimiento de los algoritmos de rutas.

## Funcionalidades

### Método Print
Imprime en consola:
- Tiempo secuencial
- Tiempo paralelo
- Speedup
- Eficiencia
- Algoritmo ganador (el más rápido que encontró solución)
- Tabla comparativa de resultados

### Método Export
Genera un archivo CSV en la carpeta /metrics con los resultados de los algoritmos.

## Métricas calculadas

- Speedup = TiempoSecuencial / TiempoParalelo
- Eficiencia = (Speedup / número de cores) * 100

## Formato del CSV

Columnas:
- Nombre
- Costo
- Paradas
- Tiempo(ms)
- Encontró

## Formato del archivo exportado

El archivo se genera automáticamente con el nombre:
`resultados_YYYYMMDD_HHmmss.csv`

Ejemplo: `resultados_20260420_231032.csv`

## Diagrama de Arquitectura

El diagrama muestra el flujo de ejecución paralela del sistema:

- **Program.cs** actúa como orquestador principal, lanzando los 4 algoritmos con Task.Run()
- **MapMatrix** provee la matriz de costos a todos los algoritmos en modo solo lectura, sin modificaciones
- **CancellationTokenSource** conecta todos los algoritmos — el primero en encontrar ruta cancela a los demás
- Cada algoritmo deposita su resultado en el **ConcurrentBag<RouteResult>** de forma thread-safe
- Esta arquitectura implementa descomposición especulativa: 4 estrategias distintas compiten en paralelo