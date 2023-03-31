using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KlawiaturaAG
{
    public static class CrossoverModule
    {
        public static Chromosom[] Select(Chromosom[] parents, int mode, int NumberOfCildren)
        {
            Chromosom[] output = new Chromosom[NumberOfCildren];
            for(int i = 0; i < NumberOfCildren; i++)
            {
                output[i] = new Chromosom();
            }

            switch (mode)
            {
                case 0:
                    if (NumberOfCildren == 1)
                        output[0].layout = OrderCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0].layout = OrderCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1].layout = OrderCrossover(parentOne: parents[1], parentTwo: parents[2]);
                    }
                    break;
                case 1:
                    if (NumberOfCildren == 1)
                        output[0].layout = CycleCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0].layout = CycleCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1].layout = CycleCrossover(parentOne: parents[1], parentTwo: parents[0]);
                    }
                    break;
                case 2:
                    if (NumberOfCildren == 1)
                        output[0].layout = EdgeRecombinationCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0].layout = EdgeRecombinationCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1].layout = EdgeRecombinationCrossover(parentOne: parents[1], parentTwo: parents[2]);
                    }
                    break;
                case 3:
                    if (NumberOfCildren == 1)
                        output[0].layout = AlternatingEdgeCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0].layout = AlternatingEdgeCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1].layout = AlternatingEdgeCrossover(parentOne: parents[1], parentTwo: parents[2]);
                    }
                    break;
            }
            return output;
        }

        public static string OrderCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            //parents DNA extraction
            string parentA,parentB;
            parentA = parentOne.layout;
            parentB = parentTwo.layout;

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
            for(int i = pointA; i <= pointB; i++)
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

        public static string CycleCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            string p1 = parentOne.layout;
            string p2 = parentTwo.layout;
            int len = p1.Length;
            int[] positions = Enumerable.Range(0, len).ToArray();
            int posIndex = 0;
            bool[] visited = new bool[len];

            char[] child = new char[len];
            while (posIndex < len)
            {
                int currPos = positions[posIndex];
                if (!visited[currPos])
                {
                    char val = p1[currPos];
                    int indexInP2 = p2.IndexOf(val);

                    while(indexInP2 != currPos)
                    {
                        visited[indexInP2] = true;
                        child[indexInP2] = p1[indexInP2];
                        indexInP2 = p2.IndexOf(p1[indexInP2]);
                    }

                    visited[currPos] = true;
                    child[currPos] = p1[currPos];
                }

                posIndex++;
            }

            return new string(child);
        }

        public static string EdgeRecombinationCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            //parents DNA extraction
            string parentA, parentB;
            parentA = parentOne.layout;
            parentB = parentTwo.layout;

            int len = parentA.Length;
            Random rnd = new Random();

            Dictionary<char, List<char>> adjacencyListA = BuildAdjacencyList(parentA);
            Dictionary<char, List<char>> adjacencyListB = BuildAdjacencyList(parentB);

            char currentNode = parentA[rnd.Next(len)];

            char[] child = new char[len];
            child[0] = currentNode;

            RemoveNodeFromAdjacencyLists(currentNode, adjacencyListA, adjacencyListB);

            for (int i = 0; i < len; i++)
            {
                List<char> neighbors = adjacencyListA[currentNode].Concat(adjacencyListB[currentNode]).ToList();

                if (neighbors.Count == 0)
                {
                    for (int j = 0; j < len; j++)
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

        public static string AlternatingEdgeCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            int length = parentOne.layout.Length;
            Random rnd = new Random();
            Dictionary<char, char>[] ArcList = new Dictionary<char, char>[2];
            ArcList[0] = BuildArcList(parentOne.layout);
            ArcList[1] = BuildArcList(parentTwo.layout);

            char[] child = new char[length];
            child[0] = parentOne.layout[0];
            int parentSwitch = 0;

            for(int i = 1; i < length; i++)
            {
                char nextone = ArcList[parentSwitch][child[i-1]];
                if (!child.Contains(nextone))
                {
                    child[i] = nextone;
                    parentSwitch += (parentSwitch + 1) % 2;
                }
                else
                {
                    var tempArr = (from a in ArcList[0] where !child.Contains(a.Key) select a.Key).ToArray();
                    int tempIndex = rnd.Next(tempArr.Length);
                    child[i] = tempArr[tempIndex];
                }
            }

            return new string(child);
        }
        /*
                public static string HeurisitcGreedyCrossover(Chromosom parentOne, Chromosom parentTwo)
                {
                    //string charset = "QWERTYUIOP[]ASDFGHJKL;\'ZXCVBNM,.?";

                    int len = parentOne.layout.Length;
                    double[,] costs = new double[len,2];
                    double[] costDiff = new double[len];
                    int indexes = new int[len];
                    char[] child = new char[len];
                    for(int i = 0; i < len; i++)
                    {
                        costs[i, 1] = GeneticAlgorithm.SingleFn(parentOne.layout[i], i);
                        costs[i, 2] = GeneticAlgorithm.SingleFn(parentTwo.layout[i], i);
                        costDiff[i] = Math.Abs(costs[i, 1] - costs[i, 2]);
                        indexes[i] = i;
                    }
                    Array.Sort(costDiff, indexes);
                    for(int i = 0; i < len; i++)
                    {
                        if(costs[i, 1]<= costs[i, 2])
                        {

                        }
                        else
                        {

                        }
                    }

                    return new string(child);
                }
        */

        private static Dictionary<char, List<char>> BuildAdjacencyList(string input)
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
        private static Dictionary<char, char> BuildArcList(string input)
        {
            int len = input.Length;
            Dictionary<char,char> ArcList = new Dictionary<char, char>();
            for(int i = 0; i < len; i++)
            {
                if (i < len-1)
                {
                    ArcList[input[i]] = input[i + 1];
                }
                else
                {
                    ArcList[input[i]] = input[0];
                }
            }
            return ArcList;
        }
    }
}
