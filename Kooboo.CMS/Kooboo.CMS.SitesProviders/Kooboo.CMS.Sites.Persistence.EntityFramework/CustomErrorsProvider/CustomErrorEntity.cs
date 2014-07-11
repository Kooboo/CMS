using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Misc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Persistence.EntityFramework.CustomErrorsProvider
{
    public interface ICustomErrorEntity
    {
        string UUID { get; set; }

        string SiteName { get; set; }

        string ObjectXml { get; set; }
    }

    public class CustomErrorEntity : ICustomErrorEntity
    {
        public string UUID { get; set; }

        public string SiteName { get; set; }

        public string ObjectXml { get; set; }
    }

    public static class CustomErrorEntityExtensions
    {
        private static Type[] KnownTypes = new Type[]{
                typeof(CustomError)
                };
        public static T ToCustomErrorEntity<T>(this CustomError model)
            where T : ICustomErrorEntity, new()
        {
            return ToCustomErrorEntity(model, new T());
        }
        public static T ToCustomErrorEntity<T>(this CustomError model, T entity)
            where T : ICustomErrorEntity
        {
            entity.UUID = model.UUID;
            if (string.IsNullOrEmpty(entity.SiteName))
            {
                entity.SiteName = model.Site.FullName;
            }

            entity.ObjectXml = DataContractSerializationHelper.SerializeAsXml(model, KnownTypes);
            return entity;
        }
        public static CustomError ToCustomError(this ICustomErrorEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var dummy = new CustomError();
            dummy.UUID = entity.UUID;
            dummy.Site = new Site(entity.SiteName);

            var customError = DataContractSerializationHelper.DeserializeFromXml<CustomError>(entity.ObjectXml, KnownTypes);
            ((IPersistable)customError).Init(dummy);

            return customError;

        }
    }
}
