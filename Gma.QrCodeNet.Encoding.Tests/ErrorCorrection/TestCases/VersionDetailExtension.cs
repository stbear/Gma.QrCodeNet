using System;

namespace Gma.QrCodeNet.Encoding.Tests.ErrorCorrection
{
	internal static class VersionDetailExtension
	{
		internal static VersionDetail FromString(string line)
		{
			string[] splitResult = line.Split(new char[]{';'});
			if(splitResult.Length != 4)
				throw new ArgumentException("Given string does not contain int variable required by struct");
			int version = int.Parse(splitResult[0]);
			int numTotalBytes = int.Parse(splitResult[1]);
			int numDataBytes = int.Parse(splitResult[2]);
			int numECBlocks = int.Parse(splitResult[3]);
			
			return new VersionDetail(version, numTotalBytes, numDataBytes, numECBlocks);
		}
	}
}
