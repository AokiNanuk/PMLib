﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMLib.Discovery.Alpha;
using PMLib.Export;
using PMLib.Import;
using PMLib.Model;
using PMLib.Model.BasicPetriNet;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLibTests
{
    [TestClass]
    public class AlphaTests
    {
        const string hardCsv = ".\\alpha.csv";
        const string easyCsv = ".\\alpha2.csv";
        const string veryHardCsv = ".\\alphaHard.csv";
        const string cycleNetCsv = ".\\cycleNet.csv";
        const string veryHardPnml = ".\\hardPetriNet.xml";
        const string hardPnml = ".\\net.xml";
        const string cycleNetPnml = ".\\cycleNet.xml";

        private IPetriNet MakeEasyPetriNet()
        {
            IPlace p0 = new Place("p0");
            IPlace p1 = new Place("p1");
            IPlace p2 = new Place("p2");
            IPlace p3 = new Place("p3");

            ITransition t0 = new Transition("t0", "a");
            ITransition t1 = new Transition("t1", "b");
            ITransition t2 = new Transition("t2", "c");
            ITransition t3 = new Transition("t3", "d");

            t0.InputPlaces.Add(p0);
            t0.OutputPlaces.Add(p1);
            t1.InputPlaces.Add(p1);
            t1.OutputPlaces.Add(p2);
            t2.InputPlaces.Add(p2);
            t2.OutputPlaces.Add(p3);
            t3.InputPlaces.Add(p1);
            t3.OutputPlaces.Add(p2);

            IPetriNet net = new PetriNet(new List<ITransition>() { t0, t1, t2, t3 },
                new List<IPlace>() { p0, p1, p2, p3 },
                p0, p3);
            return net;
        }

        [TestMethod]
        public void MakeEasyPetriNetTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(easyCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet exampleNet = MakeEasyPetriNet();

            // Act
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            // Assert
            Assert.IsNotNull(exampleNet);
            Assert.AreEqual(exampleNet.EndPlace.Id, madeNet.EndPlace.Id);
            Assert.AreEqual(exampleNet.StartPlace.Id, madeNet.StartPlace.Id);
            Assert.AreEqual(exampleNet.Places.Count, madeNet.Places.Count);
            Assert.AreEqual(exampleNet.Transitions.Count, madeNet.Transitions.Count);

            foreach (IPlace p in exampleNet.Places)
            {
                Assert.IsTrue(madeNet.Places.Exists(a => a.Id == p.Id));
            }
            foreach (ITransition t in exampleNet.Transitions)
            {
                Assert.IsTrue(madeNet.Transitions.Exists(a => a.Id == t.Id &&
                a.Activity == t.Activity &&
                a.InputPlaces.Count == t.InputPlaces.Count &&
                a.OutputPlaces.Count == t.OutputPlaces.Count));
            }
        }

        [TestMethod]
        public void MakeHardPetriNetTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(hardCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet exampleNet = PNMLImport.Deserialize(hardPnml);

            // Act
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            // Assert
            Assert.IsNotNull(exampleNet);
            Assert.AreEqual(exampleNet.EndPlace.Id, madeNet.EndPlace.Id);
            Assert.AreEqual(exampleNet.StartPlace.Id, madeNet.StartPlace.Id);
            Assert.AreEqual(exampleNet.Places.Count, madeNet.Places.Count);
            Assert.AreEqual(exampleNet.Transitions.Count, madeNet.Transitions.Count);

            foreach (IPlace p in exampleNet.Places)
            {
                Assert.IsTrue(madeNet.Places.Exists(a => a.Id == p.Id));
            }
            foreach (ITransition t in exampleNet.Transitions)
            {
                Assert.IsTrue(madeNet.Transitions.Exists(a => a.Id == t.Id &&
                a.Activity == t.Activity &&
                a.InputPlaces.Count == t.InputPlaces.Count &&
                a.OutputPlaces.Count == t.OutputPlaces.Count));
            }
        }

        [TestMethod]
        public void MakeEmptyPetriNetTest()
        {
            // Arrange
            List<string> activities = new List<string>() { "wake up", "have coffee", "have tea", "work", "go home", "go sleep", "play witcher" };
            HashSet<string> startActivities = new HashSet<string>() { "wake up" };
            HashSet<string> endActivities = new HashSet<string>() { "go sleep", "play witcher" };
            RelationMatrix matrix = new RelationMatrix(activities, startActivities, endActivities);

            // Act
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            // Assert
            Assert.IsNotNull(madeNet);
            Assert.AreEqual("p1", madeNet.EndPlace.Id);
            Assert.AreEqual("p0", madeNet.StartPlace.Id);
            Assert.AreEqual(2, madeNet.Places.Count);
            Assert.AreEqual(7, madeNet.Transitions.Count);
        }

        [TestMethod]
        public void MakeVeryHardPetriNetTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(veryHardCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet exampleNet = PNMLImport.Deserialize(veryHardPnml);

            // Act
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            // Assert
            Assert.IsNotNull(exampleNet);
            Assert.AreEqual(exampleNet.EndPlace.Id, madeNet.EndPlace.Id);
            Assert.AreEqual(exampleNet.StartPlace.Id, madeNet.StartPlace.Id);
            Assert.AreEqual(exampleNet.Places.Count, madeNet.Places.Count);
            Assert.AreEqual(exampleNet.Transitions.Count, madeNet.Transitions.Count);

            foreach (IPlace p in exampleNet.Places)
            {
                Assert.IsTrue(madeNet.Places.Exists(a => a.Id == p.Id));
            }
            foreach (ITransition t in exampleNet.Transitions)
            {
                Assert.IsTrue(madeNet.Transitions.Exists(a => a.Id == t.Id &&
                a.Activity == t.Activity &&
                a.InputPlaces.Count == t.InputPlaces.Count &&
                a.OutputPlaces.Count == t.OutputPlaces.Count));
            }
        }

        [TestMethod]
        public void MakePetriNetWithCycleTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(cycleNetCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet exampleNet = PNMLImport.Deserialize(cycleNetPnml);

            // Act
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            // Assert
            Assert.IsNotNull(exampleNet);
            Assert.AreEqual(exampleNet.EndPlace.Id, madeNet.EndPlace.Id);
            Assert.AreEqual(exampleNet.StartPlace.Id, madeNet.StartPlace.Id);
            Assert.AreEqual(exampleNet.Places.Count, madeNet.Places.Count);
            Assert.AreEqual(exampleNet.Transitions.Count, madeNet.Transitions.Count);

            foreach (IPlace p in exampleNet.Places)
            {
                Assert.IsTrue(madeNet.Places.Exists(a => a.Id == p.Id));
            }
            foreach (ITransition t in exampleNet.Transitions)
            {
                Assert.IsTrue(madeNet.Transitions.Exists(a => a.Id == t.Id &&
                a.Activity == t.Activity &&
                a.InputPlaces.Count == t.InputPlaces.Count &&
                a.OutputPlaces.Count == t.OutputPlaces.Count));
            }
        }
    }
}
