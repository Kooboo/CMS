#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.Sites.Web
{
    public class OutputTextWriterWrapper : StringWriter
    {
        public TextWriter Respone_Output_TextWriter { get; set; }

        bool rendered = false;
        public OutputTextWriterWrapper(TextWriter httpOutputWriter)
        {
            Respone_Output_TextWriter = httpOutputWriter;
        }
        public virtual TextWriter GetRawOuputWriter()
        {
            TextWriter textWriter = this;
            while (textWriter is OutputTextWriterWrapper && ((OutputTextWriterWrapper)textWriter).Respone_Output_TextWriter != null)
            {
                textWriter = ((OutputTextWriterWrapper)textWriter).Respone_Output_TextWriter;
            }
            return textWriter;
        }
        public override void Flush()
        {
            this.GetStringBuilder().Clear();
        }
        public virtual void Render(HttpResponseBase response)
        {
            if (!rendered)
            {
                var originalWriter = GetRawOuputWriter();
                response.Output = originalWriter;
                originalWriter.Write(this.ToString());
                rendered = true;
            }
        }
    }
}
