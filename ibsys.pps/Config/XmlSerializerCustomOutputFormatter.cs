using Microsoft.AspNetCore.Mvc.Formatters;
using System.Xml;
using System.Xml.Serialization;

namespace IBSYS.PPS.Config
{
    public class XmlSerializerCustomOutputFormatter : XmlSerializerOutputFormatter
    {
        protected override void Serialize(XmlSerializer xmlSerializer, XmlWriter xmlWriter, object value)
        {
            // Applying "empty" namespace will produce no namespaces
            var noNamespace = new XmlSerializerNamespaces();
            noNamespace.Add("", "");
            xmlSerializer.Serialize(xmlWriter, value, noNamespace);
        }
    }
}
