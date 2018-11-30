using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvertSystem
{
    /// <summary>
    /// Class encapsulates properties of Url address
    /// </summary>
    public class Url
    {
        /// <summary>
        /// Constructor initializes Url parts:
        /// Host Name, Segments, Parameters(Key and Value)
        /// </summary>
        /// <param name="_hostName"></param>
        /// <param name="_segments"></param>
        /// <param name="_keyValuePairs"></param>
        public Url(string _hostName, List<string> _segments, Dictionary<string, string> _keyValuePairs)
        {
            HostName = _hostName;

            Segments = new List<string>(_segments);

            KeyValuePairs = new Dictionary<string, string>(_keyValuePairs);
        }

        public string HostName { get; private set; }

        public List<string> Segments { get; private set; }

        public Dictionary<string, string> KeyValuePairs { get; private set; }
    }
}
