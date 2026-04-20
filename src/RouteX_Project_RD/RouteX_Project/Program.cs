using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RouteX_Project
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=====================================================");
            Console.WriteLine(" Bienvenido a RouteX GPS RD | Programación Paralela ");
            Console.WriteLine("=====================================================\n");

            Console.WriteLine("Generando mapa de ciudades (Matriz 1000x1000)...");
            int[,] matrix = MapMatrix.Generate();
            Console.WriteLine("Mapa generado exitosamente.\n");

            bool exitProgram = false;

            while (!exitProgram)
            {
                int origin = GetValidInput("Ingrese la ciudad de origen (1-1000): ", 1, 1000);
                int destination = GetValidInput($"Ingrese la ciudad de destino (1-1000, diferente a {origin}): ", 1, 1000, origin);
                int cores = GetValidInput("Ingrese el número de procesadores a utilizar (1-4): ", 1, 4);

                int startNode = origin - 1;
                int endNode = destination - 1;

                Console.WriteLine("\nCalculando rutas...");

                Stopwatch swSeq = Stopwatch.StartNew();
                CancellationTokenSource dummyCts = new CancellationTokenSource();

                var res1 = Algoritmo1_Dijkstra.Run(matrix, startNode, endNode, dummyCts.Token);
                var res2 = Algoritmo2_BFS.Run(matrix, startNode, endNode, dummyCts.Token);
                var res3 = Algoritmo3_Dijkstra_Threshold.Run(matrix, startNode, endNode, dummyCts.Token);
                var res4 = Algoritmo4.Run(matrix, startNode, endNode, dummyCts.Token);

                swSeq.Stop();
                long tSecuencial = swSeq.ElapsedMilliseconds;

                CancellationTokenSource cts = new CancellationTokenSource();
                ConcurrentBag<RouteResult> resultsBag = new ConcurrentBag<RouteResult>();

                Stopwatch swPar = Stopwatch.StartNew();

                Task[] tasks = new Task[4];
                tasks[0] = Task.Run(() => RunAndStore(Algoritmo1_Dijkstra.Run, matrix, startNode, endNode, cts, resultsBag));
                tasks[1] = Task.Run(() => RunAndStore(Algoritmo2_BFS.Run, matrix, startNode, endNode, cts, resultsBag));
                tasks[2] = Task.Run(() => RunAndStore(Algoritmo3_Dijkstra_Threshold.Run, matrix, startNode, endNode, cts, resultsBag));
                tasks[3] = Task.Run(() => RunAndStore(Algoritmo4.Run, matrix, startNode, endNode, cts, resultsBag));

                try
                {
                    Task.WaitAll(tasks);
                }
                catch (AggregateException) { }

                swPar.Stop();
                long tParalelo = swPar.ElapsedMilliseconds;

                var winner = resultsBag.Where(r => r.Found).OrderBy(r => r.ElapsedMs).FirstOrDefault();
                if (winner != null)
                {
                    Console.WriteLine($"\n¡Carrera ganada por {winner.AlgorithmName}! (Algoritmo {winner.AlgorithmId})");
                }

                bool inMenu = true;
                while (inMenu)
                {
                    Console.WriteLine("\n================ MENÚ DE RESULTADOS ================");
                    Console.WriteLine("[1] Ver ruta Algoritmo 1 (Dijkstra - Menor Costo)");
                    Console.WriteLine("[2] Ver ruta Algoritmo 2 (BFS - Menos Paradas)");
                    Console.WriteLine("[3] Ver ruta Algoritmo 3 (Dijkstra Filtrado)");
                    Console.WriteLine("[4] Ver ruta Algoritmo 4 (Greedy - Más directo)");
                    Console.WriteLine("[5] Nueva Búsqueda");
                    Console.WriteLine("[0] Salir y mostrar métricas");
                    Console.Write("Seleccione una opción: ");

                    string choice = Console.ReadLine() ?? "";

                    if (int.TryParse(choice, out int option) && option >= 1 && option <= 4)
                    {
                        PrintRouteResult(option, resultsBag);
                    }
                    else if (choice == "5")
                    {
                        inMenu = false;
                    }
                    else if (choice == "0")
                    {
                        inMenu = false;
                        exitProgram = true;

                        Console.WriteLine("\nGenerando reporte de métricas...");
                    }
                    else
                    {
                        Console.WriteLine("Opción no válida.");
                    }
                }
            }
        }

        static void RunAndStore(Func<int[,], int, int, CancellationToken, RouteResult> algoMethod,
                                int[,] matrix, int start, int end,
                                CancellationTokenSource cts, ConcurrentBag<RouteResult> bag)
        {
            var result = algoMethod(matrix, start, end, cts.Token);
            bag.Add(result);

            if (result.Found && !cts.IsCancellationRequested)
            {
                cts.Cancel();
            }
        }

        static int GetValidInput(string message, int min, int max, int exclude = -1)
        {
            int value;
            while (true)
            {
                Console.Write(message);
                string input = Console.ReadLine() ?? "";

                if (int.TryParse(input, out value) && value >= min && value <= max && value != exclude)
                {
                    return value;
                }
                Console.WriteLine("Entrada inválida o ciudad repetida. Intenta de nuevo.\n");
            }
        }

        static void PrintRouteResult(int algId, ConcurrentBag<RouteResult> bag)
        {
            var result = bag.FirstOrDefault(r => r.AlgorithmId == algId);

            if (result == null || !result.Found)
            {
                Console.WriteLine("\nEste algoritmo no encontró ruta con los criterios aplicados.");
                return;
            }

            Console.WriteLine($"\n--- Resultados de {result.AlgorithmName} ---");
            var pathString = string.Join(" -> ", result.Path.Select(node => $"Ciudad {(node + 1)}"));
            Console.WriteLine($"{pathString}");
            Console.WriteLine($"Costo total: {result.TotalCost} | Paradas: {result.Stops} | Tiempo de cálculo: {result.ElapsedMs} ms");
        }
    }
}