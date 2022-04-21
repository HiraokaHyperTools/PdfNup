using NUnit.Framework;
using PdfNup.PdfNupTools.Models;
using PdfNup.PdfNupTools.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PdfNupTests
{
    public class Class1
    {
        [Test]
        public void PageSizeArg1()
        {
            Assert.Throws<Exception>(() => new PageSizeArg(""));
            Assert.Throws<Exception>(() => new PageSizeArg("792"));
            Assert.Throws<Exception>(() => new PageSizeArg("792,612"));
            Assert.Throws<Exception>(() => new PageSizeArg("792 612"));
            Assert.Throws<Exception>(() => new PageSizeArg(","));
            Assert.Throws<Exception>(() => new PageSizeArg("x"));
            Assert.Throws<Exception>(() => new PageSizeArg("a"));
            Assert.Throws<Exception>(() => new PageSizeArg("!"));

            var a = new PageSizeArg("792x612");
            Assert.That(a.width, Is.EqualTo(792));
            Assert.That(a.height, Is.EqualTo(612));
        }

        [Test]
        public void PositionArg1()
        {
            Assert.Throws<Exception>(() => new PositionArg("0x305"));
            Assert.Throws<Exception>(() => new PositionArg("0 305"));
            Assert.Throws<Exception>(() => new PositionArg("a"));
            Assert.Throws<Exception>(() => new PositionArg("!"));

            var a = new PositionArg("0,305");
            Assert.That(a.x, Is.EqualTo(0));
            Assert.That(a.y, Is.EqualTo(305));
        }

        [Test]
        public void FileAndQueries1()
        {
            var a = new FileAndQueries("a.pdf");
            Assert.That(a.path, Is.EqualTo("a.pdf"));
            Assert.That(a.queries, Is.Empty);
        }

        [Test]
        public void FileAndQueries2()
        {
            var a = new FileAndQueries("a.pdf?page=1");
            Assert.That(a.path, Is.EqualTo("a.pdf"));
            CollectionAssert.AreEqual(
                new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("page", "1"),
                },
                a.queries
            );
        }

        [Test]
        public void FileAndQueries3()
        {
            var a = new FileAndQueries("a.pdf?page=%31");
            Assert.That(a.path, Is.EqualTo("a.pdf"));
            CollectionAssert.AreEqual(
                new KeyValuePair<string, string>[]
                {
                    new KeyValuePair<string, string>("page", "1"),
                },
                a.queries
            );
        }

        [Test]
        public void PageRangesSpecifier1()
        {
            var a = new PageRangesSpecifier("");
            Assert.That(a.specified, Is.False);
            Assert.That(a.pages, Is.Empty);
        }

        [Test]
        public void PageRangesSpecifier2()
        {
            var a = new PageRangesSpecifier("   ");
            Assert.That(a.specified, Is.False);
            Assert.That(a.pages, Is.Empty);
        }

        [Test]
        public void PageRangesSpecifier3()
        {
            var a = new PageRangesSpecifier(null);
            Assert.That(a.specified, Is.False);
            Assert.That(a.pages, Is.Empty);
        }

        [Test]
        public void PageRangesSpecifier4()
        {
            var a = new PageRangesSpecifier("1");
            Assert.That(a.specified, Is.True);
            Assert.That(a.pages, Is.EqualTo(new int[] { 1 }));
        }

        [Test]
        public void PageRangesSpecifier5()
        {
            var a = new PageRangesSpecifier("1,2,3");
            Assert.That(a.specified, Is.True);
            Assert.That(a.pages, Is.EqualTo(new int[] { 1, 2, 3 }));
        }

        [Test]
        public void PageRangesSpecifier6()
        {
            var a = new PageRangesSpecifier("1-3,4,3-1");
            Assert.That(a.specified, Is.True);
            Assert.That(a.pages, Is.EqualTo(new int[] { 1, 2, 3, 4, 3, 2, 1 }));
        }

        [Test]
        public void PageRangesSpecifier7()
        {
            Assert.That(() => new PageRangesSpecifier("1-"), Throws.Exception);
            Assert.That(() => new PageRangesSpecifier("-1"), Throws.Exception);
            Assert.That(() => new PageRangesSpecifier("a"), Throws.Exception);
            Assert.That(() => new PageRangesSpecifier("!"), Throws.Exception);
            Assert.That(() => new PageRangesSpecifier("1 -2"), Throws.Exception);
            Assert.That(() => new PageRangesSpecifier("1- 2"), Throws.Exception);
            Assert.That(() => new PageRangesSpecifier("1 - 2"), Throws.Exception);
        }

        private string ResolveOutPath(string path)
        {
            return Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                path
            );
        }

        private string ResolveInPath(string path)
        {
            return Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "files",
                path
            );
        }

        [Test]
        public void PdfOut1()
        {
            var pageSizeArg = new PageSizeArg("1190x842");
            var posList = new PositionArg[] { new PositionArg("0,0"), new PositionArg("595,0"), };
            var args = "pages.pdf".Split(' ');

            using (var outStream = File.Create(ResolveOutPath("out1.pdf")))
            using (var pdfOut = new PdfOut(outStream))
            using (var pdfNupWriter = new PdfNupWriter(pdfOut, pageSizeArg, posList))
            {
                foreach (var arg in args)
                {
                    var set = new FileAndQueries(arg);
                    var inStream = File.OpenRead(ResolveInPath(set.path));
                    set.queries.TryGetValue("page", out string pageSpec);
                    var pdfIn = new PdfIn(inStream, new PageRangesSpecifier(pageSpec));

                    pdfNupWriter.Add(pdfIn);
                }
            }
        }

        [Test]
        public void PdfOut2()
        {
            var pageSizeArg = new PageSizeArg("1190x842");
            var posList = new PositionArg[] { new PositionArg("0,0"), new PositionArg("595,0"), };
            var args = "pages.pdf?page=1 pages.pdf?page=3".Split(' ');

            using (var outStream = File.Create(ResolveOutPath("out2.pdf")))
            using (var pdfOut = new PdfOut(outStream))
            using (var pdfNupWriter = new PdfNupWriter(pdfOut, pageSizeArg, posList))
            {
                foreach (var arg in args)
                {
                    var set = new FileAndQueries(arg);
                    var inStream = File.OpenRead(ResolveInPath(set.path));
                    set.queries.TryGetValue("page", out string pageSpec);
                    var pdfIn = new PdfIn(inStream, new PageRangesSpecifier(pageSpec));

                    pdfNupWriter.Add(pdfIn);
                }
            }
        }

    }
}
