using PMLib.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PMLib.Export
{
    static class PNMLExport
    {
        private static XElement GetPnmlHeader()
        {
            XElement xRoot = new XElement("pnml");
            xRoot.SetAttributeValue("xmlns", "http://www.pnml.org/version-2009/grammar/pnml");
            return xRoot;
        }

        private static XElement GetNetNode(string id = "net")
        {
            XElement xNet = new XElement("net");
            xNet.SetAttributeValue("id", id);
            xNet.SetAttributeValue("type", "http://www.pnml.org/version-2009/grammar/ptnet");
            return xNet;
        }

        /*
        private static string GetPlaceId(IPlace place)
        {
            return "p" + place.Id;
        }

        private static string GetTransitionId(ITransition transition)
        {
            return "t" + transition.Id + "_" + transition.Activity;
        }*/

        private static void AddPlaces(XElement xNet, IEnumerable<IPlace> places)
        {
            foreach (IPlace p in places)
            {
                XElement xPlace = new XElement("place");
                xPlace.SetAttributeValue("id", p.Id);
                XElement xPlaceName = new XElement("name");
                xPlaceName.Add(new XElement("text") { Value = p.Id });
                xPlace.Add(xPlaceName);
                xNet.Add(xPlace);
            }
        }

        private static void AddTransitions(XElement xNet, IEnumerable<ITransition> transitions)
        {
            foreach (ITransition t in transitions)
            {
                XElement xTransition = new XElement("transition");
                xTransition.SetAttributeValue("id", t.Id);
                XElement xTransitionName = new XElement("name");
                xTransitionName.Add(new XElement("text") { Value = t.Activity });
                xTransition.Add(xTransitionName);
                xNet.Add(xTransition);
            }
        }

        private static void AddArcs(XElement xNet, IEnumerable<ITransition> transitions)
        {
            int id = 0;
            foreach (ITransition t in transitions)
            {
                foreach (IPlace ip in t.InputPlaces)
                {
                    XElement xArc = new XElement("arc");
                    xArc.SetAttributeValue("id", "a" + id);
                    id++;
                    xArc.SetAttributeValue("source", ip.Id);
                    xArc.SetAttributeValue("target", t.Id);
                    XElement xArcInscription = new XElement("inscription");     // dummy value for adding weights
                    xArcInscription.Add(new XElement("text") { Value = "1" });  // to arcs in the future
                    xArc.Add(xArcInscription);
                    xNet.Add(xArc);
                }

                foreach(IPlace op in t.OutputPlaces)
                {
                    XElement xArc = new XElement("arc");
                    xArc.SetAttributeValue("id", "a" + id);
                    id++;
                    xArc.SetAttributeValue("source", t.Id);
                    xArc.SetAttributeValue("target", op.Id);
                    XElement xArcInscription = new XElement("inscription");     // dummy value for adding weights
                    xArcInscription.Add(new XElement("text") { Value = "1" });  // to arcs in the future
                    xArc.Add(xArcInscription);
                    xNet.Add(xArc);
                }
                
            }
        }

        public static void Serialize(IPetriNet net)
        {
            XElement xRoot = GetPnmlHeader();
            XElement xNet = GetNetNode();
            xRoot.Add(xNet);

            AddPlaces(xNet, net.Places);
            AddTransitions(xNet, net.Transitions);
            AddArcs(xNet, net.Transitions);

            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = ("\t") };
            using (XmlWriter writer = XmlWriter.Create("petrinet" + DateTime.Now + ".xml", settings))
            {
                xRoot.Save(writer);
            }
        }
    }
}
