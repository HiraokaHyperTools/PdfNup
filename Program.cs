using PdfNup.PdfNupTools.Models;
using PdfNup.PdfNupTools.Utils;
using PdfNup.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfNup
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length >= 6 && args[0] == "compose")
            {
                var pageSizeArg = new PageSizeArg(args[1]);

                int x = 3;
                if (args[x] != "(")
                {
                    throw new Exception($"Needs \"(\"");
                }
                x++;
                List<PositionArg> posList = new List<PositionArg>();
                for (; ; )
                {
                    if (args[x] == ")")
                    {
                        x++;
                        break;
                    }
                    posList.Add(new PositionArg(args[x]));
                    x++;
                    if (x == args.Length)
                    {
                        throw new Exception($"Needs \")\"");
                    }
                }

                using (var outStream = File.Create(args[2]))
                using (var pdfOut = new PdfOut(outStream))
                using (var pdfNupWriter = new PdfNupWriter(pdfOut, pageSizeArg, posList))
                {
                    for (; x < args.Length; x++)
                    {
                        var set = new FileAndQueries(args[x]);
                        var inStream = File.OpenRead(set.path);
                        set.queries.TryGetValue("page", out string pageSpec);
                        var pdfIn = new PdfIn(inStream, new PageRangesSpecifier(pageSpec));

                        pdfNupWriter.Add(pdfIn);
                    }
                }
                return;
            }

            Console.Error.Write(Resources.Usage);
            Environment.ExitCode = 1;
        }
    }
}
