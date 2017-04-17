using Gma.QrCodeNet.Encoding.Masking.Scoring;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	[TestFixture]
	public class Penalty1Test : PenaltyTestBase
	{
		[Test, TestCaseSource(typeof(Penalty1TestCaseFactory), "TestCasesFromReferenceImplementation")]
		public override void Test_against_reference_implementation(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			base.Test_against_reference_implementation(input, penaltyRule, expected);
		}
		
		[Test]
        [TestCaseSource(typeof(Penalty1TestCaseFactory), "TestCasesFromTxtFile")]
        public void Test_against_DataSet(BitMatrix input, PenaltyRules penaltyRule, int expected)
        {
            base.Test_against_reference_implementation(input, penaltyRule, expected);
        }
		
		//[Test]
        public void Generate()
        {
        	new Penalty1TestCaseFactory().GenerateTestDataSet();
        }
	}
}
