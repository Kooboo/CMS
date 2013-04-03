using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.Extensions;

namespace Kooboo.CMS.Sites.Services
{
    public class CustomErrorManager : ManagerBase<CustomError, ICustomErrorProvider>
    {
        #region IManager<CustomError> Members

        public void Export(Site site, System.IO.Stream outputStream)
        {
            ((ICustomErrorProvider)Provider).Export(site, outputStream);
        }

        public void Import(Site site, System.IO.Stream zipStream, bool @override)
        {
            ((ICustomErrorProvider)Provider).Import(site, zipStream, @override);
        }
        #endregion

        public override IEnumerable<CustomError> All(Site site, string filterName)
        {
            var result = Provider.All(site);
            if (!string.IsNullOrEmpty(filterName))
            {
                result = result.Where(it => it.StatusCode.ToString().Contains(filterName, StringComparison.CurrentCultureIgnoreCase));
            }
            return result;
        }

        public override CustomError Get(Site site, string name)
        {
            return Provider.Get(new CustomError() { Site = site, StatusCode = (HttpErrorStatusCode)Enum.Parse(typeof(HttpErrorStatusCode), name, true) });
        }

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
    }
}
