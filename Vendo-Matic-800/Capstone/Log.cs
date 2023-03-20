using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq.Expressions;

namespace Capstone
{
    public class Log
    {
        public static void WriteLog(string purchaseLog)
        {
            string path = Environment.CurrentDirectory;
            string logName = "log.txt";
            string fullPath = Path.Combine(path, logName);
            try
            {
                using (StreamWriter sw = new StreamWriter(fullPath, true))
                {
                    sw.WriteLine($"{DateTime.UtcNow} {purchaseLog}");
                }                   
            }
            catch(Exception)
            {
            }
        }   
    }
}
