using System;
using System.Threading;
using Xunit;
using RouteX_Project;

namespace TestProject
{
    public class Algoritmo2Test
    {
        [Fact]
        public void Run_EncuentraRutaCorrecta()
        {
            int[,] matrix = new int[,]
            {
                { 0, 1, 1, 0 },
                { 1, 0, 0, 1 },
                { 1, 0, 0, 1 },
                { 0, 1, 1, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo2_BFS.Run(matrix, 0, 3, token);

            Assert.True(result.Found);

            // BFS encuentra la ruta con menos saltos, no necesariamente menor costo
            Assert.True(
                result.Path.SequenceEqual(new int[] { 0, 1, 3 }) ||
                result.Path.SequenceEqual(new int[] { 0, 2, 3 })
            );
        }

        [Fact]
        public void Run_NoEncuentraRuta()
        {
            int[,] matrix = new int[,]
            {
                { 0, 1, 0 },
                { 1, 0, 0 },
                { 0, 0, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo2_BFS.Run(matrix, 0, 2, token);

            Assert.False(result.Found);
        }

        [Fact]
        public void Run_OrigenIgualDestino()
        {
            int[,] matrix = new int[,]
            {
                { 0, 1 },
                { 1, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo2_BFS.Run(matrix, 0, 0, token);

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
                { 0, 1, 1 },
                { 1, 0, 1 },
                { 1, 1, 0 }
            };

            var cts = new CancellationTokenSource();
            cts.Cancel();

            var result = Algoritmo2_BFS.Run(matrix, 0, 2, cts.Token);

            Assert.False(result.Found);
        }

        [Fact]
        public void Run_CalculaCostoCorrecto()
        {
            int[,] matrix = new int[,]
            {
                { 0, 5, 1, 0 },
                { 5, 0, 0, 1 },
                { 1, 0, 0, 10 },
                { 0, 1, 10, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo2_BFS.Run(matrix, 0, 3, token);

            Assert.True(result.Found);

            // BFS elegirá 0 -> 1 -> 3 (menos saltos)
            Assert.Equal(new int[] { 0, 1, 3 }, result.Path);
            Assert.Equal(6, result.TotalCost); // 5 + 1
        }

        [Fact]
        public void Run_CalculaCantidadDeParadasCorrecta()
        {
            int[,] matrix = new int[,]
            {
                { 0, 1, 0, 0 },
                { 1, 0, 1, 0 },
                { 0, 1, 0, 1 },
                { 0, 0, 1, 0 }
            };

            var token = CancellationToken.None;

            var result = Algoritmo2_BFS.Run(matrix, 0, 3, token);

            Assert.True(result.Found);
            Assert.Equal(2, result.Stops); // nodos intermedios: 1 y 2
        }
    }
}