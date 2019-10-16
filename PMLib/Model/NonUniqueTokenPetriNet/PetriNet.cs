using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class PetriNet
    {
        public List<Transition> Transitions { get; }
        public List<Place> Places { get; }
    }
}
