using iTextSharp.text.pdf;
using PdfNup.PdfNupTools.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfNup.PdfNupTools.Utils
{
    public class PdfIn : IDisposable
    {
        public readonly PdfReader reader;
        public readonly PageRangesSpecifier pageSpec;

        public PdfIn(Stream inStream, PageRangesSpecifier pageSpec)
        {
            reader = new PdfReader(inStream);
            reader.ConsolidateNamedDestinations();

            this.pageSpec = pageSpec;
        }

        public void Dispose()
        {
            // なにもしない
        }
    }
}
