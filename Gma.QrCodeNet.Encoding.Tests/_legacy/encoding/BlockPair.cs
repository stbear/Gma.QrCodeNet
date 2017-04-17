namespace Gma.QrCodeNet.Encoding.encoding
{
	internal sealed class BlockPair
	{
        internal sbyte[] Data
		{
			get; private set;
		}

        internal sbyte[] ErrorCorrectionCodewords
		{
			get; private set;
		}

        internal BlockPair(sbyte[] data, sbyte[] errorCorrectionCodewords)
		{
			Data = data;
            ErrorCorrectionCodewords = errorCorrectionCodewords;
		}
	}
}