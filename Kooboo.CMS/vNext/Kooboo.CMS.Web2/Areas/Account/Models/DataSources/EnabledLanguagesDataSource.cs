#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web2.Areas.Account.Models.DataSources
{
    public class EnabledLanguagesDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return Kooboo.Common.Globalization.ElementRepository.DefaultRepository.EnabledLanguages().Select(it => new SelectListItem()
            {
                Text = it.NativeName,
                Value = it.Name,
                Selected = it.Name == System.Globalization.CultureInfo.CurrentUICulture.Name
            });
        }
    }
}