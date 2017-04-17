using Gma.QrCodeNet.Encoding.Common;

namespace Gma.QrCodeNet.Encoding.Tests
{
    public static class MatrixConverter
    {
        public static ByteMatrix ToByteMatrix(this BitMatrix bitMatrix)
        {
            ByteMatrix result = new ByteMatrix(bitMatrix.Width, bitMatrix.Width);
            for (int i = 0; i < bitMatrix.Width; i++)
            {
                for (int j = 0; j < bitMatrix.Width; j++)
                {
                    result[i, j] = (sbyte)(bitMatrix[i, j] ? 1 : 0);
                }
            }
            return result;
        }
    }
}