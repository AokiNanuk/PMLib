using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    static class PetriNetAnalyzer
    {
        private static Tuple<List<string>, Relation[,]> MakeEmptyFootprint(List<ITransition> transitions)
        {
            List<string> activityIndices = new List<string>();
            foreach (ITransition t in transitions)
            {
                activityIndices.Add(t.Activity);
            }
            return new Tuple<List<string>, Relation[,]>(activityIndices, new Relation[activityIndices.Count, activityIndices.Count]);
        }

        // DEPRECATED
        private static void NextStep(ref Tuple<List<string>, Relation[,]> indexedFootprint, ITransition beginningTransition, IPetriNet net)
        {
            if (beginningTransition.OutputPlaces.Count > 1)
            {
                //GoParallel(ref indexedFootprint, beginningTransition, net);
            }
            if (beginningTransition.OutputPlaces[0].Id == net.EndPlace.Id)
            {
                return;
            }

            int fromIndex = indexedFootprint.Item1.FindIndex(a => a == beginningTransition.Activity);
            List<int> toIndices = new List<int>();
            List<ITransition> nextTransitions = new List<ITransition>();
            foreach(ITransition t in net.Transitions)
            {
                foreach(IPlace p in t.InputPlaces)
                {
                    if (p.Id == beginningTransition.OutputPlaces[0].Id)
                    {
                        toIndices.Add(indexedFootprint.Item1.FindIndex(a => a == t.Activity));
                        nextTransitions.Add(t);
                    }
                }
            }

            foreach(int toIndex in toIndices)
            {
                indexedFootprint.Item2[fromIndex, toIndex] = Relation.Succession;
                indexedFootprint.Item2[toIndex, fromIndex] = Relation.Predecession;
            }
            foreach(ITransition t in nextTransitions)
            {
                NextStep(ref indexedFootprint, t, net);
            }
        }

        /*
        private static List<ITransition> NextStepParallel(ref Tuple<List<string>, Relation[,]> indexedFootprint, List<ITransition> beginningTransitions, IPetriNet net)
        {

        }

        private static ITransition GoParallel(ref Tuple<List<string>, Relation[,]> indexedFootprint, ITransition beginningTransition, IPetriNet net)
        {
            if (beginningTransition.OutputPlaces.Count < 2)
            {
                throw new ArgumentException("You can't go parallel with one output place, dummy");
            }

            foreach(IPlace outputBeginPlace in beginningTransition.OutputPlaces)
            {
                
            }
        }*/

        public static Tuple<List<string>, Relation[,]> MakeIndexedFootprint(IPetriNet net)
        {
            var indexedFootprint = MakeEmptyFootprint(net.Transitions);
            List<ITransition> startTransitions = net.GetStartTransitions();



            foreach (ITransition t in net.Transitions)
            {

            }

            return indexedFootprint;
        }
    }
}
