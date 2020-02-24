using PMLib.Discovery;
using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.TokenBasedReplay
{
    public static class Computations
    {
        public static double ComputeFitness(ImportedEventLog log, IPetriNet net)
        {
            PetriNetTokenDiagnosticsOverlay netDiagnostics = new PetriNetTokenDiagnosticsOverlay(net);
            WorkflowLog eventLog = new WorkflowLog(log);

            foreach (WorkflowTrace t in eventLog.WorkflowTraces)
            {
                netDiagnostics.ReplayTrace(t);
            }

            double sumProduced = 0;
            double sumConsumed = 0;
            double sumMissing = 0;
            double sumRemaining = 0;

            foreach (PlaceTokenDiagnosticsOverlay diagnostics in netDiagnostics.Diagnostics.Values)
            {
                sumProduced += diagnostics.Produced;
                sumConsumed += diagnostics.Consumed;
                sumMissing += diagnostics.Missing;
                sumRemaining += diagnostics.Remaining;
            }

            return 0.5 * (1 - sumMissing / sumConsumed) + 0.5 * (1 - sumRemaining / sumProduced);
        }
    }
}
