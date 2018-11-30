using System.IO;

namespace ConvertSystem
{
    /// <summary>
    /// Class logs messages about wrong type urls
    /// </summary>
    public class Logger
    {
        private string path = "LogFile";

        /// <summary>
        /// Constructor without arguments provides 
        /// writing logs to the default place
        /// </summary>
        public Logger()
        {
        }

        /// <summary>
        /// Constructor provides ability to set
        /// custom path to the log file
        /// </summary>
        /// <param name="_path"></param>
        public Logger(string _path)
        {
            path = _path;
        }

        /// <summary>
        /// Method writes messages into log file
        /// </summary>
        /// <param name="message">string data type with main information about wrong url</param>
        public void Log(string message)
        {
            using (StreamWriter sw = new StreamWriter(path, true, System.Text.Encoding.Default))
            {
                sw.WriteLine(message);
            }
        }
    }
}
