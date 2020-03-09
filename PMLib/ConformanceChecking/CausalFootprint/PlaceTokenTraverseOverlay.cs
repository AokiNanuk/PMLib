using PMLib.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    class PlaceTokenTraverseOverlay
    {
        public IPlace Place { get; }

        public string TokenFootprint { get; protected set; }

        public bool IsMarked { get; protected set; }

        public void SetFootprint(string footprint)
        {
            TokenFootprint = footprint;
            IsMarked = true;
        }

        public PlaceTokenTraverseOverlay(IPlace place)
        {
            Place = place;
            TokenFootprint = "";
            IsMarked = false;
        }
    }
}
