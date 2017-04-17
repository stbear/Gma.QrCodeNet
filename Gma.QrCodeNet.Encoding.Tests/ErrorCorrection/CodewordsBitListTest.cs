using System;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.ErrorCorrection;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	[TestFixture]
	public class CodewordsBitListTest
	{
		[Test]
        [TestCaseSource(typeof(CodewordsBitListTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(string inputString, ErrorCorrectionLevel eclevel, IEnumerable<bool> expected)
        {
        	TestOneCase(inputString, eclevel, expected);
        }
        
        private void TestOneCase(string inputString, ErrorCorrectionLevel eclevel, IEnumerable<bool> expected)
        {
        	EncodationStruct encodeStruct = DataEncode.Encode(inputString, eclevel);
			
			IEnumerable<bool> actualResult = ECGenerator.FillECCodewords(encodeStruct.DataCodewords, encodeStruct.VersionDetail);
        	
        	BitVectorTestExtensions.CompareIEnumerable(actualResult, expected, "string");
        }
	}
}
