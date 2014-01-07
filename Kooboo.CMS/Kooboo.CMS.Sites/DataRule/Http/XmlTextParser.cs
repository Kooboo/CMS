#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Kooboo.CMS.Sites.DataRule.Http
{
    public class DynamicXml : DynamicObject
    {
        XElement _root;
        private DynamicXml(XElement root)
        {
            _root = root;
        }

        public static DynamicXml Parse(string xmlString)
        {
            return new DynamicXml(XDocument.Parse(xmlString).Root);
        }

        public static DynamicXml Load(string filename)
        {
            return new DynamicXml(XDocument.Load(filename).Root);
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            var att = _root.Attribute(binder.Name);
            if (att != null)
            {
                result = att.Value;
                return true;
            }

            var nodes = _root.Elements(binder.Name);
            if (nodes.Count() > 1)
            {
                result = nodes.Select(n => new DynamicXml(n)).ToList();
                return true;
            }

            var node = _root.Element(binder.Name);
            if (node != null)
            {
                if (node.HasElements)
                {
                    result = new DynamicXml(node);
                }
                else
                {
                    result = node.Value;
                }
                return true;
            }

            return true;
        }
    }

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IResponseTextParser), Key = "XmlTextParser")]
    public class XmlTextParser : IResponseTextParser
    {
        public bool Accept(string responseText, string contentType)
        {
            contentType = (contentType ?? "").ToLower();
            return (contentType.Contains("application/xml")) || (contentType.Contains("text/xml"));
        }

        public dynamic Parse(string responseText)
        {
            return DynamicXml.Parse(responseText);
        }
    }
}
