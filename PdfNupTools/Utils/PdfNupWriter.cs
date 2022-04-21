using iTextSharp.text;
using iTextSharp.text.pdf;
using PdfNup.PdfNupTools.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PdfNup.PdfNupTools.Utils
{
    public class PdfNupWriter : IDisposable
    {
        private PdfOut pdfOut;
        private readonly PageSizeArg pageSize;
        private readonly IList<PositionArg> posList;

        public PdfNupWriter(PdfOut pdfOut, PageSizeArg pageSize, IList<PositionArg> posList)
        {
            this.pdfOut = pdfOut;
            this.pageSize = pageSize;
            this.posList = posList;
        }

        public void Dispose()
        {
            Generate();
        }

        private void Generate()
        {
            if (parts.Count == 0)
            {
                return;
            }

            pdfOut.AddPage(new Rectangle(pageSize.width, pageSize.height));

            var writer = pdfOut.writer;

            for (int y = 0; y < parts.Count; y++)
            {
                var reader = parts[y].pdfIn.reader;
                var pageNum = parts[y].y;

                PdfTemplate background = pdfOut.writer.GetImportedPage(reader, pageNum);

                var pageSizeAfterRotation = reader.GetPageSizeWithRotation(pageNum);

                float[] transferMatrix = new float[6];
                switch (reader.GetPageRotation(pageNum))
                {
                    case 0:
                    default:
                        transferMatrix[0] = 1;
                        transferMatrix[3] = 1;
                        break;
                    case 270:
                        transferMatrix[0] = (float)Math.Cos((float)(270 / 180.0f * Math.PI));
                        transferMatrix[1] = (float)-Math.Sin((float)(270 / 180.0f * Math.PI));
                        transferMatrix[2] = -transferMatrix[1];
                        transferMatrix[3] = transferMatrix[0];
                        transferMatrix[4] = pageSizeAfterRotation.Width;
                        break;
                    case 180:
                        transferMatrix[0] = (float)Math.Cos((float)(180 / 180.0f * Math.PI));
                        transferMatrix[1] = (float)-Math.Sin((float)(180 / 180.0f * Math.PI));
                        transferMatrix[2] = -transferMatrix[1];
                        transferMatrix[3] = transferMatrix[0];
                        transferMatrix[4] = pageSizeAfterRotation.Width;
                        transferMatrix[5] = pageSizeAfterRotation.Height;
                        break;
                    case 90:
                        transferMatrix[0] = (float)Math.Cos((float)(90 / 180.0f * Math.PI));
                        transferMatrix[1] = (float)-Math.Sin((float)(90 / 180.0f * Math.PI));
                        transferMatrix[2] = -transferMatrix[1];
                        transferMatrix[3] = transferMatrix[0];
                        transferMatrix[5] = pageSizeAfterRotation.Height;
                        break;
                }

                var positionArg = posList[y];

                transferMatrix[4] += positionArg.x;
                transferMatrix[5] += positionArg.y;

                var appender = writer.DirectContentUnder;

                appender.AddTemplate(
                    background,
                    transferMatrix[0], transferMatrix[1],
                    transferMatrix[2], transferMatrix[3],
                    transferMatrix[4], transferMatrix[5]
                    );

            }

            parts.Clear();
        }

        class Part
        {
            public PdfIn pdfIn;
            public int y;
        }

        List<Part> parts = new List<Part>();

        public void Add(PdfIn pdfIn)
        {
            foreach (var y in PageNumLister.From(pdfIn))
            {
                parts.Add(new Part { pdfIn = pdfIn, y = y, });
                if (parts.Count == posList.Count)
                {
                    Generate();
                }
            }
        }
    }
}
