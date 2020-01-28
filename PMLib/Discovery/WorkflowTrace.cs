using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Discovery
{
    public class WorkflowTrace
    {
        public string CaseId { get; }
        public List<string> Activities { get; }

        public WorkflowTrace(string caseId)
        {
            CaseId = caseId;
            Activities = new List<string>();
        }

        public void AddActivity(string activity)
        {
            if (activity == null)
            {
                throw new ArgumentNullException("Activity in AddActivity cannot be null");
            }

            Activities.Add(activity);
        }

        public void AddActivities(List<string> activities)
        {
            Activities.AddRange(activities);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is WorkflowTrace trace))
            {
                return false;
            }

            for (int i = 0; i < Activities.Count; i++)
            {
                if (Activities[i] != trace.Activities[i])
                {
                    return false;
                }
            }

            return CaseId == trace.CaseId;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(CaseId, Activities);
        }
    }
}
