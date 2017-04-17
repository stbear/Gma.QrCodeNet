using System;
using System.IO;
using System.Collections.Generic;
using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.Versions;
using NUnit.Framework;


namespace Gma.QrCodeNet.Encoding.Tests.Versions.TestCases
{
	public class VersionControlTestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromCsvFile
		{
			get
			{
				string path = Path.Combine(@"Versions\TestCases", s_CsvFileName);
				using(var reader = File.OpenText(path))
				{
					string header = reader.ReadLine();
					while(!reader.EndOfStream)
					{
						string line = reader.ReadLine();
						string[] parts = line.Split(s_Semicolon[0]);
						int numBits = int.Parse(parts[0]);
						int modeValue = int.Parse(parts[1]);
						int levelValue = int.Parse(parts[2]);
						string encodingName = parts[3];
						int expectVersionNum = int.Parse(parts[4]);
						yield return new TestCaseData(numBits, modeValue, levelValue, encodingName, expectVersionNum)
							.SetName(string.Format(s_VersionTestNameSet, numBits, (Mode)modeValue, (ErrorCorrectionLevel)levelValue, encodingName));
					}
				}
			}
		}
		
		
		
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			//bit length, mode, level, encoding name
			
			get
			{
				string[] encodingNames = new string[]{"iso-8859-1", "iso-8859-2"};
				Random randomizer = new Random();
				//Set to version 33. ECLevel = H
				int maxNumDataBits = (2611 - 1710) * 8;
				
				foreach(int modeValue in Enum.GetValues(typeof(Mode)))
				{
					
					foreach(int levelValue in Enum.GetValues(typeof(ErrorCorrectionLevel)))
					{
						foreach(string encodingName in encodingNames)
						{
							for(int i = 0; i < 15; i++)
							{
								int numDataBits = randomizer.Next(1, maxNumDataBits);
								
								yield return new TestCaseData(numDataBits, (Mode)modeValue, (ErrorCorrectionLevel)levelValue, encodingName)
									.SetName(string.Format(s_VersionTestNameSet, numDataBits, (Mode)modeValue, (ErrorCorrectionLevel)levelValue, encodingName));
							}
						}
					}
					
				}
			}
		}
		
		private const string s_VersionTestNameSet = "NumBits: {0} Mode: {1} Level: {2} Encoding: {3}";
		
		public IEnumerable<TestCaseData> GenerateTestCasesForDataset
		{
			get
			{
				foreach(VersionTestProperties testProperties in VersionTest.GenerateTestCase)
				{
					yield return new TestCaseData(testProperties.NumDataBitsForEncodedContent, (int)testProperties.Mode, (int)testProperties.ECLevel, testProperties.EncodingName, testProperties.ExpectVersionNum)
						.SetName(string.Format(s_VersionTestNameSet, testProperties.NumDataBitsForEncodedContent, testProperties.Mode, testProperties.ECLevel, testProperties.EncodingName));
				}
			}
		}
		
		private const string s_Semicolon = ";";
		private const string s_NumDataBitsName = "DataBitsWithoutHeader";
		private const string s_ModeName = "Mode";
		private const string s_ECLevel = "ErrorCorrectionLevel";
		private const string s_EncodingName = "EncodingName";
		private const string s_ExpectName = "ExpectVersionValue";
		private const string s_CsvFileName = "VersionControlTest.csv"; 
		
		public virtual void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_CsvFileName);
			using(var csvFile = File.CreateText(path))
			{
				string columnHeader = string.Join(s_Semicolon, s_NumDataBitsName, s_ModeName, s_ECLevel, s_EncodingName, s_ExpectName);
				csvFile.WriteLine(columnHeader);
				
				foreach(TestCaseData testCaseData in GenerateTestCasesForDataset)
				{
					int numDataBits = int.Parse(testCaseData.Arguments[0].ToString());
					int modeValue = int.Parse(testCaseData.Arguments[1].ToString());
					int ecLevel = int.Parse(testCaseData.Arguments[2].ToString());
					string encodingName = testCaseData.Arguments[3].ToString();
					int expectVersionName = int.Parse(testCaseData.Arguments[4].ToString());
					csvFile.WriteLine(string.Join(s_Semicolon, numDataBits, modeValue, ecLevel, encodingName, expectVersionName));
				}
				csvFile.Close();
			}
		}
		
	}
}
