using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public class Algoritmo2_BFS
    {
        public static RouteResult Run(int[,] matrix, int origin, int destination, CancellationToken token)
        {
            Console.WriteLine($"[BFS] Task inicio en ID del hilo: {Task.CurrentId}");

            var stopwatch = Stopwatch.StartNew();

            int n = matrix.GetLength(0);

            bool[] visited = new bool[n];
            int[] prev = new int[n];

            for (int i = 0; i < n; i++)
            {
                visited[i] = false;
                prev[i] = -1;
            }

            Queue<int> queue = new Queue<int>();

            queue.Enqueue(origin);
            visited[origin] = true;

            bool found = false;

            while (queue.Count > 0)
            {
                // Cancelación
                if (token.IsCancellationRequested)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"[BFS] Cancelado en ID del hilo: {Task.CurrentId}");

                    return new RouteResult
                    {
                        Found = false,
                        ElapsedMs = stopwatch.ElapsedMilliseconds
                    };
                }

                int current = queue.Dequeue();

                // Si llegamos al destino
                if (current == destination)
                {
                    found = true;
                    break;
                }

                // Explorar vecinos
                for (int neighbor = 0; neighbor < n; neighbor++)
                {
                    if (matrix[current, neighbor] > 0 && !visited[neighbor])
                    {
                        visited[neighbor] = true;
                        prev[neighbor] = current;
                        queue.Enqueue(neighbor);
                    }
                }
            }

            stopwatch.Stop();

            if (!found)
            {
                Console.WriteLine($"[BFS] Sin ruta en ID del hilo: {Task.CurrentId}");

                return new RouteResult
                {
                    Found = false,
                    ElapsedMs = stopwatch.ElapsedMilliseconds
                };
            }

            // Reconstruir camino
            List<int> path = new List<int>();
            int node = destination;

            while (node != -1)
            {
                path.Add(node);
                node = prev[node];
            }

            path.Reverse();

            // Calcular costo total recorriendo el camino
            int totalCost = 0;
            for (int i = 0; i < path.Count - 1; i++)
            {
                totalCost += matrix[path[i], path[i + 1]];
            }

            Console.WriteLine($"[BFS] Finalizado en ID del hilo: {Task.CurrentId}");

            return new RouteResult
            {
                AlgorithmId = 2,
                AlgorithmName = "BFS - Menos Paradas",
                Path = path,
                TotalCost = totalCost,
                Stops = path.Count - 2,
                ElapsedMs = stopwatch.ElapsedMilliseconds,
                Found = true
            };
        }
    }
}