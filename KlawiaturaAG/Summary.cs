using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    public class Summary
    {
        public string IdPokolenia = "";
        public double BestFitness = 0;
        public double AvgFitness = 0;
        public double MedianFitness = 0;

        public Summary(string idPokolenia, double bestFitness, double avgFitness, double medianFitness)
        {
            IdPokolenia = idPokolenia;
            BestFitness = bestFitness;
            AvgFitness = avgFitness;
            MedianFitness = medianFitness;
        }
    }
}
