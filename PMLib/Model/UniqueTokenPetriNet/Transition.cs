using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.UniqueTokenPetriNet
{
    class Transition
    {
        public List<Place> InputPlaces { get; }

        public List<Place> OutputPlaces { get; }

        public string Activity { get; }

        public Transition(List<Place> inputPlaces, List<Place> outputPlaces, string activity)
        {
            InputPlaces = inputPlaces;
            OutputPlaces = outputPlaces;
            Activity = activity;
        }

        public bool Fire (string caseId)
        {
            if (!InputPlaces.TrueForAll(a => a.Tokens.CaseId == caseId))
            {
                return false;
            }

            foreach (Place ip in InputPlaces)
            {
                if (!ip.Tokens.MockConsumeToken())
                {
                    foreach (Place p in InputPlaces)
                    {
                        p.Tokens.Reset();
                    }
                }
            }

            foreach (Place ip in InputPlaces)
            {
                ip.Tokens.MakeMockReal();
            }
            foreach (Place op in OutputPlaces)
            {
                op.Tokens.IncrementTokens();
            }
        }
    }
}
