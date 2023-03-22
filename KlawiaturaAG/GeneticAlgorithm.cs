using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace KlawiaturaAG
{

    public class GeneticAlgorithm
    {
        private Chromosom[] rodzice;
        private Chromosom[] dzieci;

        public GeneticAlgorithm()
        {
            rodzice = new Chromosom[1];
            rodzice[0] = new Chromosom();
            dzieci = new Chromosom[1];
            dzieci[0] = new Chromosom();
        }
        public GeneticAlgorithm(int popsize)
        {
            rodzice = new Chromosom[popsize];
            for(int i=0;i<popsize;i++)
            {
                rodzice[i] = new Chromosom();
            }
            dzieci = new Chromosom[popsize];
            for (int i = 0; i < popsize; i++)
            {
                dzieci[i] = new Chromosom();
            }
        }

        public static double Fn(string[] input)
        {
            double sum = 0;

            //koszty przycisków wg. metody ewaluacji Worksmana w układzie {2, 12, 11, 10}
            double[][] koszt = {
                new double[] { 5, 5},
                new double[] { 4, 2, 2, 3, 4, 5, 3, 2, 2, 4, 5, 5 },
                new double[] { 1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5, 3 },
                new double[] { 4, 4, 3, 2, 5, 3, 2, 3, 4, 4 }
            };

            //częstotliwości znaków w języku Angielskim
            Dictionary<char, double> charFreq = new Dictionary<char, double>() {
                {'A', 8.4966}, {'B', 2.0720}, {'C', 4.5388}, {'D', 3.3844}, {'E', 11.1607},
                {'F', 1.8121}, {'G', 2.4705}, {'H', 3.0034}, {'I', 7.5448}, {'J', 0.1965},
                {'K', 1.1016}, {'L', 5.4893}, {'M', 3.0129}, {'N', 6.6544}, {'O', 7.1635},
                {'P', 3.1671}, {'Q', 0.1962}, {'R', 7.5809}, {'S', 5.7351}, {'T', 6.9509},
                {'U', 3.6308}, {'V', 1.0074}, {'W', 1.2899}, {'X', 0.2902}, {'Y', 1.7779},
                {'Z', 0.2722},
                {'-',  0.34}, {'=', 0.01}, {'[', 0.01}, {']', 0.01}, {';', 0.0711},
                {'\'', 0.54}, {',', 1.3688}, {'.', 1.4511}, {'?', 0.1244}
            };

            for (int i = 0; i < input.Length; i++)
            {
                for (int k = 0; k < input[i].Length; k++)
                {
                    //sumowanie [koszt klawisza] * [częstotliwość znaku]
                    char tmp = input[i][k];
                    sum += koszt[i][k] * charFreq.GetValueOrDefault(input[i][k]);
                }
            }

            //zwracanie sumy
            return sum;
        }

    }
}
