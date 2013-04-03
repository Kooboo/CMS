using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Data;

namespace Kooboo.Globalization
{
    public class DirectoryCreateException:Exception,IKoobooException
    {
   
        public DirectoryCreateException()
            : base("No permission to create a directory.")
        {
        }

        public DirectoryCreateException(System.IO.IOException exception)
            : base("No permission to create a directory.", exception)
        {
           
        }

     
    }
}
