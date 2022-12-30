using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Infrastructure.Interfaces
{
    public class Parameter
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
    }
    public interface IXmlService
    {
        List<Parameter> GetNodes(string xmlString);
        string UpdateXml(List<Parameter> parameters, string xml);
    }

    public class XmlService : IXmlService
    {
        public List<Parameter> GetNodes(string xmlString)
        {
            if (string.IsNullOrEmpty(xmlString)) return new List<Parameter>();
            var xdoc = XDocument.Parse(xmlString);

            var nodes = xdoc.Root.Nodes();

            var values = xdoc.Descendants("Value");

            var items = values.Select(v =>
            {
                var name = v.Attribute("name").Value;
                var description = v.Attribute("description") != null ? v.Attribute("description").Value : "";
                var value = v.Value;
                return new Parameter { Name = name, Description = description, Value = value };
            }).ToList();

            return items;
        }

        public string UpdateXml(List<Parameter> parameters, string xml)
        {
            if (string.IsNullOrEmpty(xml)) return "";
            var xdoc = XDocument.Parse(xml);

            var nodes = xdoc.Root.Nodes();

            var values = xdoc.Descendants("Value");

            foreach (var item in values)
            {
                var name = item.Attribute("name").Value;
                item.Value = parameters.FirstOrDefault(e => e.Name == name).Value;
            }
            return xdoc.ToString();
        }
    }
}
