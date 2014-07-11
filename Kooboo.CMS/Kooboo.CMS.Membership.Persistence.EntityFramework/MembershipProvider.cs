#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.ObjectContainer.Dependency;
using Kooboo.CMS.Membership.Models;
using Kooboo.CMS.Membership.Persistence;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Membership.Persistence.EntityFramework
{
    [Dependency(typeof(IMembershipProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<Kooboo.CMS.Membership.Models.Membership>), ComponentLifeStyle.InRequestScope, Order = 100)]
    public class MembershipProvider : IMembershipProvider
    {
        #region .ctor
        MemberDBContext _dbContext = null;
        public MembershipProvider(MemberDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Import
        public Kooboo.CMS.Membership.Models.Membership Import(string membershipName, System.IO.Stream stream)
        {
            var fileSystemProvider = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<Kooboo.CMS.Membership.Persistence.Default.MembershipProvider>();
            var membership = fileSystemProvider.Import(membershipName, stream);

            var efProvider = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<MembershipProvider>();
            efProvider.Add(membership);

            //Transfer groups
            TransferData<MembershipGroup, Kooboo.CMS.Membership.Persistence.Default.MembershipGroupProvider, MembershipGroupProvider>(membership);
            //Transfer membership users
            TransferData<MembershipUser, Kooboo.CMS.Membership.Persistence.Default.MembershipUserProvider, MembershipUserProvider>(membership);

            TransferData<MembershipConnect, Kooboo.CMS.Membership.Persistence.Default.MembershipConnectProvider, MembershipConnectProvider>(membership);

            fileSystemProvider.Remove(membership);

            return membership;
        }
        #endregion

        #region Export
        public void Export(Models.Membership membership, System.IO.Stream outputStream)
        {
            var fileSystemProvider = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<Kooboo.CMS.Membership.Persistence.Default.MembershipProvider>();
            fileSystemProvider.Add(membership);

            //Transfer groups
            TransferData<MembershipGroup, MembershipGroupProvider, Kooboo.CMS.Membership.Persistence.Default.MembershipGroupProvider>(membership);
            //Transfer membership users
            TransferData<MembershipUser, MembershipUserProvider, Kooboo.CMS.Membership.Persistence.Default.MembershipUserProvider>(membership);

            TransferData<MembershipConnect, MembershipConnectProvider, Kooboo.CMS.Membership.Persistence.Default.MembershipConnectProvider>(membership);

            fileSystemProvider.Export(membership, outputStream);

            fileSystemProvider.Remove(membership);
        }

        private void TransferData<T, TSource, TTarget>(Kooboo.CMS.Membership.Models.Membership membership)
            where T : IMemberElement, IIdentifiable
            where TSource : class,IMemberElementProvider<T>
            where TTarget : class,IMemberElementProvider<T>
        {
            var efProvider = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<TSource>();
            var fileSystemProvider = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<TTarget>();
            foreach (var item in efProvider.All(membership))
            {
                item.Membership = membership;
                fileSystemProvider.Add(item);
            }
        }
        #endregion

        #region All
        public IEnumerable<Kooboo.CMS.Membership.Models.Membership> All()
        {
            return _dbContext.Set<Kooboo.CMS.Membership.Models.Membership>();
        }
        #endregion

        #region Get
        public Kooboo.CMS.Membership.Models.Membership Get(Kooboo.CMS.Membership.Models.Membership dummy)
        {
            return _dbContext.Set<Kooboo.CMS.Membership.Models.Membership>().Where(it => it.Name == dummy.Name).FirstOrDefault();
        }
        #endregion

        #region Add
        public void Add(Models.Membership item)
        {
            _dbContext.Set<Kooboo.CMS.Membership.Models.Membership>().Add(item);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Update
        public void Update(Kooboo.CMS.Membership.Models.Membership @new, Kooboo.CMS.Membership.Models.Membership old)
        {
            var entity = Get(@new);

            //Is there anyway to update the entity without reset the properties?
            entity.AuthCookieDomain = @new.AuthCookieDomain;
            entity.AuthCookieName = @new.AuthCookieName;
            entity.HashAlgorithmType = @new.HashAlgorithmType;
            entity.MaxInvalidPasswordAttempts = @new.MaxInvalidPasswordAttempts;
            entity.MinRequiredPasswordLength = @new.MinRequiredPasswordLength;
            entity.PasswordStrengthRegularExpression = @new.PasswordStrengthRegularExpression;

            _dbContext.SaveChanges();
        }
        #endregion

        #region Remove
        public void Remove(Kooboo.CMS.Membership.Models.Membership item)
        {
            var entity = Get(item);
            if (entity != null)
            {
                _dbContext.Set<Kooboo.CMS.Membership.Models.Membership>().Remove(entity);
                _dbContext.SaveChanges();
            }

        }
        #endregion
    }
}
