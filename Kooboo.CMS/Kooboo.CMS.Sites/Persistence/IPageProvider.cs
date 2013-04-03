using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Persistence
{
    public interface IPageProvider : IProvider<Page>, ILocalizableProvider<Page>
    {
        IQueryable<Page> ChildPages(Page parentPage);

        ///// <summary>
        ///// Gets the page by URL.
        ///// <example>product/detail/product1</example>
        ///// </summary>
        ///// <param name="site">The site.</param>
        ///// <param name="url">The URL.</param>
        ///// <returns></returns>
        //Page GetPageByUrl(Site site, string url);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="site"></param>
        ///// <param name="identifier"></param>
        ///// <returns></returns>
        //Page GetPageByUrlIdentifier(Site site, string identifier);

        IQueryable<Page> ByLayout(Layout layout);

        IQueryable<Page> ByView(Kooboo.CMS.Sites.Models.View view);

        IQueryable<Page> ByModule(Site site, string moduleName);

        Page Copy(Site site, string sourcePageFullName, string newPageFullName);

        void Move(Site site, string pageFullName, string newParent);

        Page GetDraft(Page page);

        void SaveAsDraft(Page page);

        void RemoveDraft(Page page);

        void Export(IEnumerable<Page> sources, System.IO.Stream outputStream);

        void Import(Site site, Page parent, System.IO.Stream zipStream, bool @override);

        void InitializePages(Site site);

        void ExportPagesToDisk(Site site);

        void Clear(Site site);
    }
}
