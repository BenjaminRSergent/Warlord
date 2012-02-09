using System;
using System.IO;

namespace Warlord.Application
{
    class ErrorLogger : IDisposable
    {
        StreamWriter errorLog;

        public ErrorLogger()
        {

        }
        public void Init(string fileName, bool append)
        {
            errorLog = new StreamWriter(File.Open(fileName, (append) ? FileMode.Append : FileMode.Create));
        }
        public void Write(string errorReport)
        {
            if(errorLog != null)
            {
                errorLog.Write(DateTime.Now.TimeOfDay + ": ");
                errorLog.WriteLine(errorReport);
                errorLog.WriteLine();
            }
        }

        public void Dispose()
        {
            errorLog.Dispose();
        }
    }
}
