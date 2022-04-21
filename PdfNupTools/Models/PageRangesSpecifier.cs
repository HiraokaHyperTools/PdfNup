using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PdfNup.PdfNupTools.Models
{
    public class PageRangesSpecifier
    {
        public PageRangesSpecifier(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                specified = false;
                pages = new int[0];
                return;
            }
            else
            {
                var pageList = new List<int>();

                foreach (var part in text.Split(','))
                {
                    if (int.TryParse(part, out int one))
                    {
                        if (one < 1)
                        {
                            throw new Exception("Bad page specified: " + part);
                        }
                        pageList.Add(one);
                    }
                    else
                    {
                        var match = Regex.Match(part, "^(?<from>\\d+)-(?<to>\\d+)$");
                        if (match.Success)
                        {
                            var from = Convert.ToInt32(match.Groups["from"].Value);
                            var to = Convert.ToInt32(match.Groups["to"].Value);

                            if (from < 1 || to < 1)
                            {
                                throw new Exception("Bad page range specified: " + part);
                            }

                            if (from <= to)
                            {
                                pageList.AddRange(
                                    Enumerable.Range(
                                        from, to - from + 1
                                    )
                                );
                            }
                            else
                            {
                                pageList.AddRange(
                                    Enumerable.Range(
                                        0, from - to + 1
                                    )
                                        .Select(it => from - it)
                                );
                            }
                        }
                        else
                        {
                            throw new Exception("Bad page specified: " + part);
                        }
                    }
                }

                specified = true;
                pages = pageList;
            }
        }

        public readonly bool specified;
        public readonly IEnumerable<int> pages;
    }
}
