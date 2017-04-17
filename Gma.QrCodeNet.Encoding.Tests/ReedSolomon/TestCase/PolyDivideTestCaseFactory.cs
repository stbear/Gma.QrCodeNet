using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.common.reedsolomon;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	public sealed class PolyDivideTestCaseFactory
	{
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				Random randomizer = new Random();
				for(int i = 0; i < 40; i++)
				{
					int alength = randomizer.Next(1, 30);
					int blength = randomizer.Next(1, 30);
					int[] aCoefficient = PolynomialExtensions.GenerateCoeff(alength, randomizer);
					int[] bCoefficient = PolynomialExtensions.GenerateCoeff(blength, randomizer);
					GF256 field = GF256.QR_CODE_FIELD;
					GF256Poly aPoly = new GF256Poly(field, aCoefficient);
					GF256Poly bPoly = new GF256Poly(field, bCoefficient);
					
					GF256Poly[] polyArray = aPoly.divide(bPoly);
					
					int[] quotient = polyArray[0].Coefficients;
					int[] remainder = polyArray[1].Coefficients;
					
					yield return new TestCaseData(aCoefficient, bCoefficient, quotient, remainder);
				}
				
			}
			
		}
		
		private const string s_Semicolon = ";";
		
		private const string s_TxtFileName = "PolyDivideTestCase.txt"; 
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
			using(var txtFile = File.CreateText(path))
			{
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					for(int index = 0; index < 4; index++)
					{
						int[] testArray = (int[])testCase.Arguments[index];
						txtFile.WriteLine(string.Join(s_Semicolon, testArray));
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
						List<int[]> testCase = new List<int[]>();
						for(int numElement = 0; numElement < 4; numElement++)
						{
							string line = txtFile.ReadLine();
							string[] strArray = line.Split(s_Semicolon[0]);
							int[] testArray = PolynomialExtensions.ConvertToInt(strArray);
							testCase.Add(testArray);
						}
						yield return new TestCaseData(testCase[0], testCase[1], testCase[2], testCase[3]);
					}
				}
			}
		}
		
	}
}
