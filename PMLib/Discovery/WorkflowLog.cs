using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    class WorkflowLog
    {
        // Bude hash set dělat to co chci?
        public HashSet<WorkflowTrace> WorkflowTraces { get; }

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
            

            for (int i = 0; i < frame.RowCount; i++)
            {
                var row = frame.TryGetRow<string>(i);
                foreach (WorkflowTrace wft in WorkflowTraces)
                {
                    if (wft.CaseId == row.Value.Get(importedData.CaseId))
                    {
                        wft.AddActivity(row.Value.Get(importedData.Activity));
                    }
                }
                WorkflowTrace newWft = new WorkflowTrace(row.Value.Get(importedData.CaseId));
                newWft.AddActivity(row.Value.Get(importedData.CaseId));
                WorkflowTraces.Add(newWft);
            }


            /*
            var caseIdCol = importedData.Contents.TryGetColumn<string>(importedData.CaseId, Deedle.Lookup.Exact);
            var activityCol = importedData.Contents.TryGetColumn<string>(importedData.Activity, Deedle.Lookup.Exact);

            if (importedData.Timestamp != null)
            {
                var timestampCol = importedData.Contents.TryGetColumn<DateTime>(importedData.Timestamp, Deedle.Lookup.Exact);
            }

            var df = Deedle.Frame.FromColumns<string, int>(caseIdCol);
            */
        }
    }
}
