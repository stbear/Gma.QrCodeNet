using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.EncodingRegion;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	[TestFixture]
	public class CodewordsTest
	{
		[Test]
        [TestCaseSource(typeof(CodewordsTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int version, TriStateMatrix expected, IEnumerable<bool> codewords)
        {
        	Test_One_Case(version, expected, codewords);
        }
        
        [Test]
        [TestCaseSource(typeof(CodewordsTestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(int version, TriStateMatrix expected, IEnumerable<bool> codewords)
        {
        	Test_One_Case(version, expected, codewords);
        }
        
        private void Test_One_Case(int version, TriStateMatrix expected, IEnumerable<bool> codewords)
        {
        	BitList dcList = new BitList();
        	dcList.Add(codewords);
        	
        	TriStateMatrix target = new TriStateMatrix(expected.Width);
            PositioninngPatternBuilder.EmbedBasicPatterns(version, target);
            target.TryEmbedCodewords(dcList);
            
        	expected.AssertEquals(target);
        }
        
        //[Test]
        public void Generate()
        {
            new CodewordsTestCaseFactory().GenerateMaskPatternTestDataSet();
        }
        
	}
}
