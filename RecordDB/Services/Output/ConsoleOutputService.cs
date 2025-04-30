using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Services.Output
{
    public class ConsoleOutputService: IOutputService
    {
        public void WriteLine(string message) => Console.WriteLine(message);

        public void WriteError(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = originalColor;
        }
    }
}
