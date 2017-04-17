using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gma.QrCodeNet.Encoding.Tests._Helper;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	public class CodewordsBitListTestCaseFactory
	{
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
                    	IEnumerable<bool> referencebits = DataEncodeExtensions.Codeword(inputString, level);
                        yield return new TestCaseData(inputString, level, referencebits);
                    }
                }
            }
        }
		
		private const char s_Semicolon = ';';
		 
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
