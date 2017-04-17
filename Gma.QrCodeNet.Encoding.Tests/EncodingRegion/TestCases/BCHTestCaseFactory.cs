using System;
using System.Collections.Generic;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	public class BCHTestCaseFactory : BCHTestCaseBase
	{
		protected override string FileName {
			get {
				return "BCHTestCases.csv";
			}
		}
		
		protected override int GenerateRandomInputNumber(Random randomizer)
		{
			return randomizer.Next(1, 64);
		}
		
		// From Appendix D in JISX0510:2004 (p. 67)
		private const int VERSION_INFO_POLY = 0x1f25; // 1 1111 0010 0101
		
		// From Appendix C in JISX0510:2004 (p.65).
		private const int TYPE_INFO_POLY = 0x537;
		
		protected override int ReferenceExpectedResult(int inputNum)
		{
			return MatrixUtil.calculateBCHCode(inputNum, VERSION_INFO_POLY);
		}
	}
}
