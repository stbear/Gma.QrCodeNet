using System;
using Gma.QrCodeNet.Encoding.Versions;
using com.google.zxing.qrcode.encoder;
using Mode = Gma.QrCodeNet.Encoding.DataEncodation.Mode;
using ZMode = com.google.zxing.qrcode.decoder.Mode;
using com.google.zxing.qrcode.decoder;
using System.Diagnostics;
using NUnit.Framework;

namespace Gma.QrCodeNet.Encoding.Tests.PerformanceTest
{
	[TestFixture]
	public class VersionControlPTest
	{
		[Test]
		public void SmallLengthTest()
		{
			PTest(40);
		}
		
		[Test]
		public void HugeLengthTest()
		{
			PTest(9776);
		}
		
		private void PTest(int contentLength)
		{
			Stopwatch sw = new Stopwatch();
			int timesofTest = 1000;
			
			string[] timeElapsed = new string[2];
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				VersionControl.InitialSetup(contentLength, Mode.Alphanumeric, ErrorCorrectionLevel.H, QRCodeConstantVariable.DefaultEncoding);
			}
			
			sw.Stop();
			
			timeElapsed[0] = sw.ElapsedMilliseconds.ToString();
			
			sw.Reset();
			
			QRCodeInternal qrInternal = new QRCodeInternal();
			
			int byteLength = contentLength / 8;
			
			sw.Start();
			
			for(int i = 0; i < timesofTest; i++)
			{
				EncoderInternal.initQRCode(byteLength, ErrorCorrectionLevelInternal.H, ZMode.ALPHANUMERIC, qrInternal);
			}
			
			sw.Stop();
			
			timeElapsed[1] = sw.ElapsedMilliseconds.ToString();
			
			
			Assert.Pass("VersionControl {0} Tests~ QrCode.Net: {1} ZXing: {2}", timesofTest, timeElapsed[0], timeElapsed[1]);
			
		}
	}
}
