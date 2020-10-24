using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using IBSYS.PPS.Models;
using IBSYS.PPS.Models.Generated;
using IBSYS.PPS.Models.Input;

namespace IBSYS.PPS.Serializer
{
    public class DataSerializer
    {
        private string defaultNamespace = "";

        public Input ReadDataAndDeserialize(string filename)
        {
            // New Instance of XmlSerializer for Class Input
            XmlSerializer serializer = new XmlSerializer(typeof(Input), defaultNamespace);

            // Handling of unknown nodes or attributes
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            // Filestream for reading the file
            FileStream fs = new FileStream(filename, FileMode.Open);

            // Object varialbe of the type to be deserialized
            Input i;

            i = (Input) serializer.Deserialize(fs);

            fs.Close();

            return i;
        }

        public Results ReadDataAndDeserializePeriodResults(string filename)
        {
            // New Instance of XmlSerializer for Class Input
            XmlSerializer serializer = new XmlSerializer(typeof(Results), defaultNamespace);

            // Handling of unknown nodes or attributes
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            // Filestream for reading the file
            FileStream fs = new FileStream(filename, FileMode.Open);

            // Object varialbe of the type to be deserialized
            Results i;

            i = (Results)serializer.Deserialize(fs);

            fs.Close();

            return i;
        }

        public Results DeserializePeriodResults(string input)
        {
            // New Instance of XmlSerializer for Class Input
            XmlSerializer serializer = new XmlSerializer(typeof(Results), defaultNamespace);

            // Handling of unknown nodes or attributes
            serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

            // Object variable of the type to be deserialized
            Results r;

            XmlDocument xmlInput = new XmlDocument();

            xmlInput.LoadXml(input);

            using (XmlReader reader = new XmlNodeReader(xmlInput))
            {
                r = (Results)serializer.Deserialize(reader);
            }

            return r;
        }

        public XDocument SerializeInput<T>(ref T i)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            
            XDocument data = new XDocument();

            using var writer = data.CreateWriter();
            serializer.WriteObject(writer, i);

            return data;
        }

        protected void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            Console.WriteLine("Unknown Node:" + e.Name + "\t" + e.Text);
        }
        protected void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            System.Xml.XmlAttribute attr = e.Attr;
            Console.WriteLine("Unknown attribute "+ attr.Name + "='" + attr.Value + "'!");
        }
    }
}