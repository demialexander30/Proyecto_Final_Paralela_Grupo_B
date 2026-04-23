using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public static class MapMatrix
    {
        public static int[,] Generate()
        {
            int size = 10000;
            int[,] matrix = new int[size, size];
            Random random = new Random();

            for (int i = 0; i < size; i++)
            {
                for (int j = i + 1; j < size; j++)
                {
                    // 30% de probabilidad de tener conexion
                    if (random.NextDouble() < 0.30)
                        matrix[i, j] = random.Next(1, 101);
                    else
                        matrix[i, j] = 0;

                    // Copiar simetricamente
                    matrix[j, i] = matrix[i, j];
                }
            }

            // Diagonal siempre 0 (ya lo es por defecto pero lo dejamos explícito)
            for (int i = 0; i < size; i++)
                matrix[i, i] = 0;

            return matrix;
        }
    }
}
