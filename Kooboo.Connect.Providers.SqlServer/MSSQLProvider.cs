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
using Kooboo;
using Kooboo.Web.Script.Serialization;

namespace Kooboo.Connect.Providers.SqlServer
{
    public class MSSQLProvider : IDataProvider
    {
        static string ObjectKey = "__UserService__MSSQLProvier";

        public string ConnectionString { get; private set; }
        public MSSQLProvider()
        {

        }
        public MSSQLProvider(string connectionString)
        {
            ConnectionString = connectionString;
        }
        ProviderContext GetDataContext()
        {
            var dataContext = CallContext.Current.GetObject<ProviderContext>(ObjectKey);

            if (dataContext == null)
            {
                if (string.IsNullOrEmpty(ConnectionString))
                {
                    dataContext = new ProviderContext();
                }
                else
                {
                    dataContext = new ProviderContext(ConnectionString);
                }
                CallContext.Current.RegisterObject<ProviderContext>(ObjectKey, dataContext);
            }
            return dataContext;
        }

        #region IDataProvider Members

        public IQueryable<Kooboo.Connect.User> LoadUsers()
        {
            return this.GetDataContext().Users
                .Select(i => new Kooboo.Connect.User
                {
                    Name = i.Name,
                    Email = i.Email,
                    Membership = new Kooboo.Connect.Membership
                    {
                        Comment = i.Comment,
                        CreateDate = i.CreateDate,
                        FailedPasswordAnswerAttemptCount = i.FailedPasswordAnswerAttemptCount,
                        FailedPasswordAnswerAttemptWindowStart = i.FailedPasswordAnswerAttemptWindowStart,
                        FailedPasswordAttemptCount = i.FailedPasswordAttemptCount,
                        FailedPasswordAttemptWindowStart = i.FailedPasswordAttemptWindowStart,
                        IsApproved = i.IsApproved,
                        IsLockedOut = i.IsLockedOut,
                        LastLockoutDate = i.LastLockoutDate,
                        LastLoginDate = i.LastLoginDate,
                        LastPasswordChangedDate = i.LastPasswordChangedDate,
                        Password = i.Password,
                        PasswordAnswer = i.PasswordAnswer,
                        PasswordQuestion = i.PasswordQuestion,
                        PasswordSalt = i.PasswordSalt
                    },
                    Profile = new Profile
                    {
                        Address = i.Address,
                        Birthday = i.Birthday,
                        City = i.City,
                        Country = i.Country,
                        FirstName = i.FirstName,
                        Gender = i.Gender,
                        LastName = i.LastName,
                        MiddleName = i.MiddleName,
                        Mobile = i.Mobile,
                        Postcode = i.Postcode,
                        Telphone = i.Telphone
                    }
                });
        }

        public Kooboo.Connect.User LoadUser(string name)
        {
            return this.GetDataContext().Users
                .Where(i => i.Name == name)
                .Select(i => new Kooboo.Connect.User
                {
                    Name = i.Name,
                    //Customer = this.LoadCustomer(i.CustomerId),
                    Email = i.Email,
                    Membership = new Kooboo.Connect.Membership
                    {
                        Comment = i.Comment,
                        CreateDate = i.CreateDate,
                        FailedPasswordAnswerAttemptCount = i.FailedPasswordAnswerAttemptCount,
                        FailedPasswordAnswerAttemptWindowStart = i.FailedPasswordAnswerAttemptWindowStart,
                        FailedPasswordAttemptCount = i.FailedPasswordAttemptCount,
                        FailedPasswordAttemptWindowStart = i.FailedPasswordAttemptWindowStart,
                        IsApproved = i.IsApproved,
                        IsLockedOut = i.IsLockedOut,
                        LastLockoutDate = i.LastLockoutDate,
                        LastLoginDate = i.LastLoginDate,
                        LastPasswordChangedDate = i.LastPasswordChangedDate,
                        Password = i.Password,
                        PasswordAnswer = i.PasswordAnswer,
                        PasswordQuestion = i.PasswordQuestion,
                        PasswordSalt = i.PasswordSalt
                    },
                    Profile = new Profile
                    {
                        Address = i.Address,
                        Birthday = i.Birthday,
                        City = i.City,
                        Country = i.Country,
                        FirstName = i.FirstName,
                        Gender = i.Gender,
                        LastName = i.LastName,
                        MiddleName = i.MiddleName,
                        Mobile = i.Mobile,
                        Postcode = i.Postcode,
                        Telphone = i.Telphone
                    }

                })
                .FirstOrDefault();
        }

