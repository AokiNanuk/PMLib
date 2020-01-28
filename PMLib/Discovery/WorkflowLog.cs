using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    public class WorkflowLog
    {
        // Bude hash set dělat to co chci?
        public HashSet<WorkflowTrace> WorkflowTraces { get; }

        private HashSet<WorkflowTrace> MakeEmptyWfts(Deedle.Series<int, int> ids)
        {
            HashSet<WorkflowTrace> traces = new HashSet<WorkflowTrace>();
            HashSet<int> uniqueIds = new HashSet<int>(ids.Values);
            foreach (var id in uniqueIds)
            {
                traces.Add(new WorkflowTrace("" + id));
            }
            Console.WriteLine("Number of traces: " + traces.Count);
            return traces;
        }

        public WorkflowLog(Model.ImportedData importedData)
        {
            // Klonuje načtená data a prořeže dataframe pouze na sloupky které jsou potřeba. Asi není nutné, třeba otestovat.

            WorkflowTraces = new HashSet<WorkflowTrace>();
            Deedle.Frame<int, string> frame = importedData.Contents.Clone();

            List<string> cols = new List<string>(importedData.Contents.ColumnKeys);
            cols.Remove(importedData.CaseId);
            cols.Remove(importedData.Activity);
            if (importedData.Timestamp != null)
            {
                cols.Remove(importedData.Timestamp);
            }

            foreach (string ck in cols)
            {
                frame.DropColumn(ck);
            }

            var emptyTraces = MakeEmptyWfts(frame.GetColumn<int>(importedData.CaseId));
            

            for (int i = 0; i < frame.RowCount; i++)
            {
                var row = frame.TryGetRow<string>(i);
                foreach (WorkflowTrace wft in emptyTraces)
                {
                    if (wft.CaseId == row.Value.Get(importedData.CaseId))
                    {
                        wft.AddActivity(row.Value.Get(importedData.Activity));
                    }
                }
                //WorkflowTrace newWft = new WorkflowTrace(row.Value.Get(importedData.CaseId));
                //newWft.AddActivity(row.Value.Get(importedData.CaseId));
                //WorkflowTraces.Add(newWft);
            }
            WorkflowTraces = emptyTraces;
        }
    }
}
