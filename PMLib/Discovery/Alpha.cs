using PMLib.Model.NonUniqueTokenPetriNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    static class Alpha
    {
        public static PetriNet MakePetriNet (RelationMatrix matrix)
        {

            List<Transition> transitions = new List<Transition>();
            List<Place> places = new List<Place>();

            foreach (string a in matrix.Activities)
            {
                transitions.Add(new Transition(a));
            }

            int idCounter = 0;
            Place startPlace = new Place(idCounter);
            places.Add(startPlace);
            idCounter++;
            foreach (string startTransition in matrix.StartActivities)
            {
                transitions.Find(a => a.Activity == startTransition).InputPlaces.Add(startPlace);
            }

            var setsAB = new HashSet<Tuple<HashSet<string>, HashSet<string>>>();
            //var lastAs = new List<HashSet<string>>();
            //var lastBs = new List<HashSet<string>>();

            int matSize = matrix.Activities.Count;
            /*for (int i = 0; i < matSize; i++)
            {
                if (matrix.Relations[i,i] == Relation.Independency)
                {
                    HashSet<string> setA = new HashSet<string>();
                    HashSet<string> setB = new HashSet<string>();
                    for (int j = 0; j < matSize; j++)
                    {
                        if (matrix.Relations[i,j] == Relation.Succession && matrix.Relations[j,j] == Relation.Independency)
                        {
                            setA.Add(matrix.Activities[i]);
                            setB.Add(matrix.Activities[j]);
                            setsAB.Add(new Tuple<HashSet<string>, HashSet<string>>(setA, setB));
                        }
                    }
                }
            }

            foreach (var setAB in setsAB)
            {
                setsAB.FindAll(a => a.Item1 == setAB.Item1);
            }*/

            
            var independentSets = new HashSet<HashSet<string>>();
            for (int i = 0; i < matSize; i++)
            {
                if (matrix.Relations[i,i] == Relation.Independency)
                {
                    var nset = new HashSet<string>();
                    for (int j = 0; j < matSize; j++)
                    {
                        if (matrix.Relations[i,j] == Relation.Independency)
                        {
                            nset.Add(matrix.Activities[j]);
                            independentSets.Add(new HashSet<string>(nset));
                        }
                    }
                }
            }

            foreach (var setA in independentSets)
            {
                foreach (var setB in independentSets)
                {
                    bool validSets = true;
                    foreach (string actA in setA)
                    {
                        foreach (string actB in setB)
                        {
                            if (!(matrix.Relations[matrix.RelationMatrixBounds[actA], matrix.RelationMatrixBounds[actB]] == Relation.Succession))
                            {
                                validSets = false;
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

                    if (setA.Count == 1 && setB.Count == 1)
                    {
                        isBiggerSet = true;
                    }

                    if (validSets && isBiggerSet)
                    {
                        setsAB.Add(new Tuple<HashSet<string>, HashSet<string>>(setA, setB));
                    }
                }
            }

            foreach (var setAB in setsAB)
            {
                Place placeAB = new Place(idCounter);
                places.Add(placeAB);
                idCounter++;
                foreach(string actA in setAB.Item1)
                {
                    transitions.Find(a => a.Activity == actA).OutputPlaces.Add(placeAB);
                }

                foreach(string actB in setAB.Item2)
                {
                    transitions.Find(a => a.Activity == actB).InputPlaces.Add(placeAB);
                }
            }

            Place endPlace = new Place(idCounter);
            places.Add(endPlace);
            foreach (string endTransition in matrix.EndActivities)
            {
                transitions.Find(a => a.Activity == endTransition).OutputPlaces.Add(endPlace);
            }
            return new PetriNet(transitions, places, startPlace, endPlace);
        }
    }
}
