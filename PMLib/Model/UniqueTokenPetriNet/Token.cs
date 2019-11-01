using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.UniqueTokenPetriNet
{
    class Token
    {
        public string CaseId { get; }

        public Token(string caseId)
        {
            CaseId = caseId;
        }
    }
}
