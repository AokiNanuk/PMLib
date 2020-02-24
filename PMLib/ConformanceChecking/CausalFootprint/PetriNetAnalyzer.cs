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
