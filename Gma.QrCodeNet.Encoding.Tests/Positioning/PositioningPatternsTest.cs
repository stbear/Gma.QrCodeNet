using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.Tests.Positioning.TestCases;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.Alignment
{
    [TestFixture]
    public class PositioningPatternsTest
    {

        [Test]
        [TestCaseSource(typeof(PositioningPatternsTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, TriStateMatrix expected)
        {
      
            TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            expected.AssertEquals(target);
        }

        [Test]
        [TestCaseSource(typeof(PositioningPatternsTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, TriStateMatrix expected)
        {
            TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            expected.AssertEquals(target);
        }

        //[Test]
        public void Generate()
        {
            new PositioningPatternsTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
    }
}
