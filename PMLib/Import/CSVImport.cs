using System;
using System.Collections.Generic;
using System.Text;
using Deedle;
using PMLib.Model;

namespace PMLib.Import
{
    /// <summary>
    ///  This class is a simple handler to reading CSV files into dataframes, both provided by Deedle library.
    ///  The rows are going to be implicitly numbered and the collumns are going to have strings as their identifiers, as provided in the CSV file.
    /// </summary>
    static class CSV
    {
        
        public static Data MakeDataFrame(string path)
        {
            var data = Frame.ReadCsv(path);
            return new Data(data);
        }
    }
}
