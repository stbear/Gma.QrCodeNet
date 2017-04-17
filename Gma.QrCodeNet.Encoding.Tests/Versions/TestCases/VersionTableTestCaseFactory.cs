using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.Tests.Versions.TestCases
{
	public class VersionTableTestCaseFactory
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
						
						int versionNum = int.Parse(parts[0]);
						int numTotalCodewords = int.Parse(parts[1]);
						int ecLevel = int.Parse(parts[2]);
						int numECCodewords = int.Parse(parts[3]);
						string ecBlockString = parts[4];
						yield return new TestCaseData(versionNum, numTotalCodewords, ecLevel, numECCodewords, ecBlockString)
							.SetName(string.Format(s_VersionTableNameSet, versionNum, ecLevel));
					}
					
				}
			}
		}
		
		
		
		private const string s_VersionTableNameSet = "Version: {0} ErrorCorrectionLevel: {1}";
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				foreach(VersionTableTestProperties properties in VersionTableTest.GetZXingVersionTable)
				{
					yield return new TestCaseData(properties.VersionNum, properties.TotalNumOfCodewords, (int)properties.ErrorCorrectionLevel, properties.NumOfECCodewords, properties.ECBlockString)
						.SetName(string.Format(s_VersionTableNameSet, properties.VersionNum, properties.ErrorCorrectionLevel));
				}
			}
		}
		
		
		private const string s_Semicolon = ";";
		private const string s_CsvFileName = "VersionTableTest.csv";
		private const string s_Version = "VersionNum";
		private const string s_NumTotalCodewords = "NumOfTotalCodewords";
		private const string s_ErrorCorrectionLevel = "ErrorCorrectionLevel";
		private const string s_NumECCodewords = "NumOfErrorCorrectionCodewords";
		private const string s_ECBlockString = "Errorblock-DataCodewords-Repeat";

		public virtual void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_CsvFileName);
			using(var csvFile = File.CreateText(path))
			{
				string columnHeader = string.Join(s_Semicolon, s_Version, s_NumTotalCodewords, s_ErrorCorrectionLevel, s_NumECCodewords, s_ECBlockString);
				csvFile.WriteLine(columnHeader);
				
				foreach(TestCaseData testCaseData in TestCasesFromReferenceImplementation)
				{
					int versionNum = int.Parse(testCaseData.Arguments[0].ToString());
					int numTotalCodewords = int.Parse(testCaseData.Arguments[1].ToString());
					int ecLevel = int.Parse(testCaseData.Arguments[2].ToString());
					int numECCodewords = int.Parse(testCaseData.Arguments[3].ToString());
					string ecBlockString = testCaseData.Arguments[4].ToString();
					
					csvFile.WriteLine(string.Join(s_Semicolon, versionNum, numTotalCodewords, ecLevel, numECCodewords, ecBlockString));
				}
				csvFile.Close();
			}
		}
		
	}
}
