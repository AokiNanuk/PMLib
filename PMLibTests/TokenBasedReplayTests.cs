using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMLib.ConformanceChecking.TokenBasedReplay;
using PMLib.Discovery.Alpha;
using PMLib.Import;
using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMLibTests
{
    [TestClass]
    public class TokenBasedReplayTests
    {
        const string hardCsv = ".\\alpha.csv";
        const string tamperedHardCsv = ".\\tamperedAlpha.csv";
        const string randomLog = ".\\randomLog.csv";

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
