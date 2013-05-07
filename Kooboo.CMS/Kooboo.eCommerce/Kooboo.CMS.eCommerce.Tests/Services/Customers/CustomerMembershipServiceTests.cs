#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;
using Kooboo.CMS.eCommerce.Services.Customers;
using Kooboo.CMS.eCommerce.Persistence.Customers;
using Kooboo.CMS.eCommerce.Models.Customers;

namespace Kooboo.CMS.eCommerce.Tests.Services.Customers
{
    [TestClass]
    public class CustomerMembershipServiceTests
    {
        #region ValidateByUserName

        [TestMethod]
        public void Test_ValidateByUserName_Return_True()
        {
            var customer = new Customer()
                {
                    Username = "test",
                    Membership = new Membership()
                    {
                        IsLockedOut = false,
                        IsApproved = true,
                        Password = "test"
                    }
                };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
                .Returns(customer);
            var customerService = new Mock<ICustomerService>();
            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword("test", null))
                .Returns("test");

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
                customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            Assert.IsTrue(customerMemberhipService.ValidateByUserName("test", "test").Success);

            Assert.AreEqual(0, customer.Membership.FailedPasswordAttemptCount);
        }

        [TestMethod]
        public void Test_ValidateByUserName_UserNotExists()
        {
            var customerProvider = new Mock<ICustomerProvider>();
            var customerService = new Mock<ICustomerService>();
            var passwordEncryptor = new Mock<IPasswordEncryptor>();

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
                customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            var result = customerMemberhipService.ValidateByUserName("test", "test");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Username and/or password are incorrect.", result.Errors[0]);
        }

        [TestMethod]
        public void Test_ValidateByUserName_PasswordInvalid()
        {
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
               .Returns(new Customer()
               {
                   Username = "test",
                   Membership = new Membership()
                   {
                       IsLockedOut = false,
                       IsApproved = true,
                       Password = "test"
                   }
               }
               );
            var customerService = new Mock<ICustomerService>();
            var passwordEncryptor = new Mock<IPasswordEncryptor>();

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
                customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            var result = customerMemberhipService.ValidateByUserName("test", "test");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Username and/or password are incorrect.", result.Errors[0]);
        }

        [TestMethod]
        public void Test_ValidateByUserName_PasswordInvalid_WithLockedOut()
        {
            var customer = new Customer()
               {
                   Username = "test",
                   Membership = new Membership()
                   {
                       IsLockedOut = false,
                       IsApproved = true,
                       Password = "test",
                       FailedPasswordAttemptCount = 4
                   }
               };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
               .Returns(customer);
            var customerService = new Mock<ICustomerService>();
            var passwordEncryptor = new Mock<IPasswordEncryptor>();

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
                customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            var result = customerMemberhipService.ValidateByUserName("test", "test");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Username and/or password are incorrect.", result.Errors[0]);

            Assert.AreEqual(5, customer.Membership.FailedPasswordAttemptCount);
            Assert.IsTrue(customer.Membership.IsLockedOut);
        }

