using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    public static class MutationModule
    {
        
        public static Chromosom MutationSelector(Chromosom child, int mutMode, double mutChance, int severity)
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
                        dna = PairSwapMutation(dna, severity);
                        break;
                    case 1:
                        dna = PartialScrambleMutation(dna, severity);
                        break;
                    case 2:
                        dna = InversionMutation(dna, severity);
                        break;
                }
                output.layout = dna;
            }
            return output;
        }

        public static string PairSwapMutation(string dnaSample, int Severity)
        {
            Random rnd = new Random();
            char[] mutantDna = dnaSample.ToCharArray();
            int mutationSpread = rnd.Next(1, Severity);
            int len = dnaSample.Length;
            for (int i = 0; i < mutationSpread; i++)
            {
                int pointA, pointB;
                pointA = rnd.Next(len);
                do
                {
                    pointB = rnd.Next(len);
                } while (pointA == pointB);
                char temp = mutantDna[pointA];
                mutantDna[pointA] = mutantDna[pointB];
                mutantDna[pointB] = temp;
            }
            return new string(mutantDna);
        }

        public static string PartialScrambleMutation(string dnaSample, int Severity)
        {
            Random rnd = new Random();
            char[] mutantDna = dnaSample.ToCharArray();
            int mutationSpread = rnd.Next(Severity);
            int len = dnaSample.Length;

            //setup 2 cutout points, a mutationSpread apart
            int pointA, pointB;
            pointA = rnd.Next(len-mutationSpread);
            pointB = pointA + mutationSpread;

            //copy over the chars betwen those points to a cutout
            char[] cutout = new char[mutationSpread];
            for(int i = pointA; i < pointB; i++)
            {
                cutout[i - pointA] = mutantDna[i];
            }
            //scramble the contents of the cutout
            for (int i = 0; i < mutationSpread; i++)
            {
                int randIndex = rnd.Next(mutationSpread);
                char temp = cutout[i];
                cutout[i] = cutout[randIndex];
                cutout[randIndex] = temp;
            }
            //write the contents of the cutout back where it came from
            for (int i = pointA; i < pointB; i++)
            {
                mutantDna[i] = cutout[i - pointA];
            }
            return new string(mutantDna);
        }

        public static string InversionMutation(string dnaSample, int Severity)
        {
            Random rnd = new Random();
            char[] mutantDna = dnaSample.ToCharArray();
            int mutationSpread = rnd.Next(Severity);
            int len = dnaSample.Length;

            //setup 2 cutout points, a mutationSpread apart
            int pointA, pointB;
            pointA = rnd.Next(len - mutationSpread);
            pointB = pointA + mutationSpread;

            //copy over the chars betwen those points to a cutout
            char[] cutout = new char[mutationSpread];

            for (int i = pointA; i < pointB; i++)
            {
                cutout[i - pointA] = mutantDna[i];
            }

            //reverse the contents of cutout
            cutout = cutout.Reverse().ToArray();

            //write the contents of the cutout back where it came from
            for (int i = pointA; i < pointB; i++)
            {
                mutantDna[i] = cutout[i - pointA];
            }
            return new string(mutantDna);
        }
    }
}
