using System;
using System.IO;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.Terminate
{
	public sealed class TerminatorTestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromCsvFile
        {
            get
            {
                string path = Path.Combine(@"Terminate\TestCases", s_CSVFileName);
                using (var reader = File.OpenText(path))
                {
                	string header = reader.ReadLine();
                    while (!reader.EndOfStream)
                    {
                    	string line = reader.ReadLine();
                        string[] parts = line.Split(s_Semicolon[0]);
                        IEnumerable<bool> dataIenum = BitVectorTestExtensions.From01String(parts[0]);
                        BitList data = new BitList();
                        data.Add(dataIenum);
                        int numDataCodewords = int.Parse(parts[1]);
                        IEnumerable<bool> expected = BitVectorTestExtensions.From01String(parts[2]);
                        yield return new TestCaseData(data, numDataCodewords, expected);
                    }
                }
            }
		}
		
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				Random randomizer = new Random();
				for(int numTest = 0; numTest < 40; numTest++)
				{
					int numBits = randomizer.Next(8, 32);
					int numTotalBytes = (numBits >> 3) + randomizer.Next(1, 10);
					BitList data = new BitList();
					data.Add(1, numBits);
					IEnumerable<bool> result = TerminatorUsingReferenceImplementation(numBits, numTotalBytes);
					yield return new TestCaseData(data, numTotalBytes, result);
				}
			}
		}
		
		private const string s_Semicolon = ";";
		private const string s_DataList = "DataBitList";
		private const string s_NumOfDataCodewords = "NumDataCodeWords";
		private const string s_ExpectedResult = "ExpectedResult";
		private const string s_CSVFileName = "TerminatorTestCase.csv";
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_CSVFileName);
            using (var csvFile = File.CreateText(path))
            {
                string columnHeader = string.Join(s_Semicolon, s_DataList, s_NumOfDataCodewords, s_ExpectedResult);
                csvFile.WriteLine(columnHeader);
                
                foreach (TestCaseData testCaseData in TestCasesFromReferenceImplementation)
                {
                	BitList data = (BitList)testCaseData.Arguments[0];
                	string dataString = data.To01String();
                	int numDataCodewords = (int)testCaseData.Arguments[1];
                	IEnumerable<bool> expectResult = (IEnumerable<bool>)testCaseData.Arguments[2];
                	string expectString = expectResult.To01String();
                	csvFile.WriteLine(string.Join(s_Semicolon, dataString, numDataCodewords, expectString));
                }
                csvFile.Close();
            }
            
		}
		
		private IEnumerable<bool> TerminatorUsingReferenceImplementation(int numDataBits, int numTotalByte)
		{
			BitVector dataBits = new BitVector();
			dataBits.Append(1, numDataBits);
			EncoderInternal.terminateBits(numTotalByte, dataBits);
			return dataBits;
		}
	}
}
