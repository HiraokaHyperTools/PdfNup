using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfNup.PdfNupTools.Models
{
    public class FileAndQueries
    {
        public FileAndQueries(string text)
        {
            var pos = text.IndexOf('?');
            if (pos < 0)
            {
                path = text;
                queries = new Dictionary<string, string>();
            }
            else
            {
                path = text.Substring(0, pos);
                queries = new Dictionary<string, string>();

                foreach (var part in text.Substring(pos + 1).Split('&'))
                {
                    var mark = part.IndexOf('=');
                    if (mark >= 1)
                    {
                        queries[Uri.UnescapeDataString(part.Substring(0, mark))] = Uri.UnescapeDataString(part.Substring(mark + 1));
                    }
                }
            }
        }

        public readonly string path;
        public readonly Dictionary<string, string> queries;
    }
}
