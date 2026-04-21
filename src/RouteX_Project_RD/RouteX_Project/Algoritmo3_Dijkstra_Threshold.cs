using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public class Algoritmo3_Dijkstra_Threshold
    {
        private const int THRESHOLD = 70;

        public static RouteResult Run(int[,] matrix, int origin, int destination, CancellationToken token)
        {
            Console.WriteLine($"[Dijkstra-Threshold] Task inicio en ID del hilo: {Task.CurrentId}");

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
                // Cancelacion
                if (token.IsCancellationRequested)
                {
                    stopwatch.Stop();
                    Console.WriteLine($"[Dijkstra-Threshold] Cancelado en ID del hilo: {Task.CurrentId}");

                    return new RouteResult
                    {
                        Found = false,
                        ElapsedMs = stopwatch.ElapsedMilliseconds,
                        AlgorithmName = "Evita costos > 70"
                    };
                }

                // Buscar nodo no visitado con menor distancia
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

                if (u == -1)
                    break;

                visited[u] = true;

                if (u == destination)
                    break;

                // Relajar vecinos con filtro de umbral
                for (int v = 0; v < n; v++)
                {
                    int weight = matrix[u, v];

                    // Diferencia clave del Algoritmo1
                    if (weight > 0 && weight <= THRESHOLD && !visited[v])
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

            // No se encontro ruta
            if (dist[destination] == int.MaxValue)
            {
                Console.WriteLine($"[Dijkstra-Threshold] Sin ruta en ID del hilo: {Task.CurrentId}");

                return new RouteResult
                {
                    Found = false,
                    ElapsedMs = stopwatch.ElapsedMilliseconds,
                    AlgorithmName = "Evita costos > 70"
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

            Console.WriteLine($"[Dijkstra-Threshold] Finalizado en ID del hilo: {Task.CurrentId}");

            return new RouteResult
            {
                Path = path,
                TotalCost = dist[destination],
                Stops = path.Count - 2,
                ElapsedMs = stopwatch.ElapsedMilliseconds,
                Found = true,
                AlgorithmId = 3,
                AlgorithmName = "Evita costos > 70"
            };
        }
    }
}