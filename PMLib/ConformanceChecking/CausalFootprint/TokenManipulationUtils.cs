using System;
using System.Collections.Generic;
using System.Text;

namespace PMLib.ConformanceChecking.CausalFootprint
{
    /// <summary>
    /// A utility class containing methods for FootprintAnalysisToken manipulation, such as merging and splitting tokens.
    /// </summary>
    static class TokenManipulationUtils
    {
        /// <summary>
        /// Does a quick check whether given list of tokens is not null or empty.
        /// </summary>
        /// <param name="tokens">List of tokens to be checked.</param>
        private static void CheckInputTokens(List<FootprintAnalysisToken> tokens)
        {
            if (tokens == null)
            {
                throw new ArgumentNullException("List of tokens cannot be null.");
            }
            if (tokens.Count < 1)
            {
                throw new ArgumentException("List of tokens cannot be empty.");
            }
        }

        /// <summary>
        /// Merges two tokens into one on a given level, updates activeGlobalIdMonitor accordingly.
        /// </summary>
        /// <param name="outputToken">Token which will be passed as a result.</param>
        /// <param name="mergedToken">Token which will be merged and dropped.</param>
        /// <param name="mergedLevel">A level on which the merge should occur.</param>
        /// <param name="activeGlobalIdMonitor">A monitor of how many tokens of each level are still active.</param>
        private static void MergeTwoTokens(ref FootprintAnalysisToken outputToken, 
            FootprintAnalysisToken mergedToken,
            string mergedLevel,
            Dictionary<string, int> activeGlobalIdMonitor)
        {
            int i = 0;
            string mergedGlobalId = StringUtils.GetGlobalId(mergedLevel);
            outputToken.MergedLevels.Add(mergedLevel);
            activeGlobalIdMonitor[mergedGlobalId]--;

            foreach (string mergedTokenCurrentLevel in mergedToken.CurrentLevels)
            {
                if (!outputToken.CurrentLevels.Contains(mergedTokenCurrentLevel) && mergedTokenCurrentLevel != mergedLevel)
                {
                    int.TryParse(StringUtils.GetGlobalId(outputToken.CurrentLevels[i]), out int outputCurrentGlobalId);
                    int.TryParse(StringUtils.GetGlobalId(mergedTokenCurrentLevel), out int mergedCurrentGlobalId);
                    while (outputCurrentGlobalId < mergedCurrentGlobalId)
                    {
                        i++;
                        int.TryParse(StringUtils.GetGlobalId(outputToken.CurrentLevels[i]), out outputCurrentGlobalId);
                    }
                    outputToken.CurrentLevels.Insert(i, mergedTokenCurrentLevel);
                }
            }
            foreach (string mergedTokenMergedLevel in mergedToken.MergedLevels)
            {
                if (!outputToken.MergedLevels.Contains(mergedTokenMergedLevel))
                {
                    outputToken.MergedLevels.Add(mergedTokenMergedLevel);
                }
            }
            if (activeGlobalIdMonitor[mergedGlobalId] == 1)
            {
                string level = outputToken.CurrentLevels.Find(a => StringUtils.GetGlobalId(a) == mergedGlobalId);
                if (level != null)
                {
                    outputToken.MergedLevels.Add(level);
                    outputToken.CurrentLevels.Remove(level);
                    activeGlobalIdMonitor.Remove(mergedGlobalId);
                }
            }
        }

        /// <summary>
        /// Merges multiple tokens into one, updates activeGlobalIdMonitor accordingly.
        /// </summary>
        /// <param name="tokens">A list of tokens to be merged into one.</param>
        /// <param name="activeGlobalIdMonitor">A monitor of how many tokens of each level are still active.</param>
        /// <returns>A token to be passed further.</returns>
        public static FootprintAnalysisToken MergeTokens(List<FootprintAnalysisToken> tokens, Dictionary<string, int> activeGlobalIdMonitor)
        {
            CheckInputTokens(tokens);
            FootprintAnalysisToken outputToken = new FootprintAnalysisToken(tokens[tokens.Count - 1]);
            for (int i = tokens.Count - 2; i >= 0; i--)
            {
                bool hasMerged = false;
                FootprintAnalysisToken mergedToken = tokens[i];
                for (int j = mergedToken.CurrentLevels.Count - 1; j >= 0 && !hasMerged; j--)
                {
                    string mergedLevel = mergedToken.CurrentLevels[j];
                    for (int k = outputToken.CurrentLevels.Count - 1; k >= 0 && !hasMerged; k--)
                    {
                        string outputLevel = outputToken.CurrentLevels[k];
                        if (StringUtils.GetGlobalId(mergedLevel) == StringUtils.GetGlobalId(outputLevel))
                        {
                            MergeTwoTokens(ref outputToken, mergedToken, mergedLevel, activeGlobalIdMonitor);
                            hasMerged = true;
                        }
                    }
                }
            }
            return outputToken;
        }

        /// <summary>
        /// Splits one token into multiple, adds new level to all of the resulting tokens, updates activeGlobalIdMonitor accordingly.
        /// </summary>
        /// <param name="token">A token to be split.</param>
        /// <param name="count">Number of how many new tokens should be created.</param>
        /// <param name="newGlobalId">New global ID of new created level.</param>
        /// <param name="activeGlobalIdMonitor">A monitor of how many tokens of each level are still active.</param>
        /// <returns>List of new tokens.</returns>
        public static List<FootprintAnalysisToken> SplitToken(FootprintAnalysisToken token, uint count, string newGlobalId, Dictionary<string, int> activeGlobalIdMonitor)
        {
            List<FootprintAnalysisToken> outputTokens = new List<FootprintAnalysisToken>();
            string localId = "A";
            for (uint i = 0; i < count; i++)
            {
                FootprintAnalysisToken newToken = new FootprintAnalysisToken(token);
                newToken.CurrentLevels.Add(newGlobalId + localId);
                outputTokens.Add(newToken);
                localId = StringUtils.IncrementId(localId);
            }
            activeGlobalIdMonitor.Add(newGlobalId, (int)count);
            return outputTokens;
        }

        /// <summary>
        /// Gets current (active) global IDs from given token.
        /// </summary>
        /// <param name="token">A token from which current global IDs should be extracted.</param>
        /// <returns>A list of global IDs of given token.</returns>
        public static List<string> GetCurrentGlobalIds(FootprintAnalysisToken token)
        {
            List<string> splitIds = new List<string>();
            foreach (string level in token.CurrentLevels)
            {
                splitIds.Add(StringUtils.GetGlobalId(level));
            }
            return splitIds;
        }
    }
}
