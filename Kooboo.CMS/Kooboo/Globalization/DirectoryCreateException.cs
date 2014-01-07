#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;


namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class DirectoryCreateException:Exception
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCreateException" /> class.
        /// </summary>
        public DirectoryCreateException()
            : base("No permission to create a directory.")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryCreateException" /> class.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public DirectoryCreateException(System.IO.IOException exception)
            : base("No permission to create a directory.", exception)
        {
           
        }

     
    }
}
