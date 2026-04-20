namespace TestProject
{
    public class MetricReportTest
    {
        [Fact]
        public void SpeedupY_Eficiencia_CalculadosCorrectamente()
        {
            long tSecuencial = 400;
            long tParalelo = 100;
            int cores = 2;

            double speedup = (double)tSecuencial / tParalelo;
            double eficiencia = (speedup / cores) * 100;

            Assert.Equal(4.0, speedup);
            Assert.Equal(200.0, eficiencia);
        }
    }
}