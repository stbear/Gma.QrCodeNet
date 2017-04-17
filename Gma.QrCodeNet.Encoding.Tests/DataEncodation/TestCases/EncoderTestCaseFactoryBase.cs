using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Tests._Helper;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Mode = com.google.zxing.qrcode.decoder.Mode;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.DataEncodation
{
    public abstract class EncoderTestCaseFactoryBase
    {

        public IEnumerable<TestCaseData> TestCasesFromCsvFile
        {
            get
            {
            	return TestCasesCSVFile("encoder");
            }
        }
        
        public IEnumerable<TestCaseData> TestCasesDataEncodeFromCsvFile
        {
            get
            {
				return TestCasesCSVFile("dataencode");
            }
        }
        
        private IEnumerable<TestCaseData> TestCasesCSVFile(string option)
        {
        	string fileName = option == "encoder" ? CsvFileName : DataEncodeCsvFile;
        	string path = Path.Combine("DataEncodation\\TestCases", fileName);
            using (var reader = File.OpenText(path))
            {
                string header = reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    string line = reader.ReadLine();
                    string[] parts = line.Split(s_Semicolon[0]);
                    string input = parts[0];
                    IEnumerable<bool> expected = BitVectorTestExtensions.From01String(parts[1]);
                    yield return new TestCaseData(input, expected);
                }
            }
        }

        public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
                Random randomizer = new Random();
                int[] testInputSizes = new[] { 0, 1, 10, 25, 36, 73, 111, 174, 255 };

                return TestCasesReferenceGenerator(randomizer, testInputSizes, "encoder");
                
            }
        }
        
        public IEnumerable<TestCaseData> TestCasesDataEncodeReferenceImplementation
        {
        	get
            {
                Random randomizer = new Random();
                int[] testInputSizes = new[] { 1, 10, 25, 36, 73, 111, 174, 255 };

                return TestCasesReferenceGenerator(randomizer, testInputSizes, "dataencode");
                
            }
        }
        
        private IEnumerable<TestCaseData> TestCasesReferenceGenerator(Random randomizer, int[] testInputSizes, string option)
        {
        	foreach (int inputSize in testInputSizes)
            {
                string inputString = GenerateRandomInputString(inputSize, randomizer);
               	QRCodeInternal qrInternal;
                IEnumerable<bool> result = option == "encoder" ? 
                	EncodeUsingReferenceImplementation(inputString) :
                	DataEncodeExtensions.DataEncodeUsingReferenceImplementation(inputString, ErrorCorrectionLevel.H, out qrInternal);
                yield return new TestCaseData(inputString, result);
            }
        }

        private const string s_Semicolon = ";";
        private const string s_InputStringColumnName = "InputString";
        private const string s_ExpectedResultColumnName = "ExpectedResult";
        protected abstract string CsvFileName { get; }
        protected abstract string DataEncodeCsvFile { get; }

        public virtual void GenerateTestDataSet(string stroption)
        {
         
        	string fileName = "";
        	switch(stroption)
        	{
        		case "encoder":
        			fileName = CsvFileName;
        			break;
        		case "dataencode":
        			fileName = DataEncodeCsvFile;
        			break;
        		default:
        			throw new ArgumentOutOfRangeException("option", stroption, string.Format("No such option: {0}", stroption));
        	}
            string path = Path.Combine(Path.GetTempPath(), fileName);
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon, s_InputStringColumnName, s_ExpectedResultColumnName);
                csvFile.WriteLine(columnHeader);

				switch(stroption)
        		{
        			case "encoder":
						InputDataSetToFile(TestCasesFromReferenceImplementation, csvFile);
        				break;
        			case "dataencode":
        				InputDataSetToFile(TestCasesDataEncodeReferenceImplementation, csvFile);
        				break;
        			default:
        				throw new ArgumentOutOfRangeException("option", stroption, string.Format("No such option: {0}", stroption));
        		}
				
                csvFile.Close();
            }
        }
        
        private void InputDataSetToFile(IEnumerable<TestCaseData> testCases, TextWriter output)
        {
        	foreach (TestCaseData testCaseData in testCases)
            {
                string inputString = testCaseData.Arguments[0].ToString();
                IEnumerable<bool> result = (IEnumerable<bool>)testCaseData.Arguments[1];
                output.WriteLine(string.Join(s_Semicolon, inputString, result.To01String()));
            }
        }

        protected abstract string GenerateRandomInputString(int inputSize, Random randomizer);

        protected string GenerateRandomInputString(int inputSize, Random randomizer, char startCharCode, char endCharCode)
        {
            StringBuilder result = new StringBuilder(inputSize);
            for (int i = 0; i < inputSize; i++)
            {
                char randomChar;
                do
                {
                    randomChar = (char)randomizer.Next(startCharCode, endCharCode);
                } while (randomChar == s_Semicolon[0]);

                result.Append(randomChar);
            }
            return result.ToString();
        }
        

        protected abstract IEnumerable<bool> EncodeUsingReferenceImplementation(string content);

        protected virtual IEnumerable<bool> EncodeUsingReferenceImplementation(string content, Mode mode)
        {
            // Step 2: Append "bytes" into "dataBits" in appropriate encoding.
            BitVector dataBits = new BitVector();
            EncoderInternal.appendBytes(content, mode, dataBits, null);


            return dataBits;
        }

    }
}
