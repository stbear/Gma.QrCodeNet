using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gma.QrCodeNet.Encoding.Tests._Helper;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public class EndToEndSmokeTestCaseFactory
    {
        public IEnumerable<TestCaseData> TestCasesFromCsvFile
        {
            get
            {
                string path = Path.Combine("EndToEnd\\TestCases", s_CsvFileName);
                using (var reader = File.OpenText(path))
                {
                    string header = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                        string line = reader.ReadLine();
                        string[] parts = line.Split(s_Semicolon);
                        string input = parts[0];
                        ErrorCorrectionLevel level = (ErrorCorrectionLevel) Enum.Parse(typeof(ErrorCorrectionLevel), parts[1]);
                        BitMatrix matrix = BitMatrixTestExtensions.FromBase64(parts[2]);
                        yield return new TestCaseData(input, level, matrix);
                    }
                }
                yield break;
            }
        }

        public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
                Random randomizer = new Random();
                int[] testInputSizes = new[] { 1, 10, 25, 36, 73, 111, 174, 255 };

                foreach (int inputSize in testInputSizes)
                {
                    string inputString = GenerateRandomString(inputSize, randomizer);
                    foreach (ErrorCorrectionLevel level in Enum.GetValues(typeof(ErrorCorrectionLevel)))
                    {
                    	BitMatrix referenceMatrix = DataEncodeExtensions.Encode(inputString, level);
                        yield return new TestCaseData(inputString, level, referenceMatrix);
                    }
                }
            }
        }


        private const char s_Semicolon = ';';
        private const string s_InputStringColumnName = "InputString";
        private const string s_ErrorCorrectionLevelColumnName = "ErrorCorrectionLevel";
        private const string s_MatrixBitsColumnName = "MatrixBits";
        private const string s_CsvFileName = "DataSet1.csv";

        public void RecordToFile()
        {
            string path = Path.Combine(Path.GetTempPath(), s_CsvFileName);
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon.ToString(), s_InputStringColumnName, s_ErrorCorrectionLevelColumnName, s_MatrixBitsColumnName);
                csvFile.WriteLine(columnHeader);

                foreach (TestCaseData testCase in TestCasesFromReferenceImplementation )
                {
                    string inputString = testCase.Arguments[0].ToString();
                    ErrorCorrectionLevel level = (ErrorCorrectionLevel) testCase.Arguments[1];
                    BitMatrix matrix = (BitMatrix) testCase.Arguments[2];
                    csvFile.WriteLine(string.Join(s_Semicolon.ToString(), inputString, level, matrix.ToBase64()));
                }
                csvFile.Close();
            }
        }

        private static string GenerateRandomString(int inputSize, Random randomizer)
        {
            const char startCharCode = '(';
            const char endCharCode = '~';
            StringBuilder result = new StringBuilder(inputSize);
            for (int i = 0; i < inputSize; i++)
            {
                char randomChar;
                do
                {
                    randomChar = (char)randomizer.Next(startCharCode, endCharCode);
                } while (randomChar == s_Semicolon);

                result.Append(randomChar);
            }
            return result.ToString();
        }


    }
}