using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    static class TokenFootprintUtils
    {
        public static bool IsNumber(char c)
        {
            return (c > 47 && c < 59);
        }

        public static bool IsLetter(char c)
        {
            return (c > 64 && c < 91) || (c > 96 && c < 123);
        }

        public static bool IsHyphen(char c)
        {
            return c == 45;
        }

        public static bool IsHashtag(char c)
        {
            return c == 35;
        }

        /* Returns true if given char is '@', which is a special baseline symbol for
         * token footprint which hasn't been paralelised.
         */
        public static bool IsAtSymbol(char c)
        {
            return c == 64;
        }

        public static List<string> GetParallelFootprints(string footprint, uint num)
        {
            List<string> newFootprints = new List<string>();

            // TODO

            return newFootprints;
        }

        public static int GetLevelsCount(string footprint)
        {
            int counter = 0;
            int i = footprint.Length - 1;

            while(!IsAtSymbol(footprint[i]))
            {
                counter++;
                while(IsNumber(footprint[i]) || IsHyphen(footprint[i]))
                {
                    i--;
                }
                while(IsLetter(footprint[i]) || IsHashtag(footprint[i])) {
                    i--;
                }
            }

            return counter;
        }

        public static string FindLowestLevelFootprint(List<string> footprints)
        {
            int lowestLevel = GetLevelsCount(footprints[0]);
            string lowestLevelFootprint = footprints[0];

            foreach (string footprint in footprints)
            {
                int currentLevel = GetLevelsCount(footprint);
                if (currentLevel < lowestLevel)
                {
                    lowestLevel = currentLevel;
                    lowestLevelFootprint = footprint;
                }
            }

            return lowestLevelFootprint;
        }


        /*
        public static string MergeFootprints(List<string> footprints)
        {
            string smallestFootprint = FindLowestLevelFootprint(footprints);
            int mergingLevel = GetLevelsCount(smallestFootprint);


        }
        */

        public static string CutLevel(string footprint)
        {
            int i = footprint.Length - 1;
            while (IsNumber(footprint[i]) || IsHyphen(footprint[i]))
            {
                i--;
            }
            while (IsLetter(footprint[i]))
            {
                i--;
            }
            return footprint.Substring(0, i + 1);
        }
    }
}
