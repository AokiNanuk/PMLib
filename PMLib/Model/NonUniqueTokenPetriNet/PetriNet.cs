using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class PetriNet
    {
        public List<Transition> Transitions { get; }

        public List<Place> Places { get; }

        public Place StartPlace { get; }

        public Place EndPlace { get; }

        public PetriNet(List<Transition> transitions, List<Place> places, Place startPlace, Place endPlace)
        {
            Transitions = transitions;
            Places = places;
            StartPlace = startPlace;
            EndPlace = endPlace;
        }
    }
}
