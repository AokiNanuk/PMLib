using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.NonUniqueTokenPetriNet
{
    class Place
    {
        public int Id { get; protected set; }
        public uint Tokens { get; protected set; } = 0;
        public uint MockTokens { get; protected set; } = 0;

        public Place(int id)
        {
            Id = id;
        }

        public void IncrementTokens()
        {
            Tokens++;
        }

        public bool MockConsumeToken()
        {
            if (Tokens == 0)
            {
                return false;
            }
            Tokens--;
            MockTokens++;
            return true;
        }

        public void MakeMockReal()
        {
            MockTokens = 0;
        }

        public void Reset()
        {
            Tokens += MockTokens;
            MockTokens = 0;
        }
    }
}
