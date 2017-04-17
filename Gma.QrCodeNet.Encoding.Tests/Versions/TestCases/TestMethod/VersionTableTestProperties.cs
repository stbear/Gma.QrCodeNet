namespace Gma.QrCodeNet.Encoding.Tests.Versions.TestCases
{
	public struct VersionTableTestProperties
	{
		public int VersionNum {get; private set;}
		public int TotalNumOfCodewords { get; private set; }
		public ErrorCorrectionLevel ErrorCorrectionLevel { get; private set; }
		public int NumOfECCodewords { get; private set; }
		public string ECBlockString { get; private set; }
		
		public VersionTableTestProperties(int versionNum, int totalCodewords, ErrorCorrectionLevel level, int numECCodewords, string ecBlockString)
			:this()
		{
			this.VersionNum = versionNum;
			this.TotalNumOfCodewords = totalCodewords;
			this.ErrorCorrectionLevel = level;
			this.NumOfECCodewords = numECCodewords;
			this.ECBlockString = ecBlockString;
		}
		
	}
}
