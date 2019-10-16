using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class Place
    {
        public Tokens Tokens { get; }

        public bool IsEndPlace { get; } = false;

        public bool IsStartPLace { get; } = false;
    }
}
