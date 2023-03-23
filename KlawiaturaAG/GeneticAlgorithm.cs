﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media.Imaging;

namespace KlawiaturaAG
{
    public enum Selections
    {
        Tournament = 0,
        Roulette = 1
    }
    public enum Crossovers
    {
        OX = 0,
        ERX = 1
    }
    public enum Mutations
    {
        PairSwap = 0, 
        PartialScramble = 1
    }
    public enum MutationSeverities
    {
        Random = 0, 
        Continuous = 1
    }
    public class GeneticAlgorithm
    {
        private Chromosom[] rodzice;
        private Chromosom[] dzieci;

        public GeneticAlgorithm()
        {
            rodzice = new Chromosom[0];
            dzieci = new Chromosom[0];
        }
        public GeneticAlgorithm(int popsize)
        {
            rodzice = new Chromosom[popsize];
            dzieci = new Chromosom[popsize];

            Random rand = new Random();

            for(int i = 0; i<popsize;i++)
            {
                rodzice[i] = new Chromosom();
                char[] chars = LayoutToString(rodzice[i].layout).ToCharArray();
                for(int k = 0; k < chars.Length; k++)
                {
                    int randIndex = rand.Next(chars.Length);
                    char temp = chars[k];
                    chars[k] = chars[randIndex];
                    chars[randIndex] = temp;
                }
                rodzice[i].layout = StringToLayout(new string(chars));
            }
        }
        /*public int popSize { get; set; } = 25;
        public int parentNumber { get; set; } = 2;
        public int childNumber { get; set; } = 1;
        public Selections currSel { get; set; } = Selections.Tournament;
        public Crossovers currX { get; set; } = Crossovers.OX;
        public Mutations currMut { get; set; } = Mutations.PairSwap;
        public double mutChance { get; set; } = 0.01;
        public MutationSeverities currMutSev {get;set;} = MutationSeverities.Random;
        public int mutSeverity { get; set; } = 1;
        public int popsToRun { get; set; } = 25;
        public double epsToStopAt { get; set; } = 0.01;
        public int currStopMode { get; set; } = 0;*/
        public (List<Summary>, Chromosom[]) StartNoMemory(int parentNumber, int childNumber, Selections currSel, Crossovers currX, 
                                                      Mutations currMut, double mutChance, MutationSeverities currMutSev, int mutSeverity,
                                                      int currStopMode, int popsToRun, double epsToStopAt) {
            List<Summary> GenSummaries = new List<Summary>();
            Chromosom[] Generation = new Chromosom[rodzice.Length];

            bool breakCondition = false;
            int gen = 0;

            do{ 
            
            }while (!breakCondition);
            

            return (GenSummaries,Generation);
        }

        public (List<Summary>, List<Chromosom[]>) StartFullMemory(int parentNumber, int childNumber, Selections currSel, Crossovers currX, 
                                                          Mutations currMut, double mutChance, MutationSeverities currMutSev, int mutSeverity, 
                                                          int currStopMode, int popsToRun, double epsToStopAt){
            List<Summary> GenSummaries = new List<Summary>();
            List<Chromosom[]> Generation = new List<Chromosom[]>();

            bool breakCondition = false;
            int gen = 0;

            do
            {

            } while (!breakCondition);


            return (GenSummaries, Generation);
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
        public static string LayoutToString(string[] input)
        {
            return string.Join("", input);
        }
        public static string[] StringToLayout(string input)
        {
            string[] output = new string[4];
            output[0] = input.Substring(0, 2);
            output[1] = input.Substring(2, 12);
            output[2] = input.Substring(14, 11);
            output[3] = input.Substring(25, 10);
            return output;
        }
        
    }
}
