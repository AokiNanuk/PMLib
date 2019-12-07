using PMLib.Model.NonUniqueTokenPetriNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    static class Alpha
    {
        private static List<Transition> GetTransitions(List<string> activities)
        {
            List<Transition> transitions = new List<Transition>();

            foreach (string a in activities)
            {
                transitions.Add(new Transition(a));
            }

            return transitions;
        }

        private static int SetupStartPlace(List<Place> places, HashSet<string> startActivities, List<Transition> transitions, int id = 0)
        {
            Place startPlace = new Place(id);
            id++;
            places.Add(startPlace);
            foreach (string startTransition in startActivities)
            {
                transitions.Find(a => a.Activity == startTransition).InputPlaces.Add(startPlace);
            }
            return id;
        }

        private static int SetupPlaces(HashSet<Tuple<HashSet<string>, HashSet<string>>> setsAB, List<Place> places, List<Transition> transitions, int id)
        {
            foreach (var setAB in setsAB)
            {
                Place placeAB = new Place(id);
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

        private static void SetupEndPlace(List<Place> places, HashSet<string> endActivities, List<Transition> transitions, int id)
        {
            Place endPlace = new Place(id);
            places.Add(endPlace);
            foreach (string endTransition in endActivities)
            {
                transitions.Find(a => a.Activity == endTransition).OutputPlaces.Add(endPlace);
            }
        }

        public static PetriNet MakePetriNet (RelationMatrix matrix)
        {
            List<Transition> transitions = GetTransitions(matrix.Activities);
            List<Place> places = new List<Place>();
            HashSet<HashSet<string>> independentSets = IndependentSetUtils.FindIndependentSets(matrix.Relations, matrix.Activities);
            HashSet<Tuple<HashSet<string>, HashSet<string>>> setsAB = IndependentSetUtils.FindMaximalIndependentSetsAB(independentSets, matrix.Relations, matrix.ActivityIndices);

            int id = SetupStartPlace(places, matrix.StartActivities, transitions);
            id = SetupPlaces(setsAB, places, transitions, id);
            SetupEndPlace(places, matrix.EndActivities, transitions, id);

            return new PetriNet(transitions, places, places[0], places[places.Count - 1]);
        }
    }
}
