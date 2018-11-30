using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConvertSystem
{
    /// <summary>
    /// Interface was created to have an ability to
    /// create parsers of different type
    /// </summary>
    public interface IParser
    {
        List<Url> Parse(string path);
    }

    /// <summary>
    /// Class represents methods for text file parsing
    /// </summary>
    public class TextParser : IParser
    {
        private const int PartsLength = 2;

        private Logger logger = new Logger();

        /// <summary>
        /// Constructor initializes neccessary variables 
        /// to form Url instances
        /// </summary>
        public TextParser()
        {
            UrlList = new List<Url>();

            Segments = new List<string>();

            KeyValuePairs = new Dictionary<string, string>();
        }

        public List<Url> UrlList { get; private set; }

        public string HostName { get; private set; }

        public List<string> Segments { get; private set; }

        public Dictionary<string, string> KeyValuePairs { get; private set; }

        /// <summary>
        /// Parse method analyzes text file and produce list of Url objects
        /// </summary>
        /// <param name="path">Custom path to the parse file</param>
        /// <returns>List of Url objects</returns>
        public List<Url> Parse(string path)
        {
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string address;

                while ((address = sr.ReadLine()) != null)
                {
                    UrlList.Add(CreateUrlInst(address));

                    Segments.Clear();

                    KeyValuePairs.Clear();
                }
            }

            return UrlList;
        }

        /// <summary>
        /// Method creates a single instance of Url object
        /// </summary>
        /// <param name="address">String for analyze</param>
        /// <returns>Url object</returns>
        private Url CreateUrlInst(string address)
        {
            if (address == null)
            {
                throw new ArgumentNullException("URL address cannot be null!");
            }

            string[] array = new string[] { address };

            try
            {
                Scheme(ref array);

                Host(ref array);

                Segment(ref array);

                Params(ref array);
            }
            catch (ArgumentException exception)
            {
                logger.Log(exception.Message + " " + address);

                return null;
            }

            return new Url(HostName, Segments, KeyValuePairs);
        }

        /// <summary>
        /// Method extracts first part of Url address
        /// </summary>
        /// <param name="address">Url address</param>
        private void Scheme(ref string[] address)
        {
            string[] array = address[0].Split(new string[] { "://" }, StringSplitOptions.RemoveEmptyEntries);

            Validation(new string[] { array[0] });

            if (array.Length != PartsLength)
            {
                throw new ArgumentException("Address has no host name!");
            }

            address = new string[] { array[1] };
        }

        /// <summary>
        /// Method extracts second part of Url address
        /// and writes to string variable
        /// </summary>
        /// <param name="address">Array of (Host and Segments and Parameters)</param>
        private void Host(ref string[] address)
        {
            string[] array = address[0].Split('/');

            array = array.Where(x => x != string.Empty).ToArray();

            Validation(new string[] { array[0] });

            HostName = array[0];

            if (array.Length < PartsLength)
            {
                address = null;

                return;
            }

            string[] temp = new string[array.Length - 1];

            Array.Copy(array, 1, temp, 0, array.Length - 1);

            address = temp;
        }

        /// <summary>
        /// Method analyzes segments of address 
        /// and adds to list of segment objects
        /// </summary>
        /// <param name="address">Array of (Segments and Parameters)</param>
        private void Segment(ref string[] address)
        {
            if (address == null)
            {
                return;
            }

            string[] temp = new string[address.Length - 1];

            Array.Copy(address, 0, temp, 0, address.Length - 1);

            Validation(temp);

            foreach (var entry in address)
            {
                if (!entry.Contains("?"))
                {
                    Validation(new string[] { entry });

                    Segments.Add(entry);
                }
                else
                {
                    address = LastSegment(address[address.Length - 1]);

                    return;
                }
            }

            address = null;
        }

        /// <summary>
        /// Method extracts last segment and
        /// prepare parameters for analyzing
        /// </summary>
        /// <param name="address">Array of (Last Segment and Parameter)</param>
        /// <returns>Url address parameters</returns>
        private string[] LastSegment(string address)
        {
            string[] array = address.Split('?');

            Validation(new string[] { array[0] });

            Segments.Add(array[0]);

            if (array.Length == 1)
            {
                return null;
            }

            if (array.Length > PartsLength)
            {
                throw new ArgumentException("Invalid symbols '?' in parameters!");
            }

            return new string[] { array[1] };
        }

        /// <summary>
        /// Method tries to separate parameter key and value
        /// </summary>
        /// <param name="address">Parameters</param>
        private void Params(ref string[] address)
        {
            if (address == null)
            {
                return;
            }

            string[] array = address[0].Split('&');

            foreach (var entry in array)
            {
                string[] pair = entry.Split('=');

                Validation(pair);

                if (pair.Length == 1)
                {
                    throw new ArgumentException("Some parameter has no value!");
                }

                KeyValuePairs.Add(pair[0], pair[1]);
            }
        }

        /// <summary>
        /// Method validates parts of Url for right form
        /// </summary>
        /// <param name="array">Array of Url parts</param>
        private void Validation(string[] array)
        {
            string[] forbidden = new string[] { "&", "?", "=" };

            foreach (var entry in array)
            {
                foreach (var symbol in forbidden)
                {
                    if (entry.Contains(symbol))
                    {
                        throw new ArgumentException("string contains invalid symbols!");
                    }
                }
            }
        }
    }
}
