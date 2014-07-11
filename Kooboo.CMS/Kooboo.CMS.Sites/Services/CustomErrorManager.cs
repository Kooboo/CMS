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
using System.Linq;
using System.Text;

using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;


namespace Kooboo.CMS.Sites.Services
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(CustomErrorManager))]
    public class CustomErrorManager : ManagerBase<CustomError, ICustomErrorProvider>
    {
        #region .ctor
        public CustomErrorManager(ICustomErrorProvider provider)
            : base(provider) { }
        #endregion

        #region Export/Import

        public void Export(Site site, IEnumerable<CustomError> customErrors, System.IO.Stream outputStream)
        {
            ((ICustomErrorProvider)Provider).Export(site, customErrors, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            ((ICustomErrorProvider)Provider).Import(site, zipStream, @override);
        }
        #endregion

        #region All
        public override IEnumerable<CustomError> All(Site site, string filterName)
        {
            var result = Provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(it => it.StatusCode.ToString().Contains(filterName, StringComparison.OrdinalIgnoreCase) || it.RedirectUrl.Contains(filterName, StringComparison.OrdinalIgnoreCase));
            }
            return result;
        }
        #endregion

        #region Get
        public override CustomError Get(Site site, string name)
        {
            return Provider.Get(new CustomError() { Site = site, StatusCode = (HttpErrorStatusCode)Enum.Parse(typeof(HttpErrorStatusCode), name, true) });
        }
        #endregion

        #region Update
        public override void Update(Site site, CustomError @new, CustomError old)
        {
            @new.Site = site;
            old.Site = site;
            if (Get(site, old.StatusCode.ToString()) == null)
            {
                throw new ItemDoesNotExistException();
            }
            if (!old.Equals(@new))
            {
                // the key has changed...
                if (Get(site, @new.StatusCode.ToString()) != null)
                {
                    throw new ItemAlreadyExistsException();
                }
            }
            Provider.Update(@new, old);
        }
        #endregion
    }
}
