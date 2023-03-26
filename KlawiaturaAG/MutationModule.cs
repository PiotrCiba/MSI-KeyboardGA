using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    public static class MutationModule
    {
        
        public static Chromosom MutationSelector(Chromosom child, int mutMode, double mutChance, int mutSev, int severity)
        {
            Chromosom output = child;
            string dna = output.layout;
            Random rnd = new Random();
            double roll = rnd.NextDouble();
            if (roll < mutChance)
            {
                switch (mutMode)
                {
                    case 0:
                        dna = PairSwapMutation(dna, mutSev, severity);
                        break;
                    case 1:
                        dna = PartialScrambleMutation(dna, mutSev, severity);
                        break;
                    case 2:
                        dna = InversionMutation(dna, mutSev, severity);
                        break;
                }
                output.layout = dna;
                output.fitness = GeneticAlgorithm.Fn(GeneticAlgorithm.StringToLayout(dna));
            }
            return output;
        }

        public static string PairSwapMutation(string dnaSample, int mutSev, int Severity)
        {
            Random rnd = new Random();
            char[] mutantDna = dnaSample.ToCharArray();
            int mutationSpread = rnd.Next(Severity);
            int len = dnaSample.Length;
            if (mutSev == 0)
            {
                for (int i = 0; i < mutationSpread; i++)
                {
                    int pointA, pointB;
                    pointA = rnd.Next(len);
                    do
                    {
                        pointB = rnd.Next(len);
                    } while (pointA != pointB);
                    char temp = mutantDna[pointA];
                    mutantDna[pointA] = mutantDna[pointB];
                    mutantDna[pointB] = temp;
                }
            }else if(mutSev == 1)
            {
                int[] pairsToSwap = new int[mutationSpread * 2];
                for(int i = 0; i < mutationSpread * 2; i++)
                {
                    int point;
                    do
                    {
                        point = rnd.Next(len);
                    } while (!pairsToSwap.Contains(point));
                    pairsToSwap[i] = point;
                }
                for(int i=0; i < mutationSpread * 2; i += 2)
                {
                    char temp = mutantDna[pairsToSwap[i]];
                    mutantDna[pairsToSwap[i]] = mutantDna[pairsToSwap[i + 1]];
                    mutantDna[pairsToSwap[i + 1]] = temp;
                }
            }
            return new string(mutantDna);
        }

        public static string PartialScrambleMutation(string dnaSample, int mutSev, int Severity)
        {
            Random rnd = new Random();
            char[] mutantDna = dnaSample.ToCharArray();
            int mutationSpread = rnd.Next(Severity);
            int len = dnaSample.Length;
            if(mutSev == 0)
            {
                int[] indexes = new int[mutationSpread];
                for(int i = 0; i < mutationSpread; i++)
                {
                    int rndindex;
                    do
                    {
                        rndindex = rnd.Next(len);
                    } while (!indexes.Contains(rndindex));
                    indexes[i] = rndindex;
                }
                int[] indexesTargetted = new int[mutationSpread];
                for (int i = 0; i < mutationSpread; i++)
                {
                    var temp = (from p in indexes where p != indexes[i] select p).Except(indexesTargetted).ToArray();
                    int target = rnd.Next(temp.Length);
                    indexesTargetted[i] = target;
                }
                for(int i = 0; i < mutationSpread; i++)
                {
                    mutantDna[indexes[i]] = dnaSample[indexesTargetted[i]];
                }

            }else if(mutSev == 1)
            {
                char[] cutout = new char[mutationSpread];
                int index = rnd.Next(len - mutationSpread);
                for(int i = index; i < index + mutationSpread; i++)
                {
                    cutout[i] = mutantDna[i];
                }
                for(int i = 0; i < mutationSpread; i++)
                {
                    int rndIndex = rnd.Next(mutationSpread);
                    char temp = cutout[i];
                    cutout[i] = cutout[rndIndex];
                    cutout[rndIndex] = temp;
                }
                for (int i = index; i < index + mutationSpread; i++)
                {
                    mutantDna[i] = cutout[i];
                }
            }
            return new string(mutantDna);
        }

        public static string InversionMutation(string dnaSample, int mutSev, int Severity)
        {
            Random rnd = new Random();
            char[] mutantDna = dnaSample.ToCharArray();
            int mutationSpread = rnd.Next(Severity);
            int len = dnaSample.Length;
            if (mutSev == 0)
            {
                int index = rnd.Next(len - mutationSpread);
                for (int i = index; i < index + mutationSpread; i++)
                {
                    mutantDna[i] = dnaSample[index + mutationSpread - i];
                }
            }
            else if (mutSev == 1)
            {
                char[] cutout = new char[mutationSpread];
                int[] indexes = new int[mutationSpread];
                for (int i = 0; i < mutationSpread; i++)
                {
                    int number;
                    do
                    {
                        number = rnd.Next(len);
                    } while (!indexes.Contains(number));
                    cutout[i] = mutantDna[number];
                    indexes[i] = number;
                }
                cutout = cutout.Reverse().ToArray();
                for (int i = 0; i < mutationSpread; i++)
                {
                    mutantDna[indexes[i]] = cutout[i];
                }
            }
            return new string(mutantDna);
        }
    }
}
