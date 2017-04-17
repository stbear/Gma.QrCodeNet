using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests
{
    [TestFixture]
    public class EndToEndSmokeTests
    {

        [Test]
        [TestCaseSource(typeof(EndToEndSmokeTestCaseFactory), "TestCasesFromReferenceImplementation")]
        public void Test_against_reference_implementation(string inputData, ErrorCorrectionLevel errorCorrectionLevel, BitMatrix expectedMatrix)
        {
            QrEncoder encoder = new QrEncoder(errorCorrectionLevel);
            BitMatrix resultMatrix = encoder.Encode(inputData).Matrix;
            expectedMatrix.AssertEquals(resultMatrix);
        }

        [Test]
        [TestCaseSource(typeof(EndToEndSmokeTestCaseFactory), "TestCasesFromCsvFile")]
        public void Test_against_csv_DataSet(string inputData, ErrorCorrectionLevel errorCorrectionLevel, BitMatrix expectedMatrix)
        {
            QrEncoder encoder = new QrEncoder(errorCorrectionLevel);
            BitMatrix resultMatrix = encoder.Encode(inputData).Matrix;
            expectedMatrix.AssertEquals(resultMatrix);
        }
        
        //[Test]
        public void Generate()
        {
        	new EndToEndSmokeTestCaseFactory().RecordToFile();
        }
    }
}
