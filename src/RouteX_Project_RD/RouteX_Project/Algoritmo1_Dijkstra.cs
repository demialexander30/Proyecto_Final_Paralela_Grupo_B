using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public class Algoritmo1_Dijkstra
    {
        public static RouteResult Run(int[,] matrix, int origin, int destination, CancellationToken token)
        {
            Console.WriteLine($"[Dijkstra] Task inicio: {Task.CurrentId}");

            var stopwatch = Stopwatch.StartNew();

            int n = matrix.GetLength(0);

            int[] dist = new int[n];
            bool[] visited = new bool[n];
            int[] prev = new int[n];

            // Inicializar
            for (int i = 0; i < n; i++)
            {
                dist[i] = int.MaxValue;
                visited[i] = false;
                prev[i] = -1;
            }

            dist[origin] = 0;

            for (int i = 0; i < n; i++)
            {
                // Verificar cancelacion
                if (token.IsCancellationRequested)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"[Dijkstra] Cancelado: {Task.CurrentId}");

                    return new RouteResult
                    {
                        Found = false,
                        ElapsedMs = stopwatch.ElapsedMilliseconds
                    };
                }

                // Encontrar nodo no visitado con menor distancia
                int u = -1;
                int minDist = int.MaxValue;

                for (int j = 0; j < n; j++)
                {
                    if (!visited[j] && dist[j] < minDist)
                    {
                        minDist = dist[j];
                        u = j;
                    }
                }

                // Si no hay nodo alcanzable
                if (u == -1)
                    break;

                visited[u] = true;

                // Si llegamos al destino
                if (u == destination)
                    break;

                // Relajar vecinos
                for (int v = 0; v < n; v++)
                {
                    int weight = matrix[u, v];

                    if (weight > 0 && !visited[v])
                    {
                        int newDist = dist[u] + weight;

                        if (newDist < dist[v])
                        {
                            dist[v] = newDist;
                            prev[v] = u;
                        }
                    }
                }
            }

            stopwatch.Stop();

            // Si no se encontró ruta
            if (dist[destination] == int.MaxValue)
            {
                Console.WriteLine($"[Dijkstra] Sin ruta: {Task.CurrentId}");

                return new RouteResult
                {
                    Found = false,
                    ElapsedMs = stopwatch.ElapsedMilliseconds
                };
            }

            // Reconstruir camino
            List<int> path = new List<int>();
            int current = destination;

            while (current != -1)
            {
                path.Add(current);
                current = prev[current];
            }

            path.Reverse();

            Console.WriteLine($"[Dijkstra] Finalizado: {Task.CurrentId}");

            return new RouteResult
            {
                Path = path,
                TotalCost = dist[destination],
                Stops = path.Count - 2,
                ElapsedMs = stopwatch.ElapsedMilliseconds,
                Found = true
            };
        }
    }
}