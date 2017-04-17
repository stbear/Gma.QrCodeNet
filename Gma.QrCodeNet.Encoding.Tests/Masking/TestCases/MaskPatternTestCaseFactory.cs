using System;
using System.Collections.Generic;
using System.IO;
using com.google.zxing.qrcode.encoder;
using com.google.zxing.qrcode.decoder;
using Gma.QrCodeNet.Encoding.Common;
using Gma.QrCodeNet.Encoding.Masking;
using Gma.QrCodeNet.Encoding.Positioning;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.Masking
{
    public class MaskPatternTestCaseFactory
    {
        public IEnumerable<TestCaseData> TestCasesFromTxtFile
        {
            get
            {
                string path = Path.Combine("Masking\\TestCases", s_TxtFileName);
                using (var file = File.OpenText(path))
                {
                    while (!file.EndOfStream)
                    {
                        BitMatrix input = BitMatrixToGraphicExtensions.FromGraphic(file);
                        TriStateMatrix inputT = input.ToTriStateMatrix();
                        int pattern = int.Parse(file.ReadLine());
                        BitMatrix expected = BitMatrixToGraphicExtensions.FromGraphic(file);
                        yield return new TestCaseData(inputT, pattern, expected).SetName(string.Format(s_TestNameFormat, input.Width, "?", pattern));
                    }
                }
            }
        }

        const string s_TestNameFormat = "Size: {0}, Content: {1}, Pattern: {2}";

        public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
                var realRandom = new Random();
                var zerosOnly = new ZeroRandomizer();
                var onesOnly = new OneRandomizer();
                int[] matrixSizes = new[] { 21, 33, 45 };

                foreach (int pattern in Enum.GetValues(typeof(MaskPatternType)))
                {
                    foreach (int matrixSize in matrixSizes)
                    {   
                        yield return GenerateRandomTestCaseData(matrixSize, zerosOnly, pattern).SetName(string.Format(s_TestNameFormat, matrixSize, "ALL 0", pattern));
                        yield return GenerateRandomTestCaseData(matrixSize, onesOnly, pattern).SetName(string.Format(s_TestNameFormat, matrixSize, "ALL 1", pattern));
                        yield return GenerateRandomTestCaseData(matrixSize, realRandom, pattern).SetName(string.Format(s_TestNameFormat, matrixSize, "RANDOM", pattern));
                    }
                }
            }
        }

        private TestCaseData GenerateRandomTestCaseData(int matrixSize, Random randomizer, int pattern)
        {
            ByteMatrix matrix;
            TriStateMatrix input = GetOriginal(matrixSize, randomizer, out matrix);
            ApplyPattern(matrix, pattern);
            BitMatrix expected = matrix.ToBitMatrix();
            return new TestCaseData(input, pattern, expected);
        }


        private class ZeroRandomizer : Random
        {
            public override int Next(int minValue, int maxValue)
            {
                return 0;
            }
        }

        private class OneRandomizer : Random
        {
            public override int Next(int minValue, int maxValue)
            {
                return 1;
            }
        }

        private const string s_DataRowIdColumnName = "DataRowId";
        private const string s_OriginalMatrixColumnName = "OriginalMatrix";
        private const string s_MaskPatternColumnName = "MaskPattern";
        private const string s_ResultingMatrixColumnName = "ResultingMatrix";
        private const string s_TxtFileName = "MaskPatternTestDataSet.txt";

        public void GenerateMaskPatternTestDataSet()
        {
            string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
            using (var file = File.CreateText(path))
            {
                foreach (TestCaseData testCaseData in TestCasesFromReferenceImplementation)
                {
                    ((BitMatrix)testCaseData.Arguments[0]).ToGraphic(file);
                    string maskPattern = testCaseData.Arguments[1].ToString();
                    file.WriteLine(maskPattern);
                    ((BitMatrix)testCaseData.Arguments[2]).ToGraphic(file);

                }
                file.Close();
            }
        }

        private TriStateMatrix GetOriginal(int matrixSize, Random randomizer, out ByteMatrix matrix)
        {
            matrix = new ByteMatrix(matrixSize, matrixSize);

            FillRandom(matrix, randomizer);
            return matrix.ToPatternBitMatrix();
        }

        private void FillRandom(ByteMatrix matrix, Random randomizer)
        {
            for (int i = 0; i < matrix.Width; i++)
            {
                for (int j = 0; j < matrix.Height; j++)
                {
                    var randomValue = randomizer.Next(0, 2);
                    matrix[i, j] = (sbyte)randomValue;
                }
            }
        }

        private void ApplyPattern(ByteMatrix matrix, int pattern)
        {
            for (int i = 0; i < matrix.Width; i++)
            {
                for (int j = 0; j < matrix.Height; j++)
                {
                    int bit = matrix[i, j];
                    if (MaskUtil.getDataMaskBit(pattern, j, i))
                    {
                        bit ^= 0x1;
                    }

                    matrix[i, j] = (sbyte)bit;
                }
            }
            MatrixUtil.embedTypeInfo(ErrorCorrectionLevelInternal.H, pattern, matrix);
        }

    }
}
