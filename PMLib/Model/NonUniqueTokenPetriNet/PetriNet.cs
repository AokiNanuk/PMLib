using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class PetriNet : IPetriNet
    {
        public List<ITransition> Transitions { get; }

        public List<IPlace> Places { get; }

        public IPlace StartPlace { get; }

        public IPlace EndPlace { get; }


        public PetriNet(List<ITransition> transitions, List<IPlace> places, IPlace startPlace, IPlace endPlace)
        {
            Transitions = transitions;
            Places = places;
            StartPlace = startPlace;
            EndPlace = endPlace;
        }
    }
}
