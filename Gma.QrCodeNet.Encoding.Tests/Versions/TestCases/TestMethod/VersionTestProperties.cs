using Gma.QrCodeNet.Encoding.DataEncodation;
using Gma.QrCodeNet.Encoding.Versions;

namespace Gma.QrCodeNet.Encoding.Tests.Versions.TestCases
{
	public struct VersionTestProperties
	{
		public int NumDataBitsForEncodedContent { get; private set;}
		
		public Mode Mode { get; private set;}
		
		public ErrorCorrectionLevel ECLevel { get; private set; }
		
		public string EncodingName { get; private set; }
		
		public int ExpectVersionNum { get; private set; }
		
		public VersionTestProperties(int numDataBits, Mode mode, ErrorCorrectionLevel level, string encodingName, int versionNum)
			: this()
		{
			this.NumDataBitsForEncodedContent = numDataBits;
			this.Mode = mode;
			this.ECLevel = level;
			this.EncodingName = encodingName;
			this.ExpectVersionNum = versionNum;
		}
	}
}
