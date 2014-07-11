using Kooboo.CMS.Content.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Query;
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Persistence;
namespace Kooboo.CMS.Content.Formatter
{
    [Obsolete]
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(ITextContentFormater), Key = "xliff")]
    public class TextContentToXLIFF : ITextContentFormater
    {
        #region .ctor
        ITextContentProvider _textContentProvider;
        TextContentManager _textContentManager;
        public TextContentToXLIFF(ITextContentProvider textContentProvider, TextContentManager textContentManager)
        {
            _textContentProvider = textContentProvider;
            _textContentManager = textContentManager;
        }
        #endregion

        #region Properties
        public string Name
        {
            get { return "XLIFF"; }
        }
        public string DisplayName
        {
            get
            {
                return "XLIFF(.XLF)";
            }
        }
        public string FileExtension
        {
            get { return ".xlf"; }
        }
        #endregion

        #region Export

        public void Export(IEnumerable<TextContent> textContents, System.IO.Stream outputStream)
        {
            var fileElements = textContents.Select(it => ToXElement(it));
            var xdoc = new XDocument(
             new XDeclaration("1.0", "utf-8", null),
             new XElement("xliff", new object[] { new XAttribute("version", "1.2") }.Concat(fileElements).ToArray())
             );
            xdoc.Save(outputStream);
        }
        private static XElement ToXElement(TextContent textContent)
        {
            var xelement = new XElement("file", new XAttribute("original", textContent.UUID), new XAttribute("source-language", textContent.Repository), new XAttribute("target-language", ""));
            var bodyElement = new XElement("body");
            xelement.Add(bodyElement);
            var schema = textContent.GetSchema().AsActual();
            if (schema != null)
            {

                foreach (var column in schema.Columns)
                {
                    if (column.DataType == DataType.String)
                    {
                        var value = textContent[column.Name];

                        bodyElement.Add(new XElement("trans-unit", new XAttribute("id", column.Name), new XAttribute("datatype", value == null ? "null" : value.GetType().ToString()),
                            new XElement("source", value), new XElement("target", value)));
                    }
                }
            }

            return xelement;
        }
        #endregion

        #region Import
        public void Import(TextFolder textFolder, System.IO.Stream inputStream)
        {
            textFolder = textFolder.AsActual();
            if (textFolder != null)
            {
                XDocument xDoc = XDocument.Load(inputStream);
                var xiffNode = xDoc.Element("xliff");
                if (xiffNode != null)
                {
                    var fileNodes = xiffNode.Elements();
                    var textContents = fileNodes.Select(it => ToTextContent(it)).Where(it => it != null);
                    foreach (var item in textContents)
                    {
                        InsertOrUpdate(textFolder, item);
                    }
                }
            }
        }
        private void InsertOrUpdate(TextFolder textFolder, TextContent textContent)
        {
            var oldContent = textFolder.CreateQuery().WhereEquals("UUID", textContent.UUID).FirstOrDefault();
            textContent.Repository = textFolder.Repository.Name;
            textContent.FolderName = textFolder.FullName;
            textContent.SchemaName = textFolder.SchemaName;
            if (oldContent != null)
            {
                foreach (var key in textContent.Keys)
                {
                    oldContent[key] = textContent[key];
                }

                _textContentProvider.Update(oldContent, oldContent);
            }
            else
            {
                var nameValues = textContent.ToNameValueCollection();
                _textContentManager.Add(textFolder.Repository, textFolder, nameValues, null, null, System.Web.HttpContext.Current.User.Identity.Name);
            }

        }
        private TextContent ToTextContent(XElement element)
        {
            TextContent textContent = null;
            var uuidAttr = element.Attribute("original");
            if (uuidAttr != null)
            {
                var uuid = uuidAttr.Value;
                if (!string.IsNullOrEmpty(uuid))
                {
                    var bodyNode = element.Element("body");

                    if (bodyNode != null)
                    {
                        textContent = new TextContent();
                        textContent.UUID = uuid;
                        var trans_unitNodes = bodyNode.Elements();
                        foreach (var item in trans_unitNodes)
                        {
                            string fieldName;
                            var fieldValue = ConvertToField(item, out fieldName);
                            if (!string.IsNullOrEmpty(fieldName))
                            {
                                textContent[fieldName] = fieldValue;
                            }
                        }
                    }
                }
            }

            return textContent;
        }
        private object ConvertToField(XElement trans_unitNode, out string fieldName)
        {
            object fieldValue = null;
            fieldName = null;
            var fieldNameAttr = trans_unitNode.Attribute("id");
            if (fieldNameAttr != null)
            {
                fieldName = fieldNameAttr.Value;

                string dataType = null;
                var dataTypeAttr = trans_unitNode.Attribute("datatype");
                if (dataTypeAttr != null)
                {
                    dataType = dataTypeAttr.Value;
                }
                var targetNode = trans_unitNode.Element("target");
                if (targetNode != null)
                {
                    var textValue = targetNode.Value;
                    fieldValue = ConvertDataType(dataType, textValue);
                }
            }
            return fieldValue;
        }
        private object ConvertDataType(string dataType, string rawValue)
        {
            if (!string.IsNullOrEmpty(dataType) && dataType != "null")
            {
                var targetType = Type.GetType(dataType);
                try
                {
                    return Convert.ChangeType(rawValue, targetType);
                }
                catch
                {
                    return rawValue;
                }
            }
            return rawValue;
        }
        #endregion
    }
}
