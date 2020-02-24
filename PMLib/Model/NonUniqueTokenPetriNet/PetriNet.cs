using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    public class PetriNet : IPetriNet
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

        public ITransition GetTransition(string activity)
        {
            return Transitions.Find(a => a.Activity == activity);
        }

        public List<ITransition> GetStartTransitions()
        {
            List<ITransition> startTransitions = new List<ITransition>();
            foreach(ITransition t in Transitions)
            {
                bool isStartTransition = true;
                foreach(IPlace ip in t.InputPlaces)
                {
                    if (ip.Id != StartPlace.Id)
                    {
                        isStartTransition = false;
                        break;
                    }
                }
                if (isStartTransition)
                {
                    startTransitions.Add(t);
                }
            }
            return startTransitions;
        }
    }
}
