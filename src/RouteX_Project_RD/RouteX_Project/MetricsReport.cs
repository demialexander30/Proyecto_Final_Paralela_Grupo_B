using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public static class MetricsReport
    {
        public static void Print(List<RouteResult> resultados, long tSecuencial, long tParalelo, int cores)
        {
            double speedup = (double)tSecuencial / tParalelo;
            double eficiencia = (speedup / cores) * 100;

            Console.WriteLine("\n====== MÉTRICAS ======");
            Console.WriteLine($"Tiempo secuencial: {tSecuencial} ms");
            Console.WriteLine($"Tiempo paralelo:   {tParalelo} ms");
            Console.WriteLine($"Speedup:           {speedup:F2}x");
            Console.WriteLine($"Eficiencia:        {eficiencia:F2}%");

            var ganador = resultados
                .Where(r => r.Found)
                .OrderBy(r => r.ElapsedMs)
                .FirstOrDefault();

            if (ganador != null)
                Console.WriteLine($"\nGanador: {ganador.AlgorithmName} - {ganador.ElapsedMs} ms");
            else
                Console.WriteLine("\n------ Comparativa de algoritmos ------");
                Console.WriteLine($"{"Algoritmo",-40} {"Costo",20} {"Paradas",8} {"Tiempo(ms)",11} {"Encontró",9}");
                Console.WriteLine(new string('-', 92));

                foreach (var r in resultados)
                {
                    string costoLabel = r.AlgorithmId switch
                    {
                        1 => $"{r.TotalCost} (menor distancia)",
                        2 => $"{r.TotalCost} (dist. total)",
                        3 => $"{r.TotalCost} (sin peajes >70)",
                        4 => $"{r.TotalCost} (costo inmediato)",
                        _ => r.TotalCost.ToString()
                    };

                    string encontro = r.Found ? "Sí" : "No";
                    Console.WriteLine($"{r.AlgorithmName,-40} {costoLabel,20} {r.Stops,8} {r.ElapsedMs,11} {encontro,9}");
                }
        }

        public static void Export(string path, List<RouteResult> resultados, long tSecuencial, long tParalelo, int cores)
        {
            double speedup = (double)tSecuencial / tParalelo;
            double eficiencia = (speedup / cores) * 100;

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string fileName = $"resultados_{timestamp}.csv";
            string fullPath = Path.Combine(path, fileName);

            var sb = new System.Text.StringBuilder();

            sb.AppendLine("=== MÉTRICAS GENERALES ===");
            sb.AppendLine($"Tiempo Secuencial (ms),{tSecuencial}");
            sb.AppendLine($"Tiempo Paralelo (ms),{tParalelo}");
            sb.AppendLine($"Speedup,{speedup:F2}");
            sb.AppendLine($"Eficiencia (%),{eficiencia:F2}");
            sb.AppendLine($"Cores,{cores}");
            sb.AppendLine();

            sb.AppendLine("=== COMPARATIVA DE ALGORITMOS ===");
            sb.AppendLine("Nota: Costo Dijkstra=distancia minima | BFS=distancia total | Dijkstra Filtrado=sin conexiones >70 | Greedy=costo inmediato por paso");
            sb.AppendLine("AlgorithmId,AlgorithmName,TotalCost,Stops,ElapsedMs,Found");

            foreach (var r in resultados)
                sb.AppendLine($"{r.AlgorithmId},{r.AlgorithmName},{r.TotalCost},{r.Stops},{r.ElapsedMs},{r.Found}");

            Directory.CreateDirectory(path);
            File.WriteAllText(fullPath, sb.ToString());

            Console.WriteLine($"\nResultados exportados a la carpeta de metrics del proyecto");
        }
    }
}

