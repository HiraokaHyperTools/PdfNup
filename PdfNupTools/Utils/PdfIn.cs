using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfNup.PdfNupTools.Utils
{
    class PdfIn : IDisposable
    {
        public PdfReader reader;

        public PdfIn(Stream inStream)
        {
            reader = new PdfReader(inStream);
            reader.ConsolidateNamedDestinations();
        }

        public void Dispose()
        {
            // なにもしない
        }
    }
}