        [TestMethod]
        public void Test_ValidateByUserName_LockedOut()
        {
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
               .Returns(new Customer()
               {
                   Username = "test",
                   Membership = new Membership()
                   {
                       IsLockedOut = true,
                       IsApproved = true,
                       Password = "test"
                   }
               }
               );
            var customerService = new Mock<ICustomerService>();
            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword("test", null))
               .Returns("test");
            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
                customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            var result = customerMemberhipService.ValidateByUserName("test", "test");
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Account was locked out or not approved.", result.Errors[0]);
        }
        #endregion

        #region Register
        [TestMethod]
        public void Test_Register()
        {
            var customerProvider = new Mock<ICustomerProvider>();
            var customerService = new Mock<ICustomerService>();
            Customer customer = null;
            customerService.Setup(it => it.Add(It.IsAny<Customer>()))
                .Callback<Customer>((c) =>
                {
                    customer = c;
                });

            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword("test", null))
                .Returns("test");

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
              customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            RegistrationRequest request = new RegistrationRequest()
            {
                Username = "test",
                Email = "test@smtp.com",
                Password = "test",
                IsApproved = true
            };
            var result = customerMemberhipService.Register(request);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(customer);
            Assert.AreEqual(request.Username, customer.Username);
            Assert.AreEqual(request.Email, customer.Email);
            Assert.AreEqual(request.Password, customer.Membership.Password);
            Assert.AreEqual(request.IsApproved, customer.Membership.IsApproved);

        }
        #endregion

        #region ChangePassword
        [TestMethod]
        public void Test_ChangePassword()
        {
            var customer = new Customer()
            {
                Username = "test",
                Email = "test@smtp.com",
                Membership = new Membership()
                {
                    Password = "old",
                    IsApproved = true
                }
            };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
                .Returns(customer);

            var customerService = new Mock<ICustomerService>();
            Customer updateCustomer = null;
            customerService.Setup(it => it.Update(It.IsAny<Customer>()))
                .Callback<Customer>((c) =>
                {
                    updateCustomer = c;
                });

            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p, salt) => p);

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
              customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            ChangePasswordRequest request = new ChangePasswordRequest()
            {
                Username = "test",
                NewPassword = "new",
                OldPassword = "old"
            };
            var result = customerMemberhipService.ChangePassword(request);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(updateCustomer);
            Assert.AreEqual(request.Username, updateCustomer.Username);
            Assert.AreEqual(request.NewPassword, updateCustomer.Membership.Password);
        }

        [TestMethod]
        public void Test_ChangePassword_Incorrect_Old_Password()
        {
            var customer = new Customer()
            {
                Username = "test",
                Email = "test@smtp.com",
                Membership = new Membership()
                {
                    Password = "old",
                    IsApproved = true
                }
            };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
                .Returns(customer);

            var customerService = new Mock<ICustomerService>();
            Customer updateCustomer = null;
            customerService.Setup(it => it.Update(It.IsAny<Customer>()))
                .Callback<Customer>((c) =>
                {
                    updateCustomer = c;
                });

            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p, salt) => p);

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
              customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            ChangePasswordRequest request = new ChangePasswordRequest()
            {
                Username = "test",
                NewPassword = "new",
                OldPassword = "incorret"
            };
            var result = customerMemberhipService.ChangePassword(request);
            Assert.IsFalse(result.Success);
            Assert.IsNull(updateCustomer);
        }

        #endregion

        #region GenerateResetPasswordToken
        [TestMethod]
        public void Test_GenerateResetPasswordToken()
        {
            var customer = new Customer()
            {
                Username = "test",
                Email = "test@smtp.com",
                Membership = new Membership()
                {
                    Password = "old",
                    IsApproved = true
                }
            };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
                .Returns(customer);

            var customerService = new Mock<ICustomerService>();
            Customer updateCustomer = null;
            customerService.Setup(it => it.Update(It.IsAny<Customer>()))
                .Callback<Customer>((c) =>
                {
                    updateCustomer = c;
                });

            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p, salt) => p);

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
              customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            var result = customerMemberhipService.GenerateResetPasswordToken(customer.Username);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(updateCustomer);
            Assert.AreEqual(result.Token, updateCustomer.Membership.ResetPasswordToken);
        }
        #endregion

        #region ResetPassowrdResult
        [TestMethod]
        public void Test_ResetPassowrdResult()
        {
            var customer = new Customer()
            {
                Username = "test",
                Email = "test@smtp.com",
                Membership = new Membership()
                {
                    Password = "old",
                    IsApproved = true,
                    ResetPasswordToken = "123456"
                }
            };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
                .Returns(customer);

            var customerService = new Mock<ICustomerService>();
            Customer updateCustomer = null;
            customerService.Setup(it => it.Update(It.IsAny<Customer>()))
                .Callback<Customer>((c) =>
                {
                    updateCustomer = c;
                });

            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p, salt) => p);

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
              customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            ResetPasswordRequest request = new ResetPasswordRequest()
            {
                Username = "test",
                NewPassword = "new",
                ResetPasswordToken = "123456"
            };

            var result = customerMemberhipService.ResetPassowrd(request);
            Assert.IsTrue(result.Success);
            Assert.IsNotNull(updateCustomer);
            Assert.AreEqual(null, updateCustomer.Membership.ResetPasswordToken);
            Assert.AreEqual("new", updateCustomer.Membership.Password);
        }

        [TestMethod]
        public void Test_ResetPassowrdResult_Invalid_Token()
        {
            var customer = new Customer()
            {
                Username = "test",
                Email = "test@smtp.com",
                Membership = new Membership()
                {
                    Password = "old",
                    IsApproved = true,
                    ResetPasswordToken = "123456"
                }
            };
            var customerProvider = new Mock<ICustomerProvider>();
            customerProvider.Setup(it => it.QueryByUserName("test"))
                .Returns(customer);

            var customerService = new Mock<ICustomerService>();
            Customer updateCustomer = null;
            customerService.Setup(it => it.Update(It.IsAny<Customer>()))
                .Callback<Customer>((c) =>
                {
                    updateCustomer = c;
                });

            var passwordEncryptor = new Mock<IPasswordEncryptor>();
            passwordEncryptor.Setup(it => it.EncryptPassword(It.IsAny<string>(), It.IsAny<string>()))
                .Returns<string, string>((p, salt) => p);

            CustomerMembershipService customerMemberhipService = new CustomerMembershipService(
              customerProvider.Object, customerService.Object, passwordEncryptor.Object);

            ResetPasswordRequest request = new ResetPasswordRequest()
            {
                Username = "test",
                NewPassword = "new",
                ResetPasswordToken = "abc123"
            };

            var result = customerMemberhipService.ResetPassowrd(request);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Invalid token.", result.Errors[0]);
            Assert.IsNull(updateCustomer);            
        }
        #endregion
    }
}
