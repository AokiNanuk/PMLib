using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.UniqueTokenPetriNet
{
    class Transition
    {
        public List<Place> InputPlaces { get; }

        public List<Place> OutputPlaces { get; }

        public string Id { get; }
        public string Activity { get; }

        public Transition(List<Place> inputPlaces, List<Place> outputPlaces, string id, string activity)
        {
            InputPlaces = inputPlaces;
            OutputPlaces = outputPlaces;
            Id = id;
            Activity = activity;
        }

        public bool Fire (string caseId)
        {
            foreach (Place ip in InputPlaces)
            {
                if (!ip.MockConsumeToken(caseId))
                {
                    foreach (Place p in InputPlaces)
                    {
                        p.Reset();
                    }
                }
            }

            foreach (Place ip in InputPlaces)
            {
                ip.MakeMockReal();
            }
            foreach (Place op in OutputPlaces)
            {
                op.AddToken(caseId);
            }
            return true;
        }
    }
}
