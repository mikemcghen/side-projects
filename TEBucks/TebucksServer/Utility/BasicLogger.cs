using System;
using System.IO;

namespace TEbucksServer.Utility
{
    public class BasicLogger
    {
        private const string LogFolder = "logs";
        public static void Log(string message)
        {
            try
            {
                //Create a log folder if it doesn't exist
                Directory.CreateDirectory(LogFolder);

                //Determine a log file name
                string logFileName = $"{LogFolder}/{DateTime.Now:yyyy-MM-dd}.log";

                //Open the file and write the message, then flush and close the file (using block does this)
                using(StreamWriter sw = new StreamWriter(logFileName, true))
                {
                    sw.WriteLine($"{DateTime.Now:yyyy-MM-dd hh:mm:ss tt} {message}");
                }
            }
            catch(FileNotFoundException e)
            {
                throw new BasicLoggerException("Error occurred opening or creating log file:\n" + e.Message, e);
            }
            catch(IOException ex)
            {
                throw new BasicLoggerException("General IO error occurred:\n" + ex.Message, ex);
            }
        }
    }
}
