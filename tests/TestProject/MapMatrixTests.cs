using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using RouteX_Project;

namespace TestProject
{
    public class MapMatrixTests
    {
        [Fact]
        public void Diagonal_SiempreEsCero()
        {
            var matrix = MapMatrix.Generate();

            for (int i = 0; i < 1000; i++)
                Assert.Equal(0, matrix[i, i]);
        }

        [Fact]
        public void Matriz_EsSimetrica()
        {
            var matrix = MapMatrix.Generate();

            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                    Assert.Equal(matrix[i, j], matrix[j, i]);
        }

        [Fact]
        public void Matriz_TieneAproximadamente70PorCientoDeZeros()
        {
            var matrix = MapMatrix.Generate();
            int zeros = 0;
            int total = 1000 * 1000;

            for (int i = 0; i < 1000; i++)
                for (int j = 0; j < 1000; j++)
                    if (matrix[i, j] == 0) zeros++;

            double porcentaje = (double)zeros / total * 100;
            Assert.InRange(porcentaje, 60, 85);
        }
    }
}
