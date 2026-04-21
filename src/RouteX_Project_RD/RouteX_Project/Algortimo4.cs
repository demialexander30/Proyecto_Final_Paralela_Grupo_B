using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public class Algoritmo4
    {
        public static RouteResult Run(int[,] matrix, int origin, int destination, CancellationToken token)
        {
            Console.WriteLine($"[Algoritmo 4] Iniciando en ID del hilo: {Task.CurrentId}");

            Stopwatch sw = Stopwatch.StartNew();
            List<int> path = new List<int> { origin };
            HashSet<int> visited = new HashSet<int> { origin };
            int current = origin;
            int totalCost = 0;
            bool found = false;

            while (current != destination)
            {
                if (token.IsCancellationRequested)
                {
                    sw.Stop();
                    Console.WriteLine($"[Algoritmo 4] Cancelado en ID del hilo: {Task.CurrentId}");
                    return new RouteResult { AlgorithmId = 4, AlgorithmName = "Greedy", Found = false };
                }

                int nextNode = -1;
                int minCost = int.MaxValue;

                for (int i = 0; i < matrix.GetLength(1); i++)
                {
                    int cost = matrix[current, i];
                    if (cost > 0 && !visited.Contains(i) && cost < minCost)
                    {
                        minCost = cost;
                        nextNode = i;
                    }
                }

                if (nextNode == -1)
                {
                    break;
                }

                visited.Add(nextNode);
                path.Add(nextNode);
                totalCost += minCost;
                current = nextNode;

                if (current == destination)
                {
                    found = true;
                }
            }

            sw.Stop();

            Console.WriteLine($"[Algoritmo 4] Finalizando en ID del hilo: {Task.CurrentId}");

            return new RouteResult
            {
                AlgorithmId = 4,
                AlgorithmName = "Greedy - Ruta más directa",
                Path = path,
                TotalCost = totalCost,
                Stops = path.Count > 0 ? path.Count - 2 : 0,
                ElapsedMs = sw.ElapsedMilliseconds,
                Found = found
            };
        }
    }
}