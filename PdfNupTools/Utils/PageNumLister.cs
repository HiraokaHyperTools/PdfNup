using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfNup.PdfNupTools.Utils
{
    internal static class PageNumLister
    {
        internal static IEnumerable<int> From(PdfIn pdfIn)
        {
            return pdfIn.pageSpec.specified
                ? pdfIn.pageSpec.pages
                : Enumerable.Range(1, pdfIn.reader.NumberOfPages);
        }
    }
}
