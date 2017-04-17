using System;
using System.Collections.Generic;
using com.google.zxing.qrcode.encoder;
using NUnit.Framework;


namespace Gma.QrCodeNet.Encoding.Tests.EncodingRegion
{
	public class BitCountTestCaseFactory : BCHTestCaseBase
	{
		protected override string FileName {
			get {
				return "BitCountTestCases.csv";
			}
		}
		
		protected override int ReferenceExpectedResult(int inputNum)
		{
			return MatrixUtil.findMSBSet(inputNum);
		}
		
		protected override int GenerateRandomInputNumber(Random randomizer)
		{
			return randomizer.Next(0, 1048575); //1048575 = 20 bits long number.
		}
	}
}
