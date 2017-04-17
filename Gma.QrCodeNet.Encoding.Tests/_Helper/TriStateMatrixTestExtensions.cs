using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gma.QrCodeNet.Encoding.Positioning;
using NUnit.Framework;
using TriStateMatrixToGraphicExtensions = Gma.QrCodeNet.Encoding.Tests.TriStateMatrixToGraphicExtensions;

namespace Gma.QrCodeNet.Encoding.Tests
{
    internal static class TriStateMatrixTestExtensions
    {
        public static void AssertEquals(this TriStateMatrix expected, TriStateMatrix actual)
        {
            if (expected == null) throw new ArgumentNullException("expected");
            if (actual == null) throw new ArgumentNullException("actual");

            if (expected.Width != actual.Width)
            {
                Assert.Fail("Mtrix must have same size. Expected {0}, Actual {1}", expected.Width, actual.Width);
            }

            
            for (int i = 0; i < expected.Width; i++)
                for (int j = 0; j < expected.Width; j++)
                {
                    if (expected.MStatus(i, j) != MatrixStatus.None && actual.MStatus(i, j) != MatrixStatus.None && expected[i, j] != actual[i, j])
                    {
                        Assert.Fail("Matrces are different.\nExpected:{0}Actual:{1}.", TriStateMatrixToGraphicExtensions.ToGraphicString(expected), TriStateMatrixToGraphicExtensions.ToGraphicString(actual));
                    }

                    if (expected.MStatus(i, j) == MatrixStatus.None && actual.MStatus(i, j) != MatrixStatus.None)
                    {
                        Assert.Fail("Matrces are different.\nExpected:{0}Actual:{1}.", TriStateMatrixToGraphicExtensions.ToGraphicString(expected), TriStateMatrixToGraphicExtensions.ToGraphicString(actual));
                    }
                    if (expected.MStatus(i, j) != MatrixStatus.None && actual.MStatus(i, j) == MatrixStatus.None)
                    {
                        Assert.Fail("Matrces are different.\nExpected:{0}Actual:{1}.", TriStateMatrixToGraphicExtensions.ToGraphicString(expected), TriStateMatrixToGraphicExtensions.ToGraphicString(actual));
                    }
                }
        }
    }
}
