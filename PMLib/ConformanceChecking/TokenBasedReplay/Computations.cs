﻿using PMLib.Discovery;
using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.TokenBasedReplay
{
    /// <summary>
    /// This class contains methods for computing token-based replay fitness of event log to a Petri Net.
    /// </summary>
    public static class Computations
    {
        /// <summary>
        /// Computes token-based replay fitness of given event log to a given Petri Net.
        /// </summary>
        /// <param name="log">Event log to be replayed.</param>
        /// <param name="net">Petri net used for replaying given traces.</param>
        /// <returns>Fitness metric of given event log to given Petri Net.</returns>
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

            if (sumConsumed == 0 && sumProduced == sumRemaining && sumProduced == sumMissing) // this should only be true if compared log is 100% different from given Petri Net.
            { 
                return 0.0;
            }

            return 0.5 * (1 - sumMissing / sumConsumed) + 0.5 * (1 - sumRemaining / sumProduced);
        }
    }
}
