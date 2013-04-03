using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Connect
{
    public  enum UserCreateStatus
    {
        // Summary:
        //     The user was successfully created.
        Success = 0,
        //
        // Summary:
        //     The user name was not found in the database.
        InvalidUserName = 1,
        //
        // Summary:
        //     The password is not formatted correctly.
        InvalidPassword = 2,
        //
        // Summary:
        //     The password question is not formatted correctly.
        InvalidQuestion = 3,
        //
        // Summary:
        //     The password answer is not formatted correctly.
        InvalidAnswer = 4,
        //
        // Summary:
        //     The e-mail address is not formatted correctly.
        InvalidEmail = 5,
        //
        // Summary:
        //     The user name already exists in the database for the application.
        DuplicateUserName = 6,
        //
        // Summary:
        //     The e-mail address already exists in the database for the application.
        DuplicateEmail = 7,
        //
        // Summary:
        //     The user was not created, for a reason defined by the provider.
        UserRejected = 8,
    
    }
    
}
