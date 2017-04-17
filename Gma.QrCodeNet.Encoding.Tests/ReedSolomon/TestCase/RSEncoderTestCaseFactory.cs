using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	/// <summary>
	/// Description of ReedSolomonEncoderTestCaseFactory.
	/// </summary>
	public sealed class RSEncoderTestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				Random randomizer = new Random();
				for(int i = 0; i < 40; i++)
				{
					int dataLength = randomizer.Next(1, 60);
					int ecLength = randomizer.Next(1, dataLength);
					
					sbyte[] data = PolynomialExtensions.GenerateSbyteArray(dataLength, randomizer);
					
					sbyte[] ecData = EncoderInternal.generateECBytes(data, ecLength);
					
					yield return new TestCaseData(PolynomialExtensions.ToByteArray(data), ecLength, PolynomialExtensions.ToByteArray(ecData));
				}
			}
		}
		
		private const string s_Semicolon = ";";
		
		private const string s_TxtFileName = "RSEncoderTestCase.txt"; 
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
			using(var txtFile = File.CreateText(path))
			{
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					for(int index = 0; index < 3; index++)
					{
						if(index == 1)
						{
							int ecLength = (int)testCase.Arguments[index];
							txtFile.WriteLine(string.Join(s_Semicolon, ecLength));
						}
						else
						{
							byte[] testArray = (byte[])testCase.Arguments[index];
							txtFile.WriteLine(string.Join(s_Semicolon, testArray));
						}
						
					}
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
						List<byte[]> testCase = new List<byte[]>();
						int ecLength = 0;
						for(int numElement = 0; numElement < 3; numElement++)
						{
							string line = txtFile.ReadLine();
							string[] strArray = line.Split(s_Semicolon[0]);
							if(numElement == 1)
							{
								ecLength = int.Parse(strArray[0]);
							}
							else
							{
								byte[] testArray = PolynomialExtensions.ConvertToByte(strArray);
								testCase.Add(testArray);
							}
						}
						yield return new TestCaseData(testCase[0], ecLength, testCase[1]);
					}
				}
			}
		}
		
		
		
	}
}
