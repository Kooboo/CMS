#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;

namespace Kooboo.Globalization.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlElementRepository : IElementRepository, IDisposable
    {
        #region Fields
        private string path;

        ResXResource resx;
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlElementRepository" /> class.
        /// </summary>
        public XmlElementRepository()
            : this(Path.Combine(Settings.BaseDirectory, "I18N"))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlElementRepository" /> class.
        /// </summary>
        /// <param name="path">The path.</param>
        public XmlElementRepository(string path)
        {
            this.path = path;

            resx = new ResXResource(path);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enableds the languages.
        /// </summary>
        /// <returns></returns>
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return resx.GetCultures().AsQueryable();
        }

        /// <summary>
        /// Elementses this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Element> Elements()
        {
            return resx.GetElements();
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public Element Get(string name, string category, string culture)
        {
            return resx.GetElement(name, category, culture);

        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Updates the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool Update(Element element)
        {
            if (element == null)
            {
                return false;
            }

            return resx.UpdateResource(element);

        }

        /// <summary>
        /// Removes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool Remove(Element element)
        {
            if (element == null)
            {
                return false;
            }

            return resx.RemoveResource(element);
        }

        /// <summary>
        /// Removes the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public bool RemoveCategory(string category, string culture)
        {
            return resx.RemoveCategory(category, culture);
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        public void AddCategory(string category, string culture)
        {
            resx.AddCategory(category, culture);
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear()
        {
            if (Directory.Exists(path))
            {
                try
                {
                    foreach (var file in Directory.EnumerateFiles(path, "*.resx", SearchOption.AllDirectories))
                    {
                        File.Delete(file);
                    }
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }

            }
        }

        #region IElementRepository Members


        /// <summary>
        /// Categorieses this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<ElementCategory> Categories()
        {
            return resx.GetCategories().AsQueryable();
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        #endregion

        #endregion

    }
}
