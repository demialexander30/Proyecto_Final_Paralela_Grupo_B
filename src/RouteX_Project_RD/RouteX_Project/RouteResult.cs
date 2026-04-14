using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RouteX_Project
{
    public class RouteResult
    {
        public int AlgorithmId { get; set; }
        public string AlgorithmName { get; set; }
        public List<int> Path { get; set; }
        public int TotalCost { get; set; }
        public int Stops { get; set; }
        public long ElapsedMs { get; set; }
        public bool Found { get; set; }
    }
}
