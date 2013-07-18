#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Globalization;
using Kooboo.CMS.Member.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace Kooboo.CMS.Member.Services
{
    public class MembershipPasswordProvider
    {
        public virtual string GenerateSalt()
        {
            byte[] data = new byte[0x10];
            new RNGCryptoServiceProvider().GetBytes(data);
            return Convert.ToBase64String(data);
        }

        public virtual string EncodePassword(Membership membership, string pass, string salt)
        {
            HashAlgorithm hashAlgorithm = this.GetHashAlgorithm(membership.HashAlgorithmType);
            if (hashAlgorithm == null)
            {
                return pass;
            }
            else
            {
                byte[] bytes = Encoding.Unicode.GetBytes(pass);
                byte[] src = Convert.FromBase64String(salt);
                byte[] inArray = null;
                if (hashAlgorithm is KeyedHashAlgorithm)
                {
                    KeyedHashAlgorithm algorithm2 = (KeyedHashAlgorithm)hashAlgorithm;
                    if (algorithm2.Key.Length == src.Length)
                    {
                        algorithm2.Key = src;
                    }
                    else if (algorithm2.Key.Length < src.Length)
                    {
                        byte[] dst = new byte[algorithm2.Key.Length];
                        Buffer.BlockCopy(src, 0, dst, 0, dst.Length);
                        algorithm2.Key = dst;
                    }
                    else
                    {
                        int num2;
                        byte[] buffer5 = new byte[algorithm2.Key.Length];
                        for (int i = 0; i < buffer5.Length; i += num2)
                        {
                            num2 = Math.Min(src.Length, buffer5.Length - i);
                            Buffer.BlockCopy(src, 0, buffer5, i, num2);
                        }
                        algorithm2.Key = buffer5;
                    }
                    inArray = algorithm2.ComputeHash(bytes);
                }
                else
                {
                    byte[] buffer6 = new byte[src.Length + bytes.Length];
                    Buffer.BlockCopy(src, 0, buffer6, 0, src.Length);
                    Buffer.BlockCopy(bytes, 0, buffer6, src.Length, bytes.Length);
                    inArray = hashAlgorithm.ComputeHash(buffer6);
                }
                return Convert.ToBase64String(inArray);
            }
        }

        private HashAlgorithm GetHashAlgorithm(string hashAlgorithmType)
        {
            HashAlgorithm hashAlgorithm = null;

            if (!string.IsNullOrEmpty(hashAlgorithmType))
            {
                hashAlgorithm = HashAlgorithm.Create(hashAlgorithmType);
            }

            return hashAlgorithm;
        }
    }
}
