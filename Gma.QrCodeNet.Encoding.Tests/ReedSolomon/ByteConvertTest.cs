using System;
using Gma.QrCodeNet.Encoding.common;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	[TestFixture]
	public class ByteConvertTest
	{
		[Test]
        [TestCaseSource(typeof(ByteConvertTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(byte[] test)
        {
        	TestOneData(test);
		}
        
        [Test]
        [TestCaseSource(typeof(ByteConvertTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(byte[] test)
        {
        	TestOneData(test);
        }
        
        private void TestOneData(byte[] test)
        {
        	BitList bitList = BitListExtensions.ToBitList(test);
        	
        	byte[] result = bitList.ToByteArray();
        	
        	if(!PolynomialExtensions.isEqual(result, test))
        		Assert.Fail("Byte convert fail. result {0}, expect {1}", result[0], test[0]);
        }
        
        
        
        //[Test]
        public void Generate()
        {
        	new ByteConvertTestCaseFactory().GenerateTestDataSet();
        }
	}
}
