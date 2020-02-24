using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery.Alpha
{
    static class IndependentSetUtils
    {
        public static HashSet<HashSet<string>> FindIndependentSets(Relation[,] relations, List<string> activities)
        {
            int matSize = activities.Count;
            var independentSets = new HashSet<HashSet<string>>();

            for (int i = 0; i < matSize; i++)
            {
                if (relations[i, i] == Relation.Independency)
                {
                    var nset = new HashSet<string>() { activities[i] };
                    independentSets.Add(new HashSet<string>(nset));

                    for (int j = 0; j < matSize; j++)
                    {
                        nset = new HashSet<string>() { activities[i] };
                        for (int k = j; k < matSize; k++)
                        {
                            bool allIndependent = true;
                            if (relations[i, k] == Relation.Independency && i != k)
                            {
                                foreach (string act in nset)
                                {
                                    int m = activities.FindIndex(a => a == act); // bude lepší poslat si sem indexer
                                    if (relations[m, k] != Relation.Independency)
                                    {
                                        allIndependent = false;
                                    }
                                }
                                if (allIndependent)
                                {
                                    nset.Add(activities[k]);
                                    independentSets.Add(new HashSet<string>(nset));
                                }
                            }
                        }
                    }
                }
            }

            // vypis
            Console.WriteLine("Number of ind sets: " + independentSets.Count);
            foreach (HashSet<string> iset in independentSets)
            {
                int i = 1;

                Console.Write("\tIndSet " + i + ": ");
                foreach (string act in iset)
                {
                    Console.Write(act + " ");
                }
                Console.Write("\n");
                i++;
            }
            return independentSets;
        }

        public static HashSet<Tuple<HashSet<string>, HashSet<string>>> FindMaximalIndependentSetsAB(HashSet<HashSet<string>> independentSets, Relation[,] relations, Dictionary<string, int> activityIndices)
        {
            var setsAB = new HashSet<Tuple<HashSet<string>, HashSet<string>>>();

            foreach (var setA in independentSets)
            {
                foreach (var setB in independentSets)
                {
                    bool isValidSet = true;
                    foreach (string activityA in setA)
                    {
                        foreach (string activityB in setB)
                        {
                            if (relations[activityIndices[activityA], activityIndices[activityB]] != Relation.Succession)
                            {
                                // šlo by zoptimalizovat nepřeváděním aktivit na stringy a zpět
                                isValidSet = false;
                            }
                        }
                    }

                    if (isValidSet)
                    {
                        bool isBiggerSet = false;
                        var setsToRemove = new HashSet<Tuple<HashSet<string>, HashSet<string>>>();

                        foreach (var setAB in setsAB)
                        {
                            if (setAB.Item1.IsSubsetOf(setA) && setAB.Item2.IsSubsetOf(setB))
                            {
                                isBiggerSet = true;
                                setsToRemove.Add(setAB);
                            }
                        }
                        setsAB.ExceptWith(setsToRemove);

                        if (!isBiggerSet && setA.Count == 1 && setB.Count == 1)
                        {
                            isBiggerSet = true;
                        }

                        if (isBiggerSet)
                        {
                            setsAB.Add(new Tuple<HashSet<string>, HashSet<string>>(setA, setB));
                        }
                    }
                }
            }

            //vypis
            Console.WriteLine("Number of sets AB: " + setsAB.Count);
            foreach(var setAB in setsAB)
            {
                Console.WriteLine("\tSet A:");
                foreach(string act in setAB.Item1)
                {
                    Console.WriteLine("\t\t- " + act);
                }
                Console.WriteLine("\tSet B:");
                foreach (string act in setAB.Item2)
                {
                    Console.WriteLine("\t\t- " + act);
                }
            }
            return setsAB;
        }
    }
}
