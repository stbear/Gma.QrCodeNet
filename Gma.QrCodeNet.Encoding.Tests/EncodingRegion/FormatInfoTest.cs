using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.EncodingRegion;
using Gma.QrCodeNet.Encoding.Masking;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	[TestFixture]
	public class FormatInfoTest
	{
		[Test]
        [TestCaseSource(typeof(FormatInfoTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, MaskPatternType patternType, TriStateMatrix expected)
        {
        	Test_One_Case(version, patternType,  expected);
        }
        
        [Test]
        [TestCaseSource(typeof(FormatInfoTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, MaskPatternType patternType, TriStateMatrix expected)
        {
        	Test_One_Case(version, patternType, expected);
        }
        
        private void Test_One_Case(int version, MaskPatternType patternType, TriStateMatrix expected)
        {
        	TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            PatternFactory pf = new PatternFactory();
            Pattern pt = pf.CreateByType(patternType);
            target.EmbedFormatInformation(ErrorCorrectionLevel.H, pt);
            
        	expected.AssertEquals(target);
        }
        
        //[Test]
        public void Generate()
        {
            new FormatInfoTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
	}
}
