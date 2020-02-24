using PMLib.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace PMLib.Export
{
    public static class PNMLExport
    {
        private static XElement GetPnmlHeader(XNamespace ns)
        {
            XElement xRoot = new XElement(ns + "pnml");
            return xRoot;
        }

        private static XElement GetNetNode(XNamespace ns, string id = "net")
        {
            XElement xNet = new XElement(ns + "net");
            xNet.SetAttributeValue("id", id);
            xNet.SetAttributeValue("type", ns.NamespaceName);
            return xNet;
        }

        private static XElement GetPageNode(XNamespace ns, int num = 0)
        {
            XElement xPage = new XElement(ns + "page");
            xPage.SetAttributeValue("id", "page-" + num);
            return xPage;
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

        private static void AddPlaces(XElement xPage, IEnumerable<IPlace> places, XNamespace ns)
        {
            foreach (IPlace p in places)
            {
                XElement xPlace = new XElement(ns + "place");
                xPlace.SetAttributeValue("id", p.Id);
                XElement xPlaceName = new XElement(ns + "name");
                xPlaceName.Add(new XElement(ns + "text") { Value = p.Id });
                xPlace.Add(xPlaceName);
                xPage.Add(xPlace);
            }
        }

        private static void AddTransitions(XElement xPage, IEnumerable<ITransition> transitions, XNamespace ns)
        {
            foreach (ITransition t in transitions)
            {
                XElement xTransition = new XElement(ns + "transition");
                xTransition.SetAttributeValue("id", t.Id);
                XElement xTransitionName = new XElement(ns + "name");
                xTransitionName.Add(new XElement(ns + "text") { Value = t.Activity });
                xTransition.Add(xTransitionName);
                xPage.Add(xTransition);
            }
        }

        private static void AddArcs(XElement xPage, IEnumerable<ITransition> transitions, XNamespace ns)
        {
            int id = 0;
            foreach (ITransition t in transitions)
            {
                foreach (IPlace ip in t.InputPlaces)
                {
                    XElement xArc = new XElement(ns + "arc");
                    xArc.SetAttributeValue("id", "a" + id);
                    id++;
                    xArc.SetAttributeValue("source", ip.Id);
                    xArc.SetAttributeValue("target", t.Id);
                    XElement xArcInscription = new XElement(ns + "inscription");     // dummy value for adding weights
                    xArcInscription.Add(new XElement(ns + "text") { Value = "1" });  // to arcs in the future
                    xArc.Add(xArcInscription);
                    xPage.Add(xArc);
                }

                foreach(IPlace op in t.OutputPlaces)
                {
                    XElement xArc = new XElement(ns + "arc");
                    xArc.SetAttributeValue("id", "a" + id);
                    id++;
                    xArc.SetAttributeValue("source", t.Id);
                    xArc.SetAttributeValue("target", op.Id);
                    XElement xArcInscription = new XElement(ns + "inscription");     // dummy value for adding weights
                    xArcInscription.Add(new XElement(ns + "text") { Value = "1" });  // to arcs in the future
                    xArc.Add(xArcInscription);
                    xPage.Add(xArc);
                }
                
            }
        }

        public static void Serialize(IPetriNet net)
        {
            XNamespace ns = "http://www.pnml.org/version-2009/grammar/pnml";
            XElement xRoot = GetPnmlHeader(ns);
            XElement xNet = GetNetNode(ns);
            xRoot.Add(xNet);
            XElement xPage = GetPageNode(ns);
            xNet.Add(xPage);
            AddPlaces(xPage, net.Places, ns);
            AddTransitions(xPage, net.Transitions, ns);
            AddArcs(xPage, net.Transitions, ns);

            XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = ("\t") };
            using (XmlWriter writer = XmlWriter.Create("petrinet" + DateTime.Now.ToString().Replace('.', '-').Replace(':', '-') + ".xml", settings))
            {
                xRoot.Save(writer);
            }
        }
    }
}
