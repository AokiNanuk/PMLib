using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.Model.BasicPetriNet
{
    /// <summary>
    /// Basic place implementation.
    /// </summary>
    public class Place : IPlace
    {
        public string Id { get; } = "";
        public uint Tokens { get; protected set; } = 0;
        public uint MockTokens { get; protected set; } = 0;

        public Place(string id)
        {
            Id = id;
        }
    }
}
