using System;
using System.Collections.Generic;

namespace ConvertSystem
{
    /// <summary>
    /// Class allows communication between
    /// Parser and Converter
    /// </summary>
    public class Service
    {
        private IParser parser;

        private IConverter converter;

        private string fileToParse = "FileToParse";

        private string convertedFile = "FinishFile";

        /// <summary>
        /// Constructor initializes entities of parser and converter
        /// with using default file paths
        /// </summary>
        /// <param name="_parser">Instance of parser type</param>
        /// <param name="_converter">Instance of converter type</param>
        public Service(IParser _parser, IConverter _converter)
        {
            parser = _parser;

            converter = _converter;
        }

        /// <summary>
        /// Constructor initializes entities of parser and converter
        /// with using custom file paths
        /// </summary>
        /// <param name="_parser">Instance of parser type</param>
        /// <param name="_converter">Instance of converter type</param>
        /// <param name="_fileToParse">Custom path to parser file</param>
        /// <param name="_convertedFile">Custom path to converted file</param>
        public Service(IParser _parser, IConverter _converter, string _fileToParse, string _convertedFile) : this(_parser, _converter)
        {
            fileToParse = _fileToParse;

            convertedFile = _convertedFile;
        }

        /// <summary>
        /// Allows to start process of parsing and convertion
        /// </summary>
        public void StartConvertion()
        {
            List<Url> urlList = parser.Parse(fileToParse);

            converter.Convert(urlList, convertedFile);
        }
    }
}
