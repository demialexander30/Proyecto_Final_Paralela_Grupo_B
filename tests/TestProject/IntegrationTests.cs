using Xunit;
using RouteX_Project;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.IO;


namespace TestProject
{
    public class IntegrationTests
    {
        [Fact]
        public void Prueba1_AlMenosUnAlgoritmoEncuentraRuta()
        {
            var matrix = MapMatrix.Generate();
            var cts = new CancellationTokenSource();
            var bag = new ConcurrentBag<RouteResult>();

            var tasks = new[]
            {
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo1_Dijkstra.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); }),
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo2_BFS.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); }),
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo3_Dijkstra_Threshold.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); }),
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo4.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); })
            };

            System.Threading.Tasks.Task.WaitAll(tasks);

            Assert.Contains(bag, r => r.Found == true);
        }

        [Fact]
        public void Prueba2_ConcurrentBagRecibeResultados()
        {
            var matrix = MapMatrix.Generate();
            var cts = new CancellationTokenSource();
            var bag = new ConcurrentBag<RouteResult>();

            var tasks = new[]
            {
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo1_Dijkstra.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); }),
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo2_BFS.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); }),
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo3_Dijkstra_Threshold.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); }),
                System.Threading.Tasks.Task.Run(() => { var r = Algoritmo4.Run(matrix, 49, 499, cts.Token); bag.Add(r); if (r.Found && !cts.IsCancellationRequested) cts.Cancel(); })
            };

            System.Threading.Tasks.Task.WaitAll(tasks);

            Assert.True(bag.Count > 0);
        }

        [Fact]
        public void Prueba3_MetricsReportPrintNoLanzaExcepcion()
        {
            var matrix = MapMatrix.Generate();
            var cts = new CancellationTokenSource();

            var r1 = Algoritmo1_Dijkstra.Run(matrix, 49, 499, cts.Token);
            var r2 = Algoritmo2_BFS.Run(matrix, 49, 499, cts.Token);
            var r3 = Algoritmo3_Dijkstra_Threshold.Run(matrix, 49, 499, cts.Token);
            var r4 = Algoritmo4.Run(matrix, 49, 499, cts.Token);

            var resultados = new List<RouteResult> { r1, r2, r3, r4 };

            var exception = Record.Exception(() =>
                MetricsReport.Print(resultados, 100, 30, 2));

            Assert.Null(exception);
        }

        [Fact]
        public void Prueba4_MetricsReportExportCreaArchivo()
        {
            var matrix = MapMatrix.Generate();
            var cts = new CancellationTokenSource();

            var r1 = Algoritmo1_Dijkstra.Run(matrix, 49, 499, cts.Token);
            var r2 = Algoritmo2_BFS.Run(matrix, 49, 499, cts.Token);
            var r3 = Algoritmo3_Dijkstra_Threshold.Run(matrix, 49, 499, cts.Token);
            var r4 = Algoritmo4.Run(matrix, 49, 499, cts.Token);

            var resultados = new List<RouteResult> { r1, r2, r3, r4 };

            string testPath = Path.Combine(Path.GetTempPath(), "routex_test_metrics");
            Directory.CreateDirectory(testPath);

            MetricsReport.Export(testPath, resultados, 100, 30, 2);

            var files = Directory.GetFiles(testPath, "resultados_*.csv");
            Assert.True(files.Length > 0);

            // Limpiar
            Directory.Delete(testPath, true);
        }

        [Fact]
        public void Prueba5_OrigenIgualDestinoNoLanzaExcepcion()
        {
            var matrix = MapMatrix.Generate();
            var cts = new CancellationTokenSource();

            var exception = Record.Exception(() =>
            {
                var r1 = Algoritmo1_Dijkstra.Run(matrix, 49, 49, cts.Token);
                var r2 = Algoritmo2_BFS.Run(matrix, 49, 49, cts.Token);
                var r3 = Algoritmo3_Dijkstra_Threshold.Run(matrix, 49, 49, cts.Token);
                var r4 = Algoritmo4.Run(matrix, 49, 49, cts.Token);
            });

            Assert.Null(exception);
        }
    }
}