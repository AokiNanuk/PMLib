using PMLib.Model;
using PMLib.Model.NonUniqueTokenPetriNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace PMLib.Import
{
    public class PNMLImport
    {
        private static List<IPlace> GetPlaces(IEnumerable<XElement> xPlaces)
        {
            List<IPlace> places = new List<IPlace>();
            foreach (XElement p in xPlaces)
            {
                places.Add(new Place(p.Attribute("id").Value));
            }
            return places;
        }

        private static List<ITransition> GetTransitions(IEnumerable<XElement> xTransitions, IEnumerable<XElement> xArcs, List<IPlace> places, XNamespace ns)
        {
            List<ITransition> transitions = new List<ITransition>();
            foreach (XElement t in xTransitions)
            {
                transitions.Add(new Transition(t.Attribute("id").Value, t.Element(ns + "name").Element(ns + "text").Value));
            }
            
            foreach (XElement a in xArcs)
            {
                string sourceId = a.Attribute("source").Value;
                string targetId = a.Attribute("target").Value;
                if (sourceId[0] == 'p')
                {
                    transitions.Find(a => a.Id == targetId).InputPlaces.Add(places.Find(a => a.Id == sourceId));
                }
                else
                {
                    transitions.Find(a => a.Id == sourceId).OutputPlaces.Add(places.Find(a => a.Id == targetId));
                }
            }

            return transitions;
        }

        private static Tuple<IPlace, IPlace> GetStartAndEndPlaces(IEnumerable<XElement> xArcs, IEnumerable<IPlace> places)
        {
            HashSet<string> targetIds = new HashSet<string>();
            HashSet<string> sourceIds = new HashSet<string>();

            foreach (XElement a in xArcs)
            {
                string targetId = a.Attribute("target").Value;
                string sourceId = a.Attribute("source").Value;
                if (targetId[0] == 'p')
                {
                    targetIds.Add(targetId);
                }
                if (sourceId[0] == 'p')
                {
                    sourceIds.Add(sourceId);
                }
            }

            IPlace startPlace = null;
            IPlace endPlace = null;
            foreach (IPlace p in places)
            {
                if (startPlace == null && !targetIds.Contains(p.Id))
                {
                    startPlace = p;
                }
                if (endPlace == null && !sourceIds.Contains(p.Id))
                {
                    endPlace = p;
                }
            }
            return new Tuple<IPlace, IPlace>(startPlace, endPlace);
        }


        public static IPetriNet Deserialize(string inputFilePath)
        {
            XElement pnmlRoot = XElement.Load(inputFilePath);
            XNamespace ns = pnmlRoot.Attribute("xmlns").Value;
            XElement netNode = pnmlRoot.Element(ns + "net");
            XElement pageNode = netNode.Element(ns + "page");
            IEnumerable<XElement> xPlaces = pageNode.Elements(ns + "place");
            IEnumerable<XElement> xTransitions = pageNode.Elements(ns + "transition");
            IEnumerable<XElement> xArcs = pageNode.Elements(ns + "arc");

            List<IPlace> places = GetPlaces(xPlaces);
            List<ITransition> transitions = GetTransitions(xTransitions, xArcs, places, ns);
            Tuple<IPlace, IPlace> startAndEndPlace = GetStartAndEndPlaces(xArcs, places);

            return new PetriNet(transitions, places, startAndEndPlace.Item1, startAndEndPlace.Item2);
        }
    }
}
