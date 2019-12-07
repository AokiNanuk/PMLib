using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
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
                    var nset = new HashSet<string>();
                    for (int j = 0; j < matSize; j++)
                    {
                        if (relations[i, j] == Relation.Independency)
                        {
                            nset.Add(activities[j]);
                            independentSets.Add(new HashSet<string>(nset));
                        }
                    }
                }
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
                    foreach (string activityA in setA)
                    {
                        foreach (string activityB in setB)
                        {
                            if (relations[activityIndices[activityA], activityIndices[activityB]] != Relation.Succession)
                            {
                                // šlo by zoptimalizovat nepřeváděním aktivit na stringy a zpět
                                continue;
                            }
                        }
                    }

                    bool isBiggerSet = false;
                    foreach (var setAB in setsAB)
                    {
                        if (setAB.Item1.IsSubsetOf(setA) && setAB.Item2.IsSubsetOf(setB))
                        {
                            isBiggerSet = true;
                            setsAB.Remove(setAB);
                        }
                    }

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
            return setsAB;
        }
    }
}
