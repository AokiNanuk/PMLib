using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.UniqueTokenPetriNet
{
    class PetriNet
    {
        public List<Transition> Transitions { get; }

        public List<Place> Places { get; }

        public List<Place> StartPlaces { get; }

        public List<Place> EndPlaces { get; }
    }
}
