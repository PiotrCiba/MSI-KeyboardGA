using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KlawiaturaAG
{
    public static class CrossoverModule
    {
        public static string[] SelectCrossoverAlgorithm(Chromosom[] parents, int mode, int NumberOfCildren)
        {
            string[] output = new string[NumberOfCildren];
            switch (mode)
            {
                case 0:
                    if (NumberOfCildren == 1)
                        output[0] = OrderCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0] = OrderCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1] = OrderCrossover(parentOne: parents[1], parentTwo: parents[2]);
                    }
                    break;
                case 1:
                    if (NumberOfCildren == 1)
                        output[0] = EdgeRecombinationCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0] = EdgeRecombinationCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1] = EdgeRecombinationCrossover(parentOne: parents[1], parentTwo: parents[2]);
                    }
                    break;
            }
            return output;
        }

        public static string OrderCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            //parents DNA extraction
            string parentA,parentB;
            parentA = GeneticAlgorithm.LayoutToString(parentOne.layout);
            parentB = GeneticAlgorithm.LayoutToString(parentTwo.layout);

            //misc variables, two random-ish crossover points
            int len,pointA,pointB;
            len = parentA.Length;
            Random rnd = new Random();
            pointA = rnd.Next(len);
            pointB = rnd.Next(len);
            
            //swap A and B if they're backwards
            if(pointA > pointB)
            {
                int tmp = pointA;
                pointA = pointB;
                pointB = tmp;
            }

            char[] child = new char[len];
            for(int i = pointA; i < pointB; i++)
                child[i] = parentA[i];

            int childIndex = pointB + 1;
            for(int i = 0; i < len; i++)
            {
                if (childIndex == len)
                    childIndex = 0;
                if (!child.Contains(parentB[i]))
                {
                    child[childIndex] = parentB[i];
                    childIndex++;
                }
                    
            }

            return new string(child);
        }
    
        public static string EdgeRecombinationCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            //parents DNA extraction
            string parentA, parentB;
            parentA = GeneticAlgorithm.LayoutToString(parentOne.layout);
            parentB = GeneticAlgorithm.LayoutToString(parentTwo.layout);

            int len = parentA.Length;
            Random rnd = new Random();

            Dictionary<char, List<char>> adjacencyListA = BuildAdjadencylist(parentA);
            Dictionary<char, List<char>> adjacencyListB = BuildAdjadencylist(parentB);

            char currentNode = parentA[rnd.Next(len)];

            char[] child = new char[len];
            child[0] = currentNode;

            RemoveNodeFromAdjacencyLists(currentNode, adjacencyListA, adjacencyListB);

            for(int i = 0; i < len; i++)
            {
                List<char> neighbors = adjacencyListA[currentNode].Concat(adjacencyListB[currentNode]).ToList();

                if(neighbors.Count == 0)
                {
                    for(int j = 0; j < len; j++)
                    {
                        if (!child.Contains(parentA[j]))
                        {
                            currentNode = parentA[j];
                            break;
                        }
                    }
                }
                else
                {
                    currentNode = neighbors.OrderBy(n => adjacencyListA[n].Count + adjacencyListB[n].Count).First();
                }

                child[i] = currentNode;

                RemoveNodeFromAdjacencyLists(currentNode, adjacencyListA, adjacencyListB);
            }

            return new string(child);
        }

        private static Dictionary<char, List<char>> BuildAdjadencylist(string input)
        {
            int len = input.Length;

            Dictionary<char, List<char>> output = new Dictionary<char, List<char>>();

            for(int i=0; i < len; i++)
            {
                char current = input[i];

                if (!output.ContainsKey(current))
                {
                    output[current] = new List<char>();
                }

                if (i == 0)
                {
                    output[current].Add(input[len - 1]);
                    output[current].Add(input[1]);
                }else if(i == len - 1)
                {
                    output[current].Add(input[len - 2]);
                    output[current].Add(input[0]);
                }
                else
                {
                    output[current].Add(input[i - 1]);
                    output[current].Add(input[i + 1]);
                }
            }

            return output;
        }

        private static void RemoveNodeFromAdjacencyLists(char node, Dictionary<char, List<char>> adjacencyList1, Dictionary<char, List<char>> adjacencyList2)
        {
            foreach (List<char> neighbors in adjacencyList1.Values)
            {
                neighbors.Remove(node);
            }

            foreach (List<char> neighbors in adjacencyList2.Values)
            {
                neighbors.Remove(node);
            }
        }
    }
}
