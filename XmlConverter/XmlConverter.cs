using System.Collections.Generic;
using System.Xml;

namespace ConvertSystem
{
    /// Interface was created to have an ability to
    /// create converters of different type
    public interface IConverter
    {
        void Convert(List<Url> urlList, string path);
    }  

    /// <summary>
    /// Class converts object or Url type to Xml file
    /// </summary>
    public class XmlConverter : IConverter
    {
        /// <summary>
        /// Method converts list of Url objects to Xml file
        /// </summary>
        /// <param name="urlList">Objects of Url type</param>
        /// <param name="path">Custom path to store converted file</param>
        public void Convert(List<Url> urlList, string path)
        {
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
         
            xmlDoc.AppendChild(xmlDec);

            XmlElement urlAddresses = xmlDoc.CreateElement("urlAddresses");

            xmlDoc.AppendChild(urlAddresses);

            for (int i = 0; i < urlList.Count; i++)
            {
                if (urlList[i] == null)
                {
                    continue;
                }

                XmlElement urlAddress = xmlDoc.CreateElement("urlAddress");

                XmlElement hostName = xmlDoc.CreateElement("host_name");

                hostName.AppendChild(xmlDoc.CreateTextNode(urlList[i].HostName));

                XmlElement uri = xmlDoc.CreateElement("uri");

                InsertSegments(xmlDoc, uri, urlList[i].Segments);

                XmlElement parameters = xmlDoc.CreateElement("parameters");

                InsertParameters(xmlDoc, parameters, urlList[i].KeyValuePairs);

                urlAddress.AppendChild(hostName);

                urlAddress.AppendChild(uri);

                urlAddress.AppendChild(parameters);

                urlAddresses.AppendChild(urlAddress);
            }

            xmlDoc.Save(@"URL.xml");
        }

        /// <summary>
        /// Method-helper to create tags for segment storage
        /// </summary>
        /// <param name="xmlDoc">Xml document file</param>
        /// <param name="uri">Segments parent container</param>
        /// <param name="segments">Segment of Url address</param>
        private void InsertSegments(XmlDocument xmlDoc, XmlElement uri, List<string> segments)
        {
            foreach (var line in segments)
            {
                XmlElement segment = xmlDoc.CreateElement("segment");

                segment.AppendChild(xmlDoc.CreateTextNode(line));

                uri.AppendChild(segment);
            }
        }

        /// <summary>
        /// Method-helper to create tags for parameters storage
        /// </summary>
        /// <param name="xmlDoc">Xml document file</param>
        /// <param name="parameters">Parent container for each parameter</param>
        /// <param name="keyValuePairs">Array of elements(key, value)</param>
        private void InsertParameters(XmlDocument xmlDoc, XmlElement parameters, Dictionary<string, string> keyValuePairs)
        {
            foreach (var arg in keyValuePairs)
            {
                XmlElement parameter = xmlDoc.CreateElement("parameter");

                XmlAttribute value = xmlDoc.CreateAttribute("value");

                XmlAttribute key = xmlDoc.CreateAttribute("key");

                value.Value = arg.Value;

                key.Value = arg.Key;

                parameter.Attributes.Append(value);

                parameter.Attributes.Append(key);

                parameters.AppendChild(parameter);
            }
        }
    }
}
