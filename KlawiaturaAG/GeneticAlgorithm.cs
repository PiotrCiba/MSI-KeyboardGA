using System;
using System.Collections.Generic;
using System.Linq;

namespace KlawiaturaAG
{
    public class GeneticAlgorithm
    {
        public static (List<Summary>, List<Chromosom[]>) StartNoMemory(int popsize, int childNumber, int currSel, int currX,
                                                      int currMut, double mutChance, int currMutSev, int mutSeverity,
                                                      int currStopMode, int popsToRun, double epsToStopAt)
        {
            //preparing the output variable;
            List<Summary> GenSummaries = new List<Summary>();
            List<Chromosom[]> OutputGen = new List<Chromosom[]>();
            //two generations that we're working with
            Chromosom[] Parents = new Chromosom[popsize];
            //prepping Parents, random scramble
            for (int i = 0; i < popsize; i++)
                Parents[i] = new Chromosom();
            Parents = ScrambleParentsLayouts(Parents);
            //calculating the Parents' fitness
            foreach (var p in Parents)
            {
                p.fitness = Fn(StringToLayout(p.layout));
            }
            //do-while control variables breakcondition to exit, get to count generations (for generation number exit)
            bool breakCondition = false;
            int gensToEmergency = 5;
            int gen = 0;

            //creating the first Summary for GenSummaries
            double bestFit = (from p in Parents orderby p.fitness select p.fitness).First();
            double avgFit = (from p in Parents orderby p.fitness select p.fitness).Average();
            Summary currGenSummary = new Summary(gen, bestFit, avgFit);
            GenSummaries.Add(currGenSummary);

            do
            {
                gen++;
                List<Chromosom> children = new List<Chromosom>();
                //populating the children generation
                while (children.Count != popsize)
                {
                    Chromosom[] selParents = ParentSelection.SelectionInterface(Parents, currSel);
                    Chromosom[] newChildren = CrossoverModule.SelectCrossoverAlgorithm(selParents, currX, childNumber);
                    for (int i = 0; i < childNumber; i++)
                    {
                        newChildren[i].fitness = Fn(StringToLayout(newChildren[i].layout));
                        children.Add(newChildren[i]);
                    }
                }
                while(children.Count > popsize)
                {
                    children.Remove((from c in children orderby c.fitness descending select c).First());
                }
                //add mutations

                //checking the selected stop mode
                if (currStopMode == 0 && popsToRun == gen)
                {
                    breakCondition = true;
                }
                else if (currStopMode == 1)
                {
                    //if stop mode is 'eps', calculate the relative improvement over he generations
                    double bestOfParents = (from p in Parents orderby p.fitness select p.fitness).First();
                    double bestOfChildren = (from c in children orderby c.fitness select c.fitness).First();
                    if (bestOfChildren >= bestOfParents)
                    {
                        if (gensToEmergency > 0)
                        {
                            gensToEmergency--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if ((bestOfChildren - bestOfParents) / bestOfParents < epsToStopAt)
                        breakCondition = true;
                    else
                        gensToEmergency = 5;
                }
                //children are now parents
                Parents = children.ToArray();

                //Updating the genSummaries
                bestFit = (from p in Parents orderby p.fitness select p.fitness).First();
                avgFit = (from p in Parents orderby p.fitness select p.fitness).Average();
                currGenSummary = new Summary(gen, bestFit, avgFit);
                GenSummaries.Add(currGenSummary);
            } while (!breakCondition);

            OutputGen.Add(Parents);
            return (GenSummaries, OutputGen);
        }

        public static (List<Summary>, List<Chromosom[]>) StartFullMemory(int popsize, int childNumber, int currSel, int currX,
                                                          int currMut, double mutChance, int currMutSev, int mutSeverity,
                                                          int currStopMode, int popsToRun, double epsToStopAt)
        {
            //preparing the output variables;
            List<Summary> GenSummaries = new List<Summary>();
            List<Chromosom[]> Pokolenia = new List<Chromosom[]>();

            //two generations that we're working with
            Chromosom[] Parents = new Chromosom[popsize];

            //prepping Parents, random scramble
            for (int i = 0; i < popsize; i++)
                Parents[i] = new Chromosom();
            Parents = ScrambleParentsLayouts(Parents);

            //calculating the Parents' fitness
            foreach (var p in Parents)
            {
                p.fitness = Fn(StringToLayout(p.layout));
            }

            //do-while control variables breakcondition to exit, get to count generations (for generation number exit)
            bool breakCondition = false;
            int gensToEmergency = 5;
            int gen = 0;

            //Parents archived do Listy Pokolenia
            Pokolenia.Add(Parents);

            //creating the first Summary for GenSummaries
            double bestFit = (from p in Parents orderby p.fitness select p.fitness).First();
            double avgFit = (from p in Parents orderby p.fitness select p.fitness).Average();
            Summary currGenSummary = new Summary(gen, bestFit, avgFit);
            GenSummaries.Add(currGenSummary);

            do
            {
                gen++;
                List<Chromosom> children = new List<Chromosom>();
                //populating the children generation
                while (children.Count != popsize)
                {
                    Chromosom[] selParents = ParentSelection.SelectionInterface(Parents, currSel);
                    Chromosom[] newChildren = CrossoverModule.SelectCrossoverAlgorithm(selParents, currX, childNumber);
                    for (int i = 0; i < childNumber; i++)
                    {
                        newChildren[i].fitness = Fn(StringToLayout(newChildren[i].layout));
                        children.Add(newChildren[i]);
                    }
                }
                while (children.Count > popsize)
                {
                    children.Remove((from c in children orderby c.fitness descending select c).First());
                }
                //add mutations

                //checking the selected stop mode
                if (currStopMode == 0 && popsToRun == gen)
                {
                    breakCondition = true;
                }
                else if (currStopMode == 1)
                {
                    //if stop mode is 'eps', calculate the relative improvement over he generations
                    double bestOfParents = (from p in Parents orderby p.fitness select p.fitness).First();
                    double bestOfChildren = (from c in children orderby c.fitness select c.fitness).First();
                    if (bestOfChildren >= bestOfParents)
                    {
                        if (gensToEmergency > 0)
                        {
                            gensToEmergency--;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else if ((bestOfChildren - bestOfParents) / bestOfParents < epsToStopAt)
                        breakCondition = true;
                    else
                        gensToEmergency = 5;
                }
                //children are now parents
                Parents = children.ToArray();

                //Archived do listy Pokolenia;
                Pokolenia.Add(Parents);

                //Updating the genSummaries
                bestFit = (from p in Parents orderby p.fitness select p.fitness).First();
                avgFit = (from p in Parents orderby p.fitness select p.fitness).Average();
                currGenSummary = new Summary(gen, bestFit, avgFit);
                GenSummaries.Add(currGenSummary);
            } while (!breakCondition);


            return (GenSummaries, Pokolenia);
        }

        public static double Fn(string[] input)
        {
            double sum = 0;

            //koszty przycisków wg. metody ewaluacji Worksmana w układzie {2, 12, 11, 10}
            double[][] koszt = {
                new double[] { 5, 5},
                new double[] { 4, 2, 2, 3, 4, 5, 3, 2, 2, 4, 4, 5 },
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
