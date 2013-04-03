using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel.Composition;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;
using System.Security.Permissions;

namespace Kooboo.Globalization.Repository
{
    [PartCreationPolicy(System.ComponentModel.Composition.CreationPolicy.Shared)]
    [Export(typeof(XmlElementRepository))]
    public class XmlElementRepository : IElementRepository, IDisposable
    {
        private string path;

        ResXResource resx;

        public XmlElementRepository()
            : this(Path.Combine(Settings.BaseDirectory, "I18N"))
        {

        }

        public XmlElementRepository(string path)
        {
            this.path = path;
            //if (Directory.Exists(path) == false)
            //{
            //    try
            //    {
            //        Directory.CreateDirectory(path);
            //    }
            //    catch (IOException exception)
            //    {
            //        throw new DirectoryCreateException(exception);
            //    }
            //}

            resx = new ResXResource(path);
        }

        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return resx.GetCultures().AsQueryable();
        }

        public IQueryable<Element> Elements()
        {
            return resx.GetElements();
        }

        public Element Get(string name, string category, string culture)
        {
            return resx.GetElement(name, category, culture);

        }

        public bool Add(Element element)
        {
            if (element == null)
            {
                return false;
            }

            if (resx.GetElement(element.Name, element.Category, element.Culture) == null)
            {
                resx.AddResource(element);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool Update(Element element)
        {
            if (element == null)
            {
                return false;
            }

            return resx.UpdateResource(element);

        }

        public bool Remove(Element element)
        {
            if (element == null)
            {
                return false;
            }

            return resx.RemoveResource(element);
        }

        #region IElementRepository Members


        public IQueryable<ElementCategory> Categories()
        {
            return resx.GetCategories().AsQueryable();
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {

        }

        #endregion

        public bool RemoveCategory(string category, string culture)
        {
            return resx.RemoveCategory(category, culture);
        }

        public void AddCategory(string category, string culture)
        {
            resx.AddCategory(category, culture);
        }

        public void Clear()
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}
