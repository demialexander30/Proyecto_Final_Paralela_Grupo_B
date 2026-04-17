# Diego Marte

## Issue #3 — Implementación de Dijkstra clásico (Algoritmo1_Dijkstra.cs)

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


## Issue #4 — Implementación de BFS (Algoritmo2_BFS.cs)

### ¿Qué se hizo?

Se implementó un algoritmo de búsqueda en anchura (BFS) para encontrar la ruta con menor número de saltos entre un origen y un destino, sin considerar el costo. También se añadió soporte de cancelación.

### Lógica del algoritmo

Se usa una Queue<int> para procesar nodos en orden.

* Se inicializan:

	* visited[] para evitar repetir nodos
	* prev[] para reconstruir la ruta
	* Se encola el nodo origen y se marca como visitado.
	* En cada iteración:
	* Se toma el nodo actual.
	* Si es el destino, se detiene la búsqueda.
	* Se recorren sus vecinos y se agregan los no visitados con conexión.
	* Se verifica la cancelación en cada ciclo.
	* Se reconstruye la ruta usando prev[].
	* El costo total se calcula recorriendo el camino encontrado.

### Resultado retornado

El algoritmo retorna RouteResult con:

	* Path (ruta encontrada)
	* TotalCost
	* Stops
	* ElapsedMs
	* Found indicando si hay ruta

### Decisiones de diseño

	* Se eligió BFS por su rapidez y porque encuentra la menor cantidad de saltos.
	* Se detiene al encontrar el destino para ahorrar tiempo.
	* El costo se calcula al final porque BFS no usa pesos.
	* Se prioriza velocidad sobre el costo mínimo.


## Issue #5 — Implementacion de Dijkstra con restricción de costo (Algoritmo3_Dijkstra_Threshold.cs)

### ¿Qué se hizo?

Se implementó una versión de Dijkstra que ignora conexiones con costo mayor a un límite. Esto simula evitar rutas caras. En algunos casos, puede no encontrar ruta.

### Lógica del algoritmo

* Se define:
	* THRESHOLD = 70
	* Se usa la misma base de Dijkstra:
		* dist[], visited[], prev[]
	* Al evaluar vecinos:
		* Solo se consideran conexiones con costo menor o igual al umbral
	* Se verifica la cancelación en cada iteración.
	* Si se alcanza el destino, se reconstruye la ruta.

###	Resultado retornado

El algoritmo retorna RouteResult con:

	* AlgorithmName = "Evita costos > 70"
	* Ruta encontrada (si existe)
	* TotalCost, Stops, ElapsedMs
	* Found indicando si hay ruta

### Decisiones de diseño

	* Se reutilizó la lógica de Dijkstra para mantener consistencia.
	* El filtro se aplicó directamente al evaluar vecinos.
	* Es posible que no exista ruta por el filtro.
	* Se enfoca en evitar costos altos, diferenciándose de los otros algoritmos.