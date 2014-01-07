using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.CMS.Modules.CMIS.Services
{

    public interface IMediaPathField
    {
        bool IsMediaPathField(string fieldValue);
        bool IsBinaryString(string str);
        string ToBinaryString(string fieldValue);
        Dictionary<string, byte[]> ToBinaryStream(string binaryString);
        string ToBinaryString(string[] fieldValues);
    }
    [Kooboo.CMS.Common.Runtime.Dependency.Dependency(typeof(IMediaPathField))]
    public class MediaPathField : IMediaPathField
    {
        #region .ctor
        const string BinaryStringHeader = "@@@BinaryStringFromMedia@@@";
        IStringBinaryConverter _converter;
        public MediaPathField(IStringBinaryConverter converter)
        {
            _converter = converter;
        }
        #endregion

        #region IsMidiaPathField
        private Regex _mediaRegex = new Regex(@"(/Cms_Data/Contents/(\w+)/Media/(.*?)/([^/./\\:*?\""<>|]+)\.(\w{2,4}))");
        public bool IsMediaPathField(string fieldValue)
        {
            return !string.IsNullOrWhiteSpace(fieldValue) && this._mediaRegex.IsMatch(fieldValue);
        }
        #endregion

        #region ToBinaryString

        private string[] GetMediaPaths(string fieldValue)
        {
            var paths = new List<string>();
            if (IsMediaPathField(fieldValue))
            {
                var matchs = this._mediaRegex.Matches(fieldValue);
                for (int i = 0, len = matchs.Count; i < len; i++)
                {
                    if (matchs[i].Success)
                    {
                        paths.Add(matchs[i].Value);
                    }
                }
            }
            return paths.ToArray();
        }

        public string ToBinaryString(string fieldValue)
        {
            var paths = GetMediaPaths(fieldValue);
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
                    string mediaPath = null;
                    var data = ToBinaryData(str, out mediaPath);
                    if (!string.IsNullOrEmpty(mediaPath) && data.Length > 0)
                    {
                        dic[mediaPath] = data;
                    }
                }
            }
            return dic;
        }
        private byte[] ToBinaryData(string binaryString, out string mediaPath)
        {
            mediaPath = null;
            byte[] data = new byte[0];
            var strArr = binaryString.Split(new string[] { "$$$" }, StringSplitOptions.RemoveEmptyEntries);
            if (strArr.Length == 2)
            {
                mediaPath = strArr[0];
                data = _converter.ToBinary(strArr[1]);
            }
            return data;
        }
        #endregion


        public string ToBinaryString(string[] fieldValues)
        {
            var paths = new List<string>();
            foreach (var val in fieldValues)
            {
                paths.AddRange(GetMediaPaths(val));
            }
            if (paths.Count > 0)
            {
                StringBuilder sb = new StringBuilder(BinaryStringHeader);
                foreach (var path in paths.Distinct(StringComparer.OrdinalIgnoreCase))
                {
                    var physicalPath = Kooboo.Web.Url.UrlUtility.MapPath(System.Web.HttpUtility.UrlDecode(path));
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
    }
}
