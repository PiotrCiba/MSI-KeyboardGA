﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    public class Summary
    {
        public int IdPokolenia = 0;
        public double BestFitness = 0;
        public double AvgFitness = 0;

        public Summary(int idPokolenia, double bestFitness, double avgFitness)
        {
            IdPokolenia = idPokolenia;
            BestFitness = bestFitness;
            AvgFitness = avgFitness;
        }
    }
}
