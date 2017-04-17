using System;
using Gma.QrCodeNet.Encoding.ReedSolomon;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	[TestFixture]
	public class PolyDivideTest
	{
		[Test]
        [TestCaseSource(typeof(PolyDivideTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(int[] aCoeff, int[] bCoeff, int[] expQuotient, int[] expRemainder)
        {
        	
        	TestOneCase(aCoeff, bCoeff, expQuotient, expRemainder);
        }
        
        [Test]
        [TestCaseSource(typeof(PolyDivideTestCaseFactory), "TestCaseFromTxtFile")]
        public void Test_against_TXT_Dataset(int[] aCoeff, int[] bCoeff, int[] expQuotient, int[] expRemainder)
        {
        	TestOneCase(aCoeff, bCoeff, expQuotient, expRemainder);
        }
        
        private void TestOneCase(int[] aCoeff, int[] bCoeff, int[] expQuotient, int[] expRemainder)
        {
        	GaloisField256 gfield = GaloisField256.QRCodeGaloisField;
        	Polynomial apoly = new Polynomial(gfield, aCoeff);
        	Polynomial bpoly = new Polynomial(gfield, bCoeff);
        	
        	PolyDivideStruct pds = apoly.Divide(bpoly);
        	int[] quotient = pds.Quotient.Coefficients;
        	int[] remainder = pds.Remainder.Coefficients;
        	
        	if(!PolynomialExtensions.isEqual(quotient, expQuotient))
        		Assert.Fail("Quotient not equal. Result {0}, Expect {1}", aCoeff.Length, bCoeff.Length);
        	if(!PolynomialExtensions.isEqual(remainder, expRemainder))
        		Assert.Fail("Remainder not equal. Result {0}, Expect {1}", remainder.Length, aCoeff.Length);
        }
        
        //[Test]
        public void Generate()
        {
        	new PolyDivideTestCaseFactory().GenerateTestDataSet();
        }
	}
}
