using System;
using Gma.QrCodeNet.Encoding.ReedSolomon;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	[TestFixture]
	public class GaloisField256Test
	{
		[Test]
        [TestCaseSource(typeof(GaloisField256TestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int i, int exp)
        {
        	GaloisField256 gfield = GaloisField256.QRCodeGaloisField;
        	
        	int result = gfield.Exponent(i);
        	
        	if( exp != result)
        		Assert.Fail("Fail. request {0} Expect {1} result {2}", i, exp, result);
        }
        
        [Test]
        [TestCaseSource(typeof(GaloisField256TestCaseFactory), "TestCaseFromCSVFile")]
        public void Test_against_CSV_Dataset(int i, int exp)
        {
        	GaloisField256 gfield = GaloisField256.QRCodeGaloisField;
        	
        	int result = gfield.Exponent(i);
        	
        	if( exp != result)
        		Assert.Fail("Fail. request {0} Expect {1} result {2}", i, exp, result);
        }
        
        //[Test]
        public void Generate()
        {
        	new GaloisField256TestCaseFactory().GenerateTestDataSet();
        }
        
        
	}
}
