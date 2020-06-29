using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMLib.ConformanceChecking.TokenBasedReplay;
using PMLib.Discovery.Alpha;
using PMLib.Import;
using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PMLibTests
{
    [TestClass]
    public class TokenBasedReplayTests
    {
        static readonly string workingDirectory = Environment.CurrentDirectory;
        static readonly string projectDirectory = Directory.GetParent(workingDirectory).Parent.Parent.FullName;

        static readonly string separator = System.IO.Path.DirectorySeparatorChar.ToString();
        readonly string hardCsv = projectDirectory + separator + "files" + separator + "alpha.csv";
        readonly string tamperedHardCsv = projectDirectory + separator + "files" + separator + "tamperedAlpha.csv";
        readonly string randomLog = projectDirectory + separator + "files" + separator + "randomLog.csv";

        [TestMethod]
        public void CompareLogWithAccordingPetriNetTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(hardCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            // Act
            double fitness = Computations.ComputeFitness(elog, madeNet);

            // Assert
            Assert.AreEqual(1.0, fitness);
        }

        [TestMethod]
        public void CompareMildlyTamperedLogWithHardPetriNetTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(hardCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            ImportedEventLog tamperedLog = CSVImport.MakeDataFrame(tamperedHardCsv);
            tamperedLog.SetActivity("act");
            tamperedLog.SetCaseId("id");

            // Act
            double fitness = Computations.ComputeFitness(tamperedLog, madeNet);

            // Assert
            Assert.AreEqual(96, (int)(fitness*100));
        }

        [TestMethod]
        public void CompareCompletelyDifferentLogWithHardPetriNetTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(hardCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            WorkflowLog wlog = new WorkflowLog(elog);
            RelationMatrix matrix = new RelationMatrix(wlog);
            IPetriNet madeNet = Alpha.MakePetriNet(matrix);

            ImportedEventLog tamperedLog = CSVImport.MakeDataFrame(randomLog);
            tamperedLog.SetActivity("act");
            tamperedLog.SetCaseId("id");

            // Act
            double fitness = Computations.ComputeFitness(tamperedLog, madeNet);

            // Assert
            Assert.AreEqual(0.0, fitness);
        }

    }
}
