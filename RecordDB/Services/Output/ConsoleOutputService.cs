using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Services.Output
{
    public class ConsoleOutputService: IOutputService
    {
        public async Task WriteLineAsync(string message)
        {
            await Console.Out.WriteLineAsync(message);
        }

        public async Task WriteErrorAsync(string message)
        {
            var originalColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            await Console.Out.WriteLineAsync(message);
            Console.ForegroundColor = originalColor;
        }
    }
}
