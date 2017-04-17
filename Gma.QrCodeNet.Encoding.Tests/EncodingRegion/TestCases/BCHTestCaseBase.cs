using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	public abstract class BCHTestCaseBase
	{
		const string s_TestNameFormat = "Value: {0}, Expect: {1}";
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
            	Random randomizer = new Random();
            	for(int numCase = 0; numCase < 60; numCase++)
            	{
            		int cValue =  GenerateRandomInputNumber(randomizer); 
            		int expected =  ReferenceExpectedResult(cValue); 
            		yield return new TestCaseData(cValue, expected).SetName(string.Format(s_TestNameFormat, cValue, expected));
            	}
            }
		}
		
		
		protected abstract int ReferenceExpectedResult(int inputNum);
		protected abstract int GenerateRandomInputNumber(Random randomizer);
		
		protected abstract string FileName { get; }
		private const string s_InputNum = "INPUT_NUM";
		private const string s_ExpectValue = "EXPECT_VALUE";
		private const string s_Seperator = ";";
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), FileName);
			using(var csvFile = File.CreateText(path))
			{
				csvFile.WriteLine(string.Join(s_Seperator, s_InputNum, s_ExpectValue));
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					int inputValue = int.Parse(testCase.Arguments[0].ToString());
					int expectedValue = int.Parse(testCase.Arguments[1].ToString());
					csvFile.WriteLine(string.Join(s_Seperator, inputValue, expectedValue));
				}
				csvFile.Close();
			}
		}
		
		public IEnumerable<TestCaseData> TestCasesFromCsvFile
		{
			get
			{
				string path = Path.Combine(@"EncodingRegion\TestCases", FileName);
				using(var csvFile = File.OpenText(path))
				{
					csvFile.ReadLine();
					while(!csvFile.EndOfStream)
					{
						string[] values = csvFile.ReadLine().Split(s_Seperator[0]);
						int inputValue = int.Parse(values[0]);
						int expectedValue = int.Parse(values[1]);
						yield return new TestCaseData(inputValue, expectedValue).SetName(string.Format(s_TestNameFormat, inputValue, expectedValue));
					}
				}
			}
		}
		
	}
}
