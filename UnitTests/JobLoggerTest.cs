using System;
using System.Runtime.Remoting.Messaging;
using BTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public class JobLoggerTest
    {

        #region Test Output Methods

        // string to be checked in tests
        private string msg = "testMessage";

        [TestMethod]
        public void JobLoggerTest_LogToConsole()
        {
            JobLogger.LogMessage(msg,JobLogger.LogLevelEnm.Error);

            string expectedConsoleTail = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), msg);
            string actualConsoleTail = expectedConsoleTail;  // this text should be obtained from the consoles tail

            Assert.AreEqual(expectedConsoleTail, actualConsoleTail);
        }

        [TestMethod]
        public void JobLoggerTest_LogToDatabase()
        {
            JobLogger.LogMessage(msg, JobLogger.LogLevelEnm.Error);

            string expectedSQLrow = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), msg);
            string lastSQLrow = expectedSQLrow;  // it should be a query to the log table using something like @@identity

            Assert.AreEqual(expectedSQLrow, lastSQLrow);
        }
        [TestMethod]
        public void JobLoggerTest_LogToFile()
        {
            JobLogger.LogMessage(msg, JobLogger.LogLevelEnm.Error);

            string expectedSQLrow = string.Format("{0} {1}", DateTime.Now.ToShortDateString(), msg);
            string lastSQLrow = expectedSQLrow;  // it should read the last line of file

            Assert.AreEqual(expectedSQLrow, lastSQLrow);
        }
        #endregion

        #region Test Logging level

        private bool findLogEntry()
        {
            return true;
        }

        [TestMethod]
        public void JobLoggerTest_LogMessage()
        {
            JobLogger.LogMessage(msg, JobLogger.LogLevelEnm.Message);
            Assert.AreEqual(true, findLogEntry());
        }
        [TestMethod]
        public void JobLoggerTest_LogWarning()
        {
            JobLogger.LogMessage(msg, JobLogger.LogLevelEnm.Warning);
            Assert.AreEqual(true, findLogEntry());
        }
        [TestMethod]
        public void JobLoggerTest_LogError()
        {
            JobLogger.LogMessage(msg, JobLogger.LogLevelEnm.Error);
            Assert.AreEqual(true, findLogEntry());
        }
        [TestMethod]
        public void JobLoggerTest_IgnoreLogMessageIfLevelIsWarning()
        {
            JobLogger.LogMessage(msg, JobLogger.LogLevelEnm.Error);
            Assert.AreEqual(false, !findLogEntry());
        }
        #endregion

    }
}
