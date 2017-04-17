using System;
using System.IO;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Masking.Scoring;
using NUnit.Framework;
using Gma.QrCodeNet.Encoding.Common;

namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	public abstract class PenaltyTestBase
	{
		public virtual void Test_against_reference_implementation(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			TestPenaltyRule(input, penaltyRule, expected);
		}
		
		private void TestPenaltyRule(BitMatrix input, PenaltyRules penaltyRule, int expected)
		{
			Penalty penalty = new PenaltyFactory().CreateByRule(penaltyRule);
			
			int result = penalty.PenaltyCalculate(input);
			
			AssertIntEquals(expected, result, input, penaltyRule);
		}
		
		protected static void AssertIntEquals(int expected, int actual, BitMatrix matrix, PenaltyRules penaltyRule)
        {
			if(expected != actual)
			{
				GenerateFaultyRecord(matrix, penaltyRule, expected, actual);
				Assert.Fail("Penalty scores are different.\nExpected:{0}Actual:{1}.", expected.ToString(), actual.ToString());
				
			}
		}
		
		private const string s_TxtFileName = "MatrixOfFailedPenaltyScore.txt";
		
        public static void GenerateFaultyRecord(BitMatrix matrix, PenaltyRules penaltyRule, int expected, int actual)
        {
            string path = Path.Combine(Path.GetTempPath(), s_TxtFileName);
            
            if(!File.Exists(path))
            {
            	using (StreamWriter file = File.CreateText(path)) 
              	{
            		file.WriteLine();
            	}
            }
            
            using (var file = File.AppendText(path))
            {
            	file.Write(penaltyRule.ToString());
            	file.Write(string.Format(" Expected: {0}, Actual: {0}", expected.ToString(), actual.ToString()));
                matrix.ToGraphic(file);
                file.WriteLine("=====");
                file.Close();
                
            }
        }
		
	}
}
