using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services
{
    public interface IStringBinaryConverter
    {
        string ToString(byte[] data);
        byte[] ToBinary(string data);
    }

    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IStringBinaryConverter))]
    public class Base64Converter : IStringBinaryConverter
    {
        public string ToString(byte[] data)
        {
            return Convert.ToBase64String(data);
        }

        public byte[] ToBinary(string data)
        {
            return Convert.FromBase64String(data);
        }
    }
}
