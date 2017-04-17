using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
	[TestFixture]
	public class KanjiEncoderTest : EncoderTestBase
	{
		[Test, TestCaseSource(typeof(KanjiEncoderTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public override void Test_against_reference_implementation(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_reference_implementation(inputString, expected);
        }

        [Test, TestCaseSource(typeof(KanjiEncoderTestCaseFactory), "TestCasesFromCsvFile")]
        public override void Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_csv_DataSet(inputString, expected);
        }
        
        [Test, TestCaseSource(typeof(KanjiEncoderTestCaseFactory), "TestCasesDataEncodeReferenceImplementation")]
        public override void DataEncode_Test_against_reference_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }
        
        [Test, TestCaseSource(typeof(KanjiEncoderTestCaseFactory), "TestCasesDataEncodeFromCsvFile")]
        public override void DataEncode_Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }

        protected override EncoderBase CreateEncoder()
        {
            return new KanjiEncoder();
        }
        
        //[Test]
        public void Generate()
        {
            new KanjiEncoderTestCaseFactory().GenerateTestDataSet("encoder");
        }
        
        //[Test]
        public void DataEncodeGenerate()
        {
            new KanjiEncoderTestCaseFactory().GenerateTestDataSet("dataencode");
        }
	}
}
