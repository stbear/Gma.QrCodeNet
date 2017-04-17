using System;
using Gma.QrCodeNet.Encoding.ReedSolomon;
using com.google.zxing.qrcode.encoder;
using System.Diagnostics;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PerformanceTest
{
	[TestFixture]
	public class ReedSolomonPTest
	{
		[Test]
		public void PerformanceTest()
		{
			Random randomizer = new Random();
			sbyte[] zxTestCase = PolynomialExtensions.GenerateSbyteArray(40, randomizer);
			int ecBytes = 50;
			byte[] testCase = PolynomialExtensions.ToByteArray(zxTestCase);
			
			Stopwatch sw = new Stopwatch();
			int timesofTest = 10000;
			
			string[] timeElapsed = new string[2];
			
			sw.Start();
			GaloisField256 gf256 = GaloisField256.QRCodeGaloisField;
			GeneratorPolynomial generator = new GeneratorPolynomial(gf256);
			for(int i = 0; i < timesofTest; i++)
			{
				ReedSolomonEncoder.Encode(testCase, ecBytes, generator);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				EncoderInternal.generateECBytes(zxTestCase, ecBytes);
			}
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("ReedSolomon performance {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
