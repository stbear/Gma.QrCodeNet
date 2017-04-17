using System;
using System.Collections.Generic;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Common;
using Gma.QrCodeNet.Encoding.Masking;
using Gma.QrCodeNet.Encoding.Masking.Scoring;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	public abstract class PenaltyScoreTestCaseFactory
	{
		 public IEnumerable<TestCaseData> TestCasesFromTxtFile
        {
            get
            {
                string path = Path.Combine("PenaltyScore\\TestCases", TxtFileName);
                using (var file = File.OpenText(path))
                {
                    while (!file.EndOfStream)
                    {
                        BitMatrix input = BitMatrixToGraphicExtensions.FromGraphic(file);
                        int penaltyRule = int.Parse(file.ReadLine());
                        int expectValue = int.Parse(file.ReadLine());
                        yield return new TestCaseData(input, penaltyRule, expectValue).SetName(string.Format(s_TestNameFormat, input.Width, penaltyRule, expectValue));
                    }
                }
            }
        }

        const string s_TestNameFormat = "Size: {0}, Rule: {1}, Expect: {2}";

        public IEnumerable<TestCaseData> TestCasesFromReferenceImplementation
        {
            get
            {
                var realRandom = new Random();
                var zerosOnly = new ZeroRandomizer();
                var onesOnly = new OneRandomizer();
                int[] matrixSizes = new[] { 1, 10, 32 };

                foreach (MaskPatternType pattern in Enum.GetValues(typeof(MaskPatternType)))
                {
                    foreach (int matrixSize in matrixSizes)
                    {   
                        yield return GenerateRandomTestCaseData(matrixSize, zerosOnly, pattern);
                        yield return GenerateRandomTestCaseData(matrixSize, onesOnly, pattern);
                        yield return GenerateRandomTestCaseData(matrixSize, realRandom, pattern);
                    }
                }
            }
        }

        protected abstract TestCaseData GenerateRandomTestCaseData(int matrixSize, Random randomizer, MaskPatternType pattern);

        
        protected virtual TestCaseData GenerateRandomTestCaseData(int matrixSize, Random randomizer, MaskPatternType pattern, PenaltyRules rules)
        {
        	ByteMatrix matrix;
            
			BitMatrix bitmatrix = GetOriginal(matrixSize, randomizer, out matrix);
			
			ApplyPattern(matrix, (int)pattern);
			
			int expect;
			
			switch(rules)
			{
				case PenaltyRules.Rule01:
					expect = MaskUtil.applyMaskPenaltyRule1(matrix);
					break;
				case PenaltyRules.Rule02:
					expect = MaskUtil.applyMaskPenaltyRule2(matrix);
					break;
				case PenaltyRules.Rule03:
					expect = MaskUtil.applyMaskPenaltyRule3(matrix);
					break;
				case PenaltyRules.Rule04:
					expect = MaskUtil.applyMaskPenaltyRule4(matrix);
					break;
				default:
					throw new InvalidOperationException(string.Format("Unsupport Rules {0}", rules.ToString()));
			}
			
			
            BitMatrix input = matrix.ToBitMatrix();
            
            return new TestCaseData(input, (int)rules, expect).SetName(string.Format(s_TestNameFormat, input.Width, rules.ToString(), expect));
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

        
        protected abstract string TxtFileName { get; }

        public virtual void GenerateTestDataSet()
        {
            string path = Path.Combine(Path.GetTempPath(), TxtFileName);
            using (var file = File.CreateText(path))
            {
                foreach (TestCaseData testCaseData in TestCasesFromReferenceImplementation)
                {
                    ((BitMatrix)testCaseData.Arguments[0]).ToGraphic(file);
                    string penaltyRule = testCaseData.Arguments[1].ToString();
                    file.WriteLine(penaltyRule);
                    string expectValue = testCaseData.Arguments[2].ToString();
                    file.WriteLine(expectValue);

                }
                file.Close();
            }
        }

        internal BitMatrix GetOriginal(int matrixSize, Random randomizer, out ByteMatrix matrix)
        {
            matrix = new ByteMatrix(matrixSize, matrixSize);

            FillRandom(matrix, randomizer);
            return matrix.ToBitMatrix();
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

        internal void ApplyPattern(ByteMatrix matrix, int pattern)
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
        }

    }
}