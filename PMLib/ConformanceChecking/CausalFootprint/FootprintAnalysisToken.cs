using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    /// <summary>
    /// A class representing a token footprint.
    /// </summary>
    class FootprintAnalysisToken
    {
        public List<string> CurrentLevels { get; } = new List<string>();

        public List<string> MergedLevels { get; } = new List<string>();

        public FootprintAnalysisToken()
        {
            CurrentLevels = new List<string>();
            MergedLevels = new List<string>();
        }

        public FootprintAnalysisToken(FootprintAnalysisToken token)
        {
            CurrentLevels = new List<string>(token.CurrentLevels);
            MergedLevels = new List<string>(token.MergedLevels);
        }
    }
}
