using PMLib.Model;
using PMLib.Model.NonUniqueTokenPetriNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    static class Alpha
    {
        private static List<ITransition> GetTransitions(List<string> activities, int id = 0)
        {
            List<ITransition> transitions = new List<ITransition>();

            foreach (string activityName in activities)
            {
                transitions.Add(new Transition("t" + id, activityName));
                id++;
            }

            return transitions;
        }

        private static int SetupStartPlace(List<IPlace> places, HashSet<string> startActivities, List<ITransition> transitions, int id = 0)
        {
            Place startPlace = new Place("p" + id);
            id++;
            places.Add(startPlace);
            foreach (string startTransition in startActivities)
            {
                transitions.Find(a => a.Activity == startTransition).InputPlaces.Add(startPlace);
            }
            return id;
        }

        private static int SetupPlaces(HashSet<Tuple<HashSet<string>, HashSet<string>>> setsAB, List<IPlace> places, List<ITransition> transitions, int id)
        {
            foreach (var setAB in setsAB)
            {
                Place placeAB = new Place("p" + id);
                id++;
                places.Add(placeAB);
                foreach (string actA in setAB.Item1)
                {
                    transitions.Find(a => a.Activity == actA).OutputPlaces.Add(placeAB);
                }

                foreach (string actB in setAB.Item2)
                {
                    transitions.Find(a => a.Activity == actB).InputPlaces.Add(placeAB);
                }
            }
            return id;
        }

        private static void SetupEndPlace(List<IPlace> places, HashSet<string> endActivities, List<ITransition> transitions, int id)
        {
            Place endPlace = new Place("p" + id);
            places.Add(endPlace);
            foreach (string endTransition in endActivities)
            {
                transitions.Find(a => a.Activity == endTransition).OutputPlaces.Add(endPlace);
            }
        }

        public static PetriNet MakePetriNet (RelationMatrix matrix)
        {
            List<ITransition> transitions = GetTransitions(matrix.Activities);
            List<IPlace> places = new List<IPlace>();
            HashSet<HashSet<string>> independentSets = IndependentSetUtils.FindIndependentSets(matrix.Relations, matrix.Activities);
            HashSet<Tuple<HashSet<string>, HashSet<string>>> setsAB = IndependentSetUtils.FindMaximalIndependentSetsAB(independentSets, matrix.Relations, matrix.ActivityIndices);

            int id = SetupStartPlace(places, matrix.StartActivities, transitions);
            id = SetupPlaces(setsAB, places, transitions, id);
            SetupEndPlace(places, matrix.EndActivities, transitions, id);

            return new PetriNet(transitions, places, places[0], places[places.Count - 1]);
        }
    }
}
