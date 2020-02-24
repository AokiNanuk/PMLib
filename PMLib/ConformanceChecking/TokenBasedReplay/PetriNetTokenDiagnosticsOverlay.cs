using PMLib.Discovery;
using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.TokenBasedReplay
{
    class PetriNetTokenDiagnosticsOverlay
    {
        IPetriNet PetriNet { get; }

        public Dictionary<string, PlaceTokenDiagnosticsOverlay> Diagnostics { get; }

        public PetriNetTokenDiagnosticsOverlay(IPetriNet net)
        {
            PetriNet = net;
            Diagnostics = new Dictionary<string, PlaceTokenDiagnosticsOverlay>();
            foreach (IPlace p in net.Places)
            {
                Diagnostics.Add(p.Id, new PlaceTokenDiagnosticsOverlay());
            }
        }

        private void SetupStartPlace()
        {
            PlaceTokenDiagnosticsOverlay startPlace = Diagnostics[PetriNet.StartPlace.Id];
            startPlace.ProduceToken();
        }

        private void Fire(ITransition transition)
        {
            foreach (IPlace ip in transition.InputPlaces)
            {
                PlaceTokenDiagnosticsOverlay inputPlace = Diagnostics[ip.Id];
                inputPlace.ConsumeToken();
            }
            foreach (IPlace op in transition.OutputPlaces)
            {
                PlaceTokenDiagnosticsOverlay outputPlace = Diagnostics[op.Id];
                outputPlace.ProduceToken();
            }
            /*
            foreach (IPlace ip in transition.InputPlaces)
            {
                PlaceTokenDiagnosticsOverlay inputPlace = Diagnostics[ip.Id];
                inputPlace.SetRemaining();
            }*/
        }

        private void SetRemnants()
        {
            foreach (PlaceTokenDiagnosticsOverlay place in Diagnostics.Values)
            {
                place.SetRemaining();
            }
        }

        private void CleanUpEndPlace()
        {
            PlaceTokenDiagnosticsOverlay endPlace = Diagnostics[PetriNet.EndPlace.Id];
            endPlace.ConsumeToken();
        }

        public void ReplayTrace(WorkflowTrace trace)
        {
            SetupStartPlace();
            foreach (string activity in trace.Activities)
            {
                Fire(PetriNet.Transitions.Find(a => a.Activity == activity));
            }
            CleanUpEndPlace();
            SetRemnants();
        }
    } 
}
