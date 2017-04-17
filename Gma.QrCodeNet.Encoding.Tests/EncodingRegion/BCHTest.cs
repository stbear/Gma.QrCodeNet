using Gma.QrCodeNet.Encoding.EncodingRegion;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	[TestFixture]
	public class BCHTest
	{
		[Test]
		[TestCaseSource(typeof(BCHTestCaseFactory), "TestCasesFromReferenceImplementation")]
		public void Test_Against_Reference_Implementation(int inputNum, int expected)
		{
			TestOneCase(inputNum, expected);
		}
		
		[Test]
		[TestCaseSource(typeof(BCHTestCaseFactory), "TestCasesFromCsvFile")]
		public void Test_Against_CSV_Dataset(int inputNum, int expected)
		{
			TestOneCase(inputNum, expected);
		}
		
		//[Test]
		public void Generate()
		{
			new BCHTestCaseFactory().GenerateTestDataSet();
		}
		
		// From Appendix D in JISX0510:2004 (p. 67)
		private const int VERSION_INFO_POLY = 0x1f25; // 1 1111 0010 0101
		
		// From Appendix C in JISX0510:2004 (p.65).
		private const int TYPE_INFO_POLY = 0x537;
		
		private void TestOneCase(int inputNum, int expected)
		{
			int bchNum = BCHCalculator.CalculateBCH(inputNum, VERSION_INFO_POLY);
			if(bchNum != expected)
				Assert.Fail("InputNum: {0} Actual: {1} Expect: {2}", inputNum, bchNum, expected);
		}
		
	}
}
