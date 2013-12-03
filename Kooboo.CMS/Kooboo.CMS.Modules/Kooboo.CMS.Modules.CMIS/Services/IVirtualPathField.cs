using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Modules.CMIS.Services
{

    public interface IVirtualPathField
    {
        bool IsVirtualPathField(string virtualPath);
        bool IsBinaryString(string str);
        string ToBinaryString(string virtualPath);
        Dictionary<string, byte[]> ToBinaryStream(string binaryString);
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IVirtualPathField))]
    public class VirtualPathField : IVirtualPathField
    {
        #region .ctor
        const string BinaryStringHeader = "@@@BinaryString@@@";
        IStringBinaryConverter _converter;
        public VirtualPathField(IStringBinaryConverter converter)
        {
            _converter = converter;
        }
        #endregion

        #region IsVirtualPathField
        public bool IsVirtualPathField(string virtualPath)
        {
            if (!string.IsNullOrEmpty(virtualPath) && (virtualPath.StartsWith("~/") || virtualPath.StartsWith("/")))
            {
                var extension = Path.GetExtension(virtualPath);
                if (!string.IsNullOrEmpty(extension))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region ToBinaryString

        private string[] GetVirtualPaths(string virtualPath)
        {
            if (IsVirtualPathField(virtualPath))
            {
                return virtualPath.Split("|".ToArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            return new string[0];
        }

        public string ToBinaryString(string virtualPath)
        {
            var paths = GetVirtualPaths(virtualPath);
            if (paths.Length > 0)
            {
                StringBuilder sb = new StringBuilder(BinaryStringHeader);
                foreach (var path in paths)
                {
                    var physicalPath = Kooboo.Web.Url.UrlUtility.MapPath(path);
                    if (File.Exists(physicalPath))
                    {
                        var stringData = BinaryFileToString(physicalPath);
                        sb.AppendFormat("{0}$$${1}|||", path, stringData);
                    }
                }

                return sb.ToString();
            }
            return string.Empty;
        }


        private string BinaryFileToString(string physicalPath)
        {
            byte[] data;
            using (var fs = new FileStream(physicalPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, Convert.ToInt32(fs.Length));
            }
            return _converter.ToString(data);
        }
        #endregion

        #region ToBinaryStream
        public bool IsBinaryString(string str)
        {
            return !string.IsNullOrEmpty(str) && str.StartsWith(BinaryStringHeader);
        }
        public Dictionary<string, byte[]> ToBinaryStream(string binaryString)
        {
            var dic = new Dictionary<string, byte[]>();
            if (IsBinaryString(binaryString))
            {
                binaryString = binaryString.Substring(BinaryStringHeader.Length);
                var binaryStrings = binaryString.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var str in binaryStrings)
                {
                    string virtualPath = null;
                    var data = ToBinaryData(str, out virtualPath);
                    if (!string.IsNullOrEmpty(virtualPath) && data.Length > 0)
                    {
                        dic[virtualPath] = data;
                    }
                }
            }
            return dic;
        }
        private byte[] ToBinaryData(string binaryString, out string virtualPath)
        {
            virtualPath = null;
            byte[] data = new byte[0];
            var strArr = binaryString.Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
            if (strArr.Length == 2)
            {
                virtualPath = strArr[0];
                data = _converter.ToBinary(strArr[1]);
            }
            return data;
        }
        #endregion
    }
}
