using PMLib.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PMLib.Export
{
    public static class DOTExport
    {
        private static string GetGraphHeader(string graphName = "G")
        {
            return "digraph " + graphName + "{\n";
        }

        private static string GetPlacesHeader(string indentation)
        {
            string doubleIndent = indentation + indentation;
            return indentation + "subgraph place {\n" +
                doubleIndent + "graph [shape=circle,color=gray];\n" +
                doubleIndent + "node [shape=circle,fixedsize=true,width=2];\n";
        }

        private static StringBuilder GetPlaces(List<IPlace> places, string indentation)
        {
            string doubleIndent = indentation + indentation;
            StringBuilder outStr = new StringBuilder("");
            outStr.Append(GetPlacesHeader(indentation));
            foreach (IPlace p in places)
            {
                outStr.Append(doubleIndent + "\"" + p.Id + "\";\n");
            }
            outStr.Append(indentation + "}\n");
            return outStr;
        }

        private static string GetTransitionsHeader(string indentation)
        {
            string doubleIndent = indentation + indentation;
            return indentation + "subgraph transitions {\n" +
                doubleIndent + "node[shape = rect, height = 0.2, width = 2];\n";
        }

        private static StringBuilder GetTransitions(List<ITransition> transitions, string indentation)
        {
            string doubleIndent = indentation + indentation;
            StringBuilder outStr = new StringBuilder("");
            outStr.Append(GetTransitionsHeader(indentation));
            foreach (ITransition t in transitions)
            {
                outStr.Append(doubleIndent + "\"" + t.Activity + "\";\n");
            }
            outStr.Append(indentation + "}\n");
            return outStr;
        }

        private static StringBuilder GetArcs(List<ITransition> transitions, string indentation)
        {
            StringBuilder outStr = new StringBuilder("");
            foreach (ITransition t in transitions)
            {
                foreach (IPlace ip in t.InputPlaces)
                {
                    outStr.Append(indentation + "\"" + ip.Id + "\" -> \"" + t.Activity + "\";\n");
                }
                foreach (IPlace op in t.OutputPlaces)
                {
                    outStr.Append(indentation + "\"" + t.Activity + "\" -> \"" + op.Id + "\";\n");
                }
            }
            outStr.Append("}");
            return outStr;
        }

        public static void Serialize(IPetriNet net, string indentation = "\t")
        {
            StringBuilder outStr = new StringBuilder("");
            outStr.Append(GetGraphHeader());
            outStr.Append(GetPlaces(net.Places, indentation));
            outStr.Append(GetTransitions(net.Transitions, indentation));
            outStr.Append(GetArcs(net.Transitions, indentation));

            using (var fileStream = new FileStream("petrinet" + DateTime.Now.ToString().Replace('.', '-').Replace(':', '-') + ".dot", FileMode.Create))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(outStr.ToString());
                }
            }
        }
    }
}
