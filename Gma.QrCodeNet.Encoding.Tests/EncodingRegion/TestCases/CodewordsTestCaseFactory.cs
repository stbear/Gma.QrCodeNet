using System;
using System.Collections.Generic;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Common;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.EncodingRegion;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	public sealed class CodewordsTestCaseFactory
	{
		const string s_TestNameFormat = "Size: {0}, Vresion: {1}";
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
            	Random randomizer = new Random();
            	for(int version = 1; version <= 40; version++)
            	{
            		int numData = VersionTable.GetVersionByNum(version).TotalCodewords;
            		yield return GenerateRandomTestCaseData(version, numData, randomizer);
            	}
            }
		}
		
		
		
		private TestCaseData GenerateRandomTestCaseData(int version, int totalCodewords, Random randomizer)
        {
			int matrixSize = VersionDetail.Width(version);
			ByteMatrix matrix = new ByteMatrix(matrixSize, matrixSize);
			BitVector codewords = GenerateDataCodewords(totalCodewords, randomizer);
			EmbedAlignmentPattern(matrix, version, codewords);
			return new TestCaseData(version, matrix.ToBitMatrix(), codewords).SetName(string.Format(s_TestNameFormat, matrixSize, version));
		}
		
		private void EmbedAlignmentPattern(ByteMatrix matrix, int version, BitVector codewords)
        {
            matrix.Clear(-1);
            MatrixUtil.embedBasicPatterns(version, matrix);
            MatrixUtil.embedDataBits(codewords, -1, matrix);
        }
		
		private const int s_bitLengthForByte = 8;
		
		private BitVector GenerateDataCodewords(int numDataCodewords, Random randomizer)
		{
			BitVector result = new BitVector();
			for(int numDC = 0; numDC < numDataCodewords; numDC++)
			{
				result.Append((randomizer.Next(0, 256) & 0xFF), s_bitLengthForByte);
			}
			if(result.sizeInBytes() == numDataCodewords)
				return result;
			else
				throw new Exception("Auto generate data codewords fail");
		}
		
		private const string s_TxtFileName = "CodewordsTestCase.txt";
		
		public void GenerateMaskPatternTestDataSet()
        {
            string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
            using (var file = File.CreateText(path))
            {
                foreach (TestCaseData testCaseData in TestCasesFromReferenceImplementation)
                {
                    string version = testCaseData.Arguments[0].ToString();
                    file.WriteLine(version);
                    TriStateMatrixToGraphicExtensions.ToGraphic(((TriStateMatrix)testCaseData.Arguments[1]), file);
                    IEnumerable<bool> ienumBool = (IEnumerable<bool>)testCaseData.Arguments[2];
                    file.WriteLine(ienumBool.To01String());
                }
                file.Close();
            }
        }
		 
		 
		public IEnumerable<TestCaseData> TestCasesFromTxtFile
        {
            get
            {
                string path = Path.Combine(@"EncodingRegion\TestCases", s_TxtFileName);
                using (var file = File.OpenText(path))
                {
                    while (!file.EndOfStream)
                    {
                        int version = int.Parse(file.ReadLine());
                        TriStateMatrix expected = TriStateMatrixToGraphicExtensions.FromGraphic(file);
                        string line = file.ReadLine();
                        IEnumerable<bool> codewords = BitVectorTestExtensions.From01String(line);
                        yield return new TestCaseData(version, expected, codewords).SetName(string.Format(s_TestNameFormat, expected.Width, version));
                    }
                }
            }
        }
		
	}
}
