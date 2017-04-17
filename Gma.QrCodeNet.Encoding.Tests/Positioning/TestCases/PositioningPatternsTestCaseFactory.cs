using System.Collections.Generic;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Common;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Positioning;

namespace Gma.QrCodeNet.Encoding.Tests.Positioning.TestCases
{
    public class PositioningPatternsTestCaseFactory
    {
        const string s_TestNameFormat = "Size: {0}, Vresion: {1}";
        public IEnumerable<TestCaseData> TestCasesFromTxtFile
        {
            get
            {
                string path = Path.Combine("Positioning\\TestCases", s_TxtFileName);
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

        public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
                int[] sizeByVersion = new[] { -1, 21, 25, 29, 33, 37, 41, 45, 49, 53, 57, 61, 65, 69, 73, 77, 81, 85, 89, 93, 97, 101, 105, 109, 113, 117, 121, 125, 129, 133, 137, 141, 145, 149, 153, 157, 161, 165, 169, 173, 177 };
                for (int version = 1; version <= 40; version++)
                {
                    int matrixSize = sizeByVersion[version];
                    yield return GenerateRandomTestCaseData(matrixSize, version).SetName(string.Format(s_TestNameFormat, matrixSize, version));
                }
            }
        }

        private TestCaseData GenerateRandomTestCaseData(int matrixSize, int version)
        {
            ByteMatrix matrix = new ByteMatrix(matrixSize, matrixSize);
            EmbedAlignmentPattern(matrix, version);
            return new TestCaseData(version, matrix.ToBitMatrix());
        }


        private const string s_TxtFileName = "PositioningPatternsTestDataSet.txt";

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

        private void EmbedAlignmentPattern(ByteMatrix matrix, int version)
        {
            matrix.Clear(-1);
            MatrixUtil.embedBasicPatterns(version, matrix);
        }
    }
}
