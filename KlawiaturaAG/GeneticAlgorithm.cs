using System;
using System.Collections.Generic;
using System.Linq;

namespace KlawiaturaAG
{
    public class GeneticAlgorithm
    {

        //częstotliwości znaków w języku Angielskim
        public static Dictionary<char, double> charFreq = new Dictionary<char, double>() {
                {'A', 8.4966}, {'B', 2.0720}, {'C', 4.5388}, {'D', 3.3844}, {'E', 11.1607},
                {'F', 1.8121}, {'G', 2.4705}, {'H', 3.0034}, {'I', 7.5448}, {'J', 0.1965},
                {'K', 1.1016}, {'L', 5.4893}, {'M', 3.0129}, {'N', 6.6544}, {'O', 7.1635},
                {'P', 3.1671}, {'Q', 0.1962}, {'R', 7.5809}, {'S', 5.7351}, {'T', 6.9509},
                {'U', 3.6308}, {'V', 1.0074}, {'W', 1.2899}, {'X', 0.2902}, {'Y', 1.7779},
                {'Z', 0.2722},
                {'[', 0.01}, {']', 0.01}, {';', 0.0711},
                {'\'', 0.54}, {',', 1.3688}, {'.', 1.4511}, {'?', 0.1244}
            };

        //koszty przycisków wg. metody ewaluacji Worksmana w układzie {2, 12, 11, 10}
        public static double[][] koszt = {
                new double[] { 4, 2, 2, 3, 4, 5, 3, 2, 2, 4, 4, 5 },
                new double[] { 1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5, 3 },
                new double[] { 4, 4, 3, 2, 5, 3, 2, 3, 4, 4 }
        };

        public static (List<Summary>, List<Chromosom[]>) Start(Settings s, IProgress<int> progress)
        {
            //Czacza idzie tak: Rodzice > Fitness > CarryOver > Selekcja > Crossover > Mutacja > Dzieci > "Rodzice = Dzieci" > repeat

            //preparing the output variable;
            List<Summary> GenSummaries = new List<Summary>();
            List<Chromosom[]> Pokolenia = new List<Chromosom[]>();

            //Parent generation
            Chromosom[] Parents = new Chromosom[s.popSize];

            //prepping Parents, random scramble
            for (int i = 0; i < s.popSize; i++)
                Parents[i] = new Chromosom();
            Parents = ScrambleParentsLayouts(Parents);

            //calculating the Parents' fitness
            foreach (var p in Parents)
            {
                p.fitness = Fn(StringToLayout(p.layout));
            }

            //sorting Parents by fitness
            Parents = Parents.OrderBy(p => p.fitness).ToArray();

            //adding the 0-th generation to Pokolenia
            Pokolenia.Add(Parents);

            //do-while control variables breakcondition to exit, get to count generations (for generation number exit)
            bool breakCondition = false;
            int gensToMaintainEps = 10;
            int gen = 0;

            //creating the first Summary for GenSummaries
            var fitnesses = (from p in Parents orderby p.fitness select p.fitness);
            double bestFit = fitnesses.First();
            double avgFit = fitnesses.Average();
            Summary currGenSummary = new Summary(gen, bestFit, avgFit);

            GenSummaries.Add(currGenSummary);

            //do-while for the GA's core
            do
            {
                gen++;  //increment gens for gen-based stop condition

                //prep children List
                List<Chromosom> children = new List<Chromosom>();

                //carryover-breedingPop from parents
                (Chromosom[] carry, Chromosom[] breeders) carryAndPop = CarryModule.Select(Parents, s.carryoverType, s.carryVar, s.culling, s.cullingRate);

                //copy carry to children
                foreach (var c in carryAndPop.carry)
                {
                    children.Add(c);
                }

                //while children < parents
                while (children.Count < s.popSize)
                {
                    //parentSelection, from breedingPop
                    Chromosom[] couple = ParentSelection.Select(carryAndPop.breeders, s.currSel);

                    //crossover operator, from selected parents
                    Chromosom[] childrenTemp = CrossoverModule.Select(couple, s.currX, s.childNumber);


                    //iterate over newly born chlidren
                    for (int i = 0; i < s.childNumber; i++)
                    {
                        //mutationModule, may hit, may not hit
                        childrenTemp[i] = MutationModule.Select(childrenTemp[i], s.currMut, s.mutChance, s.mutSeverity);
                        //add new chlidren to children
                        children.Add(childrenTemp[i]);
                    }
                }

                //calculate fitness of all children
                foreach (var c in children)
                {
                    if (c.fitness == 0)
                        c.fitness = Fn(StringToLayout(c.layout));
                }
                //sort ascending
                children = (from c in children orderby c.fitness ascending select c).ToList();

                //if children overflow, cull the lowest fitness one
                while (children.Count > s.popSize)
                {
                    children.Remove(children.Last());
                }

                //if fullmemory, add children to Pokolenia
                if (s.fullMemory)
                {
                    Pokolenia.Add(children.ToArray());
                }
                //summary prep, add to GenSummaries
                fitnesses = (from c in children orderby c.fitness select c.fitness);
                bestFit = fitnesses.First();
                avgFit = fitnesses.Average();
                currGenSummary = new Summary(gen, bestFit, avgFit);
                GenSummaries.Add(currGenSummary);

                //parents = children
                Parents = children.ToArray();

                int report = 0;

                System.Diagnostics.Debug.Write("gens = " + (int)(((double)gen / s.gensToRun) * 100));
                Summary[] tmpoo = GenSummaries.TakeLast(2).ToArray();
                double cEps = (tmpoo[0].BestFitness - tmpoo[1].BestFitness) / tmpoo[1].BestFitness;
                //claculate the report
                System.Diagnostics.Debug.WriteLine("; EPS = " + (int)((s.epsToStopAt / cEps) * 100));

                if (!s.currStopMode)
                {
                    //update the progress for progressbar
                    report = report = (int)(((double)gen / s.gensToRun) * 100);

                    //gens to run mode
                    if (gen >= s.gensToRun)
                    {
                        breakCondition = true;
                    }
                }
                else
                {
                    //stop at set improvement
                    Summary[] temp = GenSummaries.TakeLast(2).ToArray();
                    double currentEps = (temp[0].BestFitness - temp[1].BestFitness) / temp[1].BestFitness;

                    //calculate the report
                    report = (int)((s.epsToStopAt / cEps) * 100);
                    if (report < 0)
                        report = 0;

                    if (currentEps <= s.epsToStopAt)
                    {
                        //the set improvement has to keep at it for ~10 rounds before stopping
                        if (gensToMaintainEps == 0)
                            breakCondition = true;
                        else
                            gensToMaintainEps--;
                    }
                    else
                    {
                        //if the low eps streak is broken, reset the counter
                        gensToMaintainEps = 10;
                    }

                }
                progress.Report(report);
            } while (!breakCondition);

            //return the results of the algorithm
            return (GenSummaries, Pokolenia);
        }

