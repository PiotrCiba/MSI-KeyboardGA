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
        public static Chromosom[] SelectCrossoverAlgorithm(Chromosom[] parents, int mode, int NumberOfCildren)
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
                case 4:
                    if (NumberOfCildren == 1)
                        output[0].layout = HeurisitcGreedyCrossover(parentOne: parents[0], parentTwo: parents[1]);
                    else
                    {
                        output[0].layout = HeurisitcGreedyCrossover(parentOne: parents[0], parentTwo: parents[1]);
                        output[1].layout = HeurisitcGreedyCrossover(parentOne: parents[1], parentTwo: parents[2]);
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
            Dictionary<char, List<char>> adjacencyList1 = BuildAdjacencyList(parentOne.layout);
            Dictionary<char, List<char>> adjacencyList2 = BuildAdjacencyList(parentTwo.layout);
            Dictionary<char, int> edgeCount = new Dictionary<char, int>();

            // Initialize edge count
            foreach (char node in adjacencyList1.Keys.Union(adjacencyList2.Keys))
            {
                edgeCount[node] = adjacencyList1[node].Count + adjacencyList2[node].Count;
            }

            // Create child solution
            StringBuilder sb = new StringBuilder(length);
            char currentNode = parentOne.layout[0];
            sb.Append(currentNode);

            // Iterate until all nodes are visited
            for (int i = 1; i < length; i++)
            {
                // Remove last node from adjacency lists of neighbors
                RemoveNodeFromAdjacencyLists(currentNode, adjacencyList1, adjacencyList2);

                // Find neighbor with minimum number of edges
                List<char> candidates = new List<char>();
                int minEdges = int.MaxValue;

                foreach (char neighbor in adjacencyList1[currentNode].Union(adjacencyList2[currentNode]))
                {
                    if (edgeCount[neighbor] < minEdges)
                    {
                        candidates.Clear();
                        candidates.Add(neighbor);
                        minEdges = edgeCount[neighbor];
                    }
                    else if (edgeCount[neighbor] == minEdges)
                    {
                        candidates.Add(neighbor);
                    }
                }

                // Alternate between parents for ties
                int nextIndex = i % 2;
                if (candidates.Count > 1)
                {
                    if (nextIndex == 0)
                    {
                        currentNode = parentOne.layout[i];
                    }
                    else
                    {
                        currentNode = parentTwo.layout[i];
                    }
                }
                else
                {
                    currentNode = candidates[0];
                }

                // Add next node to child solution
                sb.Append(currentNode);
                edgeCount[currentNode] = int.MaxValue;
            }

            return sb.ToString();
        }

        public static string HeurisitcGreedyCrossover(Chromosom parentOne, Chromosom parentTwo)
        {
            Random rnd = new Random();
            int length = parentOne.layout.Length;
            Dictionary<char, List<char>> adjacencyList1 = BuildAdjacencyList(parentOne.layout);
            Dictionary<char, List<char>> adjacencyList2 = BuildAdjacencyList(parentTwo.layout);
            Dictionary<char, int> edgeCount = new Dictionary<char, int>();

            // Initialize edge count
            foreach (char node in adjacencyList1.Keys.Union(adjacencyList2.Keys))
            {
                edgeCount[node] = adjacencyList1[node].Count + adjacencyList2[node].Count;
            }

            // Create child solution
            StringBuilder sb = new StringBuilder(length);
            char currentNode = parentOne.layout[0];
            sb.Append(currentNode);

            // Iterate until all nodes are visited
            for (int i = 1; i < length; i++)
            {
                // Remove last node from adjacency lists of neighbors
                RemoveNodeFromAdjacencyLists(currentNode, adjacencyList1, adjacencyList2);

                // Find neighbor with highest viability score
                List<char> candidates = new List<char>();
                double maxScore = double.MinValue;

                foreach (char neighbor in adjacencyList1[currentNode].Union(adjacencyList2[currentNode]))
                {
                    double score = GeneticAlgorithm.SingleFn(neighbor, i);
                    if (score > maxScore)
                    {
                        candidates.Clear();
                        candidates.Add(neighbor);
                        maxScore = score;
                    }
                    else if (score == maxScore)
                    {
                        candidates.Add(neighbor);
                    }
                }

                // Select highest scoring neighbor, or random neighbor if no ties
                if (candidates.Count == 0)
                {
                    candidates.AddRange(adjacencyList1[currentNode].Union(adjacencyList2[currentNode]));
                }

                int randomIndex = rnd.Next(candidates.Count);
                currentNode = candidates[randomIndex];

                // Add next node to child solution
                sb.Append(currentNode);
                edgeCount[currentNode] = int.MaxValue;
            }

            return sb.ToString();
        }

        //Meybe TODO: HRndX i/lub HProX
        //public static string HeurisitcProCrossover(Chromosom parentOne, Chromosom parentTwo)
        /*
            {
                // Define the subset size to select from each parent
                int subsetSize = parent1.Length / 2;

                // Select a subset of components from each parent
                List<char> subset1 = parent1.OrderBy(c => SingleFn(c, parent1.IndexOf(c))).Take(subsetSize).ToList();
                List<char> subset2 = parent2.OrderBy(c => SingleFn(c, parent2.IndexOf(c))).Take(subsetSize).ToList();

                // Combine the selected subsets to create the offspring solution
                List<char> offspring = new List<char>();
                offspring.AddRange(subset1);

                foreach (char c in subset2)
                {
                    if (!offspring.Contains(c))
                    {
                        offspring.Add(c);
                    }
                }

                // Add any remaining components from parent 1 to the offspring
                foreach (char c in parent1)
                {
                    if (!offspring.Contains(c))
                    {
                        offspring.Add(c);
                    }
                }

                // Convert the offspring list to a string and return it
                return new string(offspring.ToArray());
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
    }
}
