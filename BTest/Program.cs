using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
            // refactored snippet usage
            JobLogger.LogMessage("this is a simple message");
            JobLogger.LogMessage("this is an error message", JobLogger.LogLevelEnm.Error);

            Console.ReadKey();
        }
    }
}
