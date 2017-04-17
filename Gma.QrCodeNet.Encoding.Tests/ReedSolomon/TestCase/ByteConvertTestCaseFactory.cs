using System;
using System.IO;
using System.Collections.Generic;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	public sealed class ByteConvertTestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				Random randomizor = new Random();
				for(int numTest = 0; numTest < 40; numTest++)
				{
					int arraySize = randomizor.Next(1, 40);
					byte[] testArray = new byte[arraySize];
					for(int index = 0; index < arraySize; index++)
					{
						testArray[index] = (byte)randomizor.Next(0, 256);
					}
					yield return new TestCaseData(testArray);
				}
			}
		}
		
		
		private const string s_Semicolon = ";";
		
		private const string s_TxtFileName = "ByteConvertTestCase.txt"; 
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
			using(var txtFile = File.CreateText(path))
			{
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					byte[] testArray = (byte[])testCase.Arguments[0];
					txtFile.WriteLine(string.Join(s_Semicolon, testArray));
				}
				
				txtFile.Close();
			}
			
		}
		
		public IEnumerable<TestCaseData> TestCaseFromTxtFile
		{
			get
			{
				string path = Path.Combine(@"ReedSolomon\TestCase", s_TxtFileName);
				using(var txtFile = File.OpenText(path))
				{
					while (!txtFile.EndOfStream)
					{
						string line = txtFile.ReadLine();
						string[] strArray = line.Split(s_Semicolon[0]);
						byte[] testArray = PolynomialExtensions.ConvertToByte(strArray);
						yield return new TestCaseData(testArray);
					}
				}
			}
		}
		
		
	}
}
