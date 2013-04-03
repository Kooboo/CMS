using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace Kooboo.Extensions
{
    public static class ObjectExtensions
    {
        /// <summary>
        /// Deeps the clone. Do not work in medium trust level.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        //public static object DeepClone(this object o)
        //{
        //    using (MemoryStream memStream = new MemoryStream())
        //    {
        //        BinaryFormatter binaryFormatter = new BinaryFormatter(null,
        //             new StreamingContext(StreamingContextStates.Clone));
        //        binaryFormatter.Serialize(memStream, o);
        //        memStream.Seek(0, SeekOrigin.Begin);
        //        return binaryFormatter.Deserialize(memStream);
        //    }
        //}
        
    }
}
