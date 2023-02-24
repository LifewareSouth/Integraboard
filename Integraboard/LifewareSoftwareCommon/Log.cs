using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lifeware.SoftwareCommon
{
    public class Log
    {
        const string LOG_FILENAME = "events.log";
        static public void Write(string eventDetail)
        {
            string logData = $"{DateTime.Now.ToString("[dd-MM-yyyy HH:mm:ss] ")}{eventDetail}{Environment.NewLine}";
            File.AppendAllText(LOG_FILENAME, logData);
        }

    }
}
