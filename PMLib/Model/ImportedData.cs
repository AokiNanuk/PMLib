using Deedle;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model
{
    public class ImportedData
    {
        public Frame<int, string> Contents { get; protected set; }
        public string CaseId { get; protected set; }
        public string Activity { get; protected set; }
        public string Timestamp { get; protected set; }


        public ImportedData(Frame<int, string> data)
        {
            Contents = data;
        }

        private bool KeyInColumns(string key)
        {
            foreach (string k in Contents.ColumnKeys)
            {
                if (k == key)
                {
                    return true;
                }
            }
            return false;
        }

        public bool SetActivity(string activity)
        {
            if (!KeyInColumns(activity))
            {
                return false;
            }

            Activity = activity;
            return true;
        }

        public bool SetCaseId(string caseId)
        {
            if (!KeyInColumns(caseId))
            {
                return false;
            }

            CaseId = caseId;
            return true;
        }

        public bool SetTimestamp(string timestamp)
        {
            if (!KeyInColumns(timestamp))
            {
                return false;
            }

            Timestamp = timestamp;
            return true;
        }

    }
}
