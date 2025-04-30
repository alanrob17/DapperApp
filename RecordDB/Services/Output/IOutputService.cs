using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDB.Services.Output
{
    public interface IOutputService
    {
        void WriteLine(string message);
        void WriteError(string message);
    }
}
