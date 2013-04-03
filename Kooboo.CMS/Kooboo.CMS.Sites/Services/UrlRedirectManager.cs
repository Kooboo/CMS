using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Extensions;

namespace Kooboo.CMS.Sites.Services
{
    public class UrlRedirectManager : ManagerBase<UrlRedirect, IUrlRedirectProvider>
    {


        #region Export & Import

        public virtual void Export(Site site, System.IO.Stream outputStream)
        {
            ((IUrlRedirectProvider)Provider).Export(site, outputStream);
        }

        public virtual void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            ((IUrlRedirectProvider)Provider).Import(site, zipStream, @override);
        }
        #endregion

        public override IEnumerable<UrlRedirect> All(Site site, string filterName)
        {
            var result = Provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(it => it.InputUrl.Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }
            return result;
        }

        public override UrlRedirect Get(Site site, string name)
        {
            return Provider.Get(new UrlRedirect { Site = site, InputUrl = name });
        }

        public override void Update(Site site, UrlRedirect @new, UrlRedirect old)
        {
            @new.Site = site;
            old.Site = site;
            if (Get(site, old.InputUrl.ToString()) == null)
            {
                throw new ItemDoesNotExistException();
            }
            if (!old.Equals(@new))
            {
                // the key has been changed...
                if (Get(site, @new.InputUrl.ToString()) != null)
                {
                    throw new ItemAlreadyExistsException();
                }
            }
            Provider.Update(@new, old);
        }
    }
}
