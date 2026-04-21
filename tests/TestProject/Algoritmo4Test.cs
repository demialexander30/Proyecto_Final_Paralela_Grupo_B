using System.Threading;
using Xunit;
using RouteX_Project; // Asegúrate de que este using coincida con tu proyecto principal

namespace RouteX_Project.Tests
{
    public class Algoritmo4Tests 
    {
        [Fact]
        public void Run_DeberiaEncontrarRuta_CuandoExisteConexion()
        {
            // Creamos una matriz pequeña 3x3 de prueba
            // Ruta obvia: Nodo 0 -> Nodo 1 -> Nodo 2

            int[,] testMatrix = new int[,]
            {
                { 0, 10, 0 },
                { 10, 0, 20 },
                { 0, 20, 0 }
            };
          
            CancellationTokenSource cts = new CancellationTokenSource();

            // Ejecutamos el algoritmo del nodo 0 al nodo 2
            var resultado = Algoritmo4.Run(testMatrix, 0, 2, cts.Token); 

            // Verificamos que Found sea true y el costo sea correcto (10 + 20 = 30)
            Assert.True(resultado.Found);
            Assert.Equal(30, resultado.TotalCost);
            Assert.Equal(3, resultado.Path.Count); // Origen + 1 parada + Destino
        }

        [Fact]
        public void Run_DeberiaRetornarFalse_CuandoEsUnCallejonSinSalida()
        {
            // Matriz donde el Nodo 2 está desconectado del resto (puros 0 en su fila/columna)
            int[,] deadEndMatrix = new int[,]
            {
                { 0, 10, 0 },
                { 10, 0, 0 },
                { 0,  0, 0 }
            };

            CancellationTokenSource cts = new CancellationTokenSource();

            // Intentamos ir del nodo 0 al 2, pero no hay forma de llegar
            var resultado = Algoritmo4.Run(deadEndMatrix, 0, 2, cts.Token);

            // Verificamos que se haya dado cuenta que no hay ruta
            Assert.False(resultado.Found);
        }
    }
}