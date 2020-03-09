using PMLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    class PetriNetTokenTraverseOverlay
    {
        IPetriNet PetriNet { get; }

        List<TransitionTokenTraverseOverlay> TransitionsWithFootprints { get; }

        private void SetUpTransitionsOverlay(List<ITransition> transitions)
        {
            foreach (ITransition t in transitions)
            {
                TransitionsWithFootprints.Add(new TransitionTokenTraverseOverlay(t));
            }
        }

        private void SetUpStartPlace(string footprint)
        {
            TransitionTokenTraverseOverlay start = TransitionsWithFootprints.Find(a => a.InputPlaces.Count == 1 
                && a.InputPlaces[0].Place.Id == PetriNet.StartPlace.Id);

            start.InputPlaces[0].SetFootprint(footprint);
        }

        private void FireTransition(TransitionTokenTraverseOverlay transition, string footprint)
        {

        }

        private void TraverseAndMakeFootprints()
        {
            string footprint = "0";
            SetUpStartPlace(footprint);

            
        }

        public PetriNetTokenTraverseOverlay(IPetriNet petriNet)
        {
            PetriNet = petriNet;
            TransitionsWithFootprints = new List<TransitionTokenTraverseOverlay>();
            SetUpTransitionsOverlay(petriNet.Transitions);
            TraverseAndMakeFootprints();
        }
    }
}
