using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class Transition
    {
        public List<Place> InputPlaces { get; }
        public List<Place> OutputPlaces { get; }

        public string Activity { get; }

        public Transition(string activity)
        {
            InputPlaces = new List<Place>();
            OutputPlaces = new List<Place>();
            Activity = activity;
        }

        public bool Fire()
        {
            foreach (Place ip in InputPlaces)
            {
                if (!ip.MockConsumeToken())
                {
                    foreach (Place p in InputPlaces)
                    {
                        p.Reset();
                    }
                    return false;
                }
            }
            foreach (Place ip in InputPlaces)
            {
                ip.MakeMockReal();
            }
            foreach (Place op in OutputPlaces)
            {
                op.IncrementTokens();
            }
            return true;
        }
    }
}
