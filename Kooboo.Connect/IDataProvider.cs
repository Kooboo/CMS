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

namespace Kooboo.Connect
{
    public interface IDataProvider
    {
        IQueryable<User> LoadUsers();

        User LoadUser(string name);

        User LoadUserByMail(string mail);

        //Customer LoadCustomer(string id);

        //Customer LoadCustomerByMail(string mail);

        //bool Save(Customer customer);

        bool Save(User account);

        //bool Save(IActivity activity);

        //T LoadActivity<T>(string id) where T : IActivity, new();

        bool DeleteUser(User account);
    }
}
