using System;
using Gma.QrCodeNet.Encoding.ReedSolomon;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	[TestFixture]
	public class RSTest
	{
		private static GaloisField256 m_gfield = GaloisField256.QRCodeGaloisField;
		private static GeneratorPolynomial m_cacheGeneratorPoly = new GeneratorPolynomial(m_gfield);
		
		[Test]
        [TestCaseSource(typeof(RSEncoderTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(byte[] data, int ecLength, byte[] expectResult)
        {
        	
        	TestOneCase(data, ecLength, expectResult);
        }
        
        private void TestOneCase(byte[] data, int ecLength, byte[] expectResult)
        {
        	byte[] result = ReedSolomonEncoder.Encode(data, ecLength, m_cacheGeneratorPoly);
        	
        	if(!PolynomialExtensions.isEqual(result, expectResult))
        		Assert.Fail("Remainder not same. result {0}, expect {1}", result.Length, expectResult.Length);
        }
        
        [Test]
        [TestCaseSource(typeof(RSEncoderTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(byte[] data, int ecLength, byte[] expectResult)
        {
        	TestOneCase(data, ecLength, expectResult);
        }
        
        //[Test]
        public void Generate()
        {
        	new RSEncoderTestCaseFactory().GenerateTestDataSet();
        }
        
        
	}
}
