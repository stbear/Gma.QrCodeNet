using System;
using System.Collections.Generic;
using com.google.zxing.qrcode.decoder;
using VersionZX = com.google.zxing.qrcode.decoder.Version;
using Version = Gma.QrCodeNet.Encoding.Versions.QRCodeVersion;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.Tests.Versions.TestCases
{
	public class VersionTableTest
	{
		public static IEnumerable<VersionTableTestProperties> GetZXingVersionTable
		{
			get
			{
				for(int versionNum = 1; versionNum <= 40; versionNum++)
				{
					VersionZX versionZX = VersionZX.getVersionForNumber(versionNum);
					int totalNumCodewords = versionZX.TotalCodewords;
					foreach(int levelValue in Enum.GetValues(typeof(ErrorCorrectionLevel)))
					{
						ErrorCorrectionLevelInternal ecLevel = ECLevelConvert((ErrorCorrectionLevel)levelValue);
						VersionZX.ECBlocks ecBlocks = versionZX.getECBlocksForLevel(ecLevel);
						int numOfECCodewords = ecBlocks.TotalECCodewords;
						string ecBlockString = ECBlocksToString(ecBlocks);
						yield return new VersionTableTestProperties(versionNum, totalNumCodewords, (ErrorCorrectionLevel)levelValue, numOfECCodewords, ecBlockString);
					}
				}
			}
		}
		
		private static ErrorCorrectionLevelInternal ECLevelConvert(ErrorCorrectionLevel levelValue)
		{
			switch(levelValue)
			{
				case ErrorCorrectionLevel.L:
					return ErrorCorrectionLevelInternal.L;
				case ErrorCorrectionLevel.M:
					return ErrorCorrectionLevelInternal.M;
				case ErrorCorrectionLevel.Q:
					return ErrorCorrectionLevelInternal.Q;
				case ErrorCorrectionLevel.H:
					return ErrorCorrectionLevelInternal.H;
				default:
					throw new InvalidOperationException(string.Format("ErrorCorrection level {0} not correct.", levelValue));
			}
		}
		
		private const string s_Separator = "-";
		
		private static string ECBlocksToString(VersionZX.ECBlocks ecBlocks)
		{
			VersionZX.ECB[] ecBlock = ecBlocks.getECBlocks();
			string returnValue = "";
			foreach(VersionZX.ECB ecb in ecBlock)
			{
				returnValue = string.Join(s_Separator, returnValue, ecb.Count, ecb.DataCodewords);
			}
			return returnValue;
		}
		
		
		public static VersionTableTestProperties GetVersionInfo(int versionNum, ErrorCorrectionLevel level)
		{
			Version version = VersionTable.GetVersionByNum(versionNum);
			int totalNumCodewords = version.TotalCodewords;
			ErrorCorrectionBlocks ecBlocks = version.GetECBlocksByLevel(level);
			int numECCodewords = ecBlocks.NumErrorCorrectionCodewards;
			string ecBlockString = ErrorCorrectionBlocksToString(ecBlocks);
			
			return new VersionTableTestProperties(versionNum, totalNumCodewords, level, numECCodewords, ecBlockString);
			
		}
		
		private static string ErrorCorrectionBlocksToString(ErrorCorrectionBlocks ecBlocks)
		{
			ErrorCorrectionBlock[] ecBlock = ecBlocks.GetECBlocks();
			string returnValue = "";
			foreach(ErrorCorrectionBlock ecb in ecBlock)
			{
				returnValue = string.Join(s_Separator, returnValue, ecb.NumErrorCorrectionBlock, ecb.NumDataCodewords);
			}
			return returnValue;
		}
	}
}
