using System;
using System.Linq;
using System.Text;
using static System.Int16;

namespace BTest
{
    public class JobLogger
    {


        // NOTE: I keep the constructor and the method in order to compile the project and notice other coders about this.
        // TODO refactor the references invoking this constructor and then remove it
        #region Things to be deprecated in the app
        public JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            throw new ApplicationException("This method is not accepted anymore: consider using LogMessage(string message, LogLevelEnm logLevel = 0)");
        }

        public static void LogMessage(string sMessage, bool bMessage, bool warning, bool error)
        {
            throw new ApplicationException("This method is not accepted anymore: consider using LogMessage(string message, LogLevelEnm logLevel = 0)");
        }
        #endregion

        #region Logger parameters
        private static bool logToFile => System.Configuration.ConfigurationManager.AppSettings["logToFile"].ToLower() == "true";
        private static bool logToDatabase => System.Configuration.ConfigurationManager.AppSettings["logToDatabase"].ToLower() == "true";
        private static bool logToConsole => System.Configuration.ConfigurationManager.AppSettings["logToConsole"].ToLower() == "true";
        private static LogLevelEnm minLogLevel =>  (LogLevelEnm)Parse(System.Configuration.ConfigurationManager.AppSettings["logLevel"]);
        #endregion


        public static void LogMessage(string message, LogLevelEnm logLevel = 0) // I consider an empty logLevel parameter as a message
        {
            
            // handle message
            if(string.IsNullOrEmpty(message)) throw new ArgumentException("Message must be specified");
            message = message.Trim();

            // check if the message must be logged
            if(logLevel < minLogLevel) return;

            // store the message
            if (logToDatabase)
            {
                try
                {
                    using (System.Data.SqlClient.SqlConnection connection = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"]))
                    {
                        connection.Open();
                        System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + message + "', " + (int)logLevel + ")");
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
                catch (Exception x)
                {
                    throw new ApplicationException("Error storing log to database, see innerException for details", x);
                }
            }

            if (logToFile)
            {
                try
                {
                    System.IO.File.AppendAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", message);
                }
                catch (Exception x)
                {
                    throw new ApplicationException("Error writing log file, see innerException for details", x);
                }
            }

            if (logToConsole)
            {
                try
                {
                    switch (logLevel)
                    {
                        case LogLevelEnm.Error:
                            Console.ForegroundColor = ConsoleColor.Red;
                            break;
                        case LogLevelEnm.Warning:
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            break;
                        case LogLevelEnm.Message:
                        default:
                            Console.ForegroundColor = ConsoleColor.White;
                            break;
                    }
                    Console.WriteLine("{0} {1}", DateTime.Now.ToShortDateString(), message);
                }
                catch (Exception x)
                {
                    throw new ApplicationException("Error writing to console, see innerException for details", x);
                }
            }
        }

        public enum LogLevelEnm
        {
            Message,
            Warning,
            Error
        }

    }
}
