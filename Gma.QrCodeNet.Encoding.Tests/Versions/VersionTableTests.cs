using System;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Versions;
using Gma.QrCodeNet.Encoding.Tests.Versions.TestCases;

namespace Gma.QrCodeNet.Encoding.Tests.Versions
{
	
	[TestFixture]
	public class VersionTableTests
	{
		private const string s_AssertFormate = "{0}: {1}, Expect: {2};";
		
		[Test]
        [TestCaseSource(typeof(VersionTableTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int versionNum, int totalCodewords, int level, int numECCodewords, string ecBlockString)
        {
        	this.TestOneDataRow(versionNum, totalCodewords, level, numECCodewords, ecBlockString);
        }
        
        [Test]
        [TestCaseSource(typeof(VersionTableTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_CSV_Dataset(int versionNum, int totalCodewords, int level, int numECCodewords, string ecBlockString)
        {
        	this.TestOneDataRow(versionNum, totalCodewords, level, numECCodewords, ecBlockString);
        }
        
        public void TestOneDataRow(int versionNum, int totalCodewords, int level, int numECCodewords, string ecBlockString)
        {
        	VersionTableTestProperties properties = VersionTableTest.GetVersionInfo(versionNum, (ErrorCorrectionLevel)level);
        	
        	string failResult = "";
        	
        	failResult = properties.TotalNumOfCodewords == totalCodewords ? failResult 
        		: string.Join(" ", failResult, string.Format(s_AssertFormate, "TotalCodewords", properties.TotalNumOfCodewords, totalCodewords));
        	
        	failResult = properties.NumOfECCodewords == numECCodewords ? failResult 
        		: string.Join(" ", failResult, string.Format(s_AssertFormate, "NumErrorCorrectionCodeWords", properties.NumOfECCodewords, numECCodewords));
        	
        	failResult = properties.ECBlockString == ecBlockString ? failResult 
        		: string.Join(" ", failResult, string.Format(s_AssertFormate, "ECBlockString", properties.ECBlockString, ecBlockString));
        	
        	if(failResult != "")
        		Assert.Fail(failResult);
        }
        
        //[Test]
        public void Generate()
        {
        	new VersionTableTestCaseFactory().GenerateTestDataSet();
        }
	}
}
