using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using com.google.zxing.common.reedsolomon;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ReedSolomon
{
	public sealed class PolyXorMultyTestCaseFactory
	{
		private const int s_Primitive = 0x011D;
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
		{
			get
			{
				Random randomizer = new Random();
				string[] mathOption = new string[]{ "xor", "multy" };
				foreach( string option in mathOption)
				{
					for(int i = 0; i < 20; i++)
					{
						int alength = randomizer.Next(1, 30);
						int blength = randomizer.Next(1, 30);
						int[] aCoefficient = PolynomialExtensions.GenerateCoeff(alength, randomizer);
						int[] bCoefficient = PolynomialExtensions.GenerateCoeff(blength, randomizer);
						int[] expectCoefficient = MathForReferenceImplementation(aCoefficient, bCoefficient, option);
						
						yield return new TestCaseData(aCoefficient, bCoefficient, option, expectCoefficient);
					}
				}
			}
		}
		
		
		private int[] MathForReferenceImplementation(int[] aCoeff, int[] bCoeff, string option)
		{
			GF256 field = GF256.QR_CODE_FIELD;
			GF256Poly aPoly = new GF256Poly(field, aCoeff);
			GF256Poly bPoly = new GF256Poly(field, bCoeff);
			
			switch(option)
			{
				case "xor":
					return aPoly.addOrSubtract(bPoly).Coefficients;
				case "multy":
					return aPoly.multiply(bPoly).Coefficients;
				default:
					throw new ArgumentException("No such test option");
			}
			        
			
		}
		
		private const string s_Semicolon = ";";
		
		private const string s_TxtFileName = "PolyXorMultyTestCase.txt"; 
		
		public void GenerateTestDataSet()
		{
			string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
			using(var txtFile = File.CreateText(path))
			{
				foreach(TestCaseData testCase in TestCasesFromReferenceImplementation)
				{
					for(int index = 0; index < 4; index++)
					{
						if(index == 2)
						{
							string option = testCase.Arguments[index].ToString();
							txtFile.WriteLine(string.Join(s_Semicolon, option));
						}
						else
						{
							int[] testArray = (int[])testCase.Arguments[index];
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
						List<int[]> testCase = new List<int[]>();
						string option = "";
						for(int numElement = 0; numElement < 4; numElement++)
						{
							string line = txtFile.ReadLine();
							string[] strArray = line.Split(s_Semicolon[0]);
							if(numElement == 2)
							{
								option = strArray[0];
							}
							else
							{
								int[] testArray = PolynomialExtensions.ConvertToInt(strArray);
								testCase.Add(testArray);
							}
						}
						yield return new TestCaseData(testCase[0], testCase[1], option, testCase[2]);
					}
				}
			}
		}
		
	}
}
