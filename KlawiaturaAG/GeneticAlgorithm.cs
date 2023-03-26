using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static (List<Summary>, List<Chromosom[]>) StartNoMemory(int popsize, int childNumber, int currSel, int currX,
                                                      int currMut, double mutChance, int currMutSev, int mutSeverity,
                                                      int currStopMode, int popsToRun, double epsToStopAt)
        {
            //preparing the output variable;
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

                //Delete any children over the popsize limit (may happen if there is more than 1 child per parents, and it "overflows"
                while(children.Count > popsize)
                {
                    children.Remove((from c in children orderby c.fitness descending select c).First());
                }

                //apply any possible mutations (if it doesn't trigger, the method will simply return it)
                for(int i = 0; i < popsize; i++)
                {
                    children[i] = MutationModule.MutationSelector(children[i], currMut, mutChance, currMutSev, mutSeverity);
                }

                //checking the selected stop mode
                if (currStopMode == 0)
                {
                    if(popsToRun == gen)
                        breakCondition = true;
                }
                else if (currStopMode == 1)
                {
                    //if stop mode is 'eps', calculate the relative improvement over he generations
                    double bestOfParents = (from p in Parents orderby p.fitness select p.fitness).First();
                    double bestOfChildren = (from c in children orderby c.fitness select c.fitness).First();
                    double eps = (bestOfChildren - bestOfParents) / bestOfParents;

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
                    else if (eps < epsToStopAt)
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

            //in this case, adding the last generation to output
            Pokolenia.Add(Parents.OrderBy(o=>o.fitness).ToArray());

            return (GenSummaries, Pokolenia);
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

                //Delete any children over the popsize limit (may happen if there is more than 1 child per parents, and it "overflows"
                while (children.Count > popsize)
                {
                    children.Remove((from c in children orderby c.fitness descending select c).First());
                }

                //apply any possible mutations (if it doesn't trigger, the method will simply return it)
                for (int i = 0; i < popsize; i++)
                {
                    children[i] = MutationModule.MutationSelector(children[i], currMut, mutChance, currMutSev, mutSeverity);
                }

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
                Pokolenia.Add(Parents.OrderBy(o=>o.fitness).ToArray());

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

        public static double SingleFn(char znak, int position)
        {
            int row = 0;
            int pos = 0;
            if (pos >= 0 && pos < 12) {
                row = 0;
                pos = position;
            } else if (pos >= 12 && pos < 23) {
                row = 1;
                pos = position - 12;
            } else if (pos >= 23 && pos < 33) {
                row = 2;
                pos = position - 23;
            }

            return GeneticAlgorithm.koszt[row][pos] * GeneticAlgorithm.charFreq.GetValueOrDefault(znak);
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
