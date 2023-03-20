using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    
    internal class GeneticAlgorithm
    {
        public static double FitnessFn(Chromosom input)
        {
            double sum = 0;
            double[,] koszt = new double[,]{ {4, 2, 2, 3, 4, 5, 3, 2, 2, 4 },
                                         { 1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5 },
                                         { 4, 4, 3, 2, 5, 3, 2, 3, 4, 4 } };
            Dictionary<char, double> charFreq = new Dictionary<char, double>() {
                {'A', 12.702}, {'T', 9.056}, {'A', 8.167}, {'O', 7.507}, {'I', 6.699}, {'N', 6.749},
                {'S', 6.327}
            };

            for (int i = 0; i < koszt.GetLength(0); i++) { 
                for(int k = 0; k < koszt.GetLength(1); k++)
                {
                    sum += koszt[i, k];
                }
            }

            return sum;
        }

    }
}
