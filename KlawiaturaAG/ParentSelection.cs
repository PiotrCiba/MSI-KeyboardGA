using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace KlawiaturaAG
{
    public static class ParentSelection
    {
        public static Chromosom[] SelectionInterface(Chromosom[] candidates, int mode)
        {
            int halflength = (int)(candidates.Length / 2);
            Chromosom[] tophalf = (from c in candidates orderby c.fitness ascending select c).ToArray()[..halflength];
            switch (mode)
            {
                case 0: //Turniej (full)
                    return TournamentSelectionAlgorithm(candidates);
                case 1: //Turniej (1/2)
                    return TournamentSelectionAlgorithm(tophalf);
                case 2: //Ruletka (full)
                    return RoulletteSelectionAlgorithm(candidates);
                case 3: //Ruletka (1/2)
                    return RoulletteSelectionAlgorithm(tophalf);
                default:    //Turniej (full)
                    return TournamentSelectionAlgorithm(candidates);
            }
        }
        public static Chromosom[] TournamentSelectionAlgorithm(Chromosom[] candidates)
        {
            Chromosom[] output = new Chromosom[2];
            //Randomizing 4 different indexes of candidates
            Random rnd= new Random();
            int maxIndex = candidates.Length;
            HashSet<int> set = new HashSet<int>();
            int number;
            while(set.Count < 4) {
                number = rnd.Next(maxIndex);
                set.Add(number);
            }
            int[] contestants = set.ToArray();

            //first parent
            if (candidates[contestants[0]].fitness < candidates[contestants[1]].fitness)
                output[0] = candidates[contestants[0]];
            else
                output[0] = candidates[contestants[1]];

            //second parent
            if (candidates[contestants[2]].fitness < candidates[contestants[3]].fitness)
                output[1] = candidates[contestants[2]];
            else
                output[1] = candidates[contestants[3]];

            return output;
        }
        public static Chromosom[] RoulletteSelectionAlgorithm(Chromosom[] candidates)
        {
            Chromosom[] output = new Chromosom[2];
            
            //preparing the probabilities for each candidate
            double[] probabilities = new double[candidates.Length];
            int len = candidates.Length;
            double total = 0;
            for (int i = 0; i < len; i++)
            {
                total += 1.0 / candidates[i].fitness;
            }

            for (int i = 0; i < len; i++)
            {
                double probability = (1.0 / candidates[i].fitness) / total;
                probabilities[i] = probability;
            }

            //preparing the list of indexes based on how big the probability is
            List<int> IndexesToRoll = new List<int>();

            for (int i = 0; i < len; i++)
            {
                int numFieldsToAdd = (int)(probabilities[i] * 100);
                for (int k = 0; k < numFieldsToAdd; k++)
                {
                    IndexesToRoll.Add(i);
                }
            }

            //shuffling the list of indexes using Fisher-Yates algorithm
            Random rng = new Random();
            int n = IndexesToRoll.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = IndexesToRoll[k];
                IndexesToRoll[k] = IndexesToRoll[n];
                IndexesToRoll[n] = value;
            }

            total = 0;
            foreach (var i in IndexesToRoll)
            {
                total += probabilities[i];
            }

            //spinning the roulette 2 times
            HashSet<int> set = new HashSet<int>();
            double spin;
            double sum;
            while (set.Count < 2)
            {
                spin = rng.NextDouble() * total;
                sum = 0;
                for (int k = 0; k < IndexesToRoll.Count; k++)
                {
                    sum += probabilities[IndexesToRoll[k]];
                    if (spin < sum)
                    {
                        set.Add(IndexesToRoll[k]);
                    }
                }
            }

            int[] chosenIdexes = set.ToArray();

            output[0] = candidates[chosenIdexes[0]];
            output[1] = candidates[chosenIdexes[1]];

            return output;
        }
    }
}
