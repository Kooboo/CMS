using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence.EntityFramework
{
    [Dependency(typeof(IProvider<>), ComponentLifeStyle.InRequestScope)]
    public class Provider<T> : IProvider<T>
        where T : class, IEntity
    {
        BlogDbContext _blogContext;
        public Provider(BlogDbContext blogContext)
        {
            this._blogContext = blogContext;
        }
        public IQueryable<T> CreateQuery()
        {
            return _blogContext.Set<T>();
        }

        public T QueryById(int id)
        {
            return _blogContext.Set<T>().Where(it => it.Id == id).FirstOrDefault();
        }

        public void Add(T obj)
        {
            _blogContext.Set<T>().Add(obj);
            _blogContext.SaveChanges();
        }

        public void Update(T obj)
        {
            _blogContext.SaveChanges();
        }

        public void Delete(T obj)
        {
            var o = QueryById(obj.Id);
            _blogContext.Set<T>().Remove(o);
            _blogContext.SaveChanges();
        }
    }
}
