#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.IO;
using System.Text;

namespace Kooboo.IO
{
    /// <summary>
    /// 
    /// </summary>
    public static class StreamExtensions
    {
        #region ReadData

        /// <summary>
        /// Reads the data.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadData(this Stream stream)
        {
            byte[] data = new byte[stream.Length];

            stream.Read(data, 0, data.Length);

            return data;
        }
        #endregion

        #region ReadString
        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <returns></returns>
        public static string ReadString(this Stream stream)
        {
            return stream.ReadString(Encoding.UTF8);
        }
        /// <summary>
        /// Reads the string.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns></returns>
        public static string ReadString(this Stream stream, Encoding encoding)
        {
            stream.Seek(0, SeekOrigin.Begin);
            TextReader reader = new StreamReader(stream, encoding);
            return reader.ReadToEnd();
        }
        #endregion

        #region SaveAs
        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="dest">The dest.</param>
        public static void CopyTo(this Stream src, Stream dest)
        {
            byte[] buffer = new byte[0x10000];
            int bytes;
            try
            {
                while ((bytes = src.Read(buffer, 0, buffer.Length)) > 0)
                {
                    dest.Write(buffer, 0, bytes);
                }
            }
            finally
            {
                dest.Flush();
            }
        }


        /// <summary>
        /// Saves as.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        public static string SaveAs(this Stream stream, string filePath)
        {
            return SaveAs(stream, filePath, true);
        }
        /// <summary>
        /// Saves as.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="isOverwrite">if set to <c>true</c> [is overwrite].</param>
        public static string SaveAs(this Stream stream, string filePath, bool isOverwrite)
        {
            var data = new byte[stream.Length];
            var length = stream.Read(data, 0, (int)stream.Length);
            return SaveAs(data, filePath, isOverwrite);
        }
        /// <summary>
        /// Saves as.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="isOverwrite">if set to <c>true</c> [is overwrite].</param>
        /// <returns>saved file absolute path</returns>
        public static string SaveAs(byte[] data, string filePath, bool isOverwrite)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (isOverwrite)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
            if (!isOverwrite && File.Exists(filePath))
            {
                string fileNameWithoutEx = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);

                int i = 1;
                do
                {
                    filePath = Path.Combine(directory, string.Format("{0}-{1}{2}", fileNameWithoutEx, i, extension));
                    i++;
                } while (File.Exists(filePath));
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fileStream.Write(data, 0, data.Length);
            }
            return filePath;
        }
        #endregion


        #region WriteString
        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="s">The s.</param>
        public static void WriteString(this Stream src, string s)
        {
            WriteString(src, s, Encoding.UTF8);
        }
        /// <summary>
        /// Writes the string.
        /// </summary>
        /// <param name="src">The SRC.</param>
        /// <param name="s">The s.</param>
        /// <param name="encoding">The encoding.</param>
        public static void WriteString(this Stream src, string s, Encoding encoding)
        {
            TextWriter writer = new StreamWriter(src, encoding);
            writer.Write(s);
            writer.Flush();
        }
        #endregion
    }
}
