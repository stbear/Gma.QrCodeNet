using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
	[TestFixture]
	public class EightBitByteEncoderTest : EncoderTestBase
	{
        [Test, TestCaseSource(typeof(EightBitByteEncoderTestCaseFactory), "TestCasesFromReferenceImplementation")]
		public override void Test_against_reference_implementation(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_reference_implementation(inputString, expected);
        }
		
		[Test, TestCaseSource(typeof(EightBitByteEncoderTestCaseFactory), "TestCasesFromCsvFile")]
		public override void Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_csv_DataSet(inputString, expected);
        }
		
		[Test, TestCaseSource(typeof(EightBitByteEncoderTestCaseFactory), "TestCasesDataEncodeReferenceImplementation")]
        public override void DataEncode_Test_against_reference_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }
        
        [Test, TestCaseSource(typeof(EightBitByteEncoderTestCaseFactory), "TestCasesDataEncodeFromCsvFile")]
        public override void DataEncode_Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }
		
		protected override EncoderBase CreateEncoder()
        {
            return new EightBitByteEncoder("shift_jis");
        }

        //[Test]
        public void Generate()
        {
            new EightBitByteEncoderTestCaseFactory().GenerateTestDataSet("encoder");
        }
        
        //[Test]
        public void DataEncodeGenerate()
        {
            new EightBitByteEncoderTestCaseFactory().GenerateTestDataSet("dataencode");
        }
	}
}
