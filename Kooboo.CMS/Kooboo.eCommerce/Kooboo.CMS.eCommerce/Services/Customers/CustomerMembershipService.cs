#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.eCommerce.Models.Customers;
using Kooboo.CMS.eCommerce.Persistence.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kooboo.Globalization;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.eCommerce.Services.Customers
{
    public class CustomerMembershipService : ICustomerMembershipService
    {
        #region Properties
        public ICustomerProvider CustomerProvider { get; private set; }
        public ICustomerService CustomerService { get; private set; }
        public IPasswordEncryptor PasswordEncryptor { get; private set; }
        #endregion

        #region .ctor
        public CustomerMembershipService(ICustomerProvider customerProvider
           , ICustomerService customerService, IPasswordEncryptor passwordEncryptor)
        {
            this.CustomerProvider = customerProvider;
            this.CustomerService = customerService;
            this.PasswordEncryptor = passwordEncryptor;
        }
        #endregion

        #region Methods
        #region Validate password
        public MembershipResult ValidateByUserName(string username, string password)
        {
            var customer = CustomerProvider.QueryByUserName(username);
            return Validate(customer, password);
        }
        private MembershipResult Validate(Customer customer, string password)
        {
            MembershipResult result = new MembershipResult();
            if (customer == null)
            {
                result.Errors.Add("Username and/or password are incorrect.".Localize());
            }

            else
            {
                if (customer.Membership.IsLockedOut || !customer.Membership.IsApproved)
                {
                    result.Errors.Add("Account was locked out or not approved.".Localize());
                }
                else
                {
                    var encryptedPwd = PasswordEncryptor.EncryptPassword(password, customer.Membership.PasswordSalt);
                    bool isValid = encryptedPwd == customer.Membership.Password;
                    if (isValid)
                    {
                        customer.Membership.UtcLastLoginDate = DateTime.UtcNow;
                        customer.Membership.FailedPasswordAttemptCount = 0;
                        CustomerService.Update(customer);
                    }
                    else
                    {

                        customer.Membership.FailedPasswordAttemptCount = customer.Membership.FailedPasswordAttemptCount + 1;
                        if (customer.Membership.FailedPasswordAttemptCount >= 5)
                        {
                            customer.Membership.IsLockedOut = true;
                        }

                        result.Errors.Add("Username and/or password are incorrect.".Localize());
                    }
                }

            }
            return result;


        }

        public MembershipResult ValidateByEmail(string email, string password)
        {
            var customer = CustomerProvider.QueryByEmail(email);
            return Validate(customer, password);
        }
        #endregion

        public MembershipResult Register(RegistrationRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            MembershipResult result = new MembershipResult();
            #region Validation
            if (string.IsNullOrEmpty(request.Username))
            {
                result.Errors.Add("Invalid username".Localize());
            }
            if (string.IsNullOrEmpty(request.Email) || !Regex.IsMatch(request.Email, Kooboo.RegexPatterns.EmailAddress))
            {
                result.Errors.Add("Invalid email.".Localize());
            }
            if (CustomerProvider.QueryByUserName(request.Username) != null)
            {
                result.Errors.Add("The username already exists.".Localize());
            }
            if (CustomerProvider.QueryByEmail(request.Email) != null)
            {
                result.Errors.Add("The email already exists.".Localize());
            }
            if (string.IsNullOrEmpty(request.Password))
            {
                result.Errors.Add("Invalid password.".Localize());
            }
            #endregion

            if (result.Success)
            {
                var passwordSalt = PasswordEncryptor.GenerateSalt();
                var encryptedPwd = PasswordEncryptor.EncryptPassword(request.Password, passwordSalt);
                var customer = new Customer()
                {
                    Username = request.Username,
                    Email = request.Email,
                    Membership = new Membership()
                    {
                        Password = encryptedPwd,
                        PasswordSalt = passwordSalt,
                        IsApproved = request.IsApproved,
                        UtcCreationDate = DateTime.UtcNow,
                        UtcLastActivityDate = DateTime.UtcNow
                    }
                };
                CustomerService.Add(customer);
            }

            return result;
        }

        public MembershipResult ChangePassword(ChangePasswordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            MembershipResult result = new MembershipResult();
            var customer = CustomerProvider.QueryByUserName(request.Username);
            #region Validation
            if (customer == null)
            {
                result.Errors.Add("The customer does not exists.".Localize());
            }
            MembershipResult validateResult = null;
            if (string.IsNullOrEmpty(request.OldPassword) || !((validateResult = Validate(customer, request.OldPassword)).Success))
            {
                if (validateResult != null)
                {
                    result.Errors.AddRange(validateResult.Errors);
                }
            }
            if (string.IsNullOrEmpty(request.NewPassword))
            {
                result.Errors.Add("Invalid new password.".Localize());
            }
            #endregion

            if (result.Success)
            {
                var encryptedPwd = PasswordEncryptor.EncryptPassword(request.NewPassword, customer.Membership.PasswordSalt);
                customer.Membership.Password = encryptedPwd;
                CustomerService.Update(customer);
            }
            return result;
        }

        public GenerateResetPasswordTokenResult GenerateResetPasswordToken(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException("username");
            }
            GenerateResetPasswordTokenResult result = new GenerateResetPasswordTokenResult();
            var customer = CustomerProvider.QueryByUserName(username);
            #region Validation
            if (customer == null)
            {
                result.Errors.Add("The customer does not exists.".Localize());
            }
            #endregion
            if (result.Success)
            {
                var token = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(10);

                result.Token = token;

                customer.Membership.ResetPasswordToken = token;
                CustomerService.Update(customer);
            }

            return result;
        }

        public MembershipResult ResetPassowrd(ResetPasswordRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            MembershipResult result = new MembershipResult();
            var customer = CustomerProvider.QueryByUserName(request.Username);
            #region Validation
            if (customer == null)
            {
                result.Errors.Add("The customer does not exists.".Localize());
            }
            if (request.ResetPasswordToken != customer.Membership.ResetPasswordToken)
            {
                result.Errors.Add("Invalid token.".Localize());
            }
            #endregion

            if (result.Success)
            {
                var encryptedPwd = PasswordEncryptor.EncryptPassword(request.NewPassword, customer.Membership.PasswordSalt);
                customer.Membership.Password = encryptedPwd;
                customer.Membership.ResetPasswordToken = null;
                CustomerService.Update(customer);
            }

            return result;
        }
        #endregion
    }
}
