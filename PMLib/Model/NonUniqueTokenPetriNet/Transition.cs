using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class Transition : ITransition
    {
        public List<IPlace> InputPlaces { get; }
        public List<IPlace> OutputPlaces { get; }
        public string Id { get; }
        public string Activity { get; }

        public Transition(string id, string activity)
        {
            InputPlaces = new List<IPlace>();
            OutputPlaces = new List<IPlace>();
            Id = id;
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
