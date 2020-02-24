using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    static class FootprintComparer
    {
        public static double Compare(RelationMatrix logMatrix, RelationMatrix modelMatrix)
        {
            int bound = logMatrix.Activities.Count;
            int match = 0;
            int size = bound * bound;
            for (int i = 0; i < bound; i++)
            {
                for (int j = 0; j < bound; j++)
                {
                    if (logMatrix.Footprint[i,j] == modelMatrix.Footprint[i,j])
                    {
                        match++;
                    }
                }
            }
            return match / size;
        }
    }
}
