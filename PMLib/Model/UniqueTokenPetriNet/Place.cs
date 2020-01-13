using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.UniqueTokenPetriNet
{
    class Place
    {
        // Možná náhrada slovníkem <caseId, count>
        public List<Token> Tokens { get; }

        public List<Token> MockTokens { get; }


        public void AddToken(string caseId)
        {
            Tokens.Add(new Token(caseId));
        }

        public void MakeMockReal()
        {
            MockTokens.Clear();
        }

        public bool MockConsumeToken(string caseId)
        {
            Token t = Tokens.Find(a => a.CaseId == caseId);
            if (t == null)
            {
                return false;
            }
            MockTokens.Add(t);
            Tokens.Remove(t);
            return true;
        }

        public void Reset()
        {
            Tokens.AddRange(MockTokens);
            MockTokens.Clear();
        }
    }
}
