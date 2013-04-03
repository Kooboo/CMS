using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Account.Models;

namespace Kooboo.CMS.Account.Persistence
{
    public interface IUserRepository : IRepository<User>
    {
        bool ValidateUser(string userName, string password);
        bool ChangePassword(string userName, string oldPassword, string newPassword);
        bool ChangePassword(string userName, string newPassword);
    }
}
