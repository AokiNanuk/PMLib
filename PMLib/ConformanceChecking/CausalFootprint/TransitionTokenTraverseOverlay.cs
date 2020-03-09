using PMLib.Model;
using System;
using System.Collections.Generic;
using System.Text;
using PMLib.ConformanceChecking.CausalFootprint;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    class TransitionTokenTraverseOverlay
    {

        public string TokenFootprint { get; protected set; }

        public bool IsMarked { get; protected set; }

        public List<PlaceTokenTraverseOverlay> InputPlaces { get; }

        public List<PlaceTokenTraverseOverlay> OutputPlaces { get; }

        public string Id { get; }

        public string Activity { get; }

        public void SetFootprint(string footprint)
        {
            TokenFootprint = footprint;
            IsMarked = true;
        }

        private void SetUpInputPlaces(List<IPlace> places)
        {
            foreach (IPlace p in places)
            {
                InputPlaces.Add(new PlaceTokenTraverseOverlay(p));
            }
        }

        private void SetUpOutputPlaces(List<IPlace> places)
        {
            foreach (IPlace p in places)
            {
                OutputPlaces.Add(new PlaceTokenTraverseOverlay(p));
            }
        }

        public TransitionTokenTraverseOverlay(ITransition transition)
        {
            InputPlaces = new List<PlaceTokenTraverseOverlay>();
            OutputPlaces = new List<PlaceTokenTraverseOverlay>();

            SetUpInputPlaces(transition.InputPlaces);
            SetUpOutputPlaces(transition.OutputPlaces);
            Id = transition.Id;
            Activity = transition.Activity;
            TokenFootprint = "";
            IsMarked = false;
        }
        
        public bool CanFire()
        {
            foreach(PlaceTokenTraverseOverlay p in InputPlaces)
            {
                if (!p.IsMarked)
                {
                    return false;
                }
            }
            return true;
        }


        /* DEPRECATED
        public void Fire()
        {
            string newFootprint = InputPlaces[0].TokenFootprint;

            if (OutputPlaces.Count > 1)
            {
                if (InputPlaces.Count > 1)
                {
                    newFootprint = TokenFootprintUtils.CutOneLevel(InputPlaces[0].TokenFootprint);
                }
                SetFootprint(newFootprint);
                FireParallel(newFootprint);
                return;
            }

            if (InputPlaces.Count > 1 && OutputPlaces.Count > 0)
            {
                newFootprint = TokenFootprintUtils.CutOneLevel(InputPlaces[0].TokenFootprint);
                foreach (PlaceTokenTraverseOverlay p in OutputPlaces)
                {
                    p.SetFootprint(newFootprint);
                }
                SetFootprint(newFootprint);
                return;
            }

            foreach (PlaceTokenTraverseOverlay p in OutputPlaces)
            {
                p.SetFootprint(newFootprint);
            }
            SetFootprint(newFootprint);
        }*/
    }
}
