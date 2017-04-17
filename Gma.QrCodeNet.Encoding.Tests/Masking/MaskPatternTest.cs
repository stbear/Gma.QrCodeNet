using System;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Masking;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Common;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Tests.Masking
{
    [TestFixture]
    public class MaskPatternTest
    {

        [Test]
        [TestCaseSource(typeof(MaskPatternTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(TriStateMatrix input, MaskPatternType patternType, BitMatrix expected)
        {
            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern, ErrorCorrectionLevel.H);

            expected.AssertEquals(result);
        }

        [Test]
        [TestCaseSource(typeof(MaskPatternTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(TriStateMatrix input, MaskPatternType patternType, BitMatrix expected)
        {
            Pattern pattern = new PatternFactory().CreateByType(patternType);

            BitMatrix result = input.Apply(pattern, ErrorCorrectionLevel.H);

            expected.AssertEquals(result);
        }

        //[Test]
        public void Generate()
        {
            new MaskPatternTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
    }
}
