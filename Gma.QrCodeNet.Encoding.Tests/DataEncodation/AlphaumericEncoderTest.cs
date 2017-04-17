using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.DataEncodation;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    [TestFixture]
    public class AlphaumericEncoderTest : EncoderTestBase
    {
        [Test, TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public override void Test_against_reference_implementation(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_reference_implementation(inputString, expected);
        }

        [Test, TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesFromCsvFile")]
        public override void Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.Test_against_csv_DataSet(inputString, expected);
        }
        
        [Test, TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesDataEncodeReferenceImplementation")]
        public override void DataEncode_Test_against_reference_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }
        
        [Test, TestCaseSource(typeof(AlphanumericEncoderTestCaseFactory), "TestCasesDataEncodeFromCsvFile")]
        public override void DataEncode_Test_against_csv_DataSet(string inputString, IEnumerable<bool> expected)
        {
            base.DataEncode_Test_against_reference_DataSet(inputString, expected);
        }

        protected override EncoderBase CreateEncoder()
        {
            return new AlphanumericEncoder();
        }
        
        //[Test]
        public void Generate()
        {
            new AlphanumericEncoderTestCaseFactory().GenerateTestDataSet("encoder");
        }
        
        //[Test]
        public void DataEncodeGenerate()
        {
            new AlphanumericEncoderTestCaseFactory().GenerateTestDataSet("dataencode");
        }
    }
}
