using System;
using System.Collections.Generic;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Common;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Positioning;
using Gma.QrCodeNet.Encoding.EncodingRegion;
using Gma.QrCodeNet.Encoding.Masking;
using com.google.zxing.qrcode.decoder;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	public class FormatInfoTestCaseFactory
	{
		const string s_TestNameFormat = "Size: {0}, Vresion: {1}";
		
		public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
            	for(int version = 1; version <= 20; version++)
            	{
            		for(int pattern = 0; pattern <= 3; pattern++)
            			yield return GenerateRandomTestCaseData(version, (MaskPatternType)pattern);
            	}
            }
		}
		
		
		
		private TestCaseData GenerateRandomTestCaseData(int version, MaskPatternType patternType)
        {
			int matrixSize = VersionDetail.Width(version);
			ByteMatrix matrix = new ByteMatrix(matrixSize, matrixSize);
			EmbedAlignmentPattern(matrix, version, patternType);
			return new TestCaseData(version, patternType, matrix.ToBitMatrix()).SetName(string.Format(s_TestNameFormat, matrixSize, version));
		}
		
		private void EmbedAlignmentPattern(ByteMatrix matrix, int version, MaskPatternType patterntype)
        {
            matrix.Clear(-1);
            MatrixUtil.embedBasicPatterns(version, matrix);
            MatrixUtil.embedTypeInfo(ErrorCorrectionLevelInternal.H, (int)patterntype, matrix);
        }
		
		private const string s_TxtFileName = "FormatInfoTestCase.txt";
		
		public void GenerateMaskPatternTestDataSet()
        {
            string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
            using (var file = File.CreateText(path))
            {
                foreach (TestCaseData testCaseData in TestCasesFromReferenceImplementation)
                {
                    string version = testCaseData.Arguments[0].ToString();
                    file.WriteLine(version);
                    file.WriteLine((int)(MaskPatternType)testCaseData.Arguments[1]);
                    TriStateMatrixToGraphicExtensions.ToGraphic(((TriStateMatrix)testCaseData.Arguments[2]), file);
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
                        int pattern = int.Parse(file.ReadLine());
                        TriStateMatrix expected = TriStateMatrixToGraphicExtensions.FromGraphic(file);
                        yield return new TestCaseData(version, (MaskPatternType)pattern, expected).SetName(string.Format(s_TestNameFormat, expected.Width, version));
                    }
                }
            }
        }
	}
}
