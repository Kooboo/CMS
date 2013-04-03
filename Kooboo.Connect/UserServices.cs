using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using Kooboo.Connect.Providers;
//using Kooboo.IoC;

namespace Kooboo.Connect
{
    public static class UserServices
    {
        public static IDataProvider DefaultProvider = new FileProvider();// ObjectContainer.CreateInstance<IDataProvider>(); 

        public static IQueryable<User> FindUsers()
        {
            return DefaultProvider.LoadUsers();
        }

        public static User FindUser(string userName)
        {
            return DefaultProvider.LoadUser(userName);
        }

        public static User ValidateUser(string userName, string password)
        {
            //todo: increase count,locking user 
            var user = DefaultProvider.LoadUser(userName);

            if (user == null)
            {
                return null;
            }

            var encryptedPassword = EncryptProvider.EncryptPassword(password, user.Membership.PasswordSalt);

            if (user.ValidatePassword(encryptedPassword))
            {
                return user;
            }
            else
            {
                return null;
            }

        }

        public static UserCreateStatus CreateUser(string userName, string password, string email)
        {

            if (DefaultProvider.LoadUser(userName) != null)
            {
                return UserCreateStatus.DuplicateUserName;
            }
            else if (DefaultProvider.LoadUserByMail(email) != null)
            {
                return UserCreateStatus.DuplicateEmail;
            }
            else
            {
                var user = new User { };
                user.Name = userName;
                user.Email = email;

                var salt = EncryptProvider.GenerateSalt();
                var encodedPassword = EncryptProvider.EncryptPassword(password, salt);

                user.Membership.PasswordSalt = salt;
                user.Membership.Password = encodedPassword;

                if (DefaultProvider.Save(user))
                {
                    return UserCreateStatus.Success;
                }
                else
                {
                    return UserCreateStatus.UserRejected;
                }

            }

        }

        public static bool ChangePassword(string userName, string newPassword)
        {
            var user = DefaultProvider.LoadUser(userName);

            var salt = EncryptProvider.GenerateSalt();
            var encodedPassword = EncryptProvider.EncryptPassword(newPassword, salt);
            user.Membership.PasswordSalt = salt;
            user.Membership.Password = encodedPassword;

            return DefaultProvider.Save(user);

        }

        public static bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var user = DefaultProvider.LoadUser(userName);

            oldPassword = EncryptProvider.EncryptPassword(oldPassword, user.Membership.PasswordSalt);
            if (user.ValidatePassword(oldPassword))
            {
                var salt = EncryptProvider.GenerateSalt();
                var encodedPassword = EncryptProvider.EncryptPassword(newPassword, salt);
                user.Membership.PasswordSalt = salt;
                user.Membership.Password = encodedPassword;

                return DefaultProvider.Save(user);
            }
            else
            {
                return false;
            }
        }
      
        public static bool Save(User user)
        {
            return DefaultProvider.Save(user);
        }
              

        public static bool Delete(User user)
        {
            return DefaultProvider.DeleteUser(user);
        }

    }
}
