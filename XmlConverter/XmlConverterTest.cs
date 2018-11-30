using System;
using ConvertSystem;

namespace XmlConverterTest
{
    public class XmlConverterTest
    {
        public static void Main(string[] args)
        {
            Service service = new Service(new TextParser(), new XmlConverter());

            service.StartConvertion();

            Console.ReadKey();
        }
    }
}
