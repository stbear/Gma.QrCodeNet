using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.common.reedsolomon;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	public sealed class GaloisField256TestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				GF256 field = GF256.QR_CODE_FIELD;
				for(int i = 0; i < 256; i++)
				{
					yield return new TestCaseData(i, field.exp(i));
				}
			}
		}
		
		private const string s_Semicolon = ";";
		private const string s_Power = "POWER";
		private const string s_Exponent = "Exponent";
		private const string s_TxtFileName = "GaloisField256TestCase.csv";
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
			using(var csvFile = File.CreateText(path))
			{
				csvFile.WriteLine(string.Join(s_Semicolon, s_Power, s_Exponent));
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					int power = int.Parse(testCase.Arguments[0].ToString());
					int exponent = int.Parse(testCase.Arguments[1].ToString());
					csvFile.WriteLine(string.Join(s_Semicolon, power, exponent));
				}
				csvFile.Close();
			}
		}
		
		public IEnumerable<TestCaseData> TestCaseFromCSVFile
		{
			get
			{
				string path = Path.Combine(@"ReedSolomon\TestCase", s_TxtFileName);
				using(var csvFile = File.OpenText(path))
				{
					string header = csvFile.ReadLine();
					while (!csvFile.EndOfStream)
					{
						string line = csvFile.ReadLine();
						string[] parts = line.Split(s_Semicolon[0]);
						int power = int.Parse(parts[0]);
						int exponent = int.Parse(parts[1]);
						yield return new TestCaseData(power, exponent);
					}
				}
			}
		}
		
	}
}
