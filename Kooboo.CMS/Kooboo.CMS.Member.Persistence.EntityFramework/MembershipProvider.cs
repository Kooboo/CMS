#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common.Runtime.Dependency;
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Member.Persistence.EntityFramework
{
    [Dependency(typeof(IMembershipProvider), ComponentLifeStyle.InRequestScope, Order = 100)]
    [Dependency(typeof(IProvider<Membership>), ComponentLifeStyle.InRequestScope, Order = 100)]
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
        public Models.Membership Import(string membershipName, System.IO.Stream stream)
        {
            var fileSystemProvider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Member.Persistence.Default.MembershipProvider>();
            var membership = fileSystemProvider.Import(membershipName, stream);

            var efProvider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<MembershipProvider>();
            efProvider.Add(membership);

            //Transfer groups
            TransferData<MembershipGroup, Kooboo.CMS.Member.Persistence.Default.MembershipGroupProvider, MembershipGroupProvider>(membership);
            //Transfer membership users
            TransferData<MembershipUser, Kooboo.CMS.Member.Persistence.Default.MembershipUserProvider, MembershipUserProvider>(membership);

            TransferData<MembershipConnect, Kooboo.CMS.Member.Persistence.Default.MembershipConnectProvider, MembershipConnectProvider>(membership);

            fileSystemProvider.Remove(membership);

            return membership;
        }
        #endregion

        #region Export
        public void Export(Models.Membership membership, System.IO.Stream outputStream)
        {
            var fileSystemProvider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<Kooboo.CMS.Member.Persistence.Default.MembershipProvider>();
            fileSystemProvider.Add(membership);

            //Transfer groups
            TransferData<MembershipGroup, MembershipGroupProvider, Kooboo.CMS.Member.Persistence.Default.MembershipGroupProvider>(membership);
            //Transfer membership users
            TransferData<MembershipUser, MembershipUserProvider, Kooboo.CMS.Member.Persistence.Default.MembershipUserProvider>(membership);

            TransferData<MembershipConnect, MembershipConnectProvider, Kooboo.CMS.Member.Persistence.Default.MembershipConnectProvider>(membership);

            fileSystemProvider.Export(membership, outputStream);

            fileSystemProvider.Remove(membership);
        }

        private void TransferData<T, TSource, TTarget>(Membership membership)
            where TSource : class,IMemberElementProvider<T>
            where TTarget : class,IMemberElementProvider<T>
        {
            var efProvider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<TSource>();
            var fileSystemProvider = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<TTarget>();
            foreach (var item in efProvider.All(membership))
            {
                fileSystemProvider.Add(item);
            }
        }
        #endregion

        #region All
        public IEnumerable<Membership> All()
        {
            return _dbContext.Memberships;
        }
        #endregion

        #region Get
        public Membership Get(Models.Membership dummy)
        {
            return _dbContext.Memberships.Where(it => it.Name == dummy.Name).FirstOrDefault();
        }
        #endregion

        #region Add
        public void Add(Models.Membership item)
        {
            _dbContext.Memberships.Add(item);
            _dbContext.SaveChanges();
        }
        #endregion

        #region Update
        public void Update(Membership @new, Membership old)
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
        public void Remove(Models.Membership item)
        {
            _dbContext.Memberships.Attach(item);
            _dbContext.Memberships.Remove(item);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}
