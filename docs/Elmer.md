
# Elmer Montilla

## Issue #6 — Algoritmo 4: Greedy - ruta más directa (Algoritmo4.cs)

### ¿Qué se hizo?
Se armó el cuarto algoritmo de búsqueda usando la estrategia Greedy (voraz). Básicamente, busca llegar al destino eligiendo la conexión más barata que tenga de frente en ese momento, sin ponerse a calcular cuánto va a costar el viaje completo al final.

### Lógica del algoritmo
* Desde la ciudad actual, revisa todos los vecinos y se mueve al que cueste menos.
* Usé un `HashSet<int>` para guardar los nodos por los que ya pasamos. Así evitamos que el algoritmo se quede dando vueltas en círculos infinitamente.
* En cada iteración del bucle `while`, se chequea `token.IsCancellationRequested`. Si otro algoritmo ya encontró la ruta primero, este se cancela de una vez para no gastar procesador.
* Si llega a un punto donde no hay salida (todos los vecinos en cero o ya visitados) y no es el destino, simplemente devuelve `Found = false`.

### Decisiones de diseño
* El profesor pidió este algoritmo para mostrar el "trade-off" (equilibrio) en la exposición. Es el más rápido calculando, pero casi nunca te da la ruta más barata en total. Sirve perfecto para comparar contra el algoritmo de Dijkstra y demostrar por qué a veces es mejor pensar a largo plazo.


## Issue #7 — Flujo principal y ejecución paralela (Program.cs parte 1)

### ¿Qué se hizo?
Se hizo la estructura principal del programa (`Program.cs`). Aquí es donde generamos la matriz compartida y ponemos a correr los algoritmos: primero uno por uno para medir el tiempo normal, y luego todos al mismo tiempo para hacer la carrera especulativa.

### Flujo de ejecución
* **Arranque:** Primero se llama a `MapMatrix.Generate()` para tener el mapa.
* **Secuencial:** Corrí los 4 algoritmos en orden pasándoles un token vacío y tomé el tiempo total usando un `Stopwatch`.
* **Paralelo:** Usé un arreglo de tareas (`Task[]`) y metí cada algoritmo en un `Task.Run()`. Al final, un `Task.WaitAll()` se queda esperando a que todos terminen (o se cancelen).

### Decisiones de diseño
* **Mecanismo de cancelación:** Creé un `CancellationTokenSource`. En el método auxiliar `RunAndStore`, cuando un algoritmo termina y encuentra ruta (`Found = true`), llama a `cts.Cancel()`. Automáticamente los demás hilos se dan cuenta y se detienen.
* **Guardar datos sin que explote:** Usé un `ConcurrentBag<RouteResult>`. Si los 4 hilos intentan guardar sus resultados al mismo tiempo en una lista normal, el programa se va a caer. El ConcurrentBag es seguro para los hilos (*thread-safe*), así que todos pueden guardar sin problemas.


## Issue #8 — Menú interactivo y validación de entrada (Program.cs parte 2)

### ¿Qué se hizo?
Se hizo el menú de la consola para que el usuario pueda meter los datos sin que el programa tire excepciones raras si escriben letras o números fuera de rango. También se armó el explorador de resultados.

### Validaciones implementadas
* Hice un método `GetValidInput` con un bucle `while(true)`. Si pones una ciudad de origen fuera del rango 1-1000, te vuelve a preguntar. 
* Valida que la ciudad de destino no sea exactamente la misma que la de origen.
* Valida que la cantidad de procesadores sea estrictamente entre 1 y 4.

### Decisiones de diseño
* **Menú rápido:** El menú es un bucle que lee los resultados que ya están guardados en el `ConcurrentBag`. Así, si quieres ver la ruta del Algoritmo 1 y luego la del 3, te las muestra al instante sin tener que volver a correr las tareas.
* **Manejo de errores:** Si le pides mostrar un algoritmo que retornó `Found = false` (como pasa a veces con el Greedy si se mete en un callejón sin salida), el menú atrapa eso y muestra un mensaje amigable en vez de romperse.
* Al elegir la opción `0`, el bucle se rompe y el programa deja todo listo para llamar a la clase `MetricsReport` de Demi antes de cerrarse.