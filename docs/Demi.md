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