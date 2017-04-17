using System;
using Gma.QrCodeNet.Encoding.Terminate;
using com.google.zxing.qrcode.encoder;
using System.Diagnostics;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PerformanceTest
{
	[TestFixture]
	public class TerminatorPTest
	{
		[Test]
		public void PerformanceTest()
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				BitList list = new BitList();
				list.TerminateBites(0, 400);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				BitVector headerAndDataBits = new BitVector();
				//headerAndDataBits.Append(1, 1);
				EncoderInternal.terminateBits(400, headerAndDataBits);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("Terminator performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
