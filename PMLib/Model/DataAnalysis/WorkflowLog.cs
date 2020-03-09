using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.DataAnalysis
{
    public class WorkflowLog
    {
        public List<WorkflowTrace> WorkflowTraces { get; }

        // timestampy

        private List<WorkflowTrace> MakeEmptyWfts(Deedle.Series<int, int> ids)
        {
            List<WorkflowTrace> traces = new List<WorkflowTrace>();
            HashSet<int> uniqueIds = new HashSet<int>(ids.Values);
            Console.WriteLine("Num of unique ids: " + uniqueIds.Count);
            foreach (var id in uniqueIds)
            {
                traces.Add(new WorkflowTrace("" + id));
            }
            Console.WriteLine("Number of traces: " + traces.Count);
            return traces;
        }

        public WorkflowLog(Model.ImportedEventLog importedData)
        {
            WorkflowTraces = new List<WorkflowTrace>();

            var emptyTraces = MakeEmptyWfts(importedData.Contents.GetColumn<int>(importedData.CaseId));
            

            for (int i = 0; i < importedData.Contents.RowCount; i++)
            {
                var row = importedData.Contents.TryGetRow<string>(i);
                foreach (WorkflowTrace wft in emptyTraces)
                {
                    if (wft.CaseId == row.Value.Get(importedData.CaseId))
                    {
                        wft.AddActivity(row.Value.Get(importedData.Activity));
                    }
                }
            }
            WorkflowTraces = emptyTraces;
        }

        public WorkflowLog(List<WorkflowTrace> workflowTraces)
        {
            WorkflowTraces = workflowTraces;
        }
    }
}
