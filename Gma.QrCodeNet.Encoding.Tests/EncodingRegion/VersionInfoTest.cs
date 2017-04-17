using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.EncodingRegion;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	[TestFixture]
	public class VersionInfoTest
	{
		[Test]
        [TestCaseSource(typeof(VersionInfoTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, TriStateMatrix expected)
        {
        	Test_One_Case(version, expected);
        }
        
        [Test]
        [TestCaseSource(typeof(VersionInfoTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, TriStateMatrix expected)
        {
        	Test_One_Case(version, expected);
        }
        
        private void Test_One_Case(int version, TriStateMatrix expected)
        {
        	TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            target.EmbedVersionInformation(version);
            
        	expected.AssertEquals(target);
        }
        
        //[Test]
        public void Generate()
        {
            new VersionInfoTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
	}
}
