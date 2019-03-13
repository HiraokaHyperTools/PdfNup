using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfNup.PdfNupTools.Models
{
    class PageSizeArg
    {
        public PageSizeArg(string text)
        {
            var match = Regex.Match(text, "(?<w>\\d+(\\.\\d+)?)x(?<h>\\d+(\\.\\d+)?)");
            if (match.Success == false)
            {
                throw new Exception($"Invalid page size: {text}");
            }

            width = float.Parse(match.Groups["w"].Value);
            height = float.Parse(match.Groups["h"].Value);
        }

        public readonly float width;
        public readonly float height;
    }
}