        public static double Fn(string[] input)
        {
            double sum = 0;

            for (int i = 0; i < input.Length; i++)
            {
                for (int k = 0; k < input[i].Length; k++)
                {
                    //sumowanie [koszt klawisza] * [częstotliwość znaku]
                    sum += GeneticAlgorithm.koszt[i][k] * GeneticAlgorithm.charFreq.GetValueOrDefault(input[i][k]);
                }
            }

            //zwracanie sumy
            return sum;
        }

        /*                     0  1  2  3  4  5  6  7  8  9  10  11
                new double[] { 4, 2, 2, 3, 4, 5, 3, 2, 2, 4, 4, 5 },
                               12   13 14 15 16 17 18 19 20 21   22
                new double[] { 1.5, 1, 1, 1, 3, 3, 1, 1, 1, 1.5, 3 },
                               23 24 25 26 27 28 29 30 31 32
                new double[] { 4, 4, 3, 2, 5, 3, 2, 3, 4, 4 }
         */

        public static double SingleFn(char znak, int position)
        {
            int row = 0;
            int col = 0;

            if (position >= 0 && position < 12)
            {
                row = 0;
                col = position;
            }
            else if (position >= 12 && position < 23)
            {
                row = 1;
                col = position - 12;
            }
            else if (position >= 23 && position < 33)
            {
                row = 2;
                col = position - 23;
            }

            return GeneticAlgorithm.koszt[row][col] * GeneticAlgorithm.charFreq.GetValueOrDefault(znak);
        }
        public static string LayoutToString(string[] input)
        {
            return string.Join("", input);
        }
        public static string[] StringToLayout(string input)
        {
            //QWERTYUIOP[]ASDFGHJKL;'ZXCVBNM,.?
            string[] output = new string[3];
            output[0] = input.Substring(0, 12);
            output[1] = input.Substring(12, 11);
            output[2] = input.Substring(23, 10);
            return output;
        }
        public static Chromosom[] ScrambleParentsLayouts(Chromosom[] input)
        {
            Chromosom[] output = input;
            int len = input.Length;

            Random rand = new Random();

            for (int i = 0; i < len; i++)
            {
                char[] chars = output[i].layout.ToCharArray();
                for (int k = 0; k < chars.Length; k++)
                {
                    int randIndex = rand.Next(chars.Length);
                    char temp = chars[k];
                    chars[k] = chars[randIndex];
                    chars[randIndex] = temp;
                }
                output[i].layout = new string(chars);
            }

            return output;
        }
    }
}
