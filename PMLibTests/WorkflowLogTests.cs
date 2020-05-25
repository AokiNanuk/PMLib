using Microsoft.VisualStudio.TestTools.UnitTesting;
using PMLib.Import;
using PMLib.Model;
using PMLib.Model.DataAnalysis;
using System;

namespace PMLibTests
{
    [TestClass]
    public class WorkflowLogTests
    {
        const string easyCsv = ".\\alpha2.csv";
        const string timestampedCsv = ".\\alpha3.csv";

        [TestMethod]
        public void ImportedEventLogSetInvalidValuesTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(timestampedCsv);

            // Act and assert
            Assert.IsFalse(elog.SetActivity("thisIsNotValidActivity"));
            Assert.IsFalse(elog.SetCaseId("thisIsNotValidCaseId"));
            Assert.IsFalse(elog.SetTimestamp("thisIsNotValidTimestamp"));
            Assert.IsNull(elog.Activity);
            Assert.IsNull(elog.CaseId);
            Assert.IsNull(elog.Timestamp);
        }

        [TestMethod]
        public void WorkflowLogBasicTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(easyCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");

            // Act
            WorkflowLog wlog = new WorkflowLog(elog);

            // Assert
            Assert.AreEqual(wlog.WorkflowTraces[0].CaseId, "1");
            Assert.AreEqual(wlog.WorkflowTraces[1].CaseId, "2");
            Assert.AreEqual(wlog.WorkflowTraces[0].Activities[0], "a");
            Assert.AreEqual(wlog.WorkflowTraces[0].Activities[1], "b");
            Assert.AreEqual(wlog.WorkflowTraces[0].Activities[2], "c");
            Assert.AreEqual(wlog.WorkflowTraces[1].Activities[0], "a");
            Assert.AreEqual(wlog.WorkflowTraces[1].Activities[1], "d");
            Assert.AreEqual(wlog.WorkflowTraces[1].Activities[2], "c");
        }

        [TestMethod]
        public void WorkflowLogTimestampedTest()
        {
            // Arrange
            ImportedEventLog elog = CSVImport.MakeDataFrame(timestampedCsv);
            elog.SetActivity("act");
            elog.SetCaseId("id");
            elog.SetTimestamp("time");

            // Act
            WorkflowLog wlog = new WorkflowLog(elog);

            // Assert
            Assert.AreEqual(wlog.WorkflowTraces[0].CaseId, "1");
            Assert.AreEqual(wlog.WorkflowTraces[1].CaseId, "2");
            Assert.AreEqual(wlog.WorkflowTraces[0].Activities[0], "a");
            Assert.AreEqual(wlog.WorkflowTraces[0].Activities[1], "b");
            Assert.AreEqual(wlog.WorkflowTraces[0].Activities[2], "c");
            Assert.AreEqual(wlog.WorkflowTraces[1].Activities[0], "a");
            Assert.AreEqual(wlog.WorkflowTraces[1].Activities[1], "d");
            Assert.AreEqual(wlog.WorkflowTraces[1].Activities[2], "c");
        }
    }
}
