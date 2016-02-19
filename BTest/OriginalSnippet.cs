using System;
using System.Linq;
using System.Text;

namespace BTest
{
    public class _JobLogger
    {

        private static bool _logToFile;
        private static bool _logToConsole;
        private static bool _logMessage;
        private static bool _logWarning;
        private static bool _logError;
        private static bool LogToDatabase;
        //TODO this private variable is never used
        private bool _initialized;


        public _JobLogger(bool logToFile, bool logToConsole, bool logToDatabase, bool logMessage, bool logWarning, bool logError)
        {
            //TODO get the configuration from the web.config
            _logError = logError;
            _logMessage = logMessage;
            _logWarning = logWarning;
            //TODO It’s a well-known best practice to differentiate private from public variables and keep the team rules: in this case, private variables must be preceded with an underscore
            LogToDatabase = logToDatabase;
            _logToFile = logToFile;
            _logToConsole = logToConsole;
        }

        //TODO the parameter message is duplicated, refactor this in order to compile.
        public static void LogMessage(string sMessage, bool message, bool warning, bool error)
        {
            //TODO first, if sMessage is null it will crash.. second, the resulting trimmed string should be stored in sMessage.
            sMessage.Trim();
            //TODO consider using  String object to validate empty or null strings: string.IsNullOrEmpty(sMessage)
            if (sMessage == null || sMessage.Length == 0)
            {
                return;
            }
            if (!_logToConsole && !_logToFile && !LogToDatabase)
            {
                throw new Exception("Invalid configuration");
            }
            if ((!_logError && !_logMessage && !_logWarning) || (!message && !warning
    && !error))
            {
                throw new Exception("Error or Warning or Message must be specified");
            }

            //TODO you must ALWAYS close the SQL Connection!! in a production enviroment it will rely on the timeout event, and in a heavy load enviroment it will crash..
            //TODO consider to use the 'using' clause to ensure the connection disposal after using it
            //TODO best practice: keep things in its place. use the 'ConnectionStrings' collection to store connection strings
            System.Data.SqlClient.SqlConnection connection = new
            System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"]);

            //TODO why open a SQL Connection if LogToDatabase is false? try to avoid useless connections
            connection.Open();
            //TODO it's a well-konwn best practice to initialize not nullable variables to prevent exceptions
            int t = 0;
            if (message && _logMessage)
            {
                t = 1;
            }
            if (error && _logError)
            {
                t = 2;
            }
            if (warning && _logWarning)
            {
                t = 3;
            }
            //TODO always write to SQL
            System.Data.SqlClient.SqlCommand command = new System.Data.SqlClient.SqlCommand("Insert into Log Values('" + sMessage + "', " + t.ToString() + ")");
            command.ExecuteNonQuery();

            //TODO it's a well-konwn best practice to initialize not nullable variables to prevent exceptions
            string l = string.Empty;
            if
            (!System.IO.File.Exists(System.Configuration.ConfigurationManager.AppSettings["Log FileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt"))
            {
                l = System.IO.File.ReadAllText(System.Configuration.ConfigurationManager.AppSettings["LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt");
            }
            if (error && _logError)
            {
                //TODO use the += operator 
                l = l + DateTime.Now.ToShortDateString() + sMessage;
            }
            if (warning && _logWarning)
            {
                l = l + DateTime.Now.ToShortDateString() + sMessage;
            }
            if (message && _logMessage)
            {
                l = l + DateTime.Now.ToShortDateString() + sMessage;
            }
            //TODO always write to File
            //TODO consider using File.AppendAllText instead of reading, appending a writing back
            System.IO.File.WriteAllText(System.Configuration.ConfigurationManager.AppSettings[
            "LogFileDirectory"] + "LogFile" + DateTime.Now.ToShortDateString() + ".txt", l);

            if (error && _logError)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            if (warning && _logWarning)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (message && _logMessage)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(DateTime.Now.ToShortDateString() + message);
        }

    }
}
