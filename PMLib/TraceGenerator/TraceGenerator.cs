using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.TraceGenerator
{
    public static class TraceGenerator
    {
        private static List<List<ITransition>> SetUpPossibleTraces(IPetriNet net)
        {
            List<List<ITransition>> possibleTraces = new List<List<ITransition>>();
            foreach (ITransition t in net.GetStartTransitions())
            {
                possibleTraces.Add(new List<ITransition>() { t });
            }
            return possibleTraces;
        }

        public static WorkflowLog GenerateTraces(IPetriNet net, uint traceLengthBound = 20)
        {
            List<WorkflowTrace> traces = new List<WorkflowTrace>();
            int id = 0;
            List<List<ITransition>> possibleTraces = SetUpPossibleTraces(net);

            for (uint i = 0; i < traceLengthBound; i++)
            {
                foreach (List<ITransition> possibleTrace in possibleTraces)
                {
                    List<List<ITransition>> newTraces = new List<List<ITransition>>();
                    foreach (IPlace outputPlace in possibleTrace[possibleTrace.Count].OutputPlaces)
                    {

                    }
                }
            }
        }
    }
}
