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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;

namespace Kooboo.Globalization.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public static class ResXResourceFileHelper
    {
        #region Methods
        /// <summary>
        /// Parses the specified file name.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="culture">The culture.</param>
        /// <param name="category">The category.</param>
        public static void Parse(string fileName, out string culture, out string category)
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetFileName(fileName));
            if (!fileNameWithoutExtension.Contains('.'))
            {
                culture = fileNameWithoutExtension;
                category = string.Empty;
            }
            else
            {
                var pointIndex = fileNameWithoutExtension.LastIndexOf('.');
                culture = fileNameWithoutExtension.Substring(pointIndex + 1);
                category = fileNameWithoutExtension.Substring(0, pointIndex);
            }
        }
        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string GetFileName(string category, string culture)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return culture + ".resx";
            }
            else
            {
                return category + "." + culture + ".resx";
            }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class ResXResource
    {
        #region Fields
        private static System.Threading.ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

        private string path;
        #endregion

        #region .ctor
        public ResXResource(string path)
        {
            this.path = path;
        }
        #endregion

        #region Methods

        private void CreateDir()
        {
            if (Directory.Exists(path) == false)
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (IOException exception)
                {
                    throw new DirectoryCreateException(exception);
                }
            }
        }

        /// <summary>
        /// Removes the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public bool RemoveCategory(string category, string culture)
        {
            locker.EnterWriteLock();
            try
            {
                string[] files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    string culture1;
                    string category1;
                    ResXResourceFileHelper.Parse(file, out culture1, out category1);
                    if (category.EqualsOrNullEmpty(category1, StringComparison.CurrentCultureIgnoreCase) && culture1.EqualsOrNullEmpty(culture, StringComparison.CurrentCultureIgnoreCase))
                    {
                        File.Delete(file);
                    }
                }
                return true;
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        public void AddCategory(string category, string culture)
        {
            string filePath = Path.Combine(this.path, ResXResourceFileHelper.GetFileName(category, culture));

            XDocument document = GetResxDocument(filePath);

            if (document == null)
            {
                document = CreateResXDocument();
            }
            locker.EnterWriteLock();
            try
            {
                document.Save(filePath);
            }
            finally
            {
                locker.ExitWriteLock();
            }
        }
        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ElementCategory> GetCategories()
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    string culture;
                    string category;
                    ResXResourceFileHelper.Parse(file, out culture, out category);

                    if (!string.IsNullOrEmpty(category))
                    {
                        yield return new ElementCategory() { Category = category, Culture = culture };
                    }

                }
            }
        }
        /// <summary>
        /// Gets the cultures.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CultureInfo> GetCultures()
        {
            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (var file in files)
                {
                    string culture;
                    string category;
                    ResXResourceFileHelper.Parse(file, out culture, out category);
                    if (!string.IsNullOrEmpty(culture))
                    {
                        yield return new CultureInfo(culture);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the elements.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Element> GetElements()
        {
            locker.EnterReadLock();
            try
            {
                IEnumerable<Element> elements = Enumerable.Empty<Element>();
                if (Directory.Exists(path))
                {
                    string[] files = Directory.GetFiles(path);

                    for (int i = 0; i < files.Length; i++)
                    {
                        string fileName = files[i];
                        if (fileName.EndsWith(".resx"))
                        {
                            XDocument doc = XDocument.Load(files[i]);
                            string culture;
                            string category;
                            ResXResourceFileHelper.Parse(fileName, out culture, out category);

                            IEnumerable<Element> newElements = doc.Root.Elements("data")
                                .Select(x => new Element()
                                {
                                    Category = category,
                                    Culture = culture,
                                    Name = x.Attribute("name").Value,
                                    Value = x.Element("value").Value
                                });

                            elements = elements.Union(newElements);
                        }
                    }
                }
                return elements.AsQueryable<Element>();
            }
            finally
            {
                locker.ExitReadLock();
            }
        }

        /// <summary>
        /// Gets the element.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public Element GetElement(string name, string category, string culture)
        {
            locker.EnterReadLock();
            try
            {
                string filePath = Path.Combine(this.path, ResXResourceFileHelper.GetFileName(category, culture));

                XDocument document = GetResxDocument(filePath);

                if (document == null)
                {
                    return null;
                }

                return document.Root.Elements("data")
                            .Where(it => it.Attribute("name").Value.EqualsOrNullEmpty(name, StringComparison.OrdinalIgnoreCase))
                            .Select(it => new Element()
                            {
                                Name = it.Attribute("name").Value,
                                Value = it.Element("value").Value,
                                Category = category,
                                Culture = culture
                            }).FirstOrDefault();
            }
            finally
            {
                locker.ExitReadLock();
            }

        }

        /// <summary>
        /// Adds the resource.
        /// </summary>
        /// <param name="element">The element.</param>
        public void AddResource(Element element)
        {
            locker.EnterWriteLock();
            try
            {
                CreateDir();
                string filePath = Path.Combine(this.path, ResXResourceFileHelper.GetFileName(element.Category, element.Culture));
                XDocument document = GetResxDocument(filePath);

                if (document == null)
                {
                    document = CreateResXDocument();
                }
                var exists = document.Root.Elements("data")
                   .FirstOrDefault(d => d.Attribute("name").Value == element.Name);
                if (exists == null)
                {
                    document.Root.Add(
                    new XElement("data",
                        new XAttribute("name", element.Name),
                        new XAttribute(XNamespace.Xml + "space", "preserve"),
                        new XElement("value", element.Value)));
                    document.Save(filePath);
                }
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        /// <summary>
        /// Updates the resource.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool UpdateResource(Element element)
        {
            locker.EnterWriteLock();
            try
            {
                CreateDir();
                string filePath = Path.Combine(this.path, ResXResourceFileHelper.GetFileName(element.Category, element.Culture));

                XDocument document = GetResxDocument(filePath);

                if (document == null)
                {
                    return false;
                }

                var newElement = document.Root.Elements("data")
                    .FirstOrDefault(d => d.Attribute("name").Value.EqualsOrNullEmpty(element.Name, StringComparison.OrdinalIgnoreCase));

                if (newElement == null)
                {
                    return false;
                }

                newElement.Element("value").Value = element.Value;
                document.Save(filePath);

                return true;
            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        /// <summary>
        /// Removes the resource.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool RemoveResource(Element element)
        {
            locker.EnterWriteLock();
            try
            {
                CreateDir();
                string filePath = Path.Combine(this.path, ResXResourceFileHelper.GetFileName(element.Category, element.Culture));

                XDocument document = GetResxDocument(filePath);

                if (document == null)
                {
                    return false;
                }

                var newElement = document.Root.Elements("data")
                    .FirstOrDefault(d => d.Attribute("name").Value.EqualsOrNullEmpty(element.Name, StringComparison.OrdinalIgnoreCase));

                if (newElement == null)
                {
                    return false;
                }

                newElement.Remove();
                document.Save(filePath);

                return true;

            }
            finally
            {
                locker.ExitWriteLock();
            }

        }

        private XDocument GetResxDocument(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return null;
            }

            return XDocument.Load(filePath);
        }

        private XDocument CreateResXDocument()
        {
            string resheader = @"<root>
                                    <xsd:schema id=""root"" xmlns="""" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:msdata=""urn:schemas-microsoft-com:xml-msdata"">
                                    <xsd:import namespace=""http://www.w3.org/XML/1998/namespace"" />
                                    <xsd:element name=""root"" msdata:IsDataSet=""true"">
                                      <xsd:complexType>
                                        <xsd:choice maxOccurs=""unbounded"">
                                          <xsd:element name=""metadata"">
                                            <xsd:complexType>
                                              <xsd:sequence>
                                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" />
                                              </xsd:sequence>
                                              <xsd:attribute name=""name"" use=""required"" type=""xsd:string"" />
                                              <xsd:attribute name=""type"" type=""xsd:string"" />
                                              <xsd:attribute name=""mimetype"" type=""xsd:string"" />
                                              <xsd:attribute ref=""xml:space"" />
                                            </xsd:complexType>
                                          </xsd:element>
                                          <xsd:element name=""assembly"">
                                            <xsd:complexType>
                                              <xsd:attribute name=""alias"" type=""xsd:string"" />
                                              <xsd:attribute name=""name"" type=""xsd:string"" />
                                            </xsd:complexType>
                                          </xsd:element>
                                          <xsd:element name=""data"">
                                            <xsd:complexType>
                                              <xsd:sequence>
                                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                                                <xsd:element name=""comment"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""2"" />
                                              </xsd:sequence>
                                              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" msdata:Ordinal=""1"" />
                                              <xsd:attribute name=""type"" type=""xsd:string"" msdata:Ordinal=""3"" />
                                              <xsd:attribute name=""mimetype"" type=""xsd:string"" msdata:Ordinal=""4"" />
                                              <xsd:attribute ref=""xml:space"" />
                                            </xsd:complexType>
                                          </xsd:element>
                                          <xsd:element name=""resheader"">
                                            <xsd:complexType>
                                              <xsd:sequence>
                                                <xsd:element name=""value"" type=""xsd:string"" minOccurs=""0"" msdata:Ordinal=""1"" />
                                              </xsd:sequence>
                                              <xsd:attribute name=""name"" type=""xsd:string"" use=""required"" />
                                            </xsd:complexType>
                                          </xsd:element>
                                        </xsd:choice>
                                      </xsd:complexType>
                                    </xsd:element>
                                  </xsd:schema>
                                  <resheader name=""resmimetype"">
                                    <value>text/microsoft-resx</value>
                                  </resheader>
                                  <resheader name=""version"">
                                    <value>2.0</value>
                                  </resheader>
                                  <resheader name=""reader"">
                                    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
                                  </resheader>
                                  <resheader name=""writer"">
                                    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
                                  </resheader>
                                    </root>";

            XElement root = XElement.Parse(resheader, LoadOptions.PreserveWhitespace);

            XDocument doc = new XDocument(root);

            return doc;
        }

        #endregion
    }
}
