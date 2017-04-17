namespace Gma.QrCodeNet.Encoding
{
	public struct VersionDetail
	{
		internal int Version { get; private set; }
		internal int NumTotalBytes { get; private set; }
		internal int NumDataBytes { get; private set; }
		internal int NumECBlocks { get; private set; }
		
		internal VersionDetail(int version, int numTotalBytes, int numDataBytes, int numECBlocks)
			: this()
		{
			this.Version = version;
			this.NumTotalBytes = numTotalBytes;
			this.NumDataBytes = numDataBytes;
			this.NumECBlocks = numECBlocks;
		}
		
		/// <summary>
		/// Width for current version
		/// </summary>
		internal int MatrixWidth
		{
			get
			{
				return Width(this.Version);
			}
		}
		
		internal static int Width(int version)
		{
			return 17 + 4 * version;
		}
		
		/// <summary>
		/// number of Error correction blocks for group 1
		/// </summary>
		internal int ECBlockGroup1
		{
			get
			{
				return this.NumECBlocks - this.ECBlockGroup2;
			}
		}
		
		/// <summary>
		/// Number of error correction blocks for group 2
		/// </summary>
		internal int ECBlockGroup2
		{
			get
			{
				return this.NumTotalBytes % this.NumECBlocks;
			}
		}
		
		/// <summary>
		/// Number of data bytes per block for group 1
		/// </summary>
		internal int NumDataBytesGroup1
		{
			get
			{
				return this.NumDataBytes / this.NumECBlocks;
			}
		}
		
		/// <summary>
		/// Number of data bytes per block for group 2
		/// </summary>
		internal int NumDataBytesGroup2
		{
			get
			{
				return this.NumDataBytesGroup1 + 1;
			}
		}
		
		/// <summary>
		/// Number of error correction bytes per block
		/// </summary>
		internal int NumECBytesPerBlock
		{
			get
			{
				return (this.NumTotalBytes - this.NumDataBytes) / this.NumECBlocks;
			}
		}
		
		public override string ToString()
		{
			return this.Version + ";" + this.NumTotalBytes + ";" + this.NumDataBytes + ";" + this.NumECBlocks;
		}
		
	}
}
