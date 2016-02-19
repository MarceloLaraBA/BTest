using System.Collections.Generic;
using System.IO;

namespace System.Configuration
{
    internal static class ConfigurationManager
    {
        // IMPORTANT NOTICE:
        // I've added this class just to emulate the behavior of the web app enviroment!!
        // in a real world app, this configuration will come from the web.config file...
        public static Dictionary<string, string> ConnectionStrings => new Dictionary < string, string>
        {
            { "ConnectionString", @"Data Source=(LocalDb);Initial Catalog=MyAppDB;Integrated Security=True"}
        };

        public static Dictionary<string, string> AppSettings => new Dictionary<string, string>
        {
            {"Log FileDirectory", Directory.GetCurrentDirectory()},

            // these items should be added to the appSettings of the web.config
            {"logToDatabase", "false" },
            {"logToFile", "false" },
            {"logToConsole", "true" },
            {"logLevel", "2"}
        };
    }
}