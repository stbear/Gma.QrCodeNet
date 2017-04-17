using System;
using System.Collections.Generic;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Common;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.EncodingRegion;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	public sealed class VersionInfoTestCaseFactory
	{
		const string s_TestNameFormat = "Size: {0}, Vresion: {1}";
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
            	for(int version = 1; version <= 40; version++)
            	{
            		yield return GenerateRandomTestCaseData(version);
            	}
            }
		}
		
		
		
		private TestCaseData GenerateRandomTestCaseData(int version)
        {
			int matrixSize = VersionDetail.Width(version);
			ByteMatrix matrix = new ByteMatrix(matrixSize, matrixSize);
			EmbedAlignmentPattern(matrix, version);
			return new TestCaseData(version, matrix.ToBitMatrix()).SetName(string.Format(s_TestNameFormat, matrixSize, version));
		}
		
		private void EmbedAlignmentPattern(ByteMatrix matrix, int version)
        {
            matrix.Clear(-1);
            MatrixUtil.embedBasicPatterns(version, matrix);
            MatrixUtil.maybeEmbedVersionInfo(version, matrix);
        }
		
		
		private const string s_TxtFileName = "VersionInfoTestCase.txt";
		
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
                        yield return new TestCaseData(version, expected).SetName(string.Format(s_TestNameFormat, expected.Width, version));
                    }
                }
            }
        }
	}
}
