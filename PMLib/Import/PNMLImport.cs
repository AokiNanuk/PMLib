using PMLib.Model;
using PMLib.Model.NonUniqueTokenPetriNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace PMLib.Import
{
    class PNMLImport
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

        private static List<ITransition> GetTransitions(IEnumerable<XElement> xTransitions, IEnumerable<XElement> xArcs, List<IPlace> places)
        {
            List<ITransition> transitions = new List<ITransition>();
            foreach (XElement t in xTransitions)
            {
                transitions.Add(new Transition(t.Attribute("id").Value, t.Element("name").Element("text").Value));
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
            XElement netNode = pnmlRoot.Element("net");
            IEnumerable<XElement> xPlaces = netNode.Elements("place");
            IEnumerable<XElement> xTransitions = netNode.Elements("transition");
            IEnumerable<XElement> xArcs = netNode.Elements("arc");

            List<IPlace> places = GetPlaces(xPlaces);
            List<ITransition> transitions = GetTransitions(xTransitions, xArcs, places);
            Tuple<IPlace, IPlace> startAndEndPlace = GetStartAndEndPlaces(xArcs, places);

            return new PetriNet(transitions, places, startAndEndPlace.Item1, startAndEndPlace.Item2);
        }
    }
}
