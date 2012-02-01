using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Warlord
{
    class ErrorLogger
    {
        StreamWriter errorLog;

        public ErrorLogger( )
        {

        }
        public void Init(String fileName, bool append)
        {
            errorLog = new StreamWriter(File.Open(fileName, (append) ? FileMode.Append : FileMode.Create ));
        }
        public void Write( string errorReport )
        {
            if(errorLog != null)
            {                
                errorLog.Write( DateTime.Now.TimeOfDay + ": ");
                errorLog.WriteLine( errorReport );
                errorLog.WriteLine( );
            }
        }
    }
}
