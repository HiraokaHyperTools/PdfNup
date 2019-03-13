using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfNup.PdfNupTools.Utils
{
    class PdfOut : IDisposable
    {
        public PdfWriter writer;
        public Document document;
        Stream outStream;

        public PdfOut(Stream outStream)
        {
            this.outStream = outStream;
        }

        public void AddPage(Rectangle pageSize)
        {
            if (document == null)
            {
                document = new Document(pageSize);
                writer = PdfWriter.GetInstance(document, outStream);
                document.Open();
            }
            else
            {
                document.SetPageSize(pageSize);
                document.NewPage();
            }
        }

        public void Close()
        {
            if (document != null)
            {
                document.Close();
                document = null;
            }
            if (writer != null)
            {
                writer.Close();
                writer = null;
            }

            if (outStream != null)
            {
                outStream.Close();
                outStream = null;
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}
