using Gma.QrCodeNet.Encoding.EncodingRegion;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	[TestFixture]
	public class BitCountTest
	{
		[Test]
        [TestCaseSource(typeof(BitCountTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int cValue, int expected)
        {
        	TestOneCase(cValue, expected);
        }
        
        [Test]
        [TestCaseSource(typeof(BitCountTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_Against_CSV_Dataset(int cValue, int expected)
        {
        	TestOneCase(cValue, expected);
        }
        
        //[Test]
        public void Generate()
        {
        	new BitCountTestCaseFactory().GenerateTestDataSet();
        }
        
        private void TestOneCase(int cValue, int expected)
        {
        	int actualResult = BCHCalculator.PosMSB(cValue);
        	if(actualResult != expected)
        		Assert.Fail(string.Format("actualResult: {0} expected: {1}", actualResult, expected));
        }
       
        
	}
}
