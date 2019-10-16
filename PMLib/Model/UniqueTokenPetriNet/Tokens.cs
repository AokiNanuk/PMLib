using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.UniqueTokenPetriNet
{
    class Tokens : NonUniqueTokenPetriNet.Tokens
    {
        public string CaseId { get; }
    }
}
