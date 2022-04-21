using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfNup.PdfNupTools.Models
{
    public class PositionArg
    {
        public PositionArg(string text)
        {
            var match = Regex.Match(text, "(?<x>\\-?\\d+(\\.\\d+)?),(?<y>\\-?\\d+(\\.\\d+)?)");
            if (match.Success == false)
            {
                throw new Exception($"Invalid position: {text}");
            }

            x = float.Parse(match.Groups["x"].Value);
            y = float.Parse(match.Groups["y"].Value);
        }

        public readonly float x;
        public readonly float y;
    }
}
