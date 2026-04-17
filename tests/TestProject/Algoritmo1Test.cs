using System;
using System.Threading;
using Xunit;
using RouteX_Project;

namespace TestProject
{
    public class Algoritmo1Test
    {
        [Fact]
        public void Run_EncuentraRutaCorrecta()
        {
            int[,] matrix = new int[,]
            {
                { 0, 1, 4, 0 },
                { 1, 0, 2, 5 },
                { 4, 2, 0, 1 },
                { 0, 5, 1, 0 }
            };

            var token = CancellationToken.None;

            var result = Algorithm1.Run(matrix, 0, 3, token);

            Assert.True(result.Found);
            Assert.Equal(4, result.TotalCost); // 0 -> 1 -> 2 -> 3
            Assert.Equal(new int[] { 0, 1, 2, 3 }, result.Path);
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

            var result = Algorithm1.Run(matrix, 0, 2, token);

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

            var result = Algorithm1.Run(matrix, 0, 0, token);

            Assert.True(result.Found);
            Assert.Equal(0, result.TotalCost);
            Assert.Single(result.Path);
            Assert.Equal(0, result.Path[0]);
        }

        [Fact]
        public void Run_CancelacionRetornaFalse()
        {
            int[,] matrix = new int[,]
            {
                { 0, 1, 4 },
                { 1, 0, 2 },
                { 4, 2, 0 }
            };

            var cts = new CancellationTokenSource();
            cts.Cancel(); // cancelar antes de ejecutar

            var result = Algorithm1.Run(matrix, 0, 2, cts.Token);

            Assert.False(result.Found);
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

            var result = Algorithm1.Run(matrix, 0, 3, token);

            Assert.True(result.Found);
            Assert.Equal(2, result.Stops); // nodos intermedios: 1 y 2
        }
    }
}