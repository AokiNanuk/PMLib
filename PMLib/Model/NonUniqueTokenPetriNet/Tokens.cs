using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class Tokens
    {
        public uint Count { get; protected set; } = 0;
        public uint MockTokens { get; protected set; } = 0;

        public void IncrementTokens()
        {
            Count++;
        }

        public bool MockConsumeToken()
        {
            if (Count == 0)
            {
                return false;
            }
            Count--;
            MockTokens++;
            return true;
        }

        public void MakeMockReal()
        {
            MockTokens = 0;
        }

        public void Reset()
        {
            Count += MockTokens;
            MockTokens = 0;
        }
    }
}
