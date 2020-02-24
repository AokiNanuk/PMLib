using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.DataAnalysis
{
    public class RelationMatrix
    {
        public List<string> Activities { get; }

        public Dictionary<string, int> ActivityIndices { get; }

        public HashSet<string> StartActivities { get; }

        public HashSet<string> EndActivities { get; }

        public Relation[,] Footprint { get; }

        private void FillActivities(List<WorkflowTrace> workflowTraces)
        {
            foreach (WorkflowTrace wft in workflowTraces)
            {
                StartActivities.Add(wft.Activities[0]);
                EndActivities.Add(wft.Activities[wft.Activities.Count - 1]);

                foreach (string a in wft.Activities)
                {
                    if (!Activities.Contains(a))
                    {
                        ActivityIndices.Add(a, Activities.Count);
                        Activities.Add(a);
                    }
                }
            }
        }

        private void FindSuccession(List<WorkflowTrace> workflowTraces)
        {
            foreach (WorkflowTrace wft in workflowTraces)
            {
                for (int i = 0; i < wft.Activities.Count - 1; i++)
                {
                    int fromIndex = ActivityIndices[wft.Activities[i]];
                    int toIndex = ActivityIndices[wft.Activities[i + 1]];
                    Footprint[fromIndex, toIndex] = Relation.Succession;
                }
            }
        }

        private void UpdateRelations()
        {
            for (int i = 0; i < Activities.Count; i++)
            {
                for (int j = 0; j < Activities.Count; j++)
                {
                    if (Footprint[i, j] == Relation.Succession && Footprint[j, i] == Relation.Succession)
                    {
                        Footprint[i, j] = Relation.Parallelism;
                        Footprint[j, i] = Relation.Parallelism;
                    }
                    if (Footprint[i, j] == Relation.Succession && Footprint[j, i] == Relation.Independency)
                    {
                        Footprint[j, i] = Relation.Predecession;
                    }
                }
            }
        }

        public RelationMatrix(WorkflowLog log)
        {
            StartActivities = new HashSet<string>();
            EndActivities = new HashSet<string>();
            Activities = new List<string>();
            ActivityIndices = new Dictionary<string, int>();
            FillActivities(log.WorkflowTraces);

            Footprint = new Relation[Activities.Count, Activities.Count];

            FindSuccession(log.WorkflowTraces);
            UpdateRelations();
        }
    }
}
