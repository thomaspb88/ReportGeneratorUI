using System;
using System.Xml;

namespace XmlNodeExtensionsMethods
{
    public static class XmlNodeExtenionMethods
    {
        public static bool HasAttributes(this XmlNode node)
        {
            return node.Attributes.Count > 0; 
        }

        public static string GetAttributeValue(this XmlNode node, string attributeName)
        {
            return String.IsNullOrWhiteSpace(node.Attributes[attributeName].Value) ? null : node.Attributes[attributeName].Value;
        }
    }
}
