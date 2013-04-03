using System;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using ExpertPdf.HtmlToPdf;
using Ionic.Utils.Zip;

namespace LDDBonR2.Lib
{
    public class PdfFunctions
    {
        public static string RenderControl(WebControl control)
        {
            StringBuilder builder = new StringBuilder();
            StringWriter strWriter = new StringWriter(builder);
            HtmlTextWriter writer = new HtmlTextWriter(strWriter);
            control.RenderControl(writer);
            return builder.ToString();
        }

        public static PdfConverter GetPdfConverter()
        {
            PdfConverter pdfConverter = new PdfConverter();

            pdfConverter.LicenseKey =
                "U+kdIabfbXREOtvgmKIYieuJbf7xubsgqazVyrnbKi0wiHUJsQn9K1XTSIMRnJKt";
            pdfConverter.PageWidth = 720;
            pdfConverter.PdfDocumentOptions.PdfPageSize = PdfPageSize.A4;
            pdfConverter.PdfDocumentOptions.PdfCompressionLevel = PdfCompressionLevel.Normal;
            pdfConverter.PdfDocumentOptions.LeftMargin = 5;
            pdfConverter.PdfDocumentOptions.TopMargin = 5;
            pdfConverter.PdfDocumentOptions.FitWidth = false;
            pdfConverter.PdfDocumentOptions.EmbedFonts = true;

            return pdfConverter;
        }

        public static void GeneratePdfFromString(string str, string fileToSave)
        {

            byte[] bytes = GetPdfConverter().GetPdfBytesFromHtmlString(str);

            using (FileStream stream = File.Create(fileToSave))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static byte[] GeneratePdfBytesFromUrl(string url)
        {
            return GetPdfConverter().GetPdfFromUrlBytes(url);
        
        }

        public static byte[] GeneratePdfBytesFromUrl(string url, int top, int left)
        {
            PdfConverter pc = GetPdfConverter();
           // pc.PdfDocumentOptions.TopMargin = top;
            //pc.PdfDocumentOptions.LeftMargin = left;
            return pc.GetPdfFromUrlBytes(url);

        }

        public static void GeneratePdfFromUrl(string url, string fileToSave)
        {
            byte[] bytes = GeneratePdfBytesFromUrl(url);

            using (FileStream stream = File.Create(fileToSave))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void GeneratePdfFromUrl(string url, string fileToSave, int top, int left)
        {
            byte[] bytes = GeneratePdfBytesFromUrl(url, top, left);

            using (FileStream stream = File.Create(fileToSave))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }

        public static void ZipFiles(IEnumerable<string> fileNames, string targetFile)
        {
            ZipFile zipedFile = new ZipFile();
            foreach (string file in fileNames)
            {
                zipedFile.AddFile(file, String.Empty);
            }
            zipedFile.Save(targetFile);
        }

        public static void DownloadStream(string fileName, byte[] stream)
        {
            HttpResponse response = HttpContext.Current.Response;
            response.Clear();
            response.AddHeader("Content-Type", "binary/octet-stream");
            response.AddHeader("Content-Disposition",
                "attachment; filename=" + fileName + "; size=" + stream.Length.ToString());
            response.Flush();
            response.BinaryWrite(stream);
            response.Flush();
            response.End();
        }

        public static void Download(string fileName, string savedFileName)
        {
            DownloadStream(fileName, File.ReadAllBytes(savedFileName));
        }
    }
}