        public Kooboo.Connect.User LoadUserByMail(string mail)
        {
            return this.GetDataContext().Users
                .Where(i => i.Email == mail)
                .Select(i => new Kooboo.Connect.User
                {
                    Name = i.Name,
                    Email = i.Email,
                    Membership = new Kooboo.Connect.Membership
                    {
                        Comment = i.Comment,
                        CreateDate = i.CreateDate,
                        FailedPasswordAnswerAttemptCount = i.FailedPasswordAnswerAttemptCount,
                        FailedPasswordAnswerAttemptWindowStart = i.FailedPasswordAnswerAttemptWindowStart,
                        FailedPasswordAttemptCount = i.FailedPasswordAttemptCount,
                        FailedPasswordAttemptWindowStart = i.FailedPasswordAttemptWindowStart,
                        IsApproved = i.IsApproved,
                        IsLockedOut = i.IsLockedOut,
                        LastLockoutDate = i.LastLockoutDate,
                        LastLoginDate = i.LastLoginDate,
                        LastPasswordChangedDate = i.LastPasswordChangedDate,
                        Password = i.Password,
                        PasswordAnswer = i.PasswordAnswer,
                        PasswordQuestion = i.PasswordQuestion,
                        PasswordSalt = i.PasswordSalt
                    },
                    Profile = new Profile
                    {
                        Address = i.Address,
                        Birthday = i.Birthday,
                        City = i.City,
                        Country = i.Country,
                        FirstName = i.FirstName,
                        Gender = i.Gender,
                        LastName = i.LastName,
                        MiddleName = i.MiddleName,
                        Mobile = i.Mobile,
                        Postcode = i.Postcode,
                        Telphone = i.Telphone
                    }

                })
                .FirstOrDefault();
        }

        public bool Save(Kooboo.Connect.User account)
        {
            var find = this.GetDataContext().Users
             .Where(i => i.Name == account.Name)
             .FirstOrDefault();

            if (find == null)
            {
                find = new User();
                this.GetDataContext().Users.InsertOnSubmit(find);
            }
            find.Name = account.Name;
            find.Email = account.Email;

            find.PasswordSalt = account.Membership.PasswordSalt;
            find.Password = account.Membership.Password;

            find.Comment = account.Membership.Comment;
            find.CreateDate = account.Membership.CreateDate;
            find.FailedPasswordAnswerAttemptCount = account.Membership.FailedPasswordAnswerAttemptCount;
            find.FailedPasswordAnswerAttemptWindowStart = account.Membership.FailedPasswordAnswerAttemptWindowStart;
            find.FailedPasswordAttemptCount = account.Membership.FailedPasswordAttemptCount;
            find.FailedPasswordAttemptWindowStart = account.Membership.FailedPasswordAttemptWindowStart;
            find.IsApproved = account.Membership.IsApproved;
            find.IsLockedOut = account.Membership.IsLockedOut;
            find.LastLockoutDate = account.Membership.LastLockoutDate;
            find.LastLoginDate = account.Membership.LastLoginDate;
            find.LastPasswordChangedDate = account.Membership.LastPasswordChangedDate;

            find.PasswordAnswer = account.Membership.PasswordAnswer;
            find.PasswordQuestion = account.Membership.PasswordQuestion;

            if (account.Profile!=null)
            {
                find.Address = account.Profile.Address;
                find.Birthday = account.Profile.Birthday;
                find.City = account.Profile.City;
                find.Country = account.Profile.Country;
                find.FirstName = account.Profile.FirstName;
                find.Gender = account.Profile.Gender;
                find.LastName = account.Profile.LastName;
                find.MiddleName = account.Profile.MiddleName;
                find.Mobile = account.Profile.Mobile;
                find.Postcode = account.Profile.Postcode;
                find.Telphone = account.Profile.Telphone;
            }
          


            this.GetDataContext().SubmitChanges();

            return true;
        }


        #endregion


        public bool DeleteUser(Connect.User account)
        {
            throw new NotImplementedException();
        }
    }
}
