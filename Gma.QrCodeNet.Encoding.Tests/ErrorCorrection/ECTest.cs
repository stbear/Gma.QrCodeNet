using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.ErrorCorrection;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	[TestFixture]
	public class ECTest
	{
		[Test]
        [TestCaseSource(typeof(ECTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(IEnumerable<bool> dataCodewords, VersionDetail vc, IEnumerable<bool> expected)
        {
        	TestOneCase(dataCodewords, vc, expected);
        }
        
        [Test]
        [TestCaseSource(typeof(ECTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(IEnumerable<bool> dataCodewords, VersionDetail vc, IEnumerable<bool> expected)
        {
        	TestOneCase(dataCodewords, vc, expected);
        }
        
        private void TestOneCase(IEnumerable<bool> dataCodewords, VersionDetail vc, IEnumerable<bool> expected)
        {
        	BitList dcList = new BitList();
        	dcList.Add(dataCodewords);
        	
        	IEnumerable<bool> actualResult = ECGenerator.FillECCodewords(dcList, vc);
        	BitVectorTestExtensions.CompareIEnumerable(actualResult, expected, "string");
        }
        
        //[Test]
        public void Generate()
        {
        	new ECTestCaseFactory().GenerateTestDataSet();
        }
	}
}
