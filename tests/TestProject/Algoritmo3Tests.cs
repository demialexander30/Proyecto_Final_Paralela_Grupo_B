using System;
using System.Threading;
using Xunit;
using RouteX_Project;

namespace TestProject
{
    public class Algoritmo3Tests
    {
        [Fact]
        public void Run_EncuentraRutaValidaBajoUmbral()
        {
            int[,] matrix = new int[,]
            {
                { 0, 10, 0, 0 },
                { 10, 0, 20, 0 },
                { 0, 20, 0, 30 },
                { 0, 0, 30, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 3, token);

            Assert.True(result.Found);
            Assert.Equal(new int[] { 0, 1, 2, 3 }, result.Path);
            Assert.Equal(60, result.TotalCost);
        }

        [Fact]
        public void Run_IgnoraAristasMayoresAlUmbral()
        {
            int[,] matrix = new int[,]
            {
                { 0, 100, 0, 0 },
                { 100, 0, 10, 0 },
                { 0, 10, 0, 10 },
                { 0, 0, 10, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 3, token);

            // No puede usar la arista 0->1 (100), así que no hay ruta
            Assert.False(result.Found);
        }

        [Fact]
        public void Run_EncuentraRutaAlternativaEvitandoAltosCostos()
        {
            int[,] matrix = new int[,]
            {
                { 0, 80, 10, 0 },
                { 80, 0, 10, 10 },
                { 10, 10, 0, 10 },
                { 0, 10, 10, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 3, token);

            Assert.True(result.Found);

            // Debe evitar 0->1 (80) y usar 0->2->3
            Assert.Equal(new int[] { 0, 2, 3 }, result.Path);
            Assert.Equal(20, result.TotalCost);
        }

        [Fact]
        public void Run_OrigenIgualDestino()
        {
            int[,] matrix = new int[,]
            {
                { 0, 10 },
                { 10, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 0, token);

            Assert.True(result.Found);
            Assert.Single(result.Path);
            Assert.Equal(0, result.Path[0]);
            Assert.Equal(0, result.TotalCost);
        }

        [Fact]
        public void Run_CancelacionRetornaFalse()
        {
            int[,] matrix = new int[,]
            {
                { 0, 10, 10 },
                { 10, 0, 10 },
                { 10, 10, 0 }
            };

            var cts = new CancellationTokenSource();
            cts.Cancel();

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 2, cts.Token);

            Assert.False(result.Found);
        }

        [Fact]
        public void Run_CalculaCantidadDeParadasCorrecta()
        {
            int[,] matrix = new int[,]
            {
                { 0, 10, 0, 0 },
                { 10, 0, 10, 0 },
                { 0, 10, 0, 10 },
                { 0, 0, 10, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 3, token);

            Assert.True(result.Found);
            Assert.Equal(2, result.Stops); // nodos intermedios: 1 y 2
        }

        [Fact]
        public void Run_AlgorithmNameEsCorrecto()
        {
            int[,] matrix = new int[,]
            {
                { 0, 10 },
                { 10, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo3_Dijkstra_Threshold.Run(matrix, 0, 1, token);

            Assert.Equal("Evita costos > 70", result.AlgorithmName);
        }
    }
}