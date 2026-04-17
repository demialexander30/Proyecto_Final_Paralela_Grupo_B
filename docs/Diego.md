# Diego Marte

## Issue #3 — Implementación de Dijkstra clásico (Algorithm1.cs)

### ¿Qué se hizo?

Se implementó el algoritmo de Dijkstra para encontrar la ruta de menor costo entre un origen y un destino en una matriz de adyacencia. También se añadió soporte de cancelación con CancellationToken para detener la ejecución si es necesario.

### Lógica del algoritmo

* Se inicializan:
	* dist[] con valores altos (infinito)
	* visited[] para nodos visitados
	* prev[] para reconstruir la ruta
	* Se asigna distancia 0 al origen.
	* En cada iteración:
	* Se elige el nodo no visitado con menor distancia.
	* Se marca como visitado.
	* Se actualizan las distancias de sus vecinos si hay conexión.
	* Se revisa la cancelación en cada ciclo.
	* Si se llega al destino, se reconstruye el camino.
	* Se mide el tiempo con Stopwatch.

### Resultado retornado

* Se devuelve un RouteResult con:

* Path (ruta encontrada)
	* TotalCost
	* Stops (nodos intermedios)
	* ElapsedMs
	* Found indicando si se encontró ruta

### Decisiones de diseño

*	Se usó una versión O(n²) para mantener el algoritmo simple y más costoso.
*	No se utilizó PriorityQueue para facilitar la implementación.
*	La cancelación se verifica dentro del ciclo principal.
*	Se trabajó con matriz de adyacencia para mantener consistencia.