using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    class RelationMatrix
    {
        public List<string> Activities { get; }

        public Dictionary<string, int> RelationMatrixBounds { get; }

        public HashSet<string> StartActivities { get; }

        public HashSet<string> EndActivities { get; }

        public Relation[,] Relations { get; }

        public RelationMatrix(WorkflowLog log)
        {
            foreach (WorkflowTrace wft in log.WorkflowTraces)
            {
                StartActivities.Add(wft.Activities[0]);
                EndActivities.Add(wft.Activities[wft.Activities.Count - 1]);

                foreach (string a in wft.Activities)
                {
                    if (!Activities.Contains(a))
                    {
                        RelationMatrixBounds.Add(a, Activities.Count);
                        Activities.Add(a);
                    }
                }
            }

            Relations = new Relation[Activities.Count, Activities.Count];

            foreach (WorkflowTrace wft in log.WorkflowTraces)
            {
                for (int i = 0; i < wft.Activities.Count - 1; i++)
                {
                    int fromIndex = RelationMatrixBounds[wft.Activities[i]];
                    int toIndex = RelationMatrixBounds[wft.Activities[i + 1]];
                    Relations[fromIndex, toIndex] = Relation.Succession;
                }
            }

            for (int i = 0; i < Activities.Count; i++)
            {
                for (int j = 0; j < Activities.Count; j++)
                {
                    if (Relations[i, j] == Relation.Succession && Relations[j, i] == Relation.Succession)
                    {
                        Relations[i, j] = Relation.Parallelism;
                        Relations[j, i] = Relation.Parallelism;
                    }
                    if (Relations[i, j] == Relation.Succession && Relations[j, i] == Relation.Independency)
                    {
                        Relations[j, i] = Relation.Predecession;
                    }
                }
            }
        }

    }
}
