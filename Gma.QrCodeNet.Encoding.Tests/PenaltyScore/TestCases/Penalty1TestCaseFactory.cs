using System;
using Gma.QrCodeNet.Encoding.Common;
using NUnit.Framework;
using com.google.zxing.qrcode.encoder;
using Gma.QrCodeNet.Encoding.Masking;
using Gma.QrCodeNet.Encoding.Masking.Scoring;

namespace Gma.QrCodeNet.Encoding.Tests.PenaltyScore
{
	
	public class Penalty1TestCaseFactory : PenaltyScoreTestCaseFactory
	{
		protected override string TxtFileName { get { return "Penalty1TestDataSet.txt"; } }
		
		protected override NUnit.Framework.TestCaseData GenerateRandomTestCaseData(int matrixSize, System.Random randomizer, MaskPatternType pattern)
		{
			return base.GenerateRandomTestCaseData(matrixSize, randomizer, pattern, PenaltyRules.Rule01);
		}
		
	}
}
